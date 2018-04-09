using System;
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

        public PortalAdminRepository(InrappAdminDbContext dbContext)
        {
            DbContext = dbContext;
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
            var orgId = DbContext.Users.Where(u => u.Id == userId).Select(o => o.OrganisationId).SingleOrDefault();
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

        public IEnumerable<ApplicationUser> GetContactPersonsForOrg(int orgId)
        {
            var contacts = DbContext.Users.Where(x => x.OrganisationId == orgId).ToList();
            return contacts;
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

        public string GetKommunkodForOrg(int orgId)
        {
            var kommunkod = DbContext.Organisation.Where(x => x.Id == orgId).Select(x => x.Kommunkod).SingleOrDefault();
            return kommunkod;
        }

        public IEnumerable<AdmFAQKategori> GetFAQCategories()
        {
            //var faqCats = DbContext.AdmFAQKategori.Include(x => x.AdmFAQ).ToList();
            var faqCats = DbContext.AdmFAQKategori.ToList();
            return faqCats;
        }

        public IEnumerable<AdmFAQ> GetFAQs(int faqCatId)
        {
            var faqs = DbContext.AdmFAQ.Where(x => x.FAQkategoriId == faqCatId).ToList();
            return faqs;
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

        public int GetPageInfoTextId(string pageType)
        {
            var pageInfoId = DbContext.AdmInformation.Where(x => x.Informationstyp == pageType).Select(x => x.Id).SingleOrDefault();
            return pageInfoId;
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

        public IEnumerable<AdmForvantadleverans> GetExpectedDeliveries()
        {
            var expDeliveries = DbContext.AdmForvantadleverans.ToList();
            return expDeliveries;
        }

        public IEnumerable<AdmForvantadfil> GetAllExpectedFiles()
        {
            var expFiles = DbContext.AdmForvantadfil.ToList();
            return expFiles;
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
                var expectedDeliveryList = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDir.Id).ToList();
                expectedDeliveriesList.AddRange(expectedDeliveryList);
            }
            return expectedDeliveriesList;
        }

        public IEnumerable<AdmRegister> GetAllRegisters()
        {
            var registersList = DbContext.AdmRegister.ToList();
            return registersList;
        }

        public IEnumerable<AdmRegister> GetAllRegistersForPortal()
        {
            var registersList = DbContext.AdmRegister.Where(x => x.Inrapporteringsportal).ToList();
            return registersList;
        }

        public AdmFAQ GetFAQ(int faqId)
        {
            var faq = DbContext.AdmFAQ.SingleOrDefault(x => x.Id == faqId);
            return faq;
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
            orgDb.AndradDatum = DateTime.Now;
            orgDb.AndradAv = "InrappAdmin";

            DbContext.SaveChanges();
        }

        public void UpdateContactPerson(ApplicationUser user)
        {
            var usrDb = DbContext.Users.Where(u => u.Id == user.Id).Select(u => u).SingleOrDefault();
            usrDb.PhoneNumber= user.PhoneNumber;
            usrDb.AktivFrom = user.AktivFrom;
            usrDb.AktivTom = user.AktivTom;
            //Sätt datum och användare (kräver referens till Identity, därför här istf i svc(?))
            usrDb.AndradDatum = DateTime.Now;
            usrDb.AndradAv = "InrappAdmin";
            DbContext.SaveChanges(); 
        }

        public void UpdateOrgUnit(Organisationsenhet orgUnit)
        {
            var orgU = DbContext.Organisationsenhet.Where(u => u.Id == orgUnit.Id).Select(u => u).SingleOrDefault();
            orgU.Enhetsnamn = orgUnit.Enhetsnamn;
            orgU.Enhetskod = orgUnit.Enhetskod;
            orgU.AktivFrom = orgUnit.AktivFrom;
            orgU.AktivTom = orgUnit.AktivTom;
            DbContext.SaveChanges(); 
        }

        public void UpdateReportObligation(AdmUppgiftsskyldighet repObligation)
        {
            var repObl = DbContext.AdmUppgiftsskyldighet.Where(u => u.Id == repObligation.Id).Select(u => u).SingleOrDefault();
            repObl.DelregisterId = repObligation.DelregisterId;
            repObl.RapporterarPerEnhet = repObligation.RapporterarPerEnhet;
            repObl.SkyldigFrom = repObligation.SkyldigFrom;
            repObl.SkyldigTom = repObligation.SkyldigTom;
            DbContext.SaveChanges();
        }

        public void UpdateFAQCategory(AdmFAQKategori faqCategory)
        {
            var faqCatDb = DbContext.AdmFAQKategori.Where(x => x.Id == faqCategory.Id).Select(x => x).SingleOrDefault();
            faqCatDb.Kategori = faqCategory.Kategori;
            DbContext.SaveChanges();
        }

        public void UpdateFAQ(AdmFAQ faq)
        {
            var faqDb = DbContext.AdmFAQ.Where(x => x.Id == faq.Id).Select(x => x).SingleOrDefault();
            faqDb.Fraga = faq.Fraga;
            faqDb.Svar = faq.Svar;
            DbContext.SaveChanges();
        }

        public void UpdateInfoText(AdmInformation infoText)
        {
            var infoTextDb = DbContext.AdmInformation.Where(x => x.Id == infoText.Id).Select(x => x).SingleOrDefault();
            infoTextDb.Informationstyp = infoText.Informationstyp;
            infoTextDb.Text = infoText.Text;
            DbContext.SaveChanges();
        }

        public void UpdateDirectory(AdmRegister directory)
        {
            var registerToUpdate = DbContext.AdmRegister.Where(x => x.Id == directory.Id).SingleOrDefault();
            registerToUpdate.Registernamn = directory.Registernamn;
            registerToUpdate.Beskrivning = directory.Beskrivning;
            registerToUpdate.Kortnamn = directory.Kortnamn;
            registerToUpdate.Inrapporteringsportal = directory.Inrapporteringsportal;
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
            DbContext.SaveChanges();
        }


        public void UpdateExpectedFile(AdmForvantadfil forvFil)
        {
            var forvFileToUpdate = DbContext.AdmForvantadfil.SingleOrDefault(x => x.Id == forvFil.Id);
            forvFileToUpdate.FilkravId = forvFil.FilkravId;
            forvFileToUpdate.Filmask = forvFil.Filmask;
            forvFileToUpdate.Regexp = forvFil.Regexp;
            forvFileToUpdate.Obligatorisk = forvFil.Obligatorisk;
            forvFileToUpdate.Tom= forvFil.Tom;
            DbContext.SaveChanges();
        }

        public void SaveOpeningHours(AdmKonfiguration admKonf)
        {
            var konfDb = DbContext.AdmKonfiguration.Where(x => x.Typ == admKonf.Typ).Select(x => x).FirstOrDefault();
            konfDb.Varde = admKonf.Varde;
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

        public void DeleteContact(string contactId)
        {
            var contactToDelete = DbContext.Users.SingleOrDefault(x => x.Id == contactId);

            var contactRoles = DbContext.Roll.Where(x => x.ApplicationUserId == contactId).ToList();

 
            if (contactToDelete != null)
            {
                foreach (var role in contactRoles)
                {
                    DbContext.Roll.Remove(role);
                }
                DbContext.Users.Remove(contactToDelete);
                DbContext.SaveChanges();
            }
        }


    }
}
