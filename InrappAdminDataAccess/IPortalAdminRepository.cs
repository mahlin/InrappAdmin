using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.DomainModel;

namespace InrappAdmin.DataAccess
{
    public interface IPortalAdminRepository
    {

        string GetLeveranserForOrganisation(int orgId);

        Organisation GetOrganisationFromKommunkod(string kommunkod);

        string GetKommunkodForOrg(int orgId);

        Organisation GetOrgForUser(string userId);

        Organisation GetOrgForOrgUnit(int orgUnitId);

        Organisation GetOrgForReportObligation(int repObligationId);

        int GetUserOrganisationId(string userId);

        int GetOrgUnitOrganisationId(int orgUnitId);

        int GetReportObligationOrganisationId(int repObligationId);

        IEnumerable<ApplicationUser> GetContactPersonsForOrg(int orgId);

        IEnumerable<Organisationsenhet> GetOrgUnitsForOrg(int orgId);

        IEnumerable<AdmUppgiftsskyldighet> GetReportObligationInformationForOrg(int orgId);

        IEnumerable<AdmFAQKategori> GetFAQCategories();

        IEnumerable<AdmInformation> GetInformationTexts();

        IEnumerable<AdmKonfiguration> GetAdmConfiguration();

        void CreateOrgUnit(Organisationsenhet orgUnit);

        void CreateFAQCategory(AdmFAQKategori faqCategory);

        void CreateInformationText(AdmInformation infoText);

        void UpdateOrganisation(Organisation org);

        void UpdateContactPerson(ApplicationUser user);

        void UpdateOrgUnit(Organisationsenhet orgUnit);

        void UpdateReportObligation(AdmUppgiftsskyldighet repObligation);

        void UpdateFAQCategory(AdmFAQKategori faqCategory);

        void UpdateInfoText(AdmInformation infoText);

        void SaveOpeningHours(AdmKonfiguration admKonf);
    }
}
