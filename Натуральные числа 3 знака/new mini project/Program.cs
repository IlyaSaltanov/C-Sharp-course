// See https://aka.ms/new-console-template for more information
int n = int.Parse(Console.ReadLine());
for (int i = 100; i <= 999; i++)
{
  // i = (string)i
  var st = Convert.ToString(i);
  if (Convert.ToInt32(st[0]) + Convert.ToInt32(st[1]) + Convert.ToInt32(st[2]) == n)
  {
    Console.WriteLine(i);
  }
}