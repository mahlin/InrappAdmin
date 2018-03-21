using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using InrappAdmin.ApplicationService.DTOModel;
using InrappAdmin.DomainModel;

namespace InrappAdmin.Web.Models.ViewModels
{
    public class SystemViewModels
    {
        public class SystemViewModel
        {
            public IEnumerable<AdmFAQKategori> FAQCategories { get; set; }

            public int SelectedFAQCategory { get; set; }

            public IEnumerable<AdmInformation> InfoPages { get; set; }

            public  AdmKonfiguration AdmConfig { get; set; }

            public OpeningHoursInfoDTO OpeningHours { get; set; }

        }

        public class FAQCategoryViewModel
        {
            public string Kategori{ get; set; }

        }

        public class FAQViewModel
        {
            public int? RegisterId { get; set; }
            public int FAQkategoriId { get; set; }
            public string Fraga { get; set; }
            public string Svar { get; set; }

        }

        public class InforTextViewModel
        {
            public string Informationstyp { get; set; }
            public string Text { get; set; }

        }

        public class OpeningHours
        {
            public int ClosedFromHour { get; set; }

            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            public DateTime OpeningTime { get; set; }

            public string OpeningTimeStr { get; set; }

            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:HH:mm}")]
            public DateTime ClosingTime { get; set; }
            public int ClosedFromMin { get; set; }
            public int ClosedToHour { get; set; }
            public int ClosedToMin { get; set; }
            [DisplayName("Stäng portalen")]
            public bool ClosedAnyway { get; set; }
            public List<OpeningDay> ClosedDaysList { get; set; }
            [AllowHtml]
            public string InfoTextForClosedPage { get; set; }

        }

   
    }
}