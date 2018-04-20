using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InrappAdmin.ApplicationService;
using InrappAdmin.ApplicationService.DTOModel;
using InrappAdmin.ApplicationService.Interface;
using InrappAdmin.DataAccess;
using InrappAdmin.DomainModel;
using InrappAdmin.Web.Helpers;
using InrappAdmin.Web.Models;
using InrappAdmin.Web.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace InrappAdmin.Web.Controllers
{
    public class SystemController : Controller
    {
        private readonly IPortalAdminService _portalAdminService;

        public SystemController()
        {
            _portalAdminService =
                new PortalAdminService(new PortalAdminRepository(new InrappAdminDbContext(), new InrappAdminIdentityDbContext()));
        }

        // GET: System
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: FAQCategories
        [Authorize]
        public ActionResult GetFAQCategories()
        {
            var model = new SystemViewModels.SystemViewModel();
            try
            {
                model.FAQCategories = _portalAdminService.HamtaFAQkategorier();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "GetFAQCategories", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av FAQ-kategorier",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditFAQCategory", model);
        }


        // GET: FAQs
        public ActionResult GetFAQs(int faqCatId = 0)
        {
            var model = new SystemViewModels.SystemViewModel();
            try
            {
                var faqs = _portalAdminService.HamtaFAQs(faqCatId);
                model.FAQs = ConvertAdmFAQToViewModel(faqs.ToList());
                model.SelectedFAQCategory = faqCatId;
                // Ladda drop down lists. 
                var registerList = _portalAdminService.HamtaAllaRegisterForPortalen();
                this.ViewBag.RegisterList = CreateRegisterDropDownList(registerList);
                //model.SelectedFAQ.SelectedRegisterId = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "GetFAQs", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av FAQs",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditFAQs", model);
        }


        // GET: InformationTexts
        [Authorize]
        [ValidateInput(false)]
        public ActionResult GetInformationTexts(string selectedInfoType = "", string selectedText = "")
        {
            var model = new SystemViewModels.SystemViewModel();
            try
            {
                model.InfoPages = _portalAdminService.HamtaInformationstexter();
                model.SelectedInfo = selectedInfoType;
                model.SelectedInfoText = selectedText;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "GetInformationTexts", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av informationstexter.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditInfoTexts", model);
        }


        ////// GET: InformationText för vald typ
        //public ActionResult GetInfoText(string typ)
        //{
        //    SystemViewModels.SystemViewModel model = new SystemViewModels.SystemViewModel();
        //    try
        //    {
        //        var infoText = _portalAdminService.HamtaInfoText(typ);
        //        AdmInformation admInfo = new AdmInformation
        //        {

        //        };
        //        model.InfoPages.Add(infoText);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        ErrorManager.WriteToErrorLog("SystemController", "GetInformationsText", e.ToString(), e.HResult, "InrappAdmin");
        //        var errorModel = new CustomErrorPageModel
        //        {
        //            Information = "Ett fel inträffade vid hämtning av specifik informationstext.",
        //            ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
        //        };
        //        return View("CustomError", errorModel);

        //    }
        //    return RedirectToAction("GetInformationTexts");
        //}

        // GET: OpeningHours (AdmKonfiguration)
        [Authorize]
        public ActionResult GetOpeningHours()
        {
            var model = new SystemViewModels.OpeningHours();
            try
            {
                var admKonf = _portalAdminService.HamtaOppettider();
                model.ClosedAnyway = admKonf.ClosedAnyway;
                model.ClosedDaysList = _portalAdminService.MarkeraStangdaDagar(admKonf.ClosedDays);
                //model.OpeningTime = SetTime(admKonf.ClosedToHour, admKonf.ClosedToMin);
                //model.ClosingTime = SetTime(admKonf.ClosedFromHour, admKonf.ClosedFromMin);
                DateTime s = DateTime.MinValue;
                TimeSpan ts = new TimeSpan(10, 30, 0);
                model.OpeningTime = s.Date + ts;
                model.OpeningTimeStr = admKonf.ClosedToHour.ToString() + ":" + admKonf.ClosedToMin.ToString();
                //model.ClosingTime = SetTime(admKonf.ClosedFromHour, admKonf.ClosedFromMin);
                model.ClosingTimeStr = admKonf.ClosedFromHour.ToString() + ":" + admKonf.ClosedFromMin.ToString();
                model.InfoTextForClosedPage = _portalAdminService.HamtaInfoText("Stangtsida");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "GetOpeningHours", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av öppettider.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditOpeningHours", model);
        }


        [HttpPost]
        public ActionResult UpdateFAQCategory(AdmFAQKategori faqCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.UppdateraFAQKategori(faqCategory, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "UpdateFAQCategory", e.ToString(), e.HResult,
                        User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid uppdatering av FAQ-kategori.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);

                }
            }
            return RedirectToAction("GetFAQCategories");
        }

        [HttpPost]
        public ActionResult UpdateFAQ(SystemViewModels.SystemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    var faqDb = ConvertViewModelToAdmFAQ(model.SelectedFAQ);
                    _portalAdminService.UppdateraFAQ(faqDb, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "UpdateFAQ", e.ToString(), e.HResult,
                        User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid uppdatering av FAQ.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
            }
            return RedirectToAction("GetFAQs", new { faqCatId = model.SelectedFAQ.FAQkategoriId });
        }

        [HttpPost]
        public ActionResult UpdateInfoText(SystemViewModels.SystemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();

                    AdmInformation info = new AdmInformation
                    {
                        Id = model.SelectedInfoId,
                        Text = model.SelectedInfoText
                    };
                    _portalAdminService.UppdateraInformationstext(info, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "UpdateInfoText", e.ToString(), e.HResult,
                        User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid uppdatering av informationstext.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
            }
            return RedirectToAction("GetInformationTexts");
        }

        // GET
        public ActionResult CreateFAQCategory()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFAQCategory(AdmFAQKategori faqCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.SkapaFAQKategori(faqCategory, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "CreateFAQCategory", e.ToString(), e.HResult, User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny FAQ-kategori skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetFAQCategories");
            }

            return View();
        }


        // GET
        public ActionResult CreateFAQ(int catId = 0)
        {
            var model = new SystemViewModels.FAQViewModel();
            model.FAQkategoriId = catId;
            // Ladda drop down lists. 
            var registerList = _portalAdminService.HamtaAllaRegisterForPortalen();
            this.ViewBag.RegisterList = CreateRegisterDropDownList(registerList);
            model.SelectedRegisterId = 0;
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFAQ(SystemViewModels.FAQViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();

                    AdmFAQ faq = new AdmFAQ
                    {
                        FAQkategoriId = model.FAQkategoriId,
                        RegisterId = model.SelectedRegisterId,
                        Fraga = model.Fraga,
                        Svar = model.Svar
                    };
                    _portalAdminService.SkapaFAQ(faq, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "CreateFAQ", e.ToString(), e.HResult, User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny faq skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetFAQs", new { faqCatId = model.FAQkategoriId });
            }

            return View();
        }


        // GET
        public ActionResult EditSelectedFAQ(int faqId = 0)
        {
            var model = new SystemViewModels.SystemViewModel();
            model.SelectedFAQ = new SystemViewModels.FAQViewModel();
            var selectedFAQDb = _portalAdminService.HamtaFAQ(faqId);
            model.SelectedFAQ.FAQkategoriId = selectedFAQDb.FAQkategoriId;
            model.SelectedFAQ.Id = selectedFAQDb.Id;
            model.SelectedFAQ.Fraga = selectedFAQDb.Fraga;
            model.SelectedFAQ.Svar = selectedFAQDb.Svar;

            if (selectedFAQDb.RegisterId != null)
            {
                model.SelectedFAQ.RegisterId = selectedFAQDb.RegisterId;
            }

            if (model.SelectedFAQ.RegisterId != null)
            {
                model.SelectedFAQ.SelectedRegisterId = model.SelectedFAQ.RegisterId;
            }
            else
            {
                model.SelectedFAQ.SelectedRegisterId = 0;
            }
            // Ladda drop down lists. 
            var registerList = _portalAdminService.HamtaAllaRegisterForPortalen();
            this.ViewBag.RegisterList = CreateRegisterDropDownList(registerList);

            
            return View("_EditSelectedFAQ", model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSelectedFAQ(SystemViewModels.SystemViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();

                    AdmFAQ faq = new AdmFAQ
                    {
                        Id = model.SelectedFAQ.Id,
                        FAQkategoriId = model.SelectedFAQ.FAQkategoriId,
                        RegisterId = model.SelectedFAQ.RegisterId,
                        Fraga = model.SelectedFAQ.Fraga,
                        Svar = model.SelectedFAQ.Svar

                    };
                    _portalAdminService.UppdateraFAQ(faq, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "EditSelectedFAQ", e.ToString(), e.HResult, User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när faq skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetFAQs", new { faqCatId = model.SelectedFAQ.FAQkategoriId });
            }

            return RedirectToAction("GetFAQs", new {faqCatId = model.SelectedFAQ.FAQkategoriId });
        }


        // GET
        public ActionResult CreateInformationText()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInformationText(AdmInformation infoText)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.SkapaInformationsText(infoText, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "CreateInfoText", e.ToString(), e.HResult, User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny informationstext skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetInformationTexts");
            }

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOpeningHoursInfo(SystemViewModels.OpeningHours openHours )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();

                    OpeningHoursInfoDTO openHoursDTO = new OpeningHoursInfoDTO();

                    openHoursDTO = SetHoursAndMinutes(openHours);
                    openHoursDTO.ClosedAnyway = openHours.ClosedAnyway;
                    openHoursDTO.InfoTextForClosedPage = openHours.InfoTextForClosedPage;

                    //Days
                    var daysListDTO = new List<string>();
                    foreach (var day in openHours.ClosedDaysList)
                    {
                        if (day.Selected)
                            daysListDTO.Add(day.NameEnglish);
                    }
                    openHoursDTO.ClosedDays = daysListDTO;

                    _portalAdminService.SparaOppettider(openHoursDTO, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "SaveOpeningHoursInfo", e.ToString(), e.HResult, User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när information om öppettider skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetOpeningHours");
            }

            //var x = Server.HtmlEncode(openHours.InfoTextForClosedPage);
            return RedirectToAction("GetOpeningHours");
        }

        [HttpPost]
        public ActionResult DeleteFAQCategory(int faqCatId)
        {
            try
            {
               _portalAdminService.TaBortFAQKategori(faqCatId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "DeleteFAQCategory", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade när FAQ-kategori skulle tas bort.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetFAQCategories");
        }

        [HttpPost]
        public ActionResult DeleteFAQ(int faqId, int faqCatId)
        {
            try
            {
                _portalAdminService.TaBortFAQ(faqId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "DeleteFAQ", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade när FAQ skulle tas bort.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetFAQs", new { faqCatId = faqCatId });
        }


        private DateTime SetTime(int hour, int minute)
        {
            DateTime time = new DateTime();

            var newDate = time.Date + new TimeSpan(hour, minute, 00);

            return newDate;
        }

        private OpeningHoursInfoDTO SetHoursAndMinutes(SystemViewModels.OpeningHours openingHours)
        {
            var openingHoursDTO = new OpeningHoursInfoDTO();

            string[] openFromSplit = openingHours.OpeningTimeStr.Split(':');
            openingHoursDTO.ClosedToHour = openFromSplit[0];
            openingHoursDTO.ClosedToMin = openFromSplit[1];

            string[] openToSplit = openingHours.ClosingTimeStr.Split(':');
            openingHoursDTO.ClosedFromHour = openToSplit[0];
            openingHoursDTO.ClosedFromMin = openToSplit[1];

            return openingHoursDTO;
        }

        /// <summary>  
        /// Create list for register-dropdown  
        /// </summary>  
        /// <returns>Return register for drop down list.</returns>  
        private IEnumerable<SelectListItem> CreateRegisterDropDownList(IEnumerable<AdmRegister> registerInfoList)
        {
            SelectList lstobj = null;

            var list = registerInfoList
                .Select(p =>
                    new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Kortnamn
                    });

            // Setting.  
            lstobj = new SelectList(list, "Value", "Text");

            return lstobj;
        }

        private List<SystemViewModels.FAQViewModel> ConvertAdmFAQToViewModel(List<AdmFAQ> faqList)
        {
            var faqViewList = new List<SystemViewModels.FAQViewModel>();
            foreach (var faq in faqList)
            {
                var faqView = new SystemViewModels.FAQViewModel
                {
                    Id = faq.Id,
                    RegisterId = faq.RegisterId,
                    SelectedRegisterId = faq.RegisterId,
                    FAQkategoriId = faq.FAQkategoriId,
                    Fraga = faq.Fraga,
                    Svar = faq.Svar
                };

                if (faq.RegisterId != null )
                {
                    var id = Convert.ToInt32(faq.RegisterId);
                    faqView.RegisterKortNamn = _portalAdminService.HamtaKortnamnForRegister(id);
                }

                faqViewList.Add(faqView);
            }
            return faqViewList;
        }

        private AdmFAQ ConvertViewModelToAdmFAQ(SystemViewModels.FAQViewModel faq)
        {
            var faqDb = new AdmFAQ
            {
                Id = faq.Id,
                FAQkategoriId = faq.FAQkategoriId,
                Fraga = faq.Fraga,
                Svar = faq.Svar
            };

            if (faq.SelectedRegisterId == 0)
            {
                faqDb.RegisterId = null;
            }
            else
            {
                faqDb.RegisterId = faq.SelectedRegisterId;
            }

            return faqDb;
        }
    }
}