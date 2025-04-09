using System;
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

int podePreparar(int prato){
    int p = Interlocked.Increment(ref pedido);

    switch(prato)
    {
        case 1:
            if(Estoque["Arroz"] <= 0){
                pedidos.Add((p, 4));
                return 4;

            } if(Estoque["Carne"] <= 0) {
                pedidos.Add((p, 5));
                return 5;

            } else {
                return 0;

            }
        case 2:
            if(Estoque["Macarrao"] <= 0){
                pedidos.Add((p, 6));
                return 6;

            } if(Estoque["Molho"] <= 0) {
                pedidos.Add((p, 7));
                return 7;

            } else {
                return 0;

            }
        case 3:
            if(Estoque["Arroz"] <= 0){
                pedidos.Add((p, 4));
                return 4;

            } if(Estoque["Carne"] <= 0) {
                pedidos.Add((p, 5));
                return 5;

            } if(Estoque["Molho"] <= 0) {
                pedidos.Add((p, 7));
                return 7;

            } else {
                return 0;

            }
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
    foreach(var item in pedidos.GetConsumingEnumerable())
    {
        var (pedido, prato) = item;

        if(podePreparar(prato) != 0)
        {
            pedido = podePreparar(prato);
        }
        ConsoleLock($"[Chef {id}] Iniciando o pedido {pedido} do prato {prato}!", 
            ConsoleColor.Red);
        ConsoleLock("[Estoque] " + 
            $"Arroz: {Estoque["Arroz"]} | " +
            $"Carne: {Estoque["Carne"]} | " +
            $"Macarrao: {Estoque["Macarrao"]} | " +
            $"Molho: {Estoque["Molho"]}", ConsoleColor.Yellow);


        switch(prato)
        {
            case 1:
                Thread.Sleep(2000);
                Estoque["Arroz"]--;
                Estoque["Carne"]--;
                break;
            case 2:
                Thread.Sleep(2000);
                Estoque["Macarrao"]--;
                Estoque["Molho"]--;
                break;
            case 3:
                Thread.Sleep(3000);
                Estoque["Arroz"]--;
                Estoque["Carne"]--;
                Estoque["Molho"]--;
                break;
            case 4:
                ConsoleLock($"[Chef {id}] produzindo arroz", ConsoleColor.Green);
                Thread.Sleep(2000);
                Estoque["Arroz"] += 3;
                ConsoleLock($"[Chef {id}] arroz produzido", ConsoleColor.Green);
                break;
            
            case 5:
                ConsoleLock($"[Chef {id}] produzindo carne", ConsoleColor.Green);
                Thread.Sleep(2000);
                Estoque["Carne"] += 2;
                ConsoleLock($"[Chef {id}] carne produzida", ConsoleColor.Green);
                break;

            case 6:
                ConsoleLock($"[Chef {id}] produzindo macarrao", ConsoleColor.Green);
                Thread.Sleep(2000);
                Estoque["Macarrao"] += 4;
                ConsoleLock($"[Chef {id}] macarrao produzido", ConsoleColor.Green);
                break;

            case 7:
                ConsoleLock($"[Chef {id}] produzindo molho", ConsoleColor.Green);
                Thread.Sleep(2000);
                Estoque["Molho"] += 2;
                ConsoleLock($"[Chef {id}] molho produzido", ConsoleColor.Green);
                break;
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