using System.Diagnostics;
using System.Threading.Tasks;
using BusinessLogic.Controllers;
using DTO.Model;

namespace MauiAdminGui;

public partial class SagerPage : ContentPage
{
    Sag ValgtSag = null;
    readonly List<Afdeling> AlleAfdelinger = CRUDController.HentAlleAfdelinger();

    public SagerPage()
	{
		InitializeComponent();
        ResetAll();
        pickerAfdelinger.ItemsSource = AlleAfdelinger;
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

    private async void VisPopUpAdvarsel(string besked)
    {
        await Application.Current.MainPage.DisplayAlert("Fejl", besked, "Okay");
    }

    private void ResetAll()
    {
        SagerView.ItemsSource = CRUDController.HentAlleSager();

        entrySagOverskrift.Text = "";
        entrySagBeskrivelse.Text = "";
        pickerAfdelinger.SelectedIndex = -1;
        tilføjSagKnap.IsEnabled = true;
        redigerSagKnap.IsEnabled = false;
        sletSagKnap.IsEnabled = false;
    }

    private void SagerView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ValgtSag = SagerView.SelectedItem as Sag;
        Afdeling afdeling = AlleAfdelinger.Where(a => a.Nr == ValgtSag.AfdelingNr).First();
        sagAfdeling.Text = afdeling.ToString();

        entrySagOverskrift.Text = ValgtSag.Overskrift;
        entrySagBeskrivelse.Text = ValgtSag.Beskrivelse;
        pickerAfdelinger.SelectedItem = afdeling;

        tilføjSagKnap.IsEnabled = false;
        redigerSagKnap.IsEnabled = true;
        sletSagKnap.IsEnabled = true;
    }

    private void redigerSagKnap_Clicked(object sender, EventArgs e)
    {
        string nyOverskrift = entrySagOverskrift.Text;
        string nyBeskrivelse = entrySagBeskrivelse.Text;
        Afdeling nyAfdeling = pickerAfdelinger.SelectedItem as Afdeling;

        try
        {
            CRUDController.OpdaterSag(ValgtSag, nyOverskrift, nyBeskrivelse, nyAfdeling);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void sletSagKnap_Clicked(object sender, EventArgs e)
    {
        try
        {
            CRUDController.SletSag(ValgtSag);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void tilføjSagKnap_Clicked(object sender, EventArgs e)
    {
        string overskrift = entrySagOverskrift.Text;
        string beskrivelse = entrySagBeskrivelse.Text;
        Afdeling afdeling = pickerAfdelinger.SelectedItem as Afdeling;

        try
        {
            CRUDController.OpretSag(overskrift, beskrivelse, afdeling);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }
}