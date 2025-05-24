using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCEmployeeGui.Models
{
    public class TidsregModel
    {
        public string LoggedInNavn { get; set; }

        public List<SelectListItem> Afdelinger { get; set; }

        public string ValgtAfdelingNr { get; set; }

        public List<SelectListItem>? Sager { get; set; }

        public string ValgtSagNr { get; set; }

        public string StartTid { get; set; }

        public string SlutTid { get; set; }
    }
}
