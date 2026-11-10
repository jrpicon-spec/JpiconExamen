namespace JpiconExamen.Views;

public partial class Login : ContentPage
{
    // Matriz de credenciales: columna 0 = usuario, columna 1 = contraseña.
    private readonly string[,] _usuarios =
    {
        { "estudiante", "moviles" },
        { "suda", "2025" }
    };

    public Login()
    {
        InitializeComponent();
    }

    private async void OnIniciarSesionClicked(object? sender, EventArgs e)
    {
        var usuario = UsuarioEntry.Text?.Trim() ?? string.Empty;
        var contrasena = ContrasenaEntry.Text ?? string.Empty;
        var valido = false;

        for (var fila = 0; fila < _usuarios.GetLength(0); fila++)
        {
            if (string.Equals(usuario, _usuarios[fila, 0], StringComparison.OrdinalIgnoreCase)
                && contrasena == _usuarios[fila, 1])
            {
                valido = true;
                break;
            }
        }

        if (!valido)
        {
            await DisplayAlertAsync("Acceso denegado", "El usuario o la contraseña son incorrectos.", "Aceptar");
            return;
        }

        await Navigation.PushAsync(new Registro(usuario));
    }
}
