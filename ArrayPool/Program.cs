using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("--- Versão Trivial ---");
        var sw = Stopwatch.StartNew();
        var initialMemory = GC.GetTotalMemory(true);
        ImageProcessorTrivial.ProcessImages();
        var finalMemory = GC.GetTotalMemory(true);
        sw.Stop();
        Console.WriteLine($"Memória inicial: {initialMemory / 1024 / 1024:F2} MB");
        Console.WriteLine($"Memória final: {finalMemory / 1024 / 1024:F2} MB");
        Console.WriteLine($"Diferença de memória: {(finalMemory - initialMemory) / 1024 / 1024:F2} MB");
        Console.WriteLine($"Coleções GC Gen0: {GC.CollectionCount(0)}");
        Console.WriteLine($"Coleções GC Gen1: {GC.CollectionCount(1)}");
        Console.WriteLine($"Coleções GC Gen2: {GC.CollectionCount(2)}");

        Console.WriteLine("\n--- Versão Otimizada (ArrayPool) ---\n");
        sw = Stopwatch.StartNew();
        initialMemory = GC.GetTotalMemory(true);
        ImageProcessorArrayPool.ProcessImages();
        finalMemory = GC.GetTotalMemory(true);
        sw.Stop();
        Console.WriteLine($"Memória inicial: {initialMemory / 1024 / 1024:F2} MB");
        Console.WriteLine($"Memória final: {finalMemory / 1024 / 1024:F2} MB");
        Console.WriteLine($"Diferença de memória: {(finalMemory - initialMemory) / 1024 / 1024:F2} MB");
        Console.WriteLine($"Coleções GC Gen0: {GC.CollectionCount(0)}");
        Console.WriteLine($"Coleções GC Gen1: {GC.CollectionCount(1)}");
        Console.WriteLine($"Coleções GC Gen2: {GC.CollectionCount(2)}");
    }
} 