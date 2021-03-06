﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.DataAccess;
using InrappAdmin.DomainModel;
using Microsoft.Owin.Security;

namespace InrappAdmin.DataAccess
{
    public class PortalAdminRepository : IPortalAdminRepository
    {

        private InrappAdminDbContext DbContext { get; }

        private InrappAdminIdentityDbContext IdentityDbContext { get; }

        public PortalAdminRepository(InrappAdminDbContext dbContext, InrappAdminIdentityDbContext identityDbContext)
        {
            DbContext = dbContext;
            IdentityDbContext = identityDbContext;
        }

        private IEnumerable<Leverans> AllaLeveranser()
        {
            return DbContext.Leverans;
        }

        public IEnumerable<Leverans> GetLeveranserForOrganisation(int orgId)
        {
            var levIdnForOrg = AllaLeveranser().Where(a => a.OrganisationId == orgId).ToList();
            return levIdnForOrg;
        }

        public Aterkoppling GetAterkopplingForLeverans(int levId)
        {
            var aterkoppling = DbContext.Aterkoppling.FirstOrDefault(x => x.LeveransId == levId);
            return aterkoppling;
        }

        public string GetEnhetskodForLeverans(int orgenhetsid)
        {
            var enhetskod = DbContext.Organisationsenhet.Where(x => x.Id == orgenhetsid).Select(x => x.Enhetskod).SingleOrDefault();
            return enhetskod;
        }

        public string GetPeriodForAktuellLeverans(int forvLevid)
        {
            var period = DbContext.AdmForvantadleverans.Where(x => x.Id == forvLevid).Select(x => x.Period).SingleOrDefault();
            return period;
        }

        public IEnumerable<LevereradFil> GetFilerForLeveransId(int leveransId)
        {
            
            var filInfo = DbContext.LevereradFil.Where(a => a.LeveransId == leveransId).OrderByDescending(x => x.LeveransId); ;

            return filInfo;
        }


        public Organisation GetOrganisationFromKommunkod(string kommunkod)
        {
            var org = DbContext.Organisation.Single(a => a.Kommunkod == kommunkod);
            return org;
        }

        public Organisation GetOrganisation(int orgId)
        {
            var org = DbContext.Organisation.Where(x => x.Id == orgId).Select(x => x).SingleOrDefault();
            return org;
        }

        public Organisation GetOrgForUser(string userId)
        {
            var orgId = GetUserOrganisationId(userId);

            var org = DbContext.Organisation.Where(o => o.Id == orgId).Select(o => o).FirstOrDefault();

            return org;
        }

        public Organisation GetOrgForOrgUnit(int orgUnitId)
        {
            var orgId = GetOrgUnitOrganisationId(orgUnitId);
            var org = DbContext.Organisation.Where(o => o.Id == orgId).Select(o => o).FirstOrDefault();

            return org;
        }

        public Organisation GetOrgForReportObligation(int repObligationId)
        {
            var orgId = GetReportObligationOrganisationId(repObligationId);
            var org = DbContext.Organisation.Where(o => o.Id == orgId).Select(o => o).FirstOrDefault();

            return org;

        }

        public int GetUserOrganisationId(string userId)
        {
            var orgId = DbContext.ApplicationUser.Where(u => u.Id == userId).Select(o => o.OrganisationId).SingleOrDefault();
            return orgId;
        }

        public int GetOrgUnitOrganisationId(int orgUnitId)
        {
            var orgId = DbContext.Organisationsenhet.Where(u => u.Id == orgUnitId).Select(o => o.OrganisationsId).SingleOrDefault();
            return orgId;
        }

        public int GetReportObligationOrganisationId(int repObligationId)
        {
            var orgId = DbContext.AdmUppgiftsskyldighet.Where(u => u.Id == repObligationId).Select(o => o.OrganisationId).SingleOrDefault();
            return orgId;
        }

        public AdmUppgiftsskyldighet GetReportObligationById(int repOblId)
        {
            var repObl = DbContext.AdmUppgiftsskyldighet.SingleOrDefault(x => x.Id == repOblId);
            return repObl;
        }

        public IEnumerable<ApplicationUser> GetContactPersonsForOrg(int orgId)
        {
            var contacts = DbContext.ApplicationUser.Where(x => x.OrganisationId == orgId).ToList();
            return contacts;
        }

        public IEnumerable<ApplicationUser> GetContactPersonsForOrgAndSubdir(int orgId, int subdirId)
        {
            var contactList = new List<ApplicationUser>();
            var contacts = DbContext.ApplicationUser.Where(x => x.OrganisationId == orgId).ToList();
            foreach (var contact in contacts)
            {
                var role = DbContext.Roll.FirstOrDefault(x => x.ApplicationUserId == contact.Id && x.DelregisterId == subdirId);
                if (role != null)
                {
                    contactList.Add(contact);
                }
            }
            return contactList;
        }

        public IEnumerable<AppUserAdmin> GetAdminUsers()
        {
            var adminUsers = IdentityDbContext.Users.ToList();
            return adminUsers;
        }

        public IEnumerable<Organisationsenhet> GetOrgUnitsForOrg(int orgId)
        {
            var orgUnits = DbContext.Organisationsenhet.Where(x => x.OrganisationsId == orgId).ToList();
            return orgUnits;
        }

        public IEnumerable<AdmUppgiftsskyldighet> GetReportObligationInformationForOrg(int orgId)
        {
            var reportObligationInfo = DbContext.AdmUppgiftsskyldighet.Where(x => x.OrganisationId == orgId).Include(x => x.AdmDelregister).ToList();
            return reportObligationInfo;
        }

        public AdmUppgiftsskyldighet GetReportObligationInformationForOrgAndSubDir(int orgId, int subdirId)
        {
            var reportObligation = DbContext.AdmUppgiftsskyldighet.SingleOrDefault(x => x.OrganisationId == orgId && x.DelregisterId == subdirId);
            return reportObligation;
        }

        public IEnumerable<AdmEnhetsUppgiftsskyldighet> GetUnitReportObligationInformationForOrgUnit(int orgUnitId)
        {
            var unitReportObligationInfo = DbContext.AdmEnhetsUppgiftsskyldighet.Where(x => x.OrganisationsenhetsId == orgUnitId).ToList();
            return unitReportObligationInfo;

        }

        public AdmEnhetsUppgiftsskyldighet GetUnitReportObligationForReportObligationAndOrg(int oblId, int orgunitId)
        {
            var unitReportObigation = DbContext.AdmEnhetsUppgiftsskyldighet.SingleOrDefault(x => x.UppgiftsskyldighetId == oblId && x.OrganisationsenhetsId == orgunitId);
            return unitReportObigation;
        }

        public string GetKommunkodForOrg(int orgId)
        {
            var kommunkod = DbContext.Organisation.Where(x => x.Id == orgId).Select(x => x.Kommunkod).SingleOrDefault();
            return kommunkod;
        }

        public IEnumerable<AdmFAQKategori> GetFAQCategories()
        {
            //var faqCats = DbContext.AdmFAQKategori.Include(x => x.AdmFAQ).ToList();
            var faqCats = DbContext.AdmFAQKategori.OrderBy(x => x.Sortering).ToList();
            return faqCats;
        }

        public IEnumerable<AdmFAQ> GetFAQs(int faqCatId)
        {
            var faqs = DbContext.AdmFAQ.Where(x => x.FAQkategoriId == faqCatId).OrderBy(x => x.Sortering).ToList();
            return faqs;
        }

        public IEnumerable<AdmHelgdag> GetAllHolidays()
        {
            var holidays = DbContext.AdmHelgdag.ToList();
            return holidays;
        }

        public IEnumerable<AdmSpecialdag> GetAllSpecialDays()
        {
            var specialDays = DbContext.AdmSpecialdag.ToList();
            return specialDays;
        }

        public IEnumerable<AdmInformation> GetInformationTexts()
        {
            var infoTexts = DbContext.AdmInformation.ToList();
            return infoTexts;
        }

        public IEnumerable<AdmKonfiguration> GetAdmConfiguration()
        {
            var configInfo = DbContext.AdmKonfiguration.ToList();
            return configInfo;
        }

        public AdmInformation GetInfoText(string infoType)
        {
            var infoText = DbContext.AdmInformation.SingleOrDefault(x => x.Informationstyp == infoType);
            return infoText;
        }

        public AdmInformation GetInfoText(int infoId)
        {
            var infoText = DbContext.AdmInformation.SingleOrDefault(x => x.Id == infoId);
            return infoText;
        }

        public int GetPageInfoTextId(string pageType)
        {
            var pageInfoId = DbContext.AdmInformation.Where(x => x.Informationstyp == pageType).Select(x => x.Id).SingleOrDefault();
            return pageInfoId;
        }

        public AdmInsamlingsfrekvens GetInsamlingsfrekvens(int insamlingsid)
        {
            var insamlingsfrekvens = DbContext.AdmInsamlingsfrekvens.SingleOrDefault(x => x.Id == insamlingsid);
            return insamlingsfrekvens;
        }

        public IEnumerable<AdmRegister> GetDirectories()
        {
            var registers = DbContext.AdmRegister.ToList();
            return registers;
        }

        public AdmRegister GetDirectoryByShortName(string shortName)
        {
            var register = DbContext.AdmRegister.SingleOrDefault(x => x.Kortnamn == shortName);
            return register;
        }

        public AdmRegister GetDirectoryById(int dirId)
        {
            var register = DbContext.AdmRegister.SingleOrDefault(x => x.Id == dirId);
            return register;
        }

        public AdmDelregister GetSubDirectoryById(int subdirId)
        {
            var delregister = DbContext.AdmDelregister.SingleOrDefault(x => x.Id == subdirId);
            return delregister;
        }

        public IEnumerable<AdmDelregister> GetSubDirectories()
        {
            var subDirectories = DbContext.AdmDelregister.ToList();
            return subDirectories;
        }

        public IEnumerable<AdmDelregister> GetSubDirectoriesForDirectory(int dirId)
        {
            var subDirectories = DbContext.AdmDelregister.Where(x => x.RegisterId == dirId).ToList();
            return subDirectories;
        }


        public AdmDelregister GetSubDirectoryByShortName(string shortName)
        {
            var subDirectory = DbContext.AdmDelregister.Where(x => x.Kortnamn == shortName).FirstOrDefault();
            return subDirectory;
        }

        public IEnumerable<AdmForvantadleverans> GetExpectedDeliveries()
        {
            var expDeliveries = DbContext.AdmForvantadleverans.OrderBy(x => x.Uppgiftsstart).ToList();
            return expDeliveries;
        }

        public string GetDirectoryShortName(int dirId)
        {
            var dirShortName = DbContext.AdmRegister.Where(x => x.Id == dirId).Select(x => x.Kortnamn).SingleOrDefault();
            return dirShortName;
        }

        public IEnumerable<AdmForvantadfil> GetAllExpectedFiles()
        {
            var expFiles = DbContext.AdmForvantadfil.ToList();
            return expFiles;
        }

        public IEnumerable<AdmFilkrav> GetAllFileRequirements()
        {
            var fileReqs = DbContext.AdmFilkrav.ToList();
            return fileReqs;
        }

        public IEnumerable<AdmInsamlingsfrekvens> GetAllCollectionFrequencies()
        {
            var collFreq = DbContext.AdmInsamlingsfrekvens.ToList();
            return collFreq;
        }

        public string GetSubDirectoryShortNameForExpectedFile(int filkravId)
        {
            var subDirId = DbContext.AdmFilkrav.Where(x => x.Id == filkravId).Select(x => x.DelregisterId).SingleOrDefault();
            var subDirShortName = DbContext.AdmDelregister.Where(x => x.Id == subDirId).Select(x => x.Kortnamn).SingleOrDefault();
            return subDirShortName;
        }

        public string GetSubDirectoryShortName(int subDirId)
        {
            var subDirShortName = DbContext.AdmDelregister.Where(x => x.Id == subDirId).Select(x => x.Kortnamn).SingleOrDefault();
            return subDirShortName;
        }

        public string GetFileRequirementName(int fileReqId)
        {
            var fileReqName = DbContext.AdmFilkrav.Where(x => x.Id == fileReqId).Select(x => x.Namn).SingleOrDefault();
            return fileReqName;
        }

        public IEnumerable<AdmForvantadfil> GetExpectedFilesForDirectory(int dirId)
        {
            var expectedFileList = new List<AdmForvantadfil>();
            var regulationsForDirectory = DbContext.AdmForeskrift.Where(x => x.RegisterId == dirId).ToList();
            foreach (var regulation in regulationsForDirectory)
            {
                var expectedFileListForRegulation = DbContext.AdmForvantadfil.Where(x => x.ForeskriftsId == regulation.Id).ToList();
                expectedFileList.AddRange(expectedFileListForRegulation);
            }
            return expectedFileList;
        }

        public IEnumerable<AdmForvantadleverans> GetExpectedDeliveriesForDirectory(int dirId)
        {
            var expectedDeliveriesList = new List<AdmForvantadleverans>();
            var subDirectoriesForDirectory = DbContext.AdmDelregister.Where(x => x.RegisterId == dirId).ToList();
            foreach (var subDir in subDirectoriesForDirectory)
            {
                var expectedDeliveryList = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDir.Id).OrderBy(x => x.Uppgiftsstart).ToList();
                expectedDeliveriesList.AddRange(expectedDeliveryList);
            }
            return expectedDeliveriesList;
        }

