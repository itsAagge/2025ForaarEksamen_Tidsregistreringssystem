using System.Threading.Tasks;
using BusinessLogic.Controllers;
using DTO.Model;

namespace MauiAdminGui;

public partial class AfdelingerPage : ContentPage
{
	public AfdelingerPage()
	{
		InitializeComponent();
        ResetAll();
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

    private void ResetAll()
    {
        AfdelingView.ItemsSource = CRUDController.HentAlleAfdelingerMedMedarbejdere();
        entryRedigerAfdeling.IsReadOnly = true;
        entryTilføjAfdeling.Text = "";
    }

    private async void VisPopUpAdvarsel(string besked)
    {
        await Application.Current.MainPage.DisplayAlert("Fejl", besked, "Okay");
    }

    private void AfdelingView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Afdeling valgtAfdeling = AfdelingView.SelectedItem as Afdeling;
        afdelingMedarbejdere.Text = valgtAfdeling.Medarbejdere.Count().ToString();
        afdelingSager.Text = CRUDController.HentAlleSagerFraAfdelingNr(valgtAfdeling.Nr).Count().ToString();
        entryRedigerAfdeling.Text = valgtAfdeling.Navn;
        entryRedigerAfdeling.IsReadOnly = false;
        redigerAfdelingKnap.IsEnabled = true;
        sletAfdelingKnap.IsEnabled = true;
    }

    private void tilføjAfdelingKnap_Clicked(object sender, EventArgs e)
    {
        string navn = entryTilføjAfdeling.Text;

        try
        {
            CRUDController.OpretAfdeling(navn);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void redigerAfdelingKnap_Clicked(object sender, EventArgs e)
    {
        Afdeling valgtAfdeling = AfdelingView.SelectedItem as Afdeling;
        string navn = entryRedigerAfdeling.Text;
        
        try
        {
            CRUDController.OpdaterAfdeling(valgtAfdeling, navn);
        }
        catch(Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        redigerAfdelingKnap.IsEnabled = false;
        sletAfdelingKnap.IsEnabled = false;
        entryRedigerAfdeling.Text = "";
        entryRedigerAfdeling.IsReadOnly = true;

        ResetAll();
    }

    private void sletAfdelingKnap_Clicked(object sender, EventArgs e)
    {
        Afdeling valgtAfdeling = AfdelingView.SelectedItem as Afdeling;

        try
        {
            CRUDController.SletAfdeling(valgtAfdeling);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        redigerAfdelingKnap.IsEnabled = false;
        sletAfdelingKnap.IsEnabled = false;
        entryRedigerAfdeling.Text = "";
        entryRedigerAfdeling.IsReadOnly = true;

        ResetAll();
    }
}