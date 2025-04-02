using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MeuProjetoAvalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClique_Click(object sender, RoutedEventArgs e)
        {
            var valor = this.FindControl<TextBox>("txtInput").Text;

            if (double.TryParse(valor, out double numero))
            {
                string resultado = string.Empty;

                switch (true)
                {
                    case var _ when this.FindControl<RadioButton>("rbCelsiusParaFahrenheit").IsChecked == true:
                        resultado = $"{numero} Celsius = {(numero * 1.8) + 32} Fahrenheit";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbFahrenheitParaCelsius").IsChecked == true:
                        resultado = $"{numero} Fahrenheit = {(numero - 32) / 1.8} Celsius";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbCelsiusParaKelvin").IsChecked == true:
                        resultado = $"{numero} Celsius = {numero + 273.15} Kelvin";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbKelvinParaCelsius").IsChecked == true:
                        resultado = $"{numero} Kelvin = {numero - 273.15} Celsius";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbMetrosParaPes").IsChecked == true:
                        resultado = $"{numero} Metros = {numero * 3.28084} Pés";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbPesParaMetros").IsChecked == true:
                        resultado = $"{numero} Pés = {numero * 0.3048} Metros";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbQuilometrosParaMilhas").IsChecked == true:
                        resultado = $"{numero} Quilômetros = {numero * 0.621371} Milhas";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbMilhasParaQuilometros").IsChecked == true:
                        resultado = $"{numero} Milhas = {numero * 1.60934} Quilômetros";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbQuilogramasParaLibras").IsChecked == true:
                        resultado = $"{numero} Quilogramas = {numero * 2.20462} Libras";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbLibrasParaQuilogramas").IsChecked == true:
                        resultado = $"{numero} Libras = {numero * 0.453592} Quilogramas";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbGramasParaOncas").IsChecked == true:
                        resultado = $"{numero} Gramas = {numero * 0.035274} Onças";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbOncasParaGramas").IsChecked == true:
                        resultado = $"{numero} Onças = {numero * 28.3495} Gramas";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbLitrosParaGaloes").IsChecked == true:
                        resultado = $"{numero} Litros = {numero * 0.264172} Galões";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbGaloesParaLitros").IsChecked == true:
                        resultado = $"{numero} Galões = {numero * 3.78541} Litros";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbMililitrosParaOncasFluidas").IsChecked == true:
                        resultado = $"{numero} Mililitros = {numero * 0.033814} Onças Fluidas";
                        break;

                    case var _ when this.FindControl<RadioButton>("rbOncasFluidasParaMililitros").IsChecked == true:
                        resultado = $"{numero} Onças Fluidas = {numero * 29.5735} Mililitros";
                        break;

                    default:
                        resultado = "Nenhuma opção selecionada!";
                        break;
                }

                this.FindControl<TextBox>("txtResultado").Text = resultado;
            }
            else
            {
                this.FindControl<TextBox>("txtResultado").Text = "Por favor, insira um número válido!";
            }
        }
    }
}