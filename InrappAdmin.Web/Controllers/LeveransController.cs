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
                var forvLevList = _portalAdminService.HamtaForvantadeLeveranser();
                model.ForvantadeLeveranser = ConvertForvLevToViewModel(forvLevList.ToList());
                // Ladda drop down lists. 
                var registerList = _portalAdminService.HamtaAllaRegisterForPortalen();
                this.ViewBag.RegisterList = CreateRegisterDropDownList(registerList);
                model.SelectedRegisterId = 0;

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

        // GET
        public ActionResult GetDirectorysExpectedDeliveries(LeveransViewModels.LeveransViewModel model, int regId = 0)
        {
            var dirId = model.SelectedRegisterId;
            if (dirId != 0)
            {
                var forvLevList = _portalAdminService.HamtaForvantadeLeveranserForRegister(dirId);
                //Lägg över i modellen
                model.ForvantadeLeveranser = ConvertForvLevToViewModel(forvLevList.ToList());
                // Ladda drop down lists. 
                var registerList = _portalAdminService.HamtaAllaRegisterForPortalen();
                this.ViewBag.RegisterList = CreateRegisterDropDownList(registerList);
                model.SelectedRegisterId = 0;
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }

        // GET
        public ActionResult GetDirectorysExpectedFiles(LeveransViewModels.LeveransViewModel model)
        {
            var dirId = model.SelectedRegisterId;
            if (dirId != 0)
            {
                var register = _portalAdminService.HamtaRegisterMedId(dirId);
                var forvFilList = _portalAdminService.HamtaForvantadeFilerForRegister(register.Id);
                //Lägg över i modellen
                model.ForvantadeFiler = ConvertForvFilToViewModel(forvFilList.ToList());
                // Ladda drop down lists. 
                var registerList = _portalAdminService.HamtaAllaRegisterForPortalen();
                this.ViewBag.RegisterList = CreateRegisterDropDownList(registerList);
                model.SelectedRegisterId = 0;
            }
            else
            {
                return RedirectToAction("GetForvantadeFiler");
            }
            return View("EditForvantadFil", model);
        }

        // GET: AdmForvantadfil
        public ActionResult GetForvantadeFiler()
        {
            var model = new LeveransViewModels.LeveransViewModel();
            try
            {
                var forvFilViewList = new List<LeveransViewModels.AdmForvantadfilViewModel>();
                var forvFiler = _portalAdminService.HamtaAllaForvantadeFiler();

                foreach (var forvFil in forvFiler)
                {
                    var forvFilView = new LeveransViewModels.AdmForvantadfilViewModel
                        {
                          Id  = forvFil.Id,
                          FilkravId = forvFil.FilkravId,
                          ForeskriftsId = forvFil.ForeskriftsId,
                          DelregisterKortnamn = _portalAdminService.HamtaKortnamnForDelregister(forvFil.FilkravId),
                          Filmask = forvFil.Filmask,
                          Regexp = forvFil.Regexp,
                          Obligatorisk = forvFil.Obligatorisk,
                          Tom = forvFil.Tom
                        };

                    forvFilViewList.Add(forvFilView);
                }

                model.ForvantadeFiler = forvFilViewList;

                // Ladda drop down lists. 
                var registerList = _portalAdminService.HamtaAllaRegisterForPortalen();
                this.ViewBag.RegisterList = CreateRegisterDropDownList(registerList);
                model.SelectedRegisterId = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("LeveransController", "GetForvantadeFiler", e.ToString(), e.HResult, "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av forvantad fil",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditForvantadFil", model);
        }

        [HttpPost]
        public ActionResult UpdateForvantadLeverans(LeveransViewModels.AdmForvantadleveransViewModel forvLevModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var forvLev = ConvertViewModelToForvLev(forvLevModel);
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

        private AdmForvantadleverans ConvertViewModelToForvLev(LeveransViewModels.AdmForvantadleveransViewModel forvLevModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult UpdateForvantadFil(AdmForvantadfil forvFil)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _portalAdminService.UppdateraForvantadFil(forvFil);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("LeveransController", "UpdateForvantadFil", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid uppadtering av förväntad fil.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);

                }
            }
            return RedirectToAction("GetForvantadeFiler");

        }

        // GET
        public ActionResult CreateForvantadLeverans()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForvantadLeverans(LeveransViewModels.AdmForvantadleveransViewModel forvantadLeverans)
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

        // GET
        public ActionResult CreateForvantadFil()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForvantadFil(LeveransViewModels.AdmForvantadfilViewModel forvantadFil)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var admForvFil = new AdmForvantadfil();
                    admForvFil.FilkravId = forvantadFil.FilkravId;
                    admForvFil.Filmask= forvantadFil.Filmask;
                    admForvFil.Regexp = forvantadFil.Regexp;
                    admForvFil.Obligatorisk = forvantadFil.Obligatorisk;
                    admForvFil.Tom = forvantadFil.Tom;
                    _portalAdminService.SkapaForvantadFil(admForvFil);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("LeveransController", "CreateForvantadFil", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny förväntad fil skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("Index");
            }

            return View();
        }

        private List<LeveransViewModels.AdmForvantadfilViewModel> ConvertForvFilToViewModel(List<AdmForvantadfil> forvFilList)
        {
            var forvFilViewList = new List<LeveransViewModels.AdmForvantadfilViewModel>();
            foreach (var forvFil in forvFilList)
            {
                var forvFilView = new LeveransViewModels.AdmForvantadfilViewModel
                {
                    Id = forvFil.Id,
                    FilkravId = forvFil.FilkravId,
                    DelregisterKortnamn = _portalAdminService.HamtaKortnamnForDelregister(forvFil.FilkravId),
                    Filmask = forvFil.Filmask,
                    Regexp = forvFil.Regexp,
                    Obligatorisk = forvFil.Obligatorisk,
                    Tom = forvFil.Tom
                };

                forvFilViewList.Add(forvFilView);
            }
            return forvFilViewList;
        }

        private List<LeveransViewModels.AdmForvantadleveransViewModel> ConvertForvLevToViewModel(List<AdmForvantadleverans> forvLevList)
        {
            var forvLevViewList = new List<LeveransViewModels.AdmForvantadleveransViewModel>();
            foreach (var forvLev in forvLevList)
            {
                var forvLevView = new LeveransViewModels.AdmForvantadleveransViewModel
                {
                    Id = forvLev.Id,
                    FilkravId = forvLev.FilkravId,
                    DelregisterId = forvLev.DelregisterId,
                    DelregisterKortnamn = _portalAdminService.HamtaKortnamnForDelregister(forvLev.FilkravId),
                    Period = forvLev.Period,
                    Uppgiftsstart = forvLev.Uppgiftsstart,
                    Uppgiftsslut = forvLev.Uppgiftsslut,
                    Rapporteringsstart = forvLev.Rapporteringsstart,
                    Rapporteringsslut = forvLev.Rapporteringsslut,
                    Rapporteringsenast = forvLev.Rapporteringsenast,
                    Paminnelse1 = forvLev.Paminnelse1,
                    Paminnelse2 = forvLev.Paminnelse2,
                    Paminnelse3 = forvLev.Paminnelse3
                };

                forvLevViewList.Add(forvLevView);
            }
            return forvLevViewList;
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
    }
}