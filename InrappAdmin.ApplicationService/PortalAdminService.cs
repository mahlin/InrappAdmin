using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.ApplicationService.Interface;
using InrappAdmin.DataAccess;
using InrappAdmin.DomainModel;

namespace InrappAdmin.ApplicationService
{
    public class PortalAdminService : IPortalAdminService
    {

        private readonly IPortalAdminRepository _portalAdminRepository;

        public PortalAdminService(IPortalAdminRepository portalRepository)
        {
            _portalAdminRepository = portalRepository;
        }

        public string HamtaHistorikForOrganisation(int orgId)
        {
            var test  = _portalAdminRepository.GetLeveranserForOrganisation(orgId);
            return test;
        }

        public string HamtaKommunkodForOrg(int orgId)
        {
            var kommunkod = _portalAdminRepository.GetKommunkodForOrg(orgId);
            return kommunkod;
        }

        public IEnumerable<ApplicationUser> HamtaKontaktpersonerForOrg(int orgId)
        {
            var contacts = _portalAdminRepository.GetContactPersonsForOrg(orgId);
            return contacts;
        }

        public Organisation HamtaOrganisationForKommunkod(string kommunkod)
        {
            var org = _portalAdminRepository.GetOrganisationFromKommunkod(kommunkod);
            return org;
        }

        public IEnumerable<Organisationsenhet> HamtaOrgEnheterForOrg(int orgId)
        {
            var orgEnheter = _portalAdminRepository.GetOrgUnitsForOrg(orgId);
            return orgEnheter;
        }

        public IEnumerable<AdmUppgiftsskyldighet> HamtaUppgiftsskyldighetForOrg(int orgId)
        {
            var uppgiftsskyldigheter = _portalAdminRepository.GetReportObligationInformationForOrg(orgId);
            return uppgiftsskyldigheter;
        }
        public Organisation HamtaOrgForAnvandare(string userId)
        {
            var org = _portalAdminRepository.GetOrgForUser(userId);
            return org;
        }

        public Organisation HamtaOrgForOrganisationsenhet(int orgUnitId)
        {
            var org = _portalAdminRepository.GetOrgForOrgUnit(orgUnitId);
            return org;
        }

        public Organisation HamtaOrgForUppgiftsskyldighet(int uppgSkId)
        {
            var org = _portalAdminRepository.GetOrgForReportObligation(uppgSkId);
            return org;
        }

        public void SkapaOrganisationsenhet(Organisationsenhet orgUnit)
        {
            //Sätt datum och användare
            orgUnit.SkapadDatum = DateTime.Now;
            orgUnit.SkapadAv = "InrappAdmin";
            orgUnit.AndradDatum = DateTime.Now;
            orgUnit.AndradAv = "InrappAdmin";

            _portalAdminRepository.CreateOrgUnit(orgUnit);
        }

        public void UppdateraOrganisation(Organisation org)
        {
            _portalAdminRepository.UpdateOrganisation(org);
        }

        public void UppdateraKontaktperson(ApplicationUser user)
        {
            _portalAdminRepository.UpdateContactPerson(user);
        }

        public void UppdateraOrganisationsenhet(Organisationsenhet orgUnit)
        {
            _portalAdminRepository.UpdateOrgUnit(orgUnit);
        }

        public void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet)
        {
            _portalAdminRepository.UpdateReportObligation(uppgSkyldighet);
        }


    }
}
