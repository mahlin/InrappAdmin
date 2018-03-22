using InrappAdmin.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.ApplicationService.DTOModel;

namespace InrappAdmin.ApplicationService.Interface
{
    public interface IPortalAdminService
    {

        string HamtaHistorikForOrganisation(int orgId);

        Organisation HamtaOrganisation(int orgId);

        Organisation HamtaOrganisationForKommunkod(string kommunkod);
        string HamtaKommunkodForOrg(int orgId);
        Organisation HamtaOrgForAnvandare(string userId);
        Organisation HamtaOrgForOrganisationsenhet(int orgUnitId);

        Organisation HamtaOrgForUppgiftsskyldighet(int uppgSkId);

        IEnumerable<ApplicationUser> HamtaKontaktpersonerForOrg(int orgId);

        IEnumerable<Organisationsenhet> HamtaOrgEnheterForOrg(int orgId);

        IEnumerable<AdmUppgiftsskyldighet> HamtaUppgiftsskyldighetForOrg(int orgId);

        IEnumerable<AdmFAQKategori> HamtaFAQkategorier();

        IEnumerable<AdmFAQ> HamtaFAQs(int faqCatId);

        IEnumerable<AdmInformation> HamtaInformationstexter();

        OpeningHoursInfoDTO HamtaOppettider();

        string HamtaInfoText(string infoTyp);

        void SkapaOrganisationsenhet(Organisationsenhet orgUnit);

        void SkapaFAQKategori(AdmFAQKategori faqKategori);

        void SkapaFAQ(AdmFAQ faq);

        void SkapaInformationsText(AdmInformation infoText);

        void UppdateraOrganisation(Organisation org);

        void UppdateraKontaktperson(ApplicationUser user);

        void UppdateraOrganisationsenhet(Organisationsenhet orgUnit);

        void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet);

        void UppdateraFAQKategori(AdmFAQKategori faqKategori);

        void UppdateraFAQ(AdmFAQ faq);

        void UppdateraInformationstext(AdmInformation infoText);

        void SparaOppettider(OpeningHoursInfoDTO oppetTider);

        void TaBortFAQKategori(int faqKategoriId);

        void TaBortFAQ(int faqId);

        void TaBortKontaktperson(string contactId);

        List<OpeningDay> MarkeraStangdaDagar(List<string> closedDays);

    }
}
