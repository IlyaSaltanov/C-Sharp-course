// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// int n = int.Parse(Console.ReadLine());
// for (int i = 100; i <= 999; i++)
// {
//   var st = i.ToString(); // В чем разница с 8 строчкой?
//                          // var st = i.Convert.ToString();
//                          // var st = Convert.ToString(i); Можно ли так?

//   // Console.WriteLine(i);
//   // int chislo = Convert.ToInt32(st[0]);
//   // Console.WriteLine(chislo);
//   // Console.WriteLine(st[0]);

//   // if ((Convert.ToInt32(st[0]) + Convert.ToInt32(st[1]) + Convert.ToInt32(st[2])) == n)
//   // {
//   //   Console.WriteLine(st);
//   // }

//   if ((int.Parse(st.Substring(0,0))) + (int.Parse(st[1]) )+ int.Parse(st[2].ToString()) == n)
//   {
//     Console.WriteLine(st);
//   }
// }













// Дана строка из символов '(' и ')'. Определить, является ли она корректным скобочным выражением. Определить максимальную глубину вложенности скобок.


string st = Console.ReadLine();
int count = st.Length;
if (count % 2 == 0)
{
  bool flag = true;
  for (int i = 0; i <= (count / 2); i++)
  {
    if (st[i] != st[^(-i - 1)])
    {
      flag = false;
      Console.WriteLine("False");
      break;
    }
  }
  if (flag == true)
  {
    Console.WriteLine("False");
  }
}
