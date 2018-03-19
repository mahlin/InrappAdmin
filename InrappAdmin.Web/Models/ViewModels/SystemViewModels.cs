using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;
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

    }
}