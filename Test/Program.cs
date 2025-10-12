

using System;

string st = Console.ReadLine();
int count = st.Length;
for (int i = count-1; i >= 0; i--)
{
  Console.Write(st[i]);
  // Console.WriteLine(st[i]);
  // Console.Write(st.Substring(i,i+1));
}
// Console.WriteLine("Hello world");