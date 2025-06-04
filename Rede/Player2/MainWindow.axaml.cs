using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace Player2;

public partial class MainWindow : Window
{
    private const int Tamanho = 10;
    private int barcosRestantes = 10;
    private Button[,] botoes = new Button[Tamanho, Tamanho];
    private bool[,] barcos = new bool[Tamanho, Tamanho];
    private TcpConnection? tcpConnection;

    public MainWindow()
    {
        InitializeComponent();        
        SetMensagem("Conectando ao servidor...");
        
            tcpConnection = new TcpConnection();
            try
            {
                tcpConnection.ConnectToServer("127.0.0.1", 12345); // IP e porta do servidor
                SetMensagem("Conectado ao servidor! Você pode começar a atacar.");
                InicializarTabuleiro(); // Inicializa o grid para ataques
            }
            catch (Exception)
            {
                SetMensagem($"Erro ao conectar ao servidor, incie o Player 1 primeiro!");
            }
        


    }

    private void InicializarTabuleiro()
    {
        var grid = this.FindControl<Grid>("TabuleiroGrid")!;
        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        // Configura as linhas e colunas do Grid
        for (int i = 0; i < Tamanho; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        // Adiciona os botões ao Grid
        for (int i = 0; i < Tamanho; i++)
        {
            for (int j = 0; j < Tamanho; j++)
            {
                var btn = new Button
                {
                    Tag = (i, j),
                    Background = Brushes.LightGray,
                    Width = 35,
                    Height = 35
                };
                btn.Click += BtnAtaque_Click; // Vincula o evento de clique ao método de ataque
                botoes[i, j] = btn;

                Grid.SetRow(btn, i);
                Grid.SetColumn(btn, j);
                grid.Children.Add(btn);
            }
        }
    }

    private void BtnCelula_Click(object? sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        if (btn == null || barcosRestantes == 0) return;

        var (i, j) = ((int, int))btn.Tag!;
        if (!barcos[i, j])
        {
            barcos[i, j] = true;
            btn.Background = Brushes.Gray;
            barcosRestantes--;
            SetMensagem($"Barcos restantes: {barcosRestantes}");
        }
        if (barcosRestantes == 0)
        {
            SetMensagem("Todos os barcos foram posicionados!");
        }
    }

    private void PosicionarBarcosAleatoriamente()
    {
        var rnd = new Random();
        int colocados = 0;
        while (colocados < 10)
        {
            int i = rnd.Next(Tamanho), j = rnd.Next(Tamanho);
            if (!barcos[i, j])
            {
                barcos[i, j] = true;
                colocados++;
            }
        }
        barcosRestantes = 0; // Todos os barcos já foram posicionados
    }

    private void AtualizarTabuleiro()
    {
        for (int i = 0; i < Tamanho; i++)
            for (int j = 0; j < Tamanho; j++)
                botoes[i, j].Background = barcos[i, j] ? Brushes.Gray : Brushes.Blue;
    }

    private void SetMensagem(string msg)
    {
        this.FindControl<TextBlock>("MensagemText")!.Text = msg;
    }

    private void BtnTentarConectar_Click(object? sender, RoutedEventArgs e)
    {
        tcpConnection = new TcpConnection();
        try
        {
            tcpConnection.ConnectToServer("127.0.0.1", 12345); // IP e porta do servidor
            SetMensagem("Conectado ao servidor! Você pode começar a atacar.");
            InicializarTabuleiro(); 
            
            this.FindControl<Button>("BtnTentarConectar")!.IsVisible = false; 
        }
        catch (Exception)
        {
            SetMensagem($"Erro ao conectar ao servidor, incie o Player 1 primeiro!");
        }
    }

    private void BtnAtaque_Click(object? sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        if (btn == null) return;

        var (i, j) = ((int, int))btn.Tag!;
        string coordenada = $"{(char)('A' + i)}{j}"; // Converte a coordenada para o formato "A0", "B7", etc.

        SetMensagem($"Enviando ataque para {coordenada}...");
        try
        {
            tcpConnection?.Send(coordenada); // Envia a coordenada ao Player 1
            string resposta = tcpConnection?.Receive() ?? "Erro ao receber resposta";

            // Atualiza o botão com base na resposta
            if (resposta == "HIT")
            {
                btn.Background = Brushes.Red;
                SetMensagem($"Ataque em {coordenada}: ACERTOU!");
            }
            else if (resposta == "MISS")
            {
                btn.Background = Brushes.Blue;
                SetMensagem($"Ataque em {coordenada}: ERROU!");
            }
            else if (resposta == "WIN")
            {
                btn.Background = Brushes.Red;
                SetMensagem("Você venceu! Todos os navios foram afundados!");
            }
        }
        catch (Exception ex)
        {
            SetMensagem($"Erro ao enviar ataque: {ex.Message}");
        }
    }
}