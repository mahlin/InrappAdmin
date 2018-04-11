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

        IEnumerable<Leverans> GetLeveranserForOrganisation(int orgId);

        Aterkoppling GetAterkopplingForLeverans(int levId);

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

        AdmFAQ GetFAQ(int faqId);

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

        IEnumerable<AdmForvantadfil> GetAllExpectedFiles();

        IEnumerable<AdmForvantadleverans> GetExpectedDeliveriesForDirectory(int dirId);

        IEnumerable<AdmForvantadfil> GetExpectedFilesForDirectory(int dirId);

        IEnumerable<AdmRegister> GetAllRegisters();

        IEnumerable<AdmRegister> GetAllRegistersForPortal();

        string GetSubDirectoryShortNameForExpectedFile(int filKravId);

        string GetSubDirectoryShortName(int subDirId);

        string GetEnhetskodForLeverans(int orgenhetsid);

        string GetPeriodForAktuellLeverans(int forvLevid);

        IEnumerable<LevereradFil> GetFilerForLeveransId(int leveransId);

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

        void UpdateUserInfo(AppUserAdmin user);

        void SaveOpeningHours(AdmKonfiguration admKonf);

        void DeleteFAQCategory(int faqCategoryId);

        void DeleteFAQ(int faqId);

        void DeleteContact(string contactId);
    }
}
