using System.Diagnostics;
using BusinessLogic.Controllers;
using DTO.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MVCEmployeeGui.Models;

namespace MVCEmployeeGui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            LoginModel loginModel = new LoginModel();

            string? loggedInNavn = HttpContext.Session.GetString("loggedInName");
            if (loggedInNavn != null) loginModel.LoggedInName = loggedInNavn;

            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection formCollection)
        {
            LoginModel loginModel = new LoginModel();

            try
            {
                Medarbejder medarbejder = CRUDController.HentMedarbejderMedAfdelinger(formCollection["cpr"]);
                HttpContext.Session.SetString("loggedInCpr", medarbejder.CPR);
                HttpContext.Session.SetString("loggedInName", medarbejder.Navn);

                loginModel.LoggedInName = HttpContext.Session.GetString("loggedInName");
            }
            catch (Exception ex)
            {
                loginModel.ErrorMessage = ex.Message;
            }

            return View(loginModel);
        }

        public IActionResult Tidsregistreringer()
        {
            string? loggedInNavn = HttpContext.Session.GetString("loggedInName");
            string? loggedInCpr = HttpContext.Session.GetString("loggedInCpr");
            if (loggedInNavn == null)
            {
                LoginModel loginModel = new LoginModel() { ErrorMessage = "Du skal være logget ind for at kunne tilgå dine tidsregistreringer " };
                return View("Index", loginModel);
            }

            List<Afdeling> afdelinger = CRUDController.HentMedarbejderMedAfdelinger(loggedInCpr).Afdelinger;
            List<SelectListItem> afdelingerSelect = new List<SelectListItem>();
            foreach (Afdeling afdeling in afdelinger)
            {
                afdelingerSelect.Add(new SelectListItem { Text = afdeling.Navn, Value = afdeling.Nr.ToString() });
            }

            TidsregModel tidsregModel = new TidsregModel() { LoggedInNavn = loggedInNavn,
                                                             Afdelinger = afdelingerSelect,
                                                             Sager = null };
            return View(tidsregModel);
        }

        [HttpPost]
        public IActionResult HentSager(TidsregModel model)
        {
            string? loggedInNavn = HttpContext.Session.GetString("loggedInName");
            string? loggedInCpr = HttpContext.Session.GetString("loggedInCpr");
            if (loggedInNavn == null)
            {
                LoginModel loginModel = new LoginModel() { ErrorMessage = "Du skal være logget ind for at kunne tilgå dine tidsregistreringer " };
                return View("Index", loginModel);
            }

            List<Afdeling> afdelinger = CRUDController.HentMedarbejderMedAfdelinger(loggedInCpr).Afdelinger;
            List<SelectListItem> afdelingerSelect = new List<SelectListItem>();
            foreach (Afdeling afdeling in afdelinger)
            {
                SelectListItem selectListItem = new SelectListItem { Text = afdeling.Navn, Value = afdeling.Nr.ToString() };
                if (selectListItem.Value == model.ValgtAfdelingNr) selectListItem.Selected = true;
                afdelingerSelect.Add(selectListItem);
            }

            List<SelectListItem> sagerSelect = new List<SelectListItem>();
            if (model.ValgtAfdelingNr != null)
            {
                List<Sag> sager = CRUDController.HentAlleSagerFraAfdelingNr(Int32.Parse(model.ValgtAfdelingNr));
                foreach (Sag sag in sager)
                {
                    sagerSelect.Add(new SelectListItem { Text = sag.Overskrift, Value = sag.Nr.ToString() });
                }
            }

            TidsregModel tidsregModel = new TidsregModel() { LoggedInNavn = loggedInNavn,
                                                             Afdelinger = afdelingerSelect,
                                                             Sager = sagerSelect,
                                                             StartTid = DateTime.Now.ToString("yyyy-MM-ddTHH:mm"),
                                                             SlutTid = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm")};
            return View("Tidsregistreringer", tidsregModel);
        }

        [HttpPost]
        public IActionResult OpretTidsregistrering(TidsregModel model)
        {
            string? loggedInCpr = HttpContext.Session.GetString("loggedInCpr");

            int? valgtSagNr = null;
            if(model.ValgtSagNr != null) valgtSagNr = Int32.Parse(model.ValgtSagNr);

            DateTime startTid = DateTime.Parse(model.StartTid);
            DateTime slutTid = DateTime.Parse(model.SlutTid);

            TidsregEnd tidsregEnd = new TidsregEnd();

            try
            {
                CRUDController.OpretTidsregistrering(startTid, slutTid, loggedInCpr, valgtSagNr);
                string s = "Din tidsregistrering fra " + startTid + " til " + slutTid + " ";
                if (valgtSagNr != null) s += "på sag: {" + CRUDController.HentSag((int) valgtSagNr).Overskrift + "} ";
                s += "blev succesfuldt oprettet";

                tidsregEnd.SuccessMessage = s;
            }
            catch (Exception ex)
            {
                tidsregEnd.ErrorMessage = ex.Message;
            }

            return View("TidsregEnd", tidsregEnd);
        }
    }
}
