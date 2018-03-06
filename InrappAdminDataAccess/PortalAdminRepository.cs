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

        public void CreateOrgUnit(Organisationsenhet orgUnit)
        {
            DbContext.Organisationsenhet.Add(orgUnit);

            DbContext.SaveChanges();
        }

        public string GetKommunkodForOrg(int orgId)
        {
            var kommunkod = DbContext.Organisation.Where(x => x.Id == orgId).Select(x => x.Kommunkod).SingleOrDefault();
            return kommunkod;
        }
    }
}
