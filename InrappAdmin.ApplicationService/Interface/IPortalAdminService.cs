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

        IEnumerable<AdmFAQKategori> HamtaFAQkategorier();

        IEnumerable<AdmInformation> HamtaInformationstexter();

        void SkapaOrganisationsenhet(Organisationsenhet orgUnit);

        void SkapaFAQKategori(AdmFAQKategori faqKategori);

        void SkapaInformationsText(AdmInformation infoText);

        void UppdateraOrganisation(Organisation org);

        void UppdateraKontaktperson(ApplicationUser user);

        void UppdateraOrganisationsenhet(Organisationsenhet orgUnit);

        void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet);

        void UppdateraFAQKategori(AdmFAQKategori faqKategori);

        void UppdateraInformationstext(AdmInformation infoText);

    }
}
