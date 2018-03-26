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
                var contacts = _portalAdminService.HamtaKontaktpersonerForOrg(model.Organisation.Id);
                model.ContactPersons = ConvertUsersViewModelUser(contacts);

                model.OrgUnits = _portalAdminService.HamtaOrgEnheterForOrg(model.Organisation.Id);
                model.ReportObligations = _portalAdminService.HamtaUppgiftsskyldighetForOrg(model.Organisation.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "GetOrganisation", e.ToString(), e.HResult, "InrappAdmin");
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
            var contacts = _portalAdminService.HamtaKontaktpersonerForOrg(model.Organisation.Id);
            model.ContactPersons = ConvertUsersViewModelUser(contacts);

            return View("EditContacts", model);
        }

        //GET
        public ActionResult GetOrgUnits()
        {
            return View("EditOrgUnits");
        }

        // GET
        public ActionResult GetOrganisationsOrgUnits(string kommunkod = "", int orgId = 0)
        {
            var model = new OrganisationViewModels.OrganisationViewModel();
            if (kommunkod != "")
            {
                model.Kommunkod = kommunkod;
                model.Organisation = _portalAdminService.HamtaOrganisationForKommunkod(kommunkod);
            }
            else if (orgId != 0)
            {
                model.Organisation = _portalAdminService.HamtaOrganisation(orgId);
                model.Kommunkod = _portalAdminService.HamtaKommunkodForOrg(orgId);
            }
            model.OrgUnits = _portalAdminService.HamtaOrgEnheterForOrg(model.Organisation.Id);

            return View("EditOrgUnits", model);
        }

        //GET
        public ActionResult GetReportObligations()
        {
            return View("EditReportObligations");
        }

        public ActionResult GetOrganisationsReportObligations(string kommunkod = "", int orgId = 0)
        {
            var model = new OrganisationViewModels.OrganisationViewModel();
            if (kommunkod != "")
            {
                model.Kommunkod = kommunkod;
                model.Organisation = _portalAdminService.HamtaOrganisationForKommunkod(kommunkod);
            }
            else if (orgId != 0)
            {
                model.Organisation = _portalAdminService.HamtaOrganisation(orgId);
                model.Kommunkod = _portalAdminService.HamtaKommunkodForOrg(orgId);
            }
            model.ReportObligations = _portalAdminService.HamtaUppgiftsskyldighetForOrg(model.Organisation.Id);

            return View("EditReportObligations", model);
        }

        [HttpPost]
        public ActionResult UpdateOrganisation(OrganisationViewModels.OrganisationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraOrganisation(model.Organisation);
            }
            return RedirectToAction("GetOrganisation", new { kommunkod = model.Organisation.Kommunkod });

        }

        [HttpPost]
        public ActionResult UpdateOrganisationsContact(ApplicationUser user)
        {
            var org = _portalAdminService.HamtaOrgForAnvandare(user.Id);
            var kommunkod = _portalAdminService.HamtaKommunkodForOrg(org.Id);
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraKontaktperson(user);
            }
            return RedirectToAction("GetOrganisationsContacts", new { kommunkod = kommunkod });

        }

        [HttpPost]
        public ActionResult UpdateOrganisationsOrgUnit(Organisationsenhet orgUnit)
        {
            var org = _portalAdminService.HamtaOrgForOrganisationsenhet(orgUnit.Id);
            var kommunkod = _portalAdminService.HamtaKommunkodForOrg(org.Id);
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraOrganisationsenhet(orgUnit);
            }
            return RedirectToAction("GetOrganisationsOrgUnits", new { kommunkod = kommunkod });

        }

        [HttpPost]
        public ActionResult UpdateOrganisationsReportObligation(AdmUppgiftsskyldighet admUppgSkyldighet)
        {
            var org = _portalAdminService.HamtaOrgForUppgiftsskyldighet(admUppgSkyldighet.Id);
            var kommunkod = _portalAdminService.HamtaKommunkodForOrg(org.Id);
            if (ModelState.IsValid)
            {
                _portalAdminService.UppdateraUppgiftsskyldighet(admUppgSkyldighet);
            }
            return RedirectToAction("GetOrganisationsReportObligations", new { kommunkod = kommunkod });

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
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateOrganisationUnit", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny organisationsenhet skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetOrganisationsOrgUnits", new { kommunkod = kommunkod});
            }

            return View();
        }

        // GET
        public ActionResult CreateReportObligation(int orgId = 0)
        {
            var model = new OrganisationViewModels.ReportObligationsViewModel();
            model.Organisationsid = orgId;
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportObligation(AdmUppgiftsskyldighet uppgSk, int orgId = 0)
        {
            var kommunkod = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _portalAdminService.SkapaUppgiftsskyldighet(uppgSk);
                    kommunkod = _portalAdminService.HamtaKommunkodForOrg(uppgSk.OrganisationId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateReportObligation", e.ToString(), e.HResult, "InrappAdmin");
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny uppgiftsskyldighet skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetOrganisationsReportObligations", new { kommunkod = kommunkod });
            }

            return View();
        }

        [HttpPost]
        public ActionResult DeleteContact(string contactId, string kommunkod)
        {
            try
            {
                _portalAdminService.TaBortKontaktperson(contactId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "DeleteContact", e.ToString(), e.HResult, "InrappAdmin");
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade när kontaktperson skulle tas bort.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetOrganisationsContacts", new {kommunkod = kommunkod});
        }

        private IEnumerable<OrganisationViewModels.ApplicationUserViewModel> ConvertUsersViewModelUser(IEnumerable<ApplicationUser> contacts)
        {
            var contactPersonsView = new List<OrganisationViewModels.ApplicationUserViewModel>();

            var okToDelete = false;

            foreach (var contact in contacts)
            {
                if (!contact.PhoneNumberConfirmed)
                {
                    okToDelete = true;
                }
                else
                {
                    okToDelete = false;
                }
                var contactView = new OrganisationViewModels.ApplicationUserViewModel
                {
                    OrganisationId = contact.OrganisationId,
                    Namn = contact.Namn,
                    AktivFrom = contact.AktivFrom,
                    AktivTom = contact.AktivTom,
                    Status = contact.Status,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber,
                    PhoneNumberConfirmed = contact.PhoneNumberConfirmed,
                    SkapadDatum = contact.SkapadDatum,
                    SkapadAv = contact.SkapadAv,
                    AndradDatum = contact.AndradDatum,
                    AndradAv = contact.AndradAv,
                    OkToDelete = okToDelete
                };

                contactPersonsView.Add(contactView);
            }
            return contactPersonsView;


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
