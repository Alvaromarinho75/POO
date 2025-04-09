using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text;

BlockingCollection<(int pedido, int prato)> pedidos = new BlockingCollection<(int pedido, int prato)>();
object lockConsole = new();

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

        ConsoleLock($"[Garcom {id}] Enviei pedido {p} do prato {prato}!", ConsoleColor.Blue);
        pedidos.Add((p, prato));
        pedido++;
    }
    pedidos.CompleteAdding();
}

void Chef()
{
    Console.WriteLine("[Chef] Estou pronto!!!");
    foreach(var item in pedidos.GetConsumingEnumerable())
    {
        var (pedido, prato) = item;
        ConsoleLock($"[Chef] Iniciando o pedido {pedido} do prato {prato}!", ConsoleColor.Red);

        if(prato is 1 or 2) Thread.Sleep(2000);
        else Thread.Sleep(3000);

        ConsoleLock($"[Chef] Finalizado o pedido {pedido} do prato {prato}!", ConsoleColor.Red);
    }
}


var g = Enumerable .Range(1, 5).Select(i => Task.Run(() => Garcom())).ToList();
var c = new Task(Chef);

c.Start();

c.Wait();
Task.WaitAll(g);