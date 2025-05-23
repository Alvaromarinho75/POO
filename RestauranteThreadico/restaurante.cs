﻿using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text;
using System.Collections.Generic;

BlockingCollection<(int pedido, int prato)> pedidos = new BlockingCollection<(int pedido, int prato)>();
object lockConsole = new();

Dictionary<string, int> Estoque = new Dictionary<string, int>
{
    { "Arroz", 3 },
    { "Carne", 2 },
    { "Macarrao", 4 },
    { "Molho", 2 }
};


void ConsoleLock(string msg, ConsoleColor color)
{
    lock(lockConsole)
    {
        var aux = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ForegroundColor = aux;
    }
}
int pedido = 0;

int podePreparar(int prato)
{
    switch (prato)
    {
        case 1:
            if (Estoque["Arroz"] <= 0) return 4; 
            if (Estoque["Carne"] <= 0) return 5; 
            return 0; 

        case 2:
            if (Estoque["Macarrao"] <= 0) return 6;
            if (Estoque["Molho"] <= 0) return 7;
            return 0; 

        case 3:
            if (Estoque["Arroz"] <= 0) return 4; 
            if (Estoque["Carne"] <= 0) return 5; 
            if (Estoque["Molho"] <= 0) return 7; 
            return 0; 

        default:
            return 1; 
    }
}

void Garcom()
{
    var rnd = new Random();
    var id = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"[Garcom {id}] Estou pronto!!!");
    while(true)
    {
        int tempo = rnd.Next(1000, 10000);
        int prato = rnd.Next(1, 4);
        int p = Interlocked.Increment(ref pedido);

        Thread.Sleep(tempo);

        ConsoleLock($"[Garcom {id}] Enviei pedido {p} do prato {prato}!", ConsoleColor.Blue);
        pedidos.Add((p, prato));
    }
    pedidos.CompleteAdding();
}

void Chef()
{
    var id = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"[Chef {id}] Estou pronto!!!");

    foreach (var item in pedidos.GetConsumingEnumerable())
    {
        var (pedido, prato) = item;
        ConsoleLock($"[Chef {id}] Iniciando o pedido {pedido} do prato {prato}!", ConsoleColor.Red);

        while (true)
        {
            int falta = podePreparar(prato);
            if (falta == 0) break; 

            switch (falta)
            {
                case 4:
                    ConsoleLock($"[Chef {id}] Produzindo arroz...", ConsoleColor.Green);
                    Thread.Sleep(2000); 
                    Estoque["Arroz"] += 3; 
                    ConsoleLock($"[Chef {id}] Arroz produzido!", ConsoleColor.Green);
                    break;

                case 5: 
                    ConsoleLock($"[Chef {id}] Produzindo carne...", ConsoleColor.Green);
                    Thread.Sleep(2000); 
                    Estoque["Carne"] += 2; 
                    ConsoleLock($"[Chef {id}] Carne produzida!", ConsoleColor.Green);
                    break;

                case 6: 
                    ConsoleLock($"[Chef {id}] Produzindo macarrão...", ConsoleColor.Green);
                    Thread.Sleep(2000); 
                    Estoque["Macarrao"] += 4;
                    ConsoleLock($"[Chef {id}] Macarrão produzido!", ConsoleColor.Green);
                    break;

                case 7: // Produzir molho
                    ConsoleLock($"[Chef {id}] Produzindo molho...", ConsoleColor.Green);
                    Thread.Sleep(2000); 
                    Estoque["Molho"] += 2;
                    ConsoleLock($"[Chef {id}] Molho produzido!", ConsoleColor.Green);
                    break;
            }
        }

        // Consome os ingredientes necessários
        lock (Estoque)
        {
            switch (prato)
            {
                case 1:
                    Estoque["Arroz"]--;
                    Estoque["Carne"]--;
                    Thread.Sleep(2000);
                    break;

                case 2:
                    Estoque["Macarrao"]--;
                    Estoque["Molho"]--;
                    Thread.Sleep(2000);
                    break;

                case 3:
                    Estoque["Arroz"]--;
                    Estoque["Carne"]--;
                    Estoque["Molho"]--;
                    Thread.Sleep(3000);
                    break;
            }
        }

        ConsoleLock($"[Chef {id}] Finalizado o pedido {pedido} do prato {prato}!", ConsoleColor.Red);

            ConsoleLock("[Estoque] " +
                $"Arroz: {Estoque["Arroz"]} | " +
                $"Carne: {Estoque["Carne"]} | " +
                $"Macarrao: {Estoque["Macarrao"]} | " +
                $"Molho: {Estoque["Molho"]}", ConsoleColor.Yellow);
    }
}

var g = Enumerable .Range(1, 6).Select(i => Task.Run(() => Garcom())).ToList();
var c = Enumerable.Range(1, 2).Select(i => Task.Run(() => Chef())).ToList();
Task.WaitAll(c.ToArray());
Task.WaitAll(g.ToArray());