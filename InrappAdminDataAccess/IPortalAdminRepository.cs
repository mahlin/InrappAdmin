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

        IEnumerable<AppUserAdmin> GetAdminUsers();

        IEnumerable<Organisationsenhet> GetOrgUnitsForOrg(int orgId);

        IEnumerable<AdmUppgiftsskyldighet> GetReportObligationInformationForOrg(int orgId);

        IEnumerable<AdmFAQKategori> GetFAQCategories();

        IEnumerable<AdmFAQ> GetFAQs(int faqCatId);
        IEnumerable<AdmHelgdag> GetAllHolidays();

        IEnumerable<AdmSpecialdag> GetAllSpecialDays();

        AdmFAQ GetFAQ(int faqId);

        AdmFAQKategori GetFAQCategory(int faqCatId);

        IEnumerable<AdmInformation> GetInformationTexts();

        IEnumerable<AdmKonfiguration> GetAdmConfiguration();

        AdmInformation GetInfoText(string infoType);

        AdmInformation GetInfoText(int infoId);

        int GetPageInfoTextId(string pageType);

        AdmRegister GetDirectoryByShortName(string shortName);

        AdmRegister GetDirectoryById(int dirId);

        IEnumerable<AdmRegister> GetDirectories();

        IEnumerable<AdmDelregister> GetSubDirectories();

        IEnumerable<AdmDelregister> GetSubDirectoriesForDirectory(int dirId);

        AdmDelregister GetSubDirectoryByShortName(string shortName);

        IEnumerable<AdmForvantadleverans> GetExpectedDeliveries();

        IEnumerable<AdmForvantadfil> GetAllExpectedFiles();

        IEnumerable<AdmFilkrav> GetAllFileRequirements();

        IEnumerable<AdmForvantadleverans> GetExpectedDeliveriesForDirectory(int dirId);
        IEnumerable<AdmFilkrav> GetFileRequirementsForDirectory(int dirId);

        IEnumerable<AdmFilkrav> GetFileRequirementsForSubDirectory(int subdirId);

        IEnumerable<AdmForvantadfil> GetExpectedFilesForDirectory(int dirId);

        IEnumerable<AdmRegister> GetAllRegisters();

        IEnumerable<AdmRegister> GetAllRegistersForPortal();

        IEnumerable<AdmDelregister> GetAllSubDirectoriesForPortal();

        string GetDirectoryShortName(int dirId);

        string GetSubDirectoryShortNameForExpectedFile(int filKravId);

        string GetSubDirectoryShortName(int subDirId);

        string GetFileRequirementName(int filereqId);

        string GetEnhetskodForLeverans(int orgenhetsid);

        string GetPeriodForAktuellLeverans(int forvLevid);

        AdmForeskrift GetForeskriftByFileReq(int fileReqId);

        IEnumerable<LevereradFil> GetFilerForLeveransId(int leveransId);

        IEnumerable<Rapporteringsresultat> GetReportResultForDirAndPeriod(int delRegId, string period);

        int CreateOrganisation(Organisation org);

        void CreateOrgUnit(Organisationsenhet orgUnit);

        void CreateFAQCategory(AdmFAQKategori faqCategory);

        void CreateFAQ(AdmFAQ faq);
        void CreateHoliday(AdmHelgdag holiday);
        void CreateSpecialDay(AdmSpecialdag specialDay);
        void CreateInformationText(AdmInformation infoText);
        void CreateReportObligation(AdmUppgiftsskyldighet uppgSk);

        void CreateDirectory(AdmRegister dir);

        void CreateSubDirectory(AdmDelregister subDir);

        void CreateExpectedDelivery(AdmForvantadleverans forvLev);

        void CreateExpectedFile(AdmForvantadfil forvFil);

        void CreateFileRequirement(AdmFilkrav filkrav);

        void UpdateOrganisation(Organisation org);

        void UpdateContactPerson(ApplicationUser user);

        void UpdateAdminUser(AppUserAdmin user);
        void UpdateOrgUnit(Organisationsenhet orgUnit);

        void UpdateReportObligation(AdmUppgiftsskyldighet repObligation);

        void UpdateFAQCategory(AdmFAQKategori faqCategory);

        void UpdateFAQ(AdmFAQ faq);

        void UpdateHoliday(AdmHelgdag holiday);

        void UpdateSpecialDay(AdmSpecialdag specialDay);

        void UpdateInfoText(AdmInformation infoText);

        void UpdateDirectory(AdmRegister directory);

        void UpdateSubDirectory(AdmDelregister subDirectory);

        void UpdateExpectedDelivery(AdmForvantadleverans forvLev);

        void UpdateExpectedFile(AdmForvantadfil forvFil);

        void UpdateFileRequirement(AdmFilkrav filkrav);

        void UpdateUserInfo(AppUserAdmin user);

        void SaveOpeningHours(AdmKonfiguration admKonf);

        void DeleteFAQCategory(int faqCategoryId);

        void DeleteFAQ(int faqId);

        void DeleteHoliday(int holidayId);

        void DeleteSpecialDay(int specialDayId);

        void DeleteContact(string contactId);

        void DeleteAdminUser(string userId);
    }
}
