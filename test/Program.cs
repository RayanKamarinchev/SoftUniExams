using test;

make make = (make)2;
if (!Enum.IsDefined(typeof(make), make) && !make.ToString().Contains(","))
{
    Console.WriteLine("ok");
}
Console.WriteLine(make);