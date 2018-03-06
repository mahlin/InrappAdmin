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

        IEnumerable<ApplicationUser> GetContactPersonsForOrg(int orgId);

        IEnumerable<Organisationsenhet> GetOrgUnitsForOrg(int orgId);

        IEnumerable<AdmUppgiftsskyldighet> GetReportObligationInformationForOrg(int orgId);

        void CreateOrgUnit(Organisationsenhet orgUnit);
    }
}
