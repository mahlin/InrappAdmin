using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InrappAdmin.ApplicationService;
using InrappAdmin.ApplicationService.Interface;
using InrappAdmin.DataAccess;
using InrappAdmin.DomainModel;
using InrappAdmin.Web.Helpers;
using InrappAdmin.Web.Models;
using InrappAdmin.Web.Models.ViewModels;

namespace InrappAdmin.Web.Controllers
{
    public class LeveransController : Controller
    {

        private readonly IPortalAdminService _portalAdminService;

        public LeveransController()
        {
            _portalAdminService =
                new PortalAdminService(new PortalAdminRepository(new InrappAdminDbContext()));
        }

        // GET: Leverans
        public ActionResult Index()
        {
            var model = new LeveransViewModels.LeveransViewModel();
            try
            {
                model.ForvantadeLeveranser = _portalAdminService.HamtaForvantadeLeveranser();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("LeveransController", "Index", e.ToString(), e.HResult, "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av förväntade leveranser",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("Index", model);
        }

        // GET: FAQCategories
        public ActionResult GetForvantadFil()
        {
            var model = new LeveransViewModels.LeveransViewModel();
            try
            {
               // model.FAQCategories = _portalAdminService.HamtaFAQkategorier();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("LeveransController", "GetForvantadFil", e.ToString(), e.HResult, "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av forvantad fil",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult UpdateForvantadLeverans(AdmForvantadleverans forvLev)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    _portalAdminService.UppdateraForvantadLeverans(forvLev);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("LeveransController", "UpdateForvantadLeverans", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid uppadtering av förväntad leverans.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);

                }
            }
            return RedirectToAction("Index");

        }

        // GET
        public ActionResult CreateForvantadLeverans()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForvantadLeverans(LeveransViewModels.AdmForvantafleveransViewModel forvantadLeverans)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var admForvlev = new AdmForvantadleverans();
                    admForvlev.DelregisterId = forvantadLeverans.DelregisterId;
                    admForvlev.FilkravId = forvantadLeverans.FilkravId;
                    admForvlev.Period = forvantadLeverans.Period;
                    admForvlev.Uppgiftsstart = forvantadLeverans.Uppgiftsstart;
                    admForvlev.Uppgiftsslut = forvantadLeverans.Uppgiftsslut;
                    admForvlev.Rapporteringsstart = forvantadLeverans.Rapporteringsstart;
                    admForvlev.Rapporteringsslut = forvantadLeverans.Rapporteringsslut;
                    admForvlev.Rapporteringsenast = forvantadLeverans.Rapporteringsenast;
                    _portalAdminService.SkapaForvantadLeverans(admForvlev);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("LeveransController", "CreateForvantadLeverans", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny förväntad leverans skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}