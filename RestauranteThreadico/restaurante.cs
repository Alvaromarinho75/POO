using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text;
using System.Collections.Generic;

BlockingCollection<(int pedido, int prato)> pedidos = new BlockingCollection<(int pedido, int prato)>();
object lockConsole = new();

Dictionary<string, int> Estoque = new Dictionary<string, int>
{
    { "Arroz", 0 },
    { "Carne", 0 },
    { "Macarrao", 0 },
    { "Molho", 0 }
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

        ConsoleLock($"[Garcom {id}] Enviei pedido {p} do prato {prato}!", 
            ConsoleColor.Blue);
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

        ConsoleLock($"[Chef {id}] Iniciando o pedido {pedido} do prato {prato}!", 
            ConsoleColor.Red);
        ConsoleLock("[Estoque] " + 
            $"Arroz: {Estoque["Arroz"]} | " +
            $"Carne: {Estoque["Carne"]} | " +
            $"Macarrao: {Estoque["Macarrao"]} | " +
            $"Molho: {Estoque["Molho"]}", ConsoleColor.Yellow);

        bool processando = true;
        while (processando) {
            lock (Estoque) {
                switch (prato) {
                    case 1:
                        if (Estoque["Arroz"] == 0) {
                            ConsoleLock($"[Chef {id}] Estoque arroz insuficiente para o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
                            prato = 4; 
                            continue; 
                        }
                        if (Estoque["Carne"] == 0) {
                            ConsoleLock($"[Chef {id}] Estoque carne insuficiente para o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
                            prato = 5; 
                            continue;
                        }
                        Estoque["Arroz"]--;
                        Estoque["Carne"]--;
                        Thread.Sleep(2000);
                        processando = false;
                        break;

                    case 2:
                        if (Estoque["Macarrao"] == 0) {
                            ConsoleLock($"[Chef {id}] Estoque macarrão insuficiente para o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
                            prato = 6; 
                            continue; 
                        }
                        if (Estoque["Molho"] == 0) {
                            ConsoleLock($"[Chef {id}] Estoque molho insuficiente para o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
                            prato = 7; 
                            continue; 
                        }
                        Estoque["Macarrao"]--;
                        Estoque["Molho"]--;
                        Thread.Sleep(2000);
                        processando = false; // Sai do loop após processar o pedido
                        break;

                    case 3:
                        if (Estoque["Arroz"] == 0) {
                            ConsoleLock($"[Chef {id}] Estoque arroz insuficiente para o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
                            prato = 4; 
                            continue; 
                        }
                        if (Estoque["Carne"] == 0) {
                            ConsoleLock($"[Chef {id}] Estoque carne insuficiente para o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
                            prato = 5; 
                            continue; 
                        }
                        if (Estoque["Molho"] == 0) {
                            ConsoleLock($"[Chef {id}] Estoque molho insuficiente para o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
                            prato = 7; 
                            continue; 
                        }
                        Estoque["Arroz"]--;
                        Estoque["Carne"]--;
                        Estoque["Molho"]--;
                        Thread.Sleep(3000);
                        processando = false; 
                        break;

                    case 4:
                        ConsoleLock($"[Chef {id}] Reabastecendo Arroz!", ConsoleColor.Green);
                        Thread.Sleep(2000);
                        Estoque["Arroz"] += 3;
                        ConsoleLock($"[Chef {id}] Arroz Reabastecido!", ConsoleColor.Green);
                        processando = false; 
                        break;

                    case 5:
                        ConsoleLock($"[Chef {id}] Reabastecendo Carne!", ConsoleColor.Green);
                        Thread.Sleep(2000);
                        Estoque["Carne"] += 2;
                        ConsoleLock($"[Chef {id}] Carne Reabastecida!", ConsoleColor.Green);
                        processando = false; 
                        break;

                    case 6:
                        ConsoleLock($"[Chef {id}] Reabastecendo Macarrão!", ConsoleColor.Green);
                        Thread.Sleep(2000);
                        Estoque["Macarrao"] += 4;
                        ConsoleLock($"[Chef {id}] Macarrão Reabastecido!", ConsoleColor.Green);
                        processando = false; 
                        break;

                    case 7:
                        ConsoleLock($"[Chef {id}] Reabastecendo Molho!", ConsoleColor.Green);
                        Thread.Sleep(2000);
                        Estoque["Molho"] += 2;
                        ConsoleLock($"[Chef {id}] Molho Reabastecido!", ConsoleColor.Green);
                        processando = false;
                        break;
                }
            }
        }

        ConsoleLock($"[Chef {id}] Finalizado o pedido {pedido} do prato {prato}!", 
            ConsoleColor.Red);
        ConsoleLock("[Estoque] " + 
            $"Arroz: {Estoque["Arroz"]} | " +
            $"Carne: {Estoque["Carne"]} | " +
            $"Macarrao: {Estoque["Macarrao"]} | " +
            $"Molho: {Estoque["Molho"]}", ConsoleColor.Yellow);
    }
} 


var g = Enumerable .Range(1, 3).Select(i => Task.Run(() => Garcom())).ToList();
var c = Enumerable.Range(1, 2).Select(i => Task.Run(() => Chef())).ToList();
Task.WaitAll(c.ToArray());
Task.WaitAll(g.ToArray());