using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

// Classes para desserializar o JSON
public class Laureate
{
    public string? id { get; set; }
    public string? firstname { get; set; }
    public string? surname { get; set; }
}

public class Prize
{
    public string? year { get; set; }
    public string? category { get; set; }
    public List<Laureate>? laureates { get; set; }
}

public class NobelData
{
    public List<Prize>? prizes { get; set; }
}

public class NobelParser
{
    public static void Main(string[] args)
    {
        string fileName = "../prize.json"; // Sobe um nível para encontrar o arquivo na raiz de REGEX

        if (!File.Exists(fileName))
        {
            Console.WriteLine($"Erro: Arquivo '{fileName}' não encontrado.");
            Console.WriteLine("Por favor, certifique-se que o arquivo 'prize.json' existe no diretório 'REGEX'.");
            return;
        }

        try
        {
            string jsonString = File.ReadAllText(fileName);
            var nobelData = JsonSerializer.Deserialize<NobelData>(jsonString);

            if (nobelData?.prizes == null)
            {
                 Console.WriteLine("Formato do JSON inválido ou lista de prêmios vazia.");
                 return;
            }

            var economicsLaureates = nobelData.prizes
                .Where(p => p.category == "economics" && p.laureates != null)
                .SelectMany(p => p.laureates!)
                .Select(l => l.firstname)
                .ToList();

            if (economicsLaureates.Any())
            {
                Console.WriteLine("Ganhadores do Prêmio Nobel de Economia encontrados:");
                foreach (var name in economicsLaureates)
                {
                    if(!string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine($"- {name}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhum ganhador do prêmio de economia encontrado no arquivo.");
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao processar o arquivo JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
        }
    }
} 