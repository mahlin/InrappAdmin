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

        IEnumerable<AdmHelgdag> HamtaAllaHelgdagar();

        IEnumerable<AdmSpecialdag> HamtaAllaSpecialdagar();

        AdmFAQ HamtaFAQ(int faqId);

        IEnumerable<AdmInformation> HamtaInformationstexter();

        OpeningHoursInfoDTO HamtaOppettider();

        AdmInformation HamtaInfoText(string infoTyp);

        AdmInformation HamtaInfo(int infoId);

        IEnumerable<AdmRegister> HamtaRegister();

        AdmRegister HamtaRegisterMedKortnamn(string regKortNamn);

        AdmRegister HamtaRegisterMedId(int regId);

        IEnumerable<AdmDelregister> HamtaDelRegister();

        IEnumerable<AdmDelregister> HamtaDelRegisterForRegister(int regId);
        AdmDelregister HamtaDelRegisterForKortnamn(string shortName);

        IEnumerable<RegisterInfo> HamtaDelregisterOchFilkrav();
        IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranser();

        IEnumerable<AdmForvantadfil> HamtaAllaForvantadeFiler();

        IEnumerable<AdmFilkrav> HamtaAllaFilkrav();

        IEnumerable<AdmForvantadfil> HamtaForvantadeFilerForRegister(int regId);
        IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranserForRegister(int regId);

        IEnumerable<AdmFilkrav> HamtaFilkravForRegister(int regId);
        IEnumerable<AdmRegister> HamtaAllaRegister();
        IEnumerable<AdmRegister> HamtaAllaRegisterForPortalen();

        IEnumerable<AdmDelregister> HamtaAllaDelregisterForPortalen();

        string HamtaKortnamnForDelregisterMedFilkravsId(int filkravsId);

        string HamtaKortnamnForDelregister(int delregId);

        string HamtaKortnamnForRegister(int regId);

        string HamtaNamnForFilkrav(int filkravId);

        AdmForeskrift HamtaForeskriftByFilkrav(int filkravId);

        IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisation(int orgId);

        IEnumerable<AppUserAdmin> HamtaAdminUsers();

        void SkapaOrganisationsenhet(Organisationsenhet orgUnit, string userName);

        void SkapaFAQKategori(AdmFAQKategori faqKategori, string userName);

        void SkapaFAQ(AdmFAQ faq, string userName);

        void SkapaHelgdag(AdmHelgdag helgdag, string userName);

        void SkapaSpecialdag(AdmSpecialdag specialdag, string userName);

        void SkapaInformationsText(AdmInformation infoText, string userName);

        void SkapaUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSk, string userName);

        void SkapaRegister(AdmRegister reg, string userName);

        void SkapaDelregister(AdmDelregister delReg, string userName);

        void SkapaForvantadLeverans(AdmForvantadleverans forvLev, string userName);

        void SkapaForvantadFil(AdmForvantadfil forvFil, string userName);

        void SkapaFilkrav(AdmFilkrav filkrav, string userName);

        void UppdateraOrganisation(Organisation org, string userName);

        void UppdateraKontaktperson(ApplicationUser user, string userName);

        void UppdateraAdminAnvandare(AppUserAdmin user, string userName);

        void UppdateraOrganisationsenhet(Organisationsenhet orgUnit, string userName);

        void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet, string userName);

        void UppdateraFAQKategori(AdmFAQKategori faqKategori, string userName);

        void UppdateraFAQ(AdmFAQ faq, string userName);

        void UppdateraHelgdag(AdmHelgdag holiday, string userName);

        void UppdateraSpecialdag(AdmSpecialdag specialDay, string userName);

        void UppdateraInformationstext(AdmInformation infoText, string userName);

        void UppdateraRegister(AdmRegister register, string userName);

        void UppdateraDelregister(AdmDelregister delregister, string userName);

        void UppdateraForvantadLeverans(AdmForvantadleverans forvLev, string userName);

        void UppdateraForvantadFil(AdmForvantadfil forvFil, string userName);

        void UppdateraFilkrav(AdmFilkrav filkrav, string userName);

        void UppdateraAnvandarInfo(AppUserAdmin user, string userName);

        void SparaOppettider(OpeningHoursInfoDTO oppetTider, string userName);

        void TaBortFAQKategori(int faqKategoriId);

        void TaBortFAQ(int faqId);

        void TaBortHelgdag(int holidayId);

        void TaBortSpecialdag(int specialDayId);

        void TaBortKontaktperson(string contactId);

        void TaBortAdminAnvandare(string userId);

        List<OpeningDay> MarkeraStangdaDagar(List<string> closedDays);

    }
}
