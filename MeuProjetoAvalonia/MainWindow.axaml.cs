    using Avalonia.Controls;
    using Avalonia.Interactivity;

    namespace MeuProjetoAvalonia
    {
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                // Inicializa a interface a partir do XAML
                InitializeComponent();
            }

            // Evento chamado quando o botão é clicado
            private void BtnClique_Click(object sender, RoutedEventArgs e)
            {
                var valor = this.FindControl<TextBox>("txtInput").Text;

                // Exibe uma mensagem alterando o texto do próprio TextBox como exemplo
                if (int.TryParse(valor, out int numero))
                {   
                    // Obtém o valor do RadioButton
                    var rad = this.FindControl<RadioButton>("radOpcao1").IsChecked;

                }
                else
                {
                    // Caso o usuário não tenha digitado nada, exibe uma mensagem padrão
                    this.FindControl<TextBox>("txtInput").Text = "Não há valor!";
                }
            }
        }
    }