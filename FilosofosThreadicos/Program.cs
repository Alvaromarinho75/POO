using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var garfos = Enumerable.Range(0, 5).Select(_ => new SemaphoreSlim(1, 1)).ToArray();

        var filosofos = Enumerable.Range(0, 5)
            .Select(i => new Filosofo(i, garfos[i], garfos[(i + 1) % 5]))
            .ToList();

        var tarefas = filosofos.Select(f => Task.Run(() => f.Viver())).ToArray();

        Task.WaitAll(tarefas);
    }
}