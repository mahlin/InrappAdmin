using InrappAdmin.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InrappAdmin.ApplicationService.Interface
{
    public interface IPortalAdminService
    {

        string HamtaHistorikForOrganisation(int orgId);

        Organisation HamtaOrganisationForKommunkod(string kommunkod);
        string HamtaKommunkodForOrg(int orgId);
        Organisation HamtaOrgForAnvandare(string userId);
        Organisation HamtaOrgForOrganisationsenhet(int orgUnitId);

        Organisation HamtaOrgForUppgiftsskyldighet(int uppgSkId);

        IEnumerable<ApplicationUser> HamtaKontaktpersonerForOrg(int orgId);

        IEnumerable<Organisationsenhet> HamtaOrgEnheterForOrg(int orgId);

        IEnumerable<AdmUppgiftsskyldighet> HamtaUppgiftsskyldighetForOrg(int orgId);

        void SkapaOrganisationsenhet(Organisationsenhet orgUnit);

        void UppdateraKontaktperson(ApplicationUser user);

        void UppdateraOrganisationsenhet(Organisationsenhet orgUnit);

        void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet);

    }
}
