using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// Classe para desserializar a resposta JSON
public class TemperaturaReading
{
    public string Unidade { get; set; }
    public double Valor { get; set; }
}

public class Program
{
    private static double? ultimaTemperatura = null;
    private static readonly HttpClient client = new HttpClient();
    private static CancellationTokenSource cts = new CancellationTokenSource();

    public static async Task Main(string[] args)
    {
        Console.CancelKeyPress += (sender, e) => {
            e.Cancel = true; // Impede que o processo termine imediatamente
            cts.Cancel();
            Console.WriteLine("\nEncerrando monitoramento de temperatura.");
        };

        // 1. Obter input do usuário
        string unidade = ObterUnidade();
        int intervalo = ObterIntervalo();
        string simboloUnidade = ObterSimboloUnidade(unidade);

        Console.WriteLine($"Iniciando monitoramento a cada {intervalo} segundos. Pressione Ctrl+C para sair.");

        // 2. Loop de leitura periódica
        while (!cts.Token.IsCancellationRequested)
        {
            try
            {
                string url = $"http://localhost:5000/temperatura/{unidade}";
                HttpResponseMessage response = await client.GetAsync(url, cts.Token);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var reading = JsonSerializer.Deserialize<TemperaturaReading>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (reading != null)
                    {
                        ProcessarLeitura(reading, simboloUnidade);
                    }
                    else
                    {
                        Console.WriteLine("Resposta inválida do servidor.");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Erro ao obter leitura: {response.StatusCode}");
                    Console.ResetColor();
                }
            }
            catch (HttpRequestException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Erro ao obter leitura. O servidor pode estar offline.");
                Console.ResetColor();
            }
            catch (JsonException)
            {
                 Console.WriteLine("Resposta inválida (não é um JSON válido).");
            }
            catch (TaskCanceledException)
            {
                // Ignora, pois isso acontece quando o usuário pressiona Ctrl+C
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
                Console.ResetColor();
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(intervalo), cts.Token);
            }
            catch (TaskCanceledException)
            {
                // O loop vai terminar
            }
        }
        
        Console.WriteLine("Aplicação encerrada.");
    }

    private static void ProcessarLeitura(TemperaturaReading leitura, string simboloUnidade)
    {
        string horario = DateTime.Now.ToString("HH:mm:ss");
        double temperaturaAtual = leitura.Valor;
        
        Console.Write($"[{horario}] Temperatura: {temperaturaAtual:F2} {simboloUnidade} → ");

        if (ultimaTemperatura.HasValue)
        {
            if (temperaturaAtual > ultimaTemperatura.Value)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SUBIU");
            }
            else if (temperaturaAtual < ultimaTemperatura.Value)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("DESCEU");
            }
            else
            {
                Console.WriteLine("SEM ALTERAÇÃO");
            }
        }
        else
        {
            Console.WriteLine("PRIMEIRA LEITURA");
        }
        
        Console.ResetColor();
        ultimaTemperatura = temperaturaAtual;
    }

    private static string ObterUnidade()
    {
        while (true)
        {
            Console.Write("Digite a unidade de temperatura (celsius, kelvin, fahrenheit): ");
            string? input = Console.ReadLine()?.ToLower().Trim();
            if (input == "celsius" || input == "kelvin" || input == "fahrenheit")
            {
                return input;
            }
            Console.WriteLine("Unidade inválida. Tente novamente.");
        }
    }

    private static int ObterIntervalo()
    {
        while (true)
        {
            Console.Write("Digite o intervalo em segundos para cada leitura: ");
            if (int.TryParse(Console.ReadLine(), out int intervalo) && intervalo > 0)
            {
                return intervalo;
            }
            Console.WriteLine("Intervalo inválido. Deve ser um número inteiro positivo.");
        }
    }
     private static string ObterSimboloUnidade(string unidade)
    {
        return unidade.ToLower() switch
        {
            "celsius" => "°C",
            "fahrenheit" => "°F",
            "kelvin" => "K",
            _ => ""
        };
    }
}
