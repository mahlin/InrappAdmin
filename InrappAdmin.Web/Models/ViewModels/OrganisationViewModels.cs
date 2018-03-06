using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using InrappAdmin.DomainModel;

namespace InrappAdmin.Web.Models.ViewModels
{
    public class OrganisationViewModels
    {
        public class OrganisationViewModel
        {
            [Display(Name = "Kommunkod")]
            public string Kommunkod { get; set; }
            public Organisation Organisation { get; set; }

            public IEnumerable<ApplicationUser> ContactPersons { get; set; }

            public IEnumerable<Organisationsenhet> OrgUnits { get; set; }

            public  IEnumerable<AdmUppgiftsskyldighet> ReportObligations { get; set; }

        }

        public class OrganisationsenhetViewModel
        {
            public int Organisationsid { get; set; }
            public string Enhetsnamn { get; set; }
            public string Enhetskod { get; set; }
            public DateTime? AktivFrom { get; set; }
            public DateTime? AktivTom { get; set; }

        }
    }
}