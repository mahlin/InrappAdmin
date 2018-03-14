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
using InrappAdmin.Web.Models.ViewModels;
using InrapporteringsPortal.Web.Models;
using Microsoft.AspNet.Identity;

namespace InrappAdmin.Web.Controllers
{
    public class OrganisationController : Controller
    {
        private readonly IPortalAdminService _portalAdminService;


        public OrganisationController()
        {
            _portalAdminService =
                new PortalAdminService(new PortalAdminRepository(new InrappAdminDbContext()));
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Organisation
        public ActionResult GetOrganisation(string kommunkod)
        {
            var model = new OrganisationViewModels.OrganisationViewModel();
            try
            {
                model.Kommunkod = kommunkod;
                model.Organisation = _portalAdminService.HamtaOrganisationForKommunkod(kommunkod);
                model.ContactPersons = _portalAdminService.HamtaKontaktpersonerForOrg(model.Organisation.Id);
                model.OrgUnits = _portalAdminService.HamtaOrgEnheterForOrg(model.Organisation.Id);
                model.ReportObligations = _portalAdminService.HamtaUppgiftsskyldighetForOrg(model.Organisation.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "GetOrganisation", e.ToString(), e.HResult, "Admin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av organisation",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);

            }
            return View("Index",model);

        }

        // GET
        public ActionResult GetContacts()
        {
            return View("EditContacts");
        }

        // GET
        public ActionResult GetOrganisationsContacts(string kommunkod)
        {
            var model = new OrganisationViewModels.OrganisationViewModel();
            model.Organisation = _portalAdminService.HamtaOrganisationForKommunkod(kommunkod);
            //TODO skicka med
            //model.Organisation = new Organisation();
            //model.Organisation.Id = 29;
            //model.Organisation.Id = orgId;
            model.ContactPersons = _portalAdminService.HamtaKontaktpersonerForOrg(model.Organisation.Id);

            return View("EditContacts", model);
        }

        [HttpPost]
        public ActionResult UpdateOrganisationsContact(ApplicationUser user)
        {
            var org = _portalAdminService.HamtaOrgForAnvandare(user.Id);
            var kommunkod = _portalAdminService.HamtaKommunkodForOrg(org.Id);
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraKontaktperson(user);
                

                //Employee emp = db.Employees.Single(em => em.Id == employee.Id);
                //emp.Name = employee.Name;
                //emp.Designation = employee.Designation;
                //emp.City = employee.City;
                //emp.State = employee.State;
                //emp.Zip = employee.Zip;
                //db.Entry(emp).State = EntityState.Modified;
                //db.SaveChanges();

            }
            return RedirectToAction("GetOrganisationsContacts", new { kommunkod = kommunkod });

            //return View("EditContacts");

        }

        // GET
        public ActionResult CreateOrganisationUnit(int orgId = 0)
        {
            var model = new OrganisationViewModels.OrganisationsenhetViewModel();
            model.Organisationsid = orgId;
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrganisationUnit(Organisationsenhet orgenhet)
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
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateOrganisationUnit", e.ToString(), e.HResult, "Admin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny organisationsenhet skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetOrganisation", new { kommunkod = kommunkod});
            }

            return View();
        }

        //// GET: Organisation/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Organisation/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Organisation/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Organisation/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Organisation/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Organisation/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Organisation/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
