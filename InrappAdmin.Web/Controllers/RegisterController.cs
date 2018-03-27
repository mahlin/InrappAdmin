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
            var model = new RegisterViewModels();
            try
            {
                model.Registers = _portalAdminService.HamtaRegister();
                //var contacts = _portalAdminService.HamtaKontaktpersonerForOrg(model.Organisation.Id);
                //model.ContactPersons = ConvertUsersViewModelUser(contacts);

                //model.OrgUnits = _portalAdminService.HamtaOrgEnheterForOrg(model.Organisation.Id);
                //model.ReportObligations = _portalAdminService.HamtaUppgiftsskyldighetForOrg(model.Organisation.Id);
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
            var model = new RegisterViewModels();
            try
            {
                var register = _portalAdminService.HamtaRegisterMedKortnamn(regShortName);
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

        // GET: Registers
        //public ActionResult GetDirectories()
        //{
        //    var model = new RegisterViewModels();
        //    try
        //    {
        //        model.Registers = _portalAdminService.HamtaRegister();
        //        //var contacts = _portalAdminService.HamtaKontaktpersonerForOrg(model.Organisation.Id);
        //        //model.ContactPersons = ConvertUsersViewModelUser(contacts);

        //        //model.OrgUnits = _portalAdminService.HamtaOrgEnheterForOrg(model.Organisation.Id);
        //        //model.ReportObligations = _portalAdminService.HamtaUppgiftsskyldighetForOrg(model.Organisation.Id);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        ErrorManager.WriteToErrorLog("RegisterController", "GetDirectories", e.ToString(), e.HResult, "InrappAdmin");
        //        var errorModel = new CustomErrorPageModel
        //        {
        //            Information = "Ett fel inträffade vid hämtning av register",
        //            ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
        //        };
        //        return View("CustomError", errorModel);

        //    }
        //    return View("Index", model);

        //}


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