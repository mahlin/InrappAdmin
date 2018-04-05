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
    public class RegisterController : Controller
    {

        private readonly IPortalAdminService _portalAdminService;

        public RegisterController()
        {
            _portalAdminService =
                new PortalAdminService(new PortalAdminRepository(new InrappAdminDbContext()));
        }

        // GET: Register
        public ActionResult Index()
        {
            var model = new RegisterViewModels.RegisterViewModel();
            var registerViewList = new List<RegisterViewModels.AdmRegisterViewModel>();
            try
            {
                 var registerList = _portalAdminService.HamtaRegister();

                foreach (var register in registerList)
                {
                    var registerView = new RegisterViewModels.AdmRegisterViewModel();
                    registerView.Id = register.Id;
                    registerView.Beskrivning = register.Beskrivning;
                    registerView.Kortnamn = register.Kortnamn;
                    registerView.Registernamn = register.Registernamn;
                    registerView.Inrapporteringsportal = register.Inrapporteringsportal;

                    registerViewList.Add(registerView);
                }

                model.Registers = registerViewList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("RegisterController", "GetDirectories", e.ToString(), e.HResult, "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av register",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("Index", model);
        }

        // GET
        public ActionResult GetSubDirectoriesForDirectory(string regShortName = "")
        {
            var model = new RegisterViewModels.RegisterViewModel();
            try
            {
                var register = _portalAdminService.HamtaRegisterMedKortnamn(regShortName);
                model.RegisterShortName = regShortName;
                model.SelectedDirectoryId = register.Id;
                model.DelRegisters = _portalAdminService.HamtaDelRegisterForRegister(register.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("RegisterController", "GetSubDirectoriesForDirectory", e.ToString(), e.HResult,
                    "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av delregister",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditSubDirectories", model);
        }

        // GET
        public ActionResult GetAllSubDirectories()
        {
            var model = new RegisterViewModels.RegisterViewModel();
            try
            {
                model.DelRegisters = _portalAdminService.HamtaDelRegister();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("RegisterController", "GetAllSubDirectories", e.ToString(), e.HResult,
                    "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av delregister",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("EditSubDirectories", model);
        }


        [HttpPost]
        public ActionResult UpdateDirectory(AdmRegister register)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _portalAdminService.UppdateraRegister(register);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("RegisterController", "UpdateDirectory", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid uppadtering av register.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);

                }
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult UpdateSubDirectory(AdmDelregister delRegister)
        {
            var regShortName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var register= _portalAdminService.HamtaRegisterMedId(delRegister.RegisterId);
                    regShortName = register.Kortnamn;
                    _portalAdminService.UppdateraDelregister(delRegister);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("RegisterController", "UpdateSubDirectory", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid uppadtering av delregister.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);

                }
            }
            return RedirectToAction("GetSubDirectoriesForDirectory", new { regShortName = regShortName });

        }

        // GET
        public ActionResult CreateDirectory()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDirectory(RegisterViewModels.AdmRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AdmRegister register = new AdmRegister
                    {
                        Registernamn = model.Registernamn,
                        Beskrivning = model.Beskrivning,
                        Kortnamn = model.Kortnamn,
                        Inrapporteringsportal = model.Inrapporteringsportal
                    };
                    _portalAdminService.SkapaRegister(register);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("RegisterController", "CreateDirectory", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när nytt register skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("Index");
            }

            return View();
        }


        // GET
        public ActionResult CreateSubDirectory(string regShortName)
        {
            var model = new RegisterViewModels.AdmDelregisterViewModel();
            var register = _portalAdminService.HamtaRegisterMedKortnamn(regShortName);
            model.RegisterShortName = register.Kortnamn;
            model.RegisterId = register.Id;
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubDirectory(AdmDelregister subDir)
        {
            var regShortName = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _portalAdminService.SkapaDelregister(subDir);
                    var register = _portalAdminService.HamtaRegisterMedId(subDir.RegisterId);
                    regShortName = register.Kortnamn;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("RegisterController", "CreateSubDirectory", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när nytt delregister skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetSubDirectoriesForDirectory", new { regShortName = regShortName });
            }

            return View();
        }
    }
}