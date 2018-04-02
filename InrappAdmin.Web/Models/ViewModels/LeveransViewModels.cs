using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InrappAdmin.DomainModel;

namespace InrappAdmin.Web.Models.ViewModels
{
    public class LeveransViewModels
    {
        public class LeveransViewModel
        {
            public IEnumerable<AdmForvantadleverans> ForvantadeLeveranser { get; set; }
            public IEnumerable<AdmForvantadfil> ForvantadeFiler { get; set; }
            public IEnumerable<AdmFilkrav> Filkrav { get; set; }
        }

        public class AdmForvantadleveransViewModel
        {
            public int Id { get; set; }
            public int FilkravId { get; set; }
            public int DelregisterId { get; set; }
            public string Period { get; set; }
            public DateTime Uppgiftsstart { get; set; }
            public DateTime Uppgiftsslut { get; set; }
            public DateTime Rapporteringsstart { get; set; }
            public DateTime Rapporteringsslut { get; set; }
            public DateTime Rapporteringsenast { get; set; }
        }

        public class AdmForvantadfilViewModel
        {
            public int Id { get; set; }
            public int FilkravId { get; set; }
            public string Filmask { get; set; }
            public string Regexp { get; set; }
            public bool Obligatorisk { get; set; }
            public bool Tom { get; set; }
        }
    }
}