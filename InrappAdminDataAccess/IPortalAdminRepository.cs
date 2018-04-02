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

        Organisation GetOrganisation(int orgId);

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

        IEnumerable<AdmFAQ> GetFAQs(int faqCatId);

        IEnumerable<AdmInformation> GetInformationTexts();

        IEnumerable<AdmKonfiguration> GetAdmConfiguration();

        AdmInformation GetInfoText(string infoType);
        
        int GetPageInfoTextId(string pageType);

        AdmRegister GetDirectoryByShortName(string shortName);

        AdmRegister GetDirectoryById(int dirId);

        IEnumerable<AdmRegister> GetDirectories();

        IEnumerable<AdmDelregister> GetSubDirectories();

        IEnumerable<AdmDelregister> GetSubDirectoriesForDirectory(int dirId);

        IEnumerable<AdmForvantadleverans> GetExpectedDeliveries();

        IEnumerable<AdmForvantadfil> GetExpectedFiles();

        void CreateOrgUnit(Organisationsenhet orgUnit);

        void CreateFAQCategory(AdmFAQKategori faqCategory);

        void CreateFAQ(AdmFAQ faq);

        void CreateInformationText(AdmInformation infoText);
        void CreateReportObligation(AdmUppgiftsskyldighet uppgSk);

        void CreateDirectory(AdmRegister dir);

        void CreateSubDirectory(AdmDelregister subDir);

        void CreateExpectedDelivery(AdmForvantadleverans forvLev);

        void CreateExpectedFile(AdmForvantadfil forvFil);

        void UpdateOrganisation(Organisation org);

        void UpdateContactPerson(ApplicationUser user);

        void UpdateOrgUnit(Organisationsenhet orgUnit);

        void UpdateReportObligation(AdmUppgiftsskyldighet repObligation);

        void UpdateFAQCategory(AdmFAQKategori faqCategory);

        void UpdateFAQ(AdmFAQ faq);

        void UpdateInfoText(AdmInformation infoText);

        void UpdateDirectory(AdmRegister directory);

        void UpdateSubDirectory(AdmDelregister subDirectory);

        void UpdateExpectedDelivery(AdmForvantadleverans forvLev);

        void UpdateExpectedFile(AdmForvantadfil forvFil);

        void SaveOpeningHours(AdmKonfiguration admKonf);

        void DeleteFAQCategory(int faqCategoryId);

        void DeleteFAQ(int faqId);

        void DeleteContact(string contactId);
    }
}
