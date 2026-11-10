using System.Globalization;
using JpiconExamen.Models;

namespace JpiconExamen.Views;

public partial class Registro : ContentPage
{
    private const decimal CostoCurso = 1500m;
    private readonly string _usuario;
    private decimal? _pagoMensual;

    public Registro(string usuario)
    {
        InitializeComponent();
        _usuario = usuario;
        UsuarioConectadoLabel.Text = $"Usuario conectado: {usuario}";
    }

    // Ajusta solo la distribución visual para escritorio y dispositivos estrechos.
    private void OnPageSizeChanged(object? sender, EventArgs e)
    {
        var escritorio = Width >= 760;
        FormLayout.ColumnDefinitions = escritorio
            ? new ColumnDefinitionCollection
            {
                new() { Width = new GridLength(1.15, GridUnitType.Star) },
                new() { Width = GridLength.Star }
            }
            : new ColumnDefinitionCollection
            {
                new() { Width = GridLength.Star }
            };

        FormLayout.RowDefinitions = escritorio
            ? new RowDefinitionCollection
            {
                new() { Height = GridLength.Auto },
                new() { Height = GridLength.Auto }
            }
            : new RowDefinitionCollection
            {
                new() { Height = GridLength.Auto },
                new() { Height = GridLength.Auto },
                new() { Height = GridLength.Auto }
            };

        Grid.SetRow(StudentCard, escritorio ? 0 : 1);
        Grid.SetColumn(StudentCard, escritorio ? 1 : 0);
        Grid.SetRow(VerResumenButton, escritorio ? 1 : 2);
        Grid.SetColumn(VerResumenButton, 0);
        Grid.SetColumnSpan(VerResumenButton, escritorio ? 2 : 1);
    }

    private void OnPaisChanged(object? sender, EventArgs e)
    {
        var ciudades = PaisPicker.SelectedItem?.ToString() switch
        {
            "Ecuador" => new[] { "Cuenca", "Quito", "Guayaquil", "Loja" },
            "Colombia" => new[] { "Bogotá", "Medellín", "Cali" },
            "Perú" => new[] { "Lima", "Arequipa", "Cusco" },
            _ => Array.Empty<string>()
        };

        CiudadPicker.ItemsSource = ciudades;
        CiudadPicker.SelectedIndex = -1;
    }

    private void OnMontoInicialChanged(object? sender, TextChangedEventArgs e)
    {
        _pagoMensual = null;
        PagoMensualEntry.Text = string.Empty;

        if (!TryParseMonto(e.NewTextValue, out var monto) || monto < 0 || monto > CostoCurso)
            return;

        var saldo = CostoCurso - monto;
        var cuotaBase = saldo / 4m;
        _pagoMensual = decimal.Round(cuotaBase * 1.04m, 2, MidpointRounding.AwayFromZero);
        PagoMensualEntry.Text = _pagoMensual.Value.ToString("C2", CultureInfo.CurrentCulture);
    }

    private static bool TryParseMonto(string? texto, out decimal monto)
    {
        const NumberStyles styles = NumberStyles.Number;
        texto = texto?.Trim();
        if (decimal.TryParse(texto, styles, CultureInfo.CurrentCulture, out monto))
            return true;
        return decimal.TryParse(texto, styles, CultureInfo.InvariantCulture, out monto);
    }

    private async void OnVerResumenClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) || string.IsNullOrWhiteSpace(ApellidoEntry.Text))
        {
            await DisplayAlertAsync("Datos incompletos", "Ingresa el nombre y el apellido del estudiante.", "Aceptar");
            return;
        }

        if (!int.TryParse(EdadEntry.Text, out var edad) || edad <= 0 || edad > 120)
        {
            await DisplayAlertAsync("Edad inválida", "Ingresa una edad numérica entre 1 y 120 años.", "Aceptar");
            return;
        }

        if (PaisPicker.SelectedItem is null || CiudadPicker.SelectedItem is null)
        {
            await DisplayAlertAsync("Ubicación incompleta", "Selecciona el país y la ciudad.", "Aceptar");
            return;
        }

        if (!TryParseMonto(MontoInicialEntry.Text, out var montoInicial) || montoInicial < 0 || montoInicial > CostoCurso)
        {
            await DisplayAlertAsync("Monto inválido", "El monto inicial debe ser numérico y estar entre $0 y $1500.", "Aceptar");
            return;
        }

        if (_pagoMensual is null)
        {
            await DisplayAlertAsync("Monto inválido", "Ingresa un monto inicial válido para calcular las cuotas.", "Aceptar");
            return;
        }

        var estudiante = new Estudiante
        {
            UsuarioConectado = _usuario,
            Nombre = NombreEntry.Text.Trim(),
            Apellido = ApellidoEntry.Text.Trim(),
            Edad = edad,
            Fecha = FechaPicker.Date ?? DateTime.Today,
            Pais = PaisPicker.SelectedItem.ToString()!,
            Ciudad = CiudadPicker.SelectedItem.ToString()!,
            MontoInicial = montoInicial,
            PagoMensual = _pagoMensual.Value
        };

        await Navigation.PushAsync(new Resumen(estudiante));
    }
}