        public IEnumerable<AdmForvantadleverans> GetExpectedDeliveriesForSubDirectory(int subdirId)
        {
            var expectedDeliveriesList = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subdirId).OrderBy(x => x.Uppgiftsstart).ToList();
            return expectedDeliveriesList;
        }

        public IEnumerable<AdmFilkrav> GetFileRequirementsForDirectory(int dirId)
        {
            var fileRequirementsList = new List<AdmFilkrav>();
            var subDirectoriesForDirectory = DbContext.AdmDelregister.Where(x => x.RegisterId == dirId).ToList();
            foreach (var subDir in subDirectoriesForDirectory)
            {
                var fileRequirementList = DbContext.AdmFilkrav.Where(x => x.DelregisterId == subDir.Id).ToList();
                fileRequirementsList.AddRange(fileRequirementList);
            }
            return fileRequirementsList;
        }

        public IEnumerable<AdmFilkrav> GetFileRequirementsForSubDirectory(int subdirId)
        {
            var fileRequirementList = DbContext.AdmFilkrav.Where(x => x.DelregisterId == subdirId).ToList();
            return fileRequirementList;
        }

        public AdmFilkrav GetFileRequirementsForSubDirectoryAndFileReqId(int subdirId, int filereqId)
        {
            var fileRequirement = DbContext.AdmFilkrav.SingleOrDefault(x => x.DelregisterId == subdirId && x.Id == filereqId);
            return fileRequirement;
        }

        public IEnumerable<AdmRegister> GetAllRegisters()
        {
            var registersList = DbContext.AdmRegister.ToList();
            return registersList;
        }

        public IEnumerable<Organisation> GetAllOrganisations()
        {
            var orgList = DbContext.Organisation.ToList();
            return orgList;
        }

        public IEnumerable<AdmRegister> GetAllRegistersForPortal()
        {
            var registersList = DbContext.AdmRegister.Where(x => x.Inrapporteringsportal).ToList();
            return registersList;
        }

        public IEnumerable<AdmDelregister> GetAllSubDirectoriesForPortal()
        {
            var delregistersList = DbContext.AdmDelregister.Where(x => x.Inrapporteringsportal).ToList();
            return delregistersList;
        }

        public AdmFAQKategori GetFAQCategory(int faqCatId)
        {
            var faqCat = DbContext.AdmFAQKategori.SingleOrDefault(x => x.Id == faqCatId);
            return faqCat;
        }

        public AdmFAQ GetFAQ(int faqId)
        {
            var faq = DbContext.AdmFAQ.SingleOrDefault(x => x.Id == faqId);
            return faq;
        }

        public AdmForeskrift GetForeskriftByFileReq(int fileReqId)
        {
            var filkrav = DbContext.AdmFilkrav.FirstOrDefault(x => x.Id == fileReqId);
            var foreskrift = DbContext.AdmForeskrift.FirstOrDefault(x => x.Id == filkrav.ForeskriftsId);
            return foreskrift;
        }

        public IEnumerable<Rapporteringsresultat> GetReportResultForSubdirAndPeriod(int delRegId, string period)
        {
            var repResults = DbContext.RapporteringsResultat.Where(x => x.DelregisterId == delRegId && x.Period == period).ToList();
            return repResults;
        }

        public IEnumerable<Rapporteringsresultat> GetReportResultForDirAndPeriod(int regId, string period)
        {
            var repResults = DbContext.RapporteringsResultat.Where(x => x.RegisterId == regId && x.Period == period).ToList();
            return repResults;
        }

        public IEnumerable<string> GetSubDirectoysPeriodsForAYear(int subdirId, int year)
        {
            var dateFrom = new DateTime(year, 01, 01);
            var dateTom = new DateTime(year, 12, 31).Date;
            var periods = DbContext.AdmForvantadleverans
                .Where(x => x.DelregisterId == subdirId && x.Uppgiftsstart >= dateFrom && x.Uppgiftsslut <= dateTom)
                .Select(x => x.Period).ToList();

            return periods;
        }

        public List<DateTime> GetTaskStartForSubdir(int subdirId)
        {
            var taskstartList = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subdirId)
                .Select(x => x.Uppgiftsstart).ToList();

            return taskstartList;
        }

        //TODO - special för EKB-År, Lös på annat sätt. Db?
        public DateTime GetReportstartForRegisterAndPeriodSpecial(int dirId, string period)
        {
            var subDir = DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId && x.Kortnamn == "EKB-År");
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDir.Id && x.Period == period)
                .Select(x => x.Rapporteringsstart).FirstOrDefault();

            return reportstart;
        }
        public DateTime GetReportstartForRegisterAndPeriod(int dirId, string period)
        {
            var firstSubDirForReg = DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId);
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == firstSubDirForReg.Id && x.Period == period)
                .Select(x => x.Rapporteringsstart).FirstOrDefault();

            return reportstart;
        }

        public DateTime GetLatestReportDateForRegisterAndPeriod(int dirId, string period)
        {
            var firstSubDirForReg = DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId);
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == firstSubDirForReg.Id && x.Period == period)
                .Select(x => x.Rapporteringsenast).FirstOrDefault();

            return reportstart;
        }

        public DateTime GetLatestReportDateForRegisterAndPeriodSpecial(int dirId, string period)
        {
            var subDir = DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId && x.Kortnamn == "EKB-År");
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDir.Id && x.Period == period)
                .Select(x => x.Rapporteringsenast).FirstOrDefault();

            return reportstart;
        }

        public IEnumerable<AdmDelregister> GetSubdirsForDirectory(int dirId)
        {
            var subdirectories = DbContext.AdmDelregister.Where(x => x.RegisterId == dirId).ToList();
            return subdirectories;
        }

        public int GetExpextedDeliveryIdForSubDirAndPeriod(int subDirId, string period)
        {
            var forvLevId = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDirId && x.Period == period)
                .Select(x => x.Id).SingleOrDefault();
            return forvLevId;
        }

        public AdmForvantadleverans GetExpectedDeliveryBySubDirAndFileReqIdAndPeriod(int subDirId, int fileReqId, string period)
        {
            var forvLev = DbContext.AdmForvantadleverans.SingleOrDefault(x => x.DelregisterId == subDirId && x.Period == period && x.FilkravId == fileReqId);
            return forvLev;
        }

        public Leverans GetLatestDeliveryForOrganisationSubDirectoryAndPeriod(int orgId, int subdirId, int forvlevId)
        {
            var latestsDeliveryForOrgAndSubdirectory = AllaLeveranser()
                .Where(a => a.OrganisationId == orgId && a.DelregisterId == subdirId &&
                            a.ForvantadleveransId == forvlevId).OrderByDescending(x => x.Id).FirstOrDefault();
            return latestsDeliveryForOrgAndSubdirectory;
        }

        public Leverans GetLatestDeliveryForOrganisationSubDirectoryPeriodAndOrgUnit(int orgId, int subdirId, int forvlevId,int orgUnitId)
        {
            var latestsDeliveryForOrgAndSubdirectory = AllaLeveranser()
                .Where(a => a.OrganisationId == orgId && a.DelregisterId == subdirId &&
                            a.ForvantadleveransId == forvlevId && a.OrganisationsenhetsId == orgUnitId).OrderByDescending(x => x.Id).FirstOrDefault();
            return latestsDeliveryForOrgAndSubdirectory;
        }

        public string GetUserEmail(string userId)
        {
            var email = IdentityDbContext.Users.Where(x => x.Id == userId).Select(x => x.Email).SingleOrDefault();
            return email;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            var user = DbContext.ApplicationUser.SingleOrDefault(x => x.Email == email);
            return user;
        }

        public IEnumerable<Roll> GetChosenDelRegistersForUser(string userId)
        {
            var rollList = new List<Roll>();

            rollList = DbContext.Roll.Where(x => x.ApplicationUserId == userId).ToList();

            return rollList;
        }

        public IEnumerable<RegisterInfo> GetAllRegisterInformationForOrganisation(int orgId)
        {
            var registerInfoList = new List<RegisterInfo>();

            var uppgSkyldighetDelRegIds = DbContext.AdmUppgiftsskyldighet.Where(x => x.OrganisationId == orgId).Select(x => x.DelregisterId).ToList();

            var delregister = DbContext.AdmDelregister
                .Include(z => z.AdmRegister)
                .Include(f => f.AdmFilkrav.Select(q => q.AdmForvantadfil))
                .Where(x => x.Inrapporteringsportal && uppgSkyldighetDelRegIds.Contains(x.Id))
                .Include(f => f.AdmFilkrav.Select(q => q.AdmForvantadleverans))
                .ToList();


            foreach (var item in delregister)
            {
                var regInfoObj = CreateRegisterInfoObj(item);
                registerInfoList.Add(regInfoObj);
            }

            return registerInfoList;
        }

        public AdmUppgiftsskyldighet GetUppgiftsskyldighetForOrganisationAndRegister(int orgId, int delregid)
        {
            var uppgiftsskyldighet = DbContext.AdmUppgiftsskyldighet.SingleOrDefault(x => x.OrganisationId == orgId && x.DelregisterId == delregid);

            return uppgiftsskyldighet;
        }


        public IEnumerable<Organisationsenhet> GetOrganisationUnits(int orgId)
        {
            var orgUnits = DbContext.Organisationsenhet.Where(x => x.OrganisationsId == orgId).ToList();
            return orgUnits;
        }


        public int CreateOrganisation(Organisation org)
        {
            DbContext.Organisation.Add(org);
            DbContext.SaveChanges();
            return org.Id;
        }
        public void CreateOrgUnit(Organisationsenhet orgUnit)
        {
            DbContext.Organisationsenhet.Add(orgUnit);
            DbContext.SaveChanges();
        }

        public void CreateFAQCategory(AdmFAQKategori faqCategory)
        {
            DbContext.AdmFAQKategori.Add(faqCategory);
            DbContext.SaveChanges();
        }

        public void CreateFAQ(AdmFAQ faq)
        {
            DbContext.AdmFAQ.Add(faq);
            DbContext.SaveChanges();
        }

        public void CreateHoliday(AdmHelgdag holiday)
        {
            DbContext.AdmHelgdag.Add(holiday);
            DbContext.SaveChanges();
        }

        public void CreateSpecialDay(AdmSpecialdag specialDay)
        {
            DbContext.AdmSpecialdag.Add(specialDay);
            DbContext.SaveChanges();
        }
        public void CreateInformationText(AdmInformation infoText)
        {
            DbContext.AdmInformation.Add(infoText);
            DbContext.SaveChanges();
        }

        public void CreateReportObligation(AdmUppgiftsskyldighet uppgSk)
        {
            DbContext.AdmUppgiftsskyldighet.Add(uppgSk);
            DbContext.SaveChanges();
        }

        public void CreateUnitReportObligation(AdmEnhetsUppgiftsskyldighet enhetsUppgSk)
        {
            DbContext.AdmEnhetsUppgiftsskyldighet.Add(enhetsUppgSk);
            DbContext.SaveChanges();
        }

        public void CreateDirectory(AdmRegister dir)
        {
            DbContext.AdmRegister.Add(dir);
            DbContext.SaveChanges();
        }

        public void CreateSubDirectory(AdmDelregister subDir)
        {
            DbContext.AdmDelregister.Add(subDir);
            DbContext.SaveChanges();
        }

        public void CreateExpectedDelivery(AdmForvantadleverans forvLev)
        {
            DbContext.AdmForvantadleverans.Add(forvLev);
            DbContext.SaveChanges();
        }

        public void CreateExpectedFile(AdmForvantadfil forvFil)
        {
            DbContext.AdmForvantadfil.Add(forvFil);
            DbContext.SaveChanges();
        }

        public void CreateFileRequirement(AdmFilkrav filkrav)
        {
            DbContext.AdmFilkrav.Add(filkrav);
            DbContext.SaveChanges();
        }

        public void CreateCollectFrequence(AdmInsamlingsfrekvens colFreq)
        {
            DbContext.AdmInsamlingsfrekvens.Add(colFreq);
            DbContext.SaveChanges();
        }

        public void UpdateOrganisation(Organisation org)
        {
            var orgDb = DbContext.Organisation.Where(u => u.Id == org.Id).Select(u => u).SingleOrDefault();
            orgDb.Landstingskod = org.Landstingskod;
            orgDb.Kommunkod = org.Kommunkod;
            orgDb.Inrapporteringskod = org.Inrapporteringskod;
            orgDb.Organisationstyp = org.Organisationstyp;
            orgDb.Organisationsnr = org.Organisationsnr;
            orgDb.Organisationsnamn = org.Organisationsnamn;
            orgDb.Hemsida = org.Hemsida;
            orgDb.EpostAdress = org.EpostAdress;
            orgDb.Telefonnr = org.Telefonnr;
            orgDb.Postnr = org.Postnr;
            orgDb.Postort = org.Postort;
            orgDb.Adress = org.Adress;
            orgDb.Epostdoman = org.Epostdoman;
            orgDb.AktivFrom = org.AktivFrom;
            orgDb.AktivTom = org.AktivTom;
            orgDb.AndradDatum = org.AndradDatum;
            orgDb.AndradAv = org.AndradAv;

            DbContext.SaveChanges();
        }

        public void UpdateAdminUser(AppUserAdmin user)
        {
            var usrDb = IdentityDbContext.Users.Where(u => u.Id == user.Id).Select(u => u).SingleOrDefault();
            usrDb.PhoneNumber= user.PhoneNumber;
            usrDb.Email = user.Email;
            usrDb.AndradDatum = user.AndradDatum;
            usrDb.AndradAv = user.AndradAv;
            IdentityDbContext.SaveChanges(); 
        }

        public void UpdateContactPerson(ApplicationUser user)
        {
            var usrDb = DbContext.ApplicationUser.Where(u => u.Id == user.Id).Select(u => u).SingleOrDefault();
            usrDb.PhoneNumber = user.PhoneNumber;
            usrDb.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            usrDb.AktivFrom = user.AktivFrom;
            usrDb.AktivTom = user.AktivTom;
            usrDb.AndradDatum = user.AndradDatum;
            usrDb.AndradAv = user.AndradAv;
            DbContext.SaveChanges();
        }

        public void UpdateOrgUnit(Organisationsenhet orgUnit)
        {
            var orgU = DbContext.Organisationsenhet.Where(u => u.Id == orgUnit.Id).Select(u => u).SingleOrDefault();
            orgU.Enhetsnamn = orgUnit.Enhetsnamn;
            orgU.Enhetskod = orgUnit.Enhetskod;
            orgU.AktivFrom = orgUnit.AktivFrom;
            orgU.AktivTom = orgUnit.AktivTom;
            orgU.AndradDatum = orgUnit.AndradDatum;
            orgU.AndradAv = orgUnit.AndradAv;
            DbContext.SaveChanges(); 
        }

        public void UpdateReportObligation(AdmUppgiftsskyldighet repObligation)
        {
            var repObl = DbContext.AdmUppgiftsskyldighet.Where(u => u.Id == repObligation.Id).Select(u => u).SingleOrDefault();
            repObl.DelregisterId = repObligation.DelregisterId;
            repObl.RapporterarPerEnhet = repObligation.RapporterarPerEnhet;
            repObl.SkyldigFrom = repObligation.SkyldigFrom;
            repObl.SkyldigTom = repObligation.SkyldigTom;
            repObl.AndradDatum = repObligation.AndradDatum;
            repObl.AndradAv = repObligation.AndradAv;

            DbContext.SaveChanges();
        }

        public void UpdateUnitReportObligation(AdmEnhetsUppgiftsskyldighet unitRepObligation)
        {
            var unitRepOblDb = DbContext.AdmEnhetsUppgiftsskyldighet.Where(u => u.Id == unitRepObligation.Id).Select(u => u).SingleOrDefault();
            unitRepOblDb.UppgiftsskyldighetId = unitRepObligation.UppgiftsskyldighetId;
            unitRepOblDb.SkyldigFrom = unitRepObligation.SkyldigFrom;
            unitRepOblDb.SkyldigTom = unitRepObligation.SkyldigTom;
            unitRepOblDb.AndradDatum = unitRepObligation.AndradDatum;
            unitRepOblDb.AndradAv = unitRepObligation.AndradAv;

            DbContext.SaveChanges();
        }

        public void UpdateFAQCategory(AdmFAQKategori faqCategory)
        {
            var faqCatDb = DbContext.AdmFAQKategori.Where(x => x.Id == faqCategory.Id).Select(x => x).SingleOrDefault();
            faqCatDb.Kategori = faqCategory.Kategori;
            faqCatDb.Sortering = faqCategory.Sortering;
            faqCatDb.AndradDatum = faqCategory.AndradDatum;
            faqCatDb.AndradAv = faqCategory.AndradAv;

            DbContext.SaveChanges();
        }

        public void UpdateFAQ(AdmFAQ faq)
        {
            var faqDb = DbContext.AdmFAQ.Where(x => x.Id == faq.Id).Select(x => x).SingleOrDefault();
            faqDb.Fraga = faq.Fraga;
            faqDb.Svar = faq.Svar;
            faqDb.Sortering = faq.Sortering;
            faqDb.RegisterId = faq.RegisterId;
            faqDb.AndradDatum = faq.AndradDatum;
            faqDb.AndradAv = faq.AndradAv;

            DbContext.SaveChanges();
        }

        public void UpdateHoliday(AdmHelgdag holiday)
        {
            var holidayDb = DbContext.AdmHelgdag.Where(x => x.Id == holiday.Id).Select(x => x).SingleOrDefault();
            holidayDb.Helgdatum = holiday.Helgdatum;
            holidayDb.Helgdag = holiday.Helgdag;
            holidayDb.AndradDatum = holiday.AndradDatum;
            holidayDb.AndradAv = holiday.AndradAv;

            DbContext.SaveChanges();
        }

        public void UpdateSpecialDay(AdmSpecialdag specialDay)
        {
            var specialDayDb = DbContext.AdmSpecialdag.Where(x => x.Id == specialDay.Id).Select(x => x).SingleOrDefault();
            specialDayDb.Specialdagdatum = specialDay.Specialdagdatum;
            specialDayDb.Oppna = specialDay.Oppna;
            specialDayDb.Stang = specialDay.Stang;
            specialDayDb.Anledning = specialDay.Anledning;
            specialDayDb.AndradAv = specialDay.AndradAv;
            specialDayDb.AndradDatum = specialDay.AndradDatum;
            DbContext.SaveChanges();
        }

        public void UpdateInfoText(AdmInformation infoText)
        {
            var infoTextDb = DbContext.AdmInformation.Where(x => x.Id == infoText.Id).Select(x => x).SingleOrDefault();
            infoTextDb.Text = infoText.Text;
            infoTextDb.AndradAv = infoText.AndradAv;
            infoTextDb.AndradDatum = infoText.AndradDatum;

            DbContext.SaveChanges();
        }

        public void UpdateDirectory(AdmRegister directory)
        {
            var registerToUpdate = DbContext.AdmRegister.Where(x => x.Id == directory.Id).SingleOrDefault();
            registerToUpdate.Registernamn = directory.Registernamn;
            registerToUpdate.Beskrivning = directory.Beskrivning;
            registerToUpdate.Kortnamn = directory.Kortnamn;
            registerToUpdate.Inrapporteringsportal = directory.Inrapporteringsportal;
            registerToUpdate.AndradAv = directory.AndradAv;
            registerToUpdate.AndradDatum = directory.AndradDatum;

            DbContext.SaveChanges();
        }

        public void UpdateSubDirectory(AdmDelregister subDirectory)
        {
            var subDirectoryToUpdate = DbContext.AdmDelregister.SingleOrDefault(x => x.Id == subDirectory.Id);
            subDirectoryToUpdate.Delregisternamn = subDirectory.Delregisternamn;
            subDirectoryToUpdate.Beskrivning = subDirectory.Beskrivning;
            subDirectoryToUpdate.Kortnamn = subDirectory.Kortnamn;
            subDirectoryToUpdate.Inrapporteringsportal = subDirectory.Inrapporteringsportal;
            subDirectoryToUpdate.Slussmapp = subDirectory.Slussmapp;
            subDirectoryToUpdate.AndradAv = subDirectory.AndradAv;
            subDirectoryToUpdate.AndradDatum = subDirectory.AndradDatum;

            DbContext.SaveChanges();
        }

        public void UpdateExpectedDelivery(AdmForvantadleverans forvLev)
        {
            var forvLevToUpdate = DbContext.AdmForvantadleverans.SingleOrDefault(x => x.Id == forvLev.Id);
            forvLevToUpdate.DelregisterId = forvLev.DelregisterId;
            forvLevToUpdate.FilkravId = forvLev.FilkravId;
            forvLevToUpdate.Period = forvLev.Period;
            forvLevToUpdate.Uppgiftsstart = forvLev.Uppgiftsstart;
            forvLevToUpdate.Uppgiftsslut = forvLev.Uppgiftsslut;
            forvLevToUpdate.Rapporteringsstart = forvLev.Rapporteringsstart;
            forvLevToUpdate.Rapporteringsslut = forvLev.Rapporteringsslut;
            forvLevToUpdate.Rapporteringsenast = forvLev.Rapporteringsenast;
            forvLevToUpdate.Paminnelse1 = forvLev.Paminnelse1;
            forvLevToUpdate.Paminnelse2 = forvLev.Paminnelse2;
            forvLevToUpdate.Paminnelse3 = forvLev.Paminnelse3;
            forvLevToUpdate.AndradAv = forvLev.AndradAv;
            forvLevToUpdate.AndradDatum = forvLev.AndradDatum;

            DbContext.SaveChanges();
        }


        public void UpdateExpectedFile(AdmForvantadfil forvFil)
        {
            var forvFileToUpdate = DbContext.AdmForvantadfil.SingleOrDefault(x => x.Id == forvFil.Id);
            forvFileToUpdate.Filmask = forvFil.Filmask;
            forvFileToUpdate.Regexp = forvFil.Regexp;
            forvFileToUpdate.Obligatorisk = forvFil.Obligatorisk;
            forvFileToUpdate.Tom= forvFil.Tom;
            forvFileToUpdate.AndradAv = forvFil.AndradAv;
            forvFileToUpdate.AndradDatum = forvFil.AndradDatum;

            DbContext.SaveChanges();
        }

        public void UpdateFileRequirement(AdmFilkrav filkrav)
        {
            var filereqToUpdate = DbContext.AdmFilkrav.SingleOrDefault(x => x.Id == filkrav.Id);
            filereqToUpdate.Namn = filkrav.Namn;
            filereqToUpdate.InsamlingsfrekvensId = filkrav.InsamlingsfrekvensId;
            filereqToUpdate.Uppgiftsstartdag = filkrav.Uppgiftsstartdag;
            filereqToUpdate.Uppgiftslutdag = filkrav.Uppgiftslutdag;
            filereqToUpdate.Rapporteringsstartdag = filkrav.Rapporteringsstartdag;
            filereqToUpdate.Rapporteringsslutdag = filkrav.Rapporteringsslutdag;
            filereqToUpdate.RapporteringSenastdag = filkrav.RapporteringSenastdag;
            filereqToUpdate.Paminnelse1dag = filkrav.Paminnelse1dag;
            filereqToUpdate.Paminnelse2dag = filkrav.Paminnelse2dag;
            filereqToUpdate.Paminnelse3dag = filkrav.Paminnelse3dag;
            filereqToUpdate.RapporteringEfterAntalManader = filkrav.RapporteringEfterAntalManader;
            filereqToUpdate.UppgifterAntalmanader = filkrav.UppgifterAntalmanader;
            filereqToUpdate.AndradAv = filkrav.AndradAv;
            filereqToUpdate.AndradDatum = filkrav.AndradDatum;

            DbContext.SaveChanges();
        }

        public void UpdateCollectFrequency(AdmInsamlingsfrekvens insamlingsfrekvens)
        {
            var collFreqToUpdate = DbContext.AdmInsamlingsfrekvens.SingleOrDefault(x => x.Id == insamlingsfrekvens.Id);
            collFreqToUpdate.Insamlingsfrekvens = insamlingsfrekvens.Insamlingsfrekvens;
            DbContext.SaveChanges();
        }

        public void UpdateUserInfo(AppUserAdmin user)
        {
            var userDb = IdentityDbContext.Users.SingleOrDefault(x => x.Id == user.Id);
            userDb.AndradAv = user.AndradAv;
            userDb.AndradDatum = user.AndradDatum;
            IdentityDbContext.SaveChanges();
        }

        public void SaveOpeningHours(AdmKonfiguration admKonf)
        {
            var konfDb = DbContext.AdmKonfiguration.Where(x => x.Typ == admKonf.Typ).Select(x => x).FirstOrDefault();
            konfDb.Varde = admKonf.Varde;
            konfDb.AndradAv = admKonf.AndradAv;
            konfDb.AndradDatum = admKonf.AndradDatum;

            DbContext.SaveChanges();
        }

        public void DeleteFAQCategory(int faqCategoryId)
        {
            var faqCatToDelete = DbContext.AdmFAQKategori.SingleOrDefault(x => x.Id == faqCategoryId);

            //Delete all children
            if (faqCatToDelete != null)
            {
                var faqsToDelete = DbContext.AdmFAQ.Where(x => x.FAQkategoriId == faqCategoryId).ToList();
                foreach (var faq in faqsToDelete)
                {
                    DbContext.AdmFAQ.Remove(faq);
                }

                //Delete category
                DbContext.AdmFAQKategori.Remove(faqCatToDelete);
                DbContext.SaveChanges();
            }
        }

        public void DeleteFAQ(int faqId)
        {
            var faqToDelete = DbContext.AdmFAQ.SingleOrDefault(x => x.Id == faqId);
            if (faqToDelete != null)
            {
                DbContext.AdmFAQ.Remove(faqToDelete);
                DbContext.SaveChanges();
            }
        }

        public void DeleteHoliday(int holidayId)
        {
            var holidayToDelete = DbContext.AdmHelgdag.SingleOrDefault(x => x.Id == holidayId);
            if (holidayToDelete != null)
            {
                DbContext.AdmHelgdag.Remove(holidayToDelete);
                DbContext.SaveChanges();
            }
        }

        public void DeleteSpecialDay(int specialDayId)
        {
            var specialDayToDelete = DbContext.AdmSpecialdag.SingleOrDefault(x => x.Id == specialDayId);
            if (specialDayToDelete != null)
            {
                DbContext.AdmSpecialdag.Remove(specialDayToDelete);
                DbContext.SaveChanges();
            }
        }

        public void DeleteContact(string contactId)
        {
            var contactToDelete = DbContext.ApplicationUser.SingleOrDefault(x => x.Id == contactId);

            var contactRoles = DbContext.Roll.Where(x => x.ApplicationUserId == contactId).ToList();

 
            if (contactToDelete != null)
            {
                foreach (var role in contactRoles)
                {
                    DbContext.Roll.Remove(role);
                    DbContext.SaveChanges();
                }
                DbContext.ApplicationUser.Remove(contactToDelete);
                DbContext.SaveChanges();
            }
        }

        public void DeleteAdminUser(string userId)
        {
            var cuserToDelete = IdentityDbContext.Users.SingleOrDefault(x => x.Id == userId);
            IdentityDbContext.Users.Remove(cuserToDelete);
            IdentityDbContext.SaveChanges();
        }




        private RegisterInfo CreateRegisterInfoObj(AdmDelregister delReg)
        {
            var regInfo = new RegisterInfo
            {
                Id = delReg.Id,
                Namn = delReg.Delregisternamn,
                Kortnamn = delReg.Kortnamn,
                InfoText = delReg.AdmRegister.Beskrivning + "<br>" + delReg.Beskrivning,
                Slussmapp = delReg.Slussmapp
            };


            var filkravList = new List<RegisterFilkrav>();
            var i = 1;

            foreach (var filkrav in delReg.AdmFilkrav)
            {
                var regFilkrav = new RegisterFilkrav();
                var filmaskList = new List<string>();
                var regExpList = new List<string>();
                if (filkrav.Namn != null)
                {
                    regFilkrav.Namn = filkrav.Namn;
                }
                else
                {
                    regFilkrav.Namn = "";
                }

                //Sök forväntad fil för varje filkrav istället för alla forvantade filer för registret!!
                //var forvantadFil = delReg.AdmFilkrav.Select(x => x.AdmForvantadfil);
                var forvantadeFiler = filkrav.AdmForvantadfil.ToList();
                regFilkrav.AntalFiler = forvantadeFiler.Count();

                foreach (var forvFil in forvantadeFiler)
                {
                    filmaskList.Add(forvFil.Filmask);
                    regExpList.Add(forvFil.Regexp);
                    regFilkrav.InfoText = regFilkrav.InfoText + "<br> Filformat: " + forvFil.Filmask;
                    regFilkrav.Obligatorisk = forvFil.Obligatorisk;
                }

                //get period och forvantadleveransId
                GetPeriodsForAktuellLeverans(filkrav, regFilkrav);
                regFilkrav.InfoText = regFilkrav.InfoText + "<br> Antal filer: " + regFilkrav.AntalFiler;
                regFilkrav.Id = i;
                regFilkrav.FilMasker = filmaskList;
                regFilkrav.RegExper = regExpList;


                //Om inga aktuella perioder finns för filkravet ska det inte läggas med i RegInfo
                if (regFilkrav.Perioder != null)
                {
                    if (regFilkrav.Perioder.ToList().Count != 0)
                    {
                        filkravList.Add(regFilkrav);
                        i++;
                    }
                }
            }

            regInfo.Filkrav = filkravList;
            return regInfo;
        }

        public void GetPeriodsForAktuellLeverans(AdmFilkrav filkrav, RegisterFilkrav regFilkrav)
        {
            string period = String.Empty;
            DateTime startDate;
            DateTime endDate;

            DateTime dagensDatum = DateTime.Now.Date;
            var perioder = new List<string>();

            //hämta varje förväntad leverans och sätt rätt period utifrån dagens datum
            foreach (var item in filkrav.AdmForvantadleverans)
            {
                if (item != null)
                {
                    startDate = item.Rapporteringsstart;
                    endDate = item.Rapporteringsslut;
                    if (dagensDatum >= startDate && dagensDatum <= endDate)
                    {
                        //regInfo.Period = item.Period;
                        perioder.Add(item.Period);
                        //regInfo.ForvantadLevransId = item.Id;
                    }
                }
                regFilkrav.Perioder = perioder;
            }
        }


    }
}
