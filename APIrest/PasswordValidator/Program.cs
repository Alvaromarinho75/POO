using System;
using System.Text.RegularExpressions;

public class PasswordValidator
{
    public static void Main(string[] args)
    {
        // Regex para validar a senha
        // Explicação detalhada abaixo
        var strongPasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()+=_\-{}\[\]:;""'?<>,.]).{7,16}$");

        while (true)
        {
            Console.Write("Digite uma senha para validação: ");
            string? password = Console.ReadLine();

            if (password != null && strongPasswordRegex.IsMatch(password))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Senha forte! Atende a todos os critérios.");
                Console.ResetColor();
                break;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Senha fraca. Tente novamente.");
                Console.WriteLine("Lembre-se que a senha deve conter:");
                Console.WriteLine("- Entre 7 e 16 caracteres.");
                Console.WriteLine("- Pelo menos uma letra minúscula (a-z).");
                Console.WriteLine("- Pelo menos uma letra maiúscula (A-Z).");
                Console.WriteLine("- Pelo menos um dígito (0-9).");
                Console.WriteLine("- Pelo menos um caractere especial (!@#$%^&*()+=_-{}[]:;\"'?<>,.).");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
} 