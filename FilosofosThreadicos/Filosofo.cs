using System;
using System.Threading;

public class Filosofo
{
    private readonly int id;
    private readonly SemaphoreSlim garfoEsquerdo;
    private readonly SemaphoreSlim garfoDireito;

    // Array de cores para os garfos
    private static readonly ConsoleColor[] coresGarfo = 
    {
        ConsoleColor.Blue,    // Garfo 0
        ConsoleColor.Green,   // Garfo 1
        ConsoleColor.Yellow,  // Garfo 2
        ConsoleColor.Cyan,    // Garfo 3
        ConsoleColor.Magenta  // Garfo 4
    };

    public Filosofo(int id, SemaphoreSlim garfoEsquerdo, SemaphoreSlim garfoDireito)
    {
        this.id = id;
        this.garfoEsquerdo = garfoEsquerdo;
        this.garfoDireito = garfoDireito;
    }

    public void Viver()
    {
        while (true)
        {
            Pensar();
            Comer();
        }
    }

    private void Pensar()
    {
        Console.WriteLine($"Filósofo {id} está pensando...");
        Thread.Sleep(new Random().Next(1000, 3000));
    }

    private void EscreverMensagem(string mensagem, int garfo)
    {
        lock (Console.Out)
        {
            var corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = coresGarfo[garfo % coresGarfo.Length];
            Console.WriteLine(mensagem);
            Console.ForegroundColor = corOriginal;
        }
    }
    private void EscreverMensagem(string mensagem)
    {
        lock (Console.Out)
        {
            Console.WriteLine(mensagem);
        }
    }

    private void Comer()
    {
        EscreverMensagem($"Filósofo {id} está tentando pegar os garfos...");

        bool pegouGarfoEsquerdo = false;
        bool pegouGarfoDireito = false;

        try
        {
            // Tenta pegar os garfos na ordem definida
            if (id % 2 == 0)
            {
                garfoEsquerdo.Wait();
                pegouGarfoEsquerdo = true;
                EscreverMensagem($"Filósofo {id} pegou o garfo esquerdo.", id);

                if (!garfoDireito.Wait(1000)) 
                {
                    throw new Exception("Não conseguiu pegar o garfo direito.");
                }
                pegouGarfoDireito = true;
                EscreverMensagem($"Filósofo {id} pegou o garfo direito.", (id + 1) % 5);
            }
            else
            {
                garfoDireito.Wait();
                pegouGarfoDireito = true;
                EscreverMensagem($"Filósofo {id} pegou o garfo direito.", (id + 1) % 5);

                if (!garfoEsquerdo.Wait(1000)) 
                {
                    throw new Exception("Não conseguiu pegar o garfo esquerdo.");
                }
                pegouGarfoEsquerdo = true;
                EscreverMensagem($"Filósofo {id} pegou o garfo esquerdo.", id);
            }

            // Se conseguiu pegar ambos os garfos, come
            EscreverMensagem($"Filósofo {id} está comendo...");
            Thread.Sleep(new Random().Next(1000, 2000));
        }
        catch (Exception ex)
        {
            EscreverMensagem($"Filósofo {id} não conseguiu pegar os garfos: {ex.Message}");
        }
        finally
        {
            // Solta os garfos que foram pegos
            if (pegouGarfoEsquerdo)
            {
                garfoEsquerdo.Release();
                EscreverMensagem($"Filósofo {id} soltou o garfo esquerdo.", id);
            }
            if (pegouGarfoDireito)
            {
                garfoDireito.Release();
                EscreverMensagem($"Filósofo {id} soltou o garfo direito.", (id + 1) % 5);
            }
        }
    }
}