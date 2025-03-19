using System;
using CAS;

class Program
{
    static void Main()
    {   
        Expressao x = "x";
        Expressao expr = new Soma(new Divisao(2, x), 3);
        Console.WriteLine($"Expressão original: {expr}");
        Expressao exprD = expr.Derivar((Simbolo)x);
        Console.WriteLine($"Expressão derivada: {exprD}");
        Console.WriteLine($"Expressão substituída (x = 5): {expr.Substituir((Simbolo)x, new Numero(5))}");
    }
}