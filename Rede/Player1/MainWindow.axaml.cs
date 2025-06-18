using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Player1;

public partial class MainWindow : Window
{
    private const int Tamanho = 10;
    private int barcosRestantes = 10;
    private bool jogoAcabou = false;
    private Button[,] botoes = new Button[Tamanho, Tamanho];
    private bool[,] barcos = new bool[Tamanho, Tamanho];
    private bool[,] ataques = new bool[Tamanho, Tamanho];
    private bool[,] acertos = new bool[Tamanho, Tamanho]; // Adicionado para rastrear acertos

    public MainWindow()
    {
        InitializeComponent();

        this.FindControl<Button>("BtnAleatorio")!.Click += BtnAleatorio_Click;
        this.FindControl<Button>("BtnManual")!.Click += BtnManual_Click;

        InicializarTabuleiro();
    }

    private void InicializarTabuleiro()
    {
        var grid = this.FindControl<Grid>("TabuleiroGrid")!;
        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        grid.ColumnSpacing = 1; 

        for (int i = 0; i < Tamanho; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        for (int i = 0; i < Tamanho; i++)
        {
            for (int j = 0; j < Tamanho; j++)
            {
                var btn = new Button
                {
                    Tag = (i, j),
                    Background = Brushes.Blue,
                    Width = 30,
                    Height = 30
                };
                btn.Click += BtnCelula_Click;
                botoes[i, j] = btn;
                barcos[i, j] = false;

                Grid.SetRow(btn, i);
                Grid.SetColumn(btn, j);
                grid.Children.Add(btn);
            }
        }
        barcosRestantes = 10;
    }

    private void BtnAleatorio_Click(object? sender, RoutedEventArgs e)
    {
        SetMensagem("Posicionamento aleatório selecionado!");
        InicializarTabuleiro();
        PosicionarBarcosAleatoriamente();
        AtualizarTabuleiro();

        // Exibe o grid e os botões de ação, oculta os botões de seleção
        this.FindControl<Grid>("TabuleiroGrid")!.IsVisible = true;
        this.FindControl<StackPanel>("AcoesStack")!.IsVisible = true;
        this.FindControl<StackPanel>("SelecaoStack")!.IsVisible = false;
    }

    private void BtnManual_Click(object? sender, RoutedEventArgs e)
    {
        SetMensagem("Clique em 10 células para posicionar os barcos.");
        InicializarTabuleiro();

        // Exibe o grid e os botões de ação, oculta os botões de seleção
        this.FindControl<Grid>("TabuleiroGrid")!.IsVisible = true;
        this.FindControl<StackPanel>("AcoesStack")!.IsVisible = true;
        this.FindControl<StackPanel>("SelecaoStack")!.IsVisible = false;
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
        {
            for (int j = 0; j < Tamanho; j++)
            {
                if (ataques[i, j])
                {
                    if (acertos[i, j])
                        botoes[i, j].Background = Brushes.Green; // Acertou navio
                    else
                        botoes[i, j].Background = Brushes.Red;   // Errou
                }
                else
                {
                    botoes[i, j].Background = barcos[i, j] ? Brushes.Gray : Brushes.Blue;
                }
            }
        }
    }

    private void SetMensagem(string msg)
    {
        this.FindControl<TextBlock>("MensagemText")!.Text = msg;
    }

    private void SetMensagemThreadSafe(string msg)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            this.FindControl<TextBlock>("MensagemText")!.Text = msg;
        });
    }

    private void BtnCancelar_Click(object? sender, RoutedEventArgs e)
    {
        // Oculta o grid, os botões de ação e a mensagem, exibe os botões de seleção
        this.FindControl<TextBlock>("MensagemText")!.Text = string.Empty;
        this.FindControl<Grid>("TabuleiroGrid")!.IsVisible = false;
        this.FindControl<StackPanel>("AcoesStack")!.IsVisible = false;
        this.FindControl<StackPanel>("SelecaoStack")!.IsVisible = true;
    }

    private TcpConnection? tcpConnection;

    private async void BtnConectar_Click(object? sender, RoutedEventArgs e)
    {
        int Porta = 12345; // Porta escolhida
        SetMensagem("Iniciando servidor...");
        if (barcosRestantes > 0)
        {
            SetMensagem("Por favor, posicione todos os barcos antes de iniciar o servidor.");
            return;
        }
        tcpConnection = new TcpConnection();
        try
        {
            tcpConnection.StartServer(Porta); // Porta escolhida
            SetMensagem("Servidor iniciado. Aguardando ataques...");
            this.FindControl<Button>("BtnConectar")!.IsVisible= false;
            this.FindControl<Button>("BtnCancelar")!.IsVisible = false;

            // Executa o processamento de ataques em um thread separado
            await Task.Run(() => ProcessarAtaques());
        }
        catch (Exception ex)
        {
            SetMensagem($"Erro ao iniciar o servidor: {ex.Message}");
        }
    }

    private void ProcessarAtaques()
    {
        while (true)
        {
            if (jogoAcabou) break; // Interrompe o loop se o jogo acabou

            try
            {
                string coordenada = tcpConnection?.Receive() ?? string.Empty;
                if (string.IsNullOrEmpty(coordenada)) continue;

                // Converte a coordenada (ex.: "A5") para índices
                int linha = coordenada[0] - 'A';
                int coluna = int.Parse(coordenada.Substring(1));

                // Verifica se há um navio na posição
                if (barcos[linha, coluna])
                {
                    ataques[linha, coluna] = true; // Marca como atacado
                    acertos[linha, coluna] = true; // Marca como acerto

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        AtualizarTabuleiro();
                    });

                    barcos[linha, coluna] = false; // Marca o navio como afundado

                    if (AreAllShipsSunk())
                    {
                        jogoAcabou = true;
                        tcpConnection?.Send("WIN");
                        SetMensagemThreadSafe("Todos os navios foram afundados! Você perdeu!");
                        break;
                    }
                    else
                    {
                        tcpConnection?.Send("HIT");
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            AtualizarTabuleiro();
                        });
                        SetMensagemThreadSafe($"Ataque em {coordenada}: ACERTOU!");
                    }
                }
                else
                {
                    ataques[linha, coluna] = true;
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        AtualizarTabuleiro();
                    });
                    tcpConnection?.Send("MISS");
                    SetMensagemThreadSafe($"Ataque em {coordenada}: ERROU!");
                }
            }
            catch (Exception ex)
            {
                SetMensagemThreadSafe($"Erro ao processar ataque: {ex.Message}");
                break;
            }
        }
    }

    private bool AreAllShipsSunk()
    {
        for (int i = 0; i < Tamanho; i++)
            for (int j = 0; j < Tamanho; j++)
                if (barcos[i, j]) return false;
        // Não precisa setar mensagem aqui, pois já é feito em ProcessarAtaques
        return true;
    }
}