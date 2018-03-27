using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InrappAdmin.DomainModel;

namespace InrappAdmin.Web.Models.ViewModels
{
    public class RegisterViewModels
    {
        [Display(Name = "Registers kortnamn")]
        public string RegisterShortName { get; set; }
        public IEnumerable<AdmRegister> Registers { get; set; }

        public IEnumerable<AdmDelregister> DelRegisters { get; set; }
        public string SelectedDirectoryId { get; set; }

        public string SelectedSubDirectoryId { get; set; }
    }

    public class AdmDelregisterViewModel
    {
        public int Id { get; set; }
        public int RegisterId { get; set; }
        public string Delregisternamn { get; set; }
        public string Kortnamn { get; set; }
        public string Beskrivning { get; set; }
        public string Slussmapp { get; set; }
        public bool Inrapporteringsportal { get; set; }

    }
}