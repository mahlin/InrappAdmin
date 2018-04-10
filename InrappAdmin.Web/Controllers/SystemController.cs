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
        public ActionResult Index()
        {
            return View();
        }

        // GET: FAQCategories
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
                ErrorManager.WriteToErrorLog("SystemController", "GetFAQCategories", e.ToString(), e.HResult, "InrappAdmin");
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
                model.FAQs = _portalAdminService.HamtaFAQs(faqCatId);
                model.SelectedFAQCategory = faqCatId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "GetFAQs", e.ToString(), e.HResult, "InrappAdmin");
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
                ErrorManager.WriteToErrorLog("SystemController", "GetInformationTexts", e.ToString(), e.HResult, "InrappAdmin");
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
                ErrorManager.WriteToErrorLog("SystemController", "GetOpeningHours", e.ToString(), e.HResult, "InrappAdmin");
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
                _portalAdminService.UppdateraFAQKategori(faqCategory);
            }
            return RedirectToAction("GetFAQCategories");

        }

        [HttpPost]
        public ActionResult UpdateFAQ(AdmFAQ faq)
        {
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraFAQ(faq);
            }
            return RedirectToAction("GetFAQs", new { faqCatId = faq.FAQkategoriId });

        }

        [HttpPost]
        public ActionResult UpdateInfoText(SystemViewModels.SystemViewModel model)
        {
            if (ModelState.IsValid)
            {
                    AdmInformation info = new AdmInformation
                    {
                        Informationstyp = model.SelectedInfo,
                        Text = model.SelectedInfoText
                    };
                    _portalAdminService.UppdateraInformationstext(info);
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
                    _portalAdminService.SkapaFAQKategori(faqCategory);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "CreateFAQCategory", e.ToString(), e.HResult, "InrappAdmin");
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
                    AdmFAQ faq = new AdmFAQ
                    {
                        FAQkategoriId = model.FAQkategoriId,
                        RegisterId = model.RegisterId,
                        Fraga = model.Fraga,
                        Svar = model.Svar
                    };
                    _portalAdminService.SkapaFAQ(faq);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "CreateFAQ", e.ToString(), e.HResult, "InrappAdmin");
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
                    AdmFAQ faq = new AdmFAQ
                    {
                        Id = model.SelectedFAQ.Id,
                        FAQkategoriId = model.SelectedFAQ.FAQkategoriId,
                        RegisterId = model.SelectedFAQ.RegisterId,
                        Fraga = model.SelectedFAQ.Fraga,
                        Svar = model.SelectedFAQ.Svar

                    };
                    _portalAdminService.UppdateraFAQ(faq);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "EditSelectedFAQ", e.ToString(), e.HResult, "InrappAdmin");
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
                    _portalAdminService.SkapaInformationsText(infoText);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "CreateInfoText", e.ToString(), e.HResult, "InrappAdmin");
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

                    _portalAdminService.SparaOppettider(openHoursDTO);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("SystemController", "SaveOpeningHoursInfo", e.ToString(), e.HResult, "InrappAdmin");
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
                ErrorManager.WriteToErrorLog("SystemController", "DeleteFAQCategory", e.ToString(), e.HResult, "InrappAdmin");
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
                ErrorManager.WriteToErrorLog("SystemController", "DeleteFAQ", e.ToString(), e.HResult, "InrappAdmin");
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
    }
}