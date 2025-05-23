using System.Runtime.ConstrainedExecution;
using BusinessLogic.Controllers;
using DTO.Model;

namespace MauiAdminGui;

public partial class MedarbejderePage : ContentPage
{
    Medarbejder ValgtMedarbejder = null;
    List<Tidsregistrering> Tidsregistreringer = null;

	public MedarbejderePage()
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

    private async void VisPopUpAdvarsel(string besked)
    {
        await Application.Current.MainPage.DisplayAlert("Fejl", besked, "Okay");
    }

    private void ResetAll()
    {
        AfdelingView.ItemsSource = null;
        TidsregistreringView.ItemsSource = null;
        MedarbejderView.ItemsSource = CRUDController.HentAlleMedarbejdereMedAfdelinger();


        pickerAfdelingerMangler.IsEnabled = false;
        tilføjAfdelingKnap.IsEnabled = false;
        fjernAfdelingKnap.IsEnabled = false;

        entryCPR.IsEnabled = true;
        entryCPR.Text = "";
        entryInitialer.Text = "";
        entryNavn.Text = "";
        pickerAfdelinger.ItemsSource = CRUDController.HentAlleAfdelinger();
        pickerAfdelinger.IsEnabled = true;
        tilføjMedarbejderKnap.IsEnabled = true;
        redigerMedarbejderKnap.IsEnabled = false;
        sletMedarbjederKnap.IsEnabled = false;

        filtrerTidsregistreringerKnap.IsEnabled = false;
        rydFiltrerTidsregistreringerKnap.IsEnabled = false;

        ValgtMedarbejder = null;
        Tidsregistreringer = null;
    }

    private void MedarbejderView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ValgtMedarbejder = MedarbejderView.SelectedItem as Medarbejder;
        AfdelingView.ItemsSource = ValgtMedarbejder.Afdelinger;
        if (AfdelingView.SelectedItem != null) AfdelingView.SelectedItem = null;
        Tidsregistreringer = CRUDController.HentAlleTidsregistreringerFraMedarbejderCPR(ValgtMedarbejder.CPR);
        TidsregistreringView.ItemsSource = Tidsregistreringer;
        if (TidsregistreringView.SelectedItem != null) TidsregistreringView.SelectedItem = null;

        Dispatcher.Dispatch(() =>
        {
            pickerAfdelingerMangler.SelectedIndex = -1;
            pickerAfdelingerMangler.SelectedItem = null;
            pickerAfdelingerMangler.IsEnabled = true;
            List<Afdeling> manglendeAfdelinger = CRUDController.HentAfdelingerSomMedarbejderIkkeErI(ValgtMedarbejder.CPR);
            pickerAfdelingerMangler.ItemsSource = manglendeAfdelinger;
        });
        tilføjAfdelingKnap.IsEnabled = true;
        fjernAfdelingKnap.IsEnabled = false;

        tilføjMedarbejderKnap.IsEnabled = false;
        redigerMedarbejderKnap.IsEnabled = true;
        sletMedarbjederKnap.IsEnabled = true;
        entryCPR.Text = "";
        entryCPR.IsEnabled = false;
        entryInitialer.Text = ValgtMedarbejder.Initialer;
        entryNavn.Text = ValgtMedarbejder.Navn;
        pickerAfdelinger.IsEnabled = false;

        if (Tidsregistreringer.Count > 0) filtrerTidsregistreringerKnap.IsEnabled = true;
    }

    private void tilføjMedarbejderKnap_Clicked(object sender, EventArgs e)
    {
        string cpr = entryCPR.Text;
        string initialer = entryInitialer.Text;
        string navn = entryNavn.Text;
        Afdeling afdeling = pickerAfdelinger.SelectedItem as Afdeling;

        try
        {
            CRUDController.OpretMedarbejder(afdeling, cpr, initialer, navn);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void redigerMedarbejderKnap_Clicked(object sender, EventArgs e)
    {
        string nyeInitialer = entryInitialer.Text;
        string nytNavn = entryNavn.Text;

        try
        {
            CRUDController.OpdaterMedarbejder(ValgtMedarbejder, nyeInitialer, nytNavn);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void sletMedarbjederKnap_Clicked(object sender, EventArgs e)
    {
        try
        {
            CRUDController.SletMedarbejder(ValgtMedarbejder);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void AfdelingView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        fjernAfdelingKnap.IsEnabled = true;
    }

    private void filtrerTidsregistreringerKnap_Clicked(object sender, EventArgs e)
    {
        DateTime startTid = fraDatoDP.Date;
        DateTime slutTid = tilDatoDP.Date;

        IEnumerable<Tidsregistrering> filtreredeTidsregistreringer = from t in Tidsregistreringer
                                                                     where t.StartTid.Date >= startTid && t.SlutTid.Date <= slutTid
                                                                     select t;

        if (filtreredeTidsregistreringer.ToList().Count <= 0) VisPopUpAdvarsel("Der er ingen tidsregistreringer i den valgte periode. Venligt vælg andre datoer.");
        else
        {
            TidsregistreringView.ItemsSource = filtreredeTidsregistreringer;
            rydFiltrerTidsregistreringerKnap.IsEnabled = true;
        }
    }

    private void tilføjAfdelingKnap_Clicked(object sender, EventArgs e)
    {
        Afdeling valgtAfdeling = pickerAfdelingerMangler.SelectedItem as Afdeling;
        try
        {
            CRUDController.TilføjMedarbejderTilAfdeling(valgtAfdeling, ValgtMedarbejder);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void fjernAfdelingKnap_Clicked(object sender, EventArgs e)
    {
        Afdeling valgtAfdeling = AfdelingView.SelectedItem as Afdeling;
        try
        {
            CRUDController.FjernMedarbejderFraAfdeling(valgtAfdeling, ValgtMedarbejder);
        }
        catch (Exception ex)
        {
            VisPopUpAdvarsel(ex.Message);
        }

        ResetAll();
    }

    private void rydFiltrerTidsregistreringerKnap_Clicked(object sender, EventArgs e)
    {
        TidsregistreringView.ItemsSource = Tidsregistreringer;
        rydFiltrerTidsregistreringerKnap.IsEnabled = false;
    }
}