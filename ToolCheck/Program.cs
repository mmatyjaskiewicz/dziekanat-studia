static int PeselCheck(string s) {
    int[] w = {1,3,7,9,1,3,7,9,1,3};
    int sum = 0;
    for (int i = 0; i < 10; i++) sum += (s[i] - '0') * w[i];
    return (10 - sum % 10) % 10;
}

string[] dates = {"000101","950101","000915"};
int s = 100;
foreach (var d in dates) {
    string body = d + s.ToString("D4");
    int c = PeselCheck(body);
    Console.WriteLine(body + c);
    s++;
}
