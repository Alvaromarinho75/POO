using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

// 1) Crie uma aplicação de console em C# que solicite ao usuário a digitação de uma senha.
// O programa deve verificar se a senha inserida é considerada "forte" de acordo com as seguintes regras:
// A senha deve ter entre 7 e 16 caracteres.
// Deve conter pelo menos uma letra minúscula (a-z).
// Deve conter pelo menos uma letra maiúscula (A-Z).
// Deve conter pelo menos um dígito (0-9).
// Deve conter pelo menos um caractere especial entre os seguintes:
// ! @ # $ % ^ & * ( ) + = _ - { } [ ] : ; " ' ? < > , .
// Se a senha for forte, o programa exibe uma mensagem de sucesso e termina.
// Caso contrário, deve informar ao usuário que a senha não é forte e pedir para digitar novamente,
// repetindo esse processo até que uma senha forte seja fornecida.

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Escolha a tarefa a ser executada:");
        Console.WriteLine("1: Validador de Senha");
        Console.WriteLine("2: Analisador de Prêmios Nobel");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ValidatePassword();
                break;
            case "2":
                ParseNobelPrizes();
                break;
            default:
                Console.WriteLine("Escolha inválida. Saindo.");
                break;
        }
    }

    public static void ValidatePassword()
    {
        while (true)
        {
            Console.WriteLine("\n--- Validador de Senha ---");
            Console.WriteLine("Uma senha forte deve atender aos seguintes critérios:");
            Console.WriteLine("- Ter entre 7 e 16 caracteres.");
            Console.WriteLine("- Conter pelo menos uma letra minúscula (a-z).");
            Console.WriteLine("- Conter pelo menos uma letra maiúscula (A-Z).");
            Console.WriteLine("- Conter pelo menos um dígito (0-9).");
            Console.WriteLine("- Conter pelo menos um caractere especial (!@#$%^&*()+=_-{}[]:;\"'?<>,.)");

            Console.WriteLine("\nPor favor, digite uma senha:");
            string? password = Console.ReadLine();

            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("A senha não pode ser vazia. Tente novamente.");
                continue;
            }
            
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()+=_\-{}\[\]:;""'?<>,.]).{7,16}$");

            if (regex.IsMatch(password))
            {
                Console.WriteLine("Sucesso! A senha é forte.");
                break;
            }
            else
            {
                Console.WriteLine("A senha não é forte. Por favor, certifique-se de que sua senha atenda a todos os critérios e tente novamente.");
            }
        }
    }

    public static void ParseNobelPrizes()
    {
        Console.WriteLine("\n--- Analisador de Prêmios Nobel ---");
        string fileName = "prize.json";
        try
        {
            string jsonString = File.ReadAllText(fileName);
            var root = JsonSerializer.Deserialize<RootObject>(jsonString);

            if (root?.Prizes == null)
            {
                Console.WriteLine("Não foi possível encontrar a lista de prêmios no arquivo JSON.");
                return;
            }

            var economicsLaureates = root.Prizes
                .Where(p => p.Category == "economics" && p.Laureates != null)
                .SelectMany(p => p.Laureates);
            
            Console.WriteLine("\nNomes dos ganhadores do prêmio de economia:");
            foreach (var laureate in economicsLaureates)
            {
                if (!string.IsNullOrEmpty(laureate.Firstname))
                {
                    Console.WriteLine(laureate.Firstname);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Erro: O arquivo '{fileName}' não foi encontrado.");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao analisar o arquivo JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
        }
    }
}

// Classes para deserialização do JSON
public class RootObject
{
    [JsonPropertyName("prizes")]
    public List<Prize>? Prizes { get; set; }
}

public class Prize
{
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("laureates")]
    public List<Laureate>? Laureates { get; set; }
}

public class Laureate
{
    [JsonPropertyName("firstname")]
    public string? Firstname { get; set; }
}
