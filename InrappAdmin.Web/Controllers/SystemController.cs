﻿using System;
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
                new PortalAdminService(new PortalAdminRepository(new InrappAdminDbContext()));
        }

        // GET: System
        public ActionResult Index()
        {
            return View();
        }

        // GET: FAQs
        public ActionResult GetFAQs()
        {
            var model = new SystemViewModels.SystemViewModel();
            try
            {
                model.FAQCategories = _portalAdminService.HamtaFAQkategorier();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("SystemController", "GetFAQs", e.ToString(), e.HResult, "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av FAQ-kategorier",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditFAQCategory", model);
        }

        // GET: InformationTexts
        public ActionResult GetInformationTexts()
        {
            var model = new SystemViewModels.SystemViewModel();
            try
            {
                model.InfoPages = _portalAdminService.HamtaInformationstexter();
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
                model.ClosingTime = SetTime(admKonf.ClosedFromHour, admKonf.ClosedFromMin);
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
        public ActionResult UpdateInformationText(AdmInformation infoText)
        {
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraInformationstext(infoText);
            }
            return RedirectToAction("GetInformationTexts");

        }

        [HttpPost]
        public ActionResult UpdateFAQCategory(AdmFAQKategori faqCategory)
        {
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraFAQKategori(faqCategory);
            }
            return RedirectToAction("GetFAQs");

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
                return RedirectToAction("GetFAQs");
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
        public ActionResult CreateFAQ(Organisationsenhet orgenhet)
        {
            var kommunkod = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _portalAdminService.SkapaOrganisationsenhet(orgenhet);
                    kommunkod = _portalAdminService.HamtaKommunkodForOrg(orgenhet.OrganisationsId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateOrganisationUnit", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny organisationsenhet skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetFAQs");
            }

            return View();
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
                    OpeningHoursInfoDTO openHoursDTO = new OpeningHoursInfoDTO
                    {
                        ClosedFromHour = openHours.ClosedFromHour,
                        ClosedFromMin = openHours.ClosedFromMin,
                        ClosedToHour = openHours.ClosedToHour,
                        ClosedToMin = openHours.ClosedToMin,
                        ClosedAnyway = openHours.ClosedAnyway,
                        InfoTextForClosedPage = openHours.InfoTextForClosedPage
                    };

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


        private DateTime SetTime(int hour, int minute)
        {
            DateTime time = new DateTime();

            var newDate = time.Date + new TimeSpan(hour, minute, 00);

            return newDate;
        }
    }
}