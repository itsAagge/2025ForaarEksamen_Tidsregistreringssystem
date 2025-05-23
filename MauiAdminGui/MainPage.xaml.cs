using System.Threading.Tasks;

namespace MauiAdminGui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Afdelinger_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AfdelingerPage());
        }

        private async void Medarbejdere_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MedarbejderePage());
        }

        private async void Sager_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SagerPage());
        }
    }
}
