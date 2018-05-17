using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InrappAdmin.ApplicationService.DTOModel;
using InrappAdmin.DomainModel;

namespace InrappAdmin.Web.Models.ViewModels
{
    public class LeveransViewModels
    {
        public class LeveransViewModel
        {
            public IEnumerable<AdmForvantadleveransViewModel> ForvantadeLeveranser { get; set; }
            public IEnumerable<AdmForvantadfilViewModel> ForvantadeFiler { get; set; }
            public IEnumerable<AdmFilkravViewModel> Filkrav { get; set; }
            public IEnumerable<FilloggDetaljDTO> Leveranser { get; set; }
            public int SelectedRegisterId { get; set; }
            [DisplayName("Visa endast pågående")]
            public bool FilterPagaende{ get; set; }
            public string SelectedCountyCode { get; set; }

            [Display(Name = "Kommunkod")]
            public string Kommunkod { get; set; }
        }

        public class AdmForvantadleveransViewModel
        {
            public int Id { get; set; }
            public int FilkravId { get; set; }
            public string FilkravNamn { get; set; }
            public int DelregisterId { get; set; }
            public int SelectedDelregisterId { get; set; }
            public string DelregisterKortnamn { get; set; }
            public IEnumerable<RegisterInfo> RegisterList { get; set; }
            public IEnumerable<FilloggDetaljDTO> Leveranser { get; set; }
            public string Period { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Uppgiftsstart { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Uppgiftsslut { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Rapporteringsstart { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Rapporteringsslut { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Rapporteringsenast { get; set; }
            public DateTime? Paminnelse1 { get; set; }
            public DateTime? Paminnelse2 { get; set; }
            public DateTime? Paminnelse3 { get; set; }
            public bool Pagaende { get; set; }
            public bool Sen { get; set; }
        }

        public class AdmForvantadfilViewModel
        {
            public int Id { get; set; }
            public int FilkravId { get; set; }
            public string FilkravNamn { get; set; }
            public int? ForeskriftsId { get; set; }
            public string DelregisterKortnamn { get; set; }
            public string Filmask { get; set; }
            public string Regexp { get; set; }
            public bool Obligatorisk { get; set; }
            public bool Tom { get; set; }
        }

        public class AdmFilkravViewModel
        {
            public int Id { get; set; }
            public int DelregisterId { get; set; }
            public int SelectedDelregisterId { get; set; }
            public string DelregisterKortnamn { get; set; }
            public int? ForeskriftsId { get; set; }
            public string Namn { get; set; }

        }
    }
}