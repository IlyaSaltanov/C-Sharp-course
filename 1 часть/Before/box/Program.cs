// See https://aka.ms/new-console-template for more information




var a = int.Parse(Console.ReadLine());
var b = int.Parse(Console.ReadLine());
var c = int.Parse(Console.ReadLine());

var x = int.Parse(Console.ReadLine());
var y = int.Parse(Console.ReadLine());

if ((a <= x && b <= y) || (a <= y && b <= x) || (c <= x && a <= y) || (c <= y && a <= x))
{
  return true;
}
return false;