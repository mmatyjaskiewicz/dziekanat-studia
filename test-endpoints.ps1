$baseUrl = "http://localhost:5000"
$pass = 0
$fail = 0
$results = @()

function Test-Endpoint {
    param([string]$Name, [string]$Method, [string]$Url, [string]$Token = "", [string]$Body = "", [int]$ExpectedStatus = 200)
    $headers = @{}
    if ($Token) { $headers["Authorization"] = "Bearer $Token" }
    if ($Body) { $headers["Content-Type"] = "application/json" }
    try {
        $params = @{
            Uri = $Url
            Method = $Method
            Headers = $headers
            UseBasicParsing = $true
        }
        if ($Body) { $params["Body"] = $Body }
        $r = Invoke-WebRequest @params -ErrorAction Stop
        $status = $r.StatusCode
        $bodyOut = $r.Content
    } catch {
        $status = $_.Exception.Response.StatusCode.value__
        $bodyOut = $_.Exception.Response.StatusDescription
    }
    $ok = $status -eq $ExpectedStatus
    $script:results += [PSCustomObject]@{
        Name = $Name
        Expected = $ExpectedStatus
        Actual = $status
        OK = $ok
        Body = if ($bodyOut.Length -gt 100) { $bodyOut.Substring(0,100) } else { $bodyOut }
    }
    if ($ok) { $script:pass++ } else { $script:fail++ }
}

Write-Host "=== LOGIN ===" -ForegroundColor Cyan
$adminToken = (Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -ContentType "application/json" -Body '{"email":"admin@app.pl","password":"Admin@123!"}' -UseBasicParsing).Content | ConvertFrom-Json | Select-Object -ExpandProperty accessToken
$deanToken = (Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -ContentType "application/json" -Body '{"email":"jan.kowalski@app.pl","password":"Dean@123!"}' -UseBasicParsing).Content | ConvertFrom-Json | Select-Object -ExpandProperty accessToken
$lectToken = (Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -ContentType "application/json" -Body '{"email":"anna.nowak@app.pl","password":"Lect@123!"}' -UseBasicParsing).Content | ConvertFrom-Json | Select-Object -ExpandProperty accessToken
Write-Host "All 3 tokens issued"

Write-Host "`n=== AUTH ===" -ForegroundColor Cyan
Test-Endpoint "Login admin wrong" "POST" "$baseUrl/api/auth/login" -Body '{"email":"admin@app.pl","password":"WRONG"}' -ExpectedStatus 401
Test-Endpoint "Login no user" "POST" "$baseUrl/api/auth/login" -Body '{"email":"nobody@app.pl","password":"x"}' -ExpectedStatus 401

Write-Host "`n=== STUDENTS ===" -ForegroundColor Cyan
Test-Endpoint "GET students (dean)" "GET" "$baseUrl/api/students" $deanToken -ExpectedStatus 200
Test-Endpoint "GET students (no auth)" "GET" "$baseUrl/api/students" -ExpectedStatus 401
Test-Endpoint "GET students (lecturer)" "GET" "$baseUrl/api/students" $lectToken -ExpectedStatus 403
Test-Endpoint "GET students (admin)" "GET" "$baseUrl/api/students" $adminToken -ExpectedStatus 200
Test-Endpoint "POST student (dean) valid" "POST" "$baseUrl/api/students" $deanToken -Body '{"firstName":"Jan","lastName":"Kowalski","email":"jan.kowalski.student@app.pl","nationalId":"00301512345","studentId":"S12345","yearOfStudy":1,"programCode":"INF","enrollmentYearFrom":2024}' -ExpectedStatus 201
Test-Endpoint "POST student (dean) invalid pesel" "POST" "$baseUrl/api/students" $deanToken -Body '{"firstName":"X","lastName":"Y","email":"x@y.pl","nationalId":"123","studentId":"S1","yearOfStudy":1,"programCode":"X","enrollmentYearFrom":2024}' -ExpectedStatus 400
Test-Endpoint "POST student (no auth)" "POST" "$baseUrl/api/students" -Body '{"firstName":"X","lastName":"Y","email":"x@y.pl","nationalId":"00301512345","studentId":"S2","yearOfStudy":1,"programCode":"X","enrollmentYearFrom":2024}' -ExpectedStatus 401

Write-Host "`n=== LECTURERS ===" -ForegroundColor Cyan
Test-Endpoint "GET lecturers (dean)" "GET" "$baseUrl/api/lecturers" $deanToken -ExpectedStatus 200
Test-Endpoint "GET lecturers (no auth)" "GET" "$baseUrl/api/lecturers" -ExpectedStatus 401
Test-Endpoint "GET lecturers (lecturer)" "GET" "$baseUrl/api/lecturers" $lectToken -ExpectedStatus 403
Test-Endpoint "GET lecturers (admin)" "GET" "$baseUrl/api/lecturers" $adminToken -ExpectedStatus 200
Test-Endpoint "POST lecturer (dean)" "POST" "$baseUrl/api/lecturers" $deanToken -Body '{"firstName":"Adam","lastName":"Nowak","email":"adam.nowak@app.pl","title":"dr","department":"Math"}' -ExpectedStatus 201

Write-Host "`n=== STUDENT/GRADE FLOW ===" -ForegroundColor Cyan
Test-Endpoint "GET student (dean) existing" "GET" "$baseUrl/api/students" $deanToken -ExpectedStatus 200
$studentList = (Invoke-WebRequest -Uri "$baseUrl/api/students?page=1&size=1" -Method GET -Headers @{Authorization="Bearer $deanToken"} -UseBasicParsing).Content | ConvertFrom-Json
if ($studentList.items.Count -gt 0) {
    $sid = $studentList.items[0].id
    Write-Host "  Using student id: $sid"
    Test-Endpoint "GET student by id" "GET" "$baseUrl/api/students/$sid" $deanToken -ExpectedStatus 200
    Test-Endpoint "GET student grades" "GET" "$baseUrl/api/students/$sid/grades" $deanToken -ExpectedStatus 200
    Test-Endpoint "POST student grade" "POST" "$baseUrl/api/students/$sid/grades" $deanToken -Body '{"courseId":"00000000-0000-0000-0000-000000000001","lecturerId":"259ff25d-e119-42a8-a92c-00180a83b7d4","academicYearId":"00000000-0000-0000-0000-000000000003","issueDate":"2026-06-01T00:00:00Z","gradeValue":40}' -ExpectedStatus 201
    Test-Endpoint "POST assign-program invalid" "POST" "$baseUrl/api/students/$sid/assign-program" $deanToken -Body '{"degreeProgramId":"00000000-0000-0000-0000-000000000000","academicYearId":null}' -ExpectedStatus 400
    Test-Endpoint "POST change-status valid" "POST" "$baseUrl/api/students/$sid/change-status" $deanToken -Body '{"status":2,"reason":"test"}' -ExpectedStatus 204
}

Write-Host "`n=== LECTURER DETAIL ===" -ForegroundColor Cyan
$lectList = (Invoke-WebRequest -Uri "$baseUrl/api/lecturers?page=1&size=1" -Method GET -Headers @{Authorization="Bearer $deanToken"} -UseBasicParsing).Content | ConvertFrom-Json
if ($lectList.items.Count -gt 0) {
    $lid = $lectList.items[0].id
    Write-Host "  Using lecturer id: $lid"
    Test-Endpoint "GET lecturer by id" "GET" "$baseUrl/api/lecturers/$lid" $deanToken -ExpectedStatus 200
    Test-Endpoint "GET lecturer courses" "GET" "$baseUrl/api/lecturers/$lid/courses" $deanToken -ExpectedStatus 200
    Test-Endpoint "GET lecturer students" "GET" "$baseUrl/api/lecturers/$lid/students" $deanToken -ExpectedStatus 200
}

Write-Host "`n=== REFRESH TOKEN ===" -ForegroundColor Cyan
$rtBody = (Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -ContentType "application/json" -Body '{"email":"admin@app.pl","password":"Admin@123!"}' -UseBasicParsing).Content | ConvertFrom-Json
Test-Endpoint "Refresh token" "POST" "$baseUrl/api/auth/refresh" -Body (@{accessToken=$rtBody.accessToken; refreshToken=$rtBody.refreshToken} | ConvertTo-Json) -ExpectedStatus 200

Write-Host "`n=== RESULTS ===" -ForegroundColor Cyan
$results | Format-Table -AutoSize
Write-Host "PASS: $pass  FAIL: $fail  TOTAL: $($pass+$fail)" -ForegroundColor Yellow
