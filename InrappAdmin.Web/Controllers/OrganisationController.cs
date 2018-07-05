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
    public class OrganisationController : Controller
    {
        private readonly IPortalAdminService _portalAdminService;


        public OrganisationController()
        {
            _portalAdminService =
                new PortalAdminService(new PortalAdminRepository(new InrappAdminDbContext(), new InrappAdminIdentityDbContext()));
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
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
                var reportObligationsDb = _portalAdminService.HamtaUppgiftsskyldighetForOrg(model.Organisation.Id);
                model.ReportObligations = ConvertAdmUppgiftsskyldighetToViewModel(reportObligationsDb.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "GetOrganisation", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av organisation",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                if (e.Message == "Sequence contains no elements")
                {
                    errorModel.Information = "Felaktig kommunkod";
                }

                return View("CustomError", errorModel);

            }
            return View("Index",model);

        }

        // GET
        [Authorize]
        public ActionResult GetContacts()
        {
            return View("EditContacts");
        }


        // GET
        [Authorize]
        public ActionResult GetOrganisationsContacts(string kommunkod)
        {
            var model = new OrganisationViewModels.OrganisationViewModel();
            try
            {
                model.Organisation = _portalAdminService.HamtaOrganisationForKommunkod(kommunkod);
                var contacts = _portalAdminService.HamtaKontaktpersonerForOrg(model.Organisation.Id);
                model.ContactPersons = ConvertUsersViewModelUser(contacts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "GetOrganisationsContacts", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av kontakter för organisation.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                if (e.Message == "Sequence contains no elements")
                {
                    errorModel.Information = "Felaktig kommunkod";
                }
                return View("CustomError", errorModel);
            }
            return View("EditContacts", model);
        }

        //GET
        [Authorize]
        public ActionResult GetOrgUnits()
        {
            return View("EditOrgUnits");
        }

        // GET
        [Authorize]
        public ActionResult GetOrganisationsOrgUnits(string kommunkod = "", int orgId = 0)
        {
            var model = new OrganisationViewModels.OrganisationViewModel();
            try
            {
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "GetOrganisationsOrgUnits", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av organisationsenheter för organisation.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                if (e.Message == "Sequence contains no elements")
                {
                    errorModel.Information = "Felaktig kommunkod";
                }
                return View("CustomError", errorModel);
            }

            return View("EditOrgUnits", model);
        }

        //GET
        [Authorize]
        public ActionResult GetReportObligations()
        {
            return View("EditReportObligations");
        }

        [Authorize]
        public ActionResult GetOrganisationsReportObligations(string kommunkod = "", int orgId = 0)
        {
            var model = new OrganisationViewModels.OrganisationViewModel();
            try
            {
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
                var admUppgSkyldighetList = _portalAdminService.HamtaUppgiftsskyldighetForOrg(model.Organisation.Id);
                model.ReportObligations = ConvertAdmUppgiftsskyldighetToViewModel(admUppgSkyldighetList.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "GetOrganisationsReportObligations", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av uppgiftsskyldighet för organisation.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                if (e.Message == "Sequence contains no elements")
                {
                    errorModel.Information = "Felaktig kommunkod";
                }
                return View("CustomError", errorModel);
            }

            return View("EditReportObligations", model);
        }

        //GET
        [Authorize]
        public ActionResult GetUnitReportObligations()
        {
            var model = new OrganisationViewModels.UnitReportObligationsViewModel();
            //// Ladda drop down lists. 
            model = GetOrgDropDownLists(model);
            return View("EditUnitReportObligations", model);
        }


        [Authorize]
        public ActionResult GetOrganisationsUnitReportObligations(OrganisationViewModels.UnitReportObligationsViewModel model, int orgId = 0, int orgenhetsId = 0)
        {
            try
            {
                if (orgId != 0)
                {
                    model.SelectedOrganisationId = orgId;
                }
                if (orgenhetsId != 0)
                {
                    model.SelectedOrganisationsenhetsId = orgenhetsId;
                }
                var admEnhetUppgSkyldighetList = _portalAdminService.HamtaEnhetsUppgiftsskyldighetForOrgEnhet(model.SelectedOrganisationsenhetsId).ToList();
                model.UnitReportObligations = admEnhetUppgSkyldighetList;
                // Ladda drop down lists. 
                model = GetOrgDropDownLists(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "GetOrganisationsUnitReportObligations", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid hämtning av enhetsuppgiftsskyldighet.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                if (e.Message == "Sequence contains no elements")
                {
                    errorModel.Information = "Felaktig kommunkod";
                }
                return View("CustomError", errorModel);
            }

            return View("EditUnitReportObligations", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateOrganisation(OrganisationViewModels.OrganisationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.UppdateraOrganisation(model.Organisation, userName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "UpdateOrganisation", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid uppdatering av organisation.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetOrganisation", new { kommunkod = model.Organisation.Kommunkod });
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateOrganisationsContact(ApplicationUser user)
        {
            var kommunkod = String.Empty;
            try
            {
                var org = _portalAdminService.HamtaOrgForAnvandare(user.Id);
                kommunkod = _portalAdminService.HamtaKommunkodForOrg(org.Id);
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.UppdateraKontaktperson(user, userName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "UpdateOrganisationsContact", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid uppdatering av kontaktperson.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetOrganisationsContacts", new { kommunkod = kommunkod });

        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateOrganisationsOrgUnit(Organisationsenhet orgUnit)
        {
            var kommunkod = String.Empty;
            try
            {

                var org = _portalAdminService.HamtaOrgForOrganisationsenhet(orgUnit.Id);
                kommunkod = _portalAdminService.HamtaKommunkodForOrg(org.Id);
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.UppdateraOrganisationsenhet(orgUnit, userName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "UpdateOrganisationsOrgUnit", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid uppdatering av organisationsenhet.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetOrganisationsOrgUnits", new { kommunkod = kommunkod });

        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateOrganisationsReportObligation(OrganisationViewModels.ReportObligationsViewModel admUppgSkyldighet)
        {
            var kommunkod = String.Empty;
            try
            {

                var org = _portalAdminService.HamtaOrgForUppgiftsskyldighet(admUppgSkyldighet.Id);
                kommunkod = _portalAdminService.HamtaKommunkodForOrg(org.Id);
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.GetUserName();
                    var admUppgiftsskyldighetToDb = ConvertViewModelToAdmUppgiftsskyldighet(admUppgSkyldighet);
                    _portalAdminService.UppdateraUppgiftsskyldighet(admUppgiftsskyldighetToDb, userName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "UpdateOrganisationsReportObligation", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid uppdatering av uppgiftsskyldighet.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetOrganisationsReportObligations", new { kommunkod = kommunkod });

        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateOrganisationsUnitReportObligation(AdmEnhetsUppgiftsskyldighet admEnhetsUppgSkyldighet)
        {
            var org = new Organisation();
            try
            {
                org = _portalAdminService.HamtaOrgForUppgiftsskyldighet(admEnhetsUppgSkyldighet.UppgiftsskyldighetId);
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.UppdateraEnhetsUppgiftsskyldighet(admEnhetsUppgSkyldighet, userName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "UpdateOrganisationsUnitReportObligation", e.ToString(), e.HResult,
                    User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid uppdatering av enhetsuppgiftsskyldighet.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetOrganisationsUnitReportObligations", new { orgId = org.Id, orgenhetsId = admEnhetsUppgSkyldighet.OrganisationsenhetsId});

        }

        [Authorize]
        public ActionResult CreateOrganisation()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateOrganisation(OrganisationViewModels.OrganisationViewModel model)
        {
            var kommunkod = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    var orgId = _portalAdminService.SkapaOrganisation(model.Organisation, userName);
                    kommunkod = _portalAdminService.HamtaKommunkodForOrg(orgId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateOrganisation", e.ToString(), e.HResult, User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny organisation skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetOrganisation", new { kommunkod = kommunkod });
            }

            return View();
        }

        [Authorize]
        public ActionResult CreateOrganisationUnit(int orgId = 0)
        {
            var model = new OrganisationViewModels.OrganisationsenhetViewModel();
            model.Organisationsid = orgId;
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateOrganisationUnit(Organisationsenhet orgenhet)
        {
            var kommunkod = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    _portalAdminService.SkapaOrganisationsenhet(orgenhet, userName);
                    kommunkod = _portalAdminService.HamtaKommunkodForOrg(orgenhet.OrganisationsId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateOrganisationUnit", e.ToString(), e.HResult, User.Identity.Name);
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

        [Authorize]
        public ActionResult CreateReportObligation(int orgId = 0)
        {
            var model = new OrganisationViewModels.ReportObligationsViewModel();
            model.OrganisationId = orgId;
            var delregisterList = _portalAdminService.HamtaAllaDelregisterForPortalen();
            this.ViewBag.DelregisterList = CreateDelRegisterDropDownList(delregisterList);
            model.DelregisterId = 0;
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateReportObligation(OrganisationViewModels.ReportObligationsViewModel uppgSk, int orgId)
        {
            var kommunkod = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    var admUppgSkyldighet = ConvertToDbFromVM(uppgSk);
                    admUppgSkyldighet.OrganisationId = orgId;
                    _portalAdminService.SkapaUppgiftsskyldighet(admUppgSkyldighet, userName);
                    kommunkod = _portalAdminService.HamtaKommunkodForOrg(uppgSk.OrganisationId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateReportObligation", e.ToString(), e.HResult, User.Identity.Name);
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

        [Authorize]
        public ActionResult CreateUnitReportObligation(int orgId = 0, int orgenhetsId = 0)
        {
            var model = new OrganisationViewModels.UnitReportObligationsViewModel();
            model.SelectedOrganisationId = orgId;
            model.SelectedOrganisationsenhetsId = orgenhetsId;
            var delregisterList = _portalAdminService.HamtaAllaDelregisterForPortalen();
            this.ViewBag.DelregisterList = CreateDelRegisterDropDownList(delregisterList);

            var orgenhetsList = _portalAdminService.HamtaOrgEnheterForOrg(orgId);
            this.ViewBag.OrgenhetList = CreateOrgenhetDropDownList(orgenhetsList);
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateUnitReportObligation(OrganisationViewModels.UnitReportObligationsViewModel enhetsUppgSk)
        {
            var kommunkod = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.GetUserName();
                    var admEnhetsUppgSkyldighet = ConvertViewModelToAdmEnhetsUppgiftsskyldighet(enhetsUppgSk);
                    admEnhetsUppgSkyldighet.UppgiftsskyldighetId = _portalAdminService.HamtaUppgiftsskyldighetForOrgOchDelreg(Convert.ToInt32(enhetsUppgSk.SelectedOrganisationId),
                            Convert.ToInt32(enhetsUppgSk.SelectedDelregisterId)).Id;
                    _portalAdminService.SkapaEnhetsUppgiftsskyldighet(admEnhetsUppgSkyldighet, userName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("OrganisationController", "CreateUnitReportObligation", e.ToString(), e.HResult, User.Identity.Name);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade när ny enhetsuppgiftsskyldighet skulle sparas.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    };
                    return View("CustomError", errorModel);
                }
                return RedirectToAction("GetOrganisationsUnitReportObligations", new { orgId = Convert.ToInt32(enhetsUppgSk.SelectedOrganisationId), orgenhetsId = Convert.ToInt32(enhetsUppgSk.SelectedDelregisterId) });
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteContact(string contactId, string kommunkod)
        {
            try
            {
                _portalAdminService.TaBortKontaktperson(contactId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("OrganisationController", "DeleteContact", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade när kontaktperson skulle tas bort.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
            return RedirectToAction("GetOrganisationsContacts", new {kommunkod = kommunkod});
        }


        private OrganisationViewModels.UnitReportObligationsViewModel GetOrgDropDownLists(OrganisationViewModels.UnitReportObligationsViewModel model)
        {
            var orgList = _portalAdminService.HamtaAllaOrganisationer();
            var orgListDTO = new List<OrganisationDTO>();

            foreach (var org in orgList)
            {
                var organisationDTO = new OrganisationDTO
                {
                    Id = org.Id,
                    Kommunkod = org.Kommunkod,
                    Landstingskod = org.Landstingskod,
                    Organisationsnamn = org.Organisationsnamn
                };
                var orgenheter = _portalAdminService.HamtaOrgEnheterForOrg(org.Id).ToList();
                var orgenhetsListDTO = new List<OrganisationsenhetDTO>();

                foreach (var orgenhet in orgenheter)
                {
                    var orgenhetDTO = new OrganisationsenhetDTO
                    {
                        Id = orgenhet.Id,
                        Enhetsnamn = orgenhet.Enhetsnamn,
                        Enhetskod = orgenhet.Enhetskod
                    };
                    orgenhetsListDTO.Add(orgenhetDTO);
                }
                organisationDTO.Organisationsenheter = orgenhetsListDTO;
                orgListDTO.Add(organisationDTO);
                
            }

            model.OrganisationList = orgListDTO;
            ViewBag.OrganisationList = new SelectList(orgListDTO, "Id", "Kommunkod");

            return model;

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
                    ID = contact.Id,
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

        private AdmUppgiftsskyldighet ConvertViewModelToAdmUppgiftsskyldighet(OrganisationViewModels.ReportObligationsViewModel admUppgskylldighetView)
        {
            var uppgSkyldighet = new AdmUppgiftsskyldighet()
            {
                Id = admUppgskylldighetView.Id,
                OrganisationId = admUppgskylldighetView.OrganisationId,
                DelregisterId = admUppgskylldighetView.DelregisterId,
                SkyldigFrom = admUppgskylldighetView.SkyldigFrom,
                SkyldigTom = admUppgskylldighetView.SkyldigTom,
                RapporterarPerEnhet = admUppgskylldighetView.RapporterarPerEnhet
            };

            return uppgSkyldighet;
        }

        private List<OrganisationViewModels.ReportObligationsViewModel> ConvertAdmUppgiftsskyldighetToViewModel(List<AdmUppgiftsskyldighet> admUppgskyldighetList)
        {
            var uppgSkyldigheter = new List<OrganisationViewModels.ReportObligationsViewModel>();
            foreach (var admUppgskyldighet in admUppgskyldighetList)
            {
                var uppgSkyldighetView = new OrganisationViewModels.ReportObligationsViewModel()
                {
                    Id = admUppgskyldighet.Id,
                    OrganisationId = admUppgskyldighet.OrganisationId,
                    DelregisterId = admUppgskyldighet.DelregisterId,
                    DelregisterKortnamn = _portalAdminService.HamtaKortnamnForDelregister(admUppgskyldighet.DelregisterId),
                    SkyldigFrom = admUppgskyldighet.SkyldigFrom,
                    SkyldigTom = admUppgskyldighet.SkyldigTom,
                    RapporterarPerEnhet = admUppgskyldighet.RapporterarPerEnhet
                };

                uppgSkyldigheter.Add(uppgSkyldighetView);
            }
            

            return uppgSkyldigheter;
        }

        private AdmEnhetsUppgiftsskyldighet ConvertViewModelToAdmEnhetsUppgiftsskyldighet(OrganisationViewModels.UnitReportObligationsViewModel admEnhetsUppgskyldView)
        {
            var enhetsUppgskyldighet = new AdmEnhetsUppgiftsskyldighet()
            {
                Id = admEnhetsUppgskyldView.Id,
                OrganisationsenhetsId = admEnhetsUppgskyldView.SelectedOrganisationsenhetsId,
                UppgiftsskyldighetId = admEnhetsUppgskyldView.UppgiftsskyldighetId,
                SkyldigFrom = admEnhetsUppgskyldView.SkyldigFrom,
                SkyldigTom = admEnhetsUppgskyldView.SkyldigTom
            };

            return enhetsUppgskyldighet;
        }

        //private List<OrganisationViewModels.UnitReportObligationsViewModel> ConvertAdmEnhetsUppgiftsskyldighetToViewModel(List<AdmEnhetsUppgiftsskyldighet> admEnhetsUppgskyldighetList)
        //{
        //    var enhetsuppgkyldigheter = new List<OrganisationViewModels.UnitReportObligationsViewModel>();
        //    foreach (var admEnhetsUppgskyldighet in admEnhetsUppgskyldighetList)
        //    {
        //        var enhetsuppgSkyldighetView = new OrganisationViewModels.UnitReportObligationsViewModel()
        //        {
        //            Id = admEnhetsUppgskyldighet.Id,
        //            OrganisationenhetsId = admEnhetsUppgskyldighet.OrganisationsenhetsId,
        //            SkyldigFrom = admEnhetsUppgskyldighet.SkyldigFrom,
        //            SkyldigTom = admEnhetsUppgskyldighet.SkyldigTom,
        //        };

        //        enhetsuppgkyldigheter.Add(enhetsuppgSkyldighetView);
        //    }

        //    return enhetsuppgkyldigheter;
        //}


        private AdmUppgiftsskyldighet ConvertToDbFromVM(OrganisationViewModels.ReportObligationsViewModel uppgSkVM)
        {
            var admUppgSk = new AdmUppgiftsskyldighet
            {
                Id = uppgSkVM.Id,
                DelregisterId = uppgSkVM.DelregisterId,
                SkyldigFrom = uppgSkVM.SkyldigFrom,
                SkyldigTom = uppgSkVM.SkyldigTom,
                RapporterarPerEnhet =uppgSkVM.RapporterarPerEnhet
            };

            return admUppgSk;
        }

        private IEnumerable<SelectListItem> CreateDelRegisterDropDownList(IEnumerable<AdmDelregister> delregisterList)
        {
            SelectList lstobj = null;

            var list = delregisterList
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

        private IEnumerable<SelectListItem> CreateOrgenhetDropDownList(IEnumerable<Organisationsenhet> orgenhetList)
        {
            SelectList lstobj = null;

            var list = orgenhetList
                .Select(p =>
                    new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Enhetsnamn
                    });

            // Setting.  
            lstobj = new SelectList(list, "Value", "Text");

            return lstobj;
        }



        

    }
}
