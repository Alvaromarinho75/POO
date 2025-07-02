using System;
using System.Diagnostics;
using System.Numerics;

class Program
{
    static void Main()
    {
        TestarPerformanceCompleta();
    }

    public static void TestarPerformanceCompleta()
    {
        int[] tamanhosExercito = { 10_000, 50_000, 100_000, 500_000, 1_000_000 };

        Console.WriteLine("=== BENCHMARK DE SISTEMA DE COMBATE ===");
        Console.WriteLine($"SIMD Suportado: {Vector.IsHardwareAccelerated}");
        Console.WriteLine($"Elementos por Vetor: {Vector<int>.Count}");
        Console.WriteLine();

        foreach (int tamanho in tamanhosExercito)
        {
            Console.WriteLine($"Testando exércitos de {tamanho:N0} personagens:");

            // Gera exércitos
            var atacantes = SimuladorCombate.GerarExercito(tamanho, "atacante");
            var defensores = SimuladorCombate.GerarExercito(tamanho, "defensor");

            // Versão original
            var sw = Stopwatch.StartNew();
            int danoOriginal = SimuladorCombate.SimularRodadaCombate(atacantes, defensores);
            sw.Stop();
            double tempoOriginal = sw.Elapsed.TotalMilliseconds;

            // Versão SIMD
            var exAtacantes = new ExercitoSIMD(tamanho);
            var exDefensores = new ExercitoSIMD(tamanho);
            exAtacantes.ConverterDePersonagens(atacantes);
            exDefensores.ConverterDePersonagens(defensores);

            sw.Restart();
            int danoSIMD = SimuladorCombateSIMD.CalcularDanoVetorizado(exAtacantes, exDefensores);
            sw.Stop();
            double tempoSIMD = sw.Elapsed.TotalMilliseconds;

            double speedup = tempoOriginal / tempoSIMD;
            double dpsOriginal = (double)danoOriginal * 1000.0 / Math.Max(1.0, tempoOriginal);
            double dpsSIMD = (double)danoSIMD * 1000.0 / Math.Max(1.0, tempoSIMD);

            Console.WriteLine($"Original: {tempoOriginal:F2}ms, DPS: {dpsOriginal:N0}");
            Console.WriteLine($"SIMD:     {tempoSIMD:F2}ms, DPS: {dpsSIMD:N0}");
            Console.WriteLine($"Speedup:  {speedup:F2}x");
            Console.WriteLine();
        }
    }
} 