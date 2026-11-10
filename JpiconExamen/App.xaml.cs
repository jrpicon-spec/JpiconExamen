namespace JpiconExamen
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new Views.Login())
            {
                BarBackgroundColor = Color.FromArgb("#1E3A5F"),
                BarTextColor = Colors.White
            }) { Title = "Instituto Superior Sudamericano" };
        }
    }
}
