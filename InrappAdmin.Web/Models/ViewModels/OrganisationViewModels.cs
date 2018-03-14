﻿using System;
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


        public class ApplicationUserViewModel
        {
            public int OrganisationId { get; set; }
            public string Namn { get; set; }
            public DateTime? AktivFrom { get; set; }
            public DateTime? AktivTom { get; set; }
            public int? Status { get; set; }
            public DateTime SkapadDatum { get; set; }
            public string SkapadAv { get; set; }
            public DateTime AndradDatum { get; set; }
            public string AndradAv { get; set; }

        }


    }
}