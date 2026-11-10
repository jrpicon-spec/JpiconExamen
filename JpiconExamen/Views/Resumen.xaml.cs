using System.Globalization;
using JpiconExamen.Models;

namespace JpiconExamen.Views;

public partial class Resumen : ContentPage
{
    public Resumen(Estudiante estudiante)
    {
        InitializeComponent();

        var cultura = CultureInfo.CurrentCulture;
        UsuarioConectadoLabel.Text = $"Usuario conectado: {estudiante.UsuarioConectado}";
        NombreLabel.Text = estudiante.Nombre;
        ApellidoLabel.Text = estudiante.Apellido;
        EdadLabel.Text = estudiante.Edad.ToString(cultura);
        FechaLabel.Text = estudiante.Fecha.ToString("dd/MM/yyyy", cultura);
        CiudadLabel.Text = estudiante.Ciudad;
        PaisLabel.Text = estudiante.Pais;
        MontoInicialLabel.Text = estudiante.MontoInicial.ToString("C2", cultura);
        PagoMensualLabel.Text = estudiante.PagoMensual.ToString("C2", cultura);
        PagoTotalLabel.Text = estudiante.PagoTotal.ToString("C2", cultura);
    }
}
