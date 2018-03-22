using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.DataAccess;
using InrappAdmin.DomainModel;

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

        public string GetLeveranserForOrganisation(int orgId)
        {
            var levIdnForOrg = AllaLeveranser().Where(a => a.OrganisationId == orgId).ToList();

            return "Hejsan";
        }

        public Organisation GetOrganisationFromKommunkod(string kommunkod)
        {
            var org = DbContext.Organisation.Single(a => a.Kommunkod == kommunkod);
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
            var faqCats = DbContext.AdmFAQKategori.Include(x => x.AdmFAQ).ToList();
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
            var usr = DbContext.Users.Where(u => u.Id == user.Id).Select(u => u).SingleOrDefault();
            usr.Namn = user.Namn;
            DbContext.SaveChanges(); 
        }

        public void UpdateOrgUnit(Organisationsenhet orgUnit)
        {
            var orgU = DbContext.Organisationsenhet.Where(u => u.Id == orgUnit.Id).Select(u => u).SingleOrDefault();
            orgU.Enhetsnamn = orgUnit.Enhetsnamn;
            orgU.Enhetskod = orgUnit.Enhetskod;
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


    }
}
