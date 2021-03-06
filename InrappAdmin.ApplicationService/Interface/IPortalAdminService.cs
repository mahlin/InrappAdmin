﻿using InrappAdmin.DomainModel;
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

        AdmUppgiftsskyldighet HamtaUppgiftsskyldighetForOrgOchDelreg(int orgId, int delregId);

        IEnumerable<AdmEnhetsUppgiftsskyldighet> HamtaEnhetsUppgiftsskyldighetForOrgEnhet(int orgenhetId);

        AdmEnhetsUppgiftsskyldighet HamtaEnhetsUppgiftsskyldighetForUppgiftsskyldighetOchOrgEnhet(int uppgskhId, int orgenhetId);

        IEnumerable<AdmFAQKategori> HamtaFAQkategorier();

        IEnumerable<AdmFAQ> HamtaFAQs(int faqCatId);

        IEnumerable<AdmHelgdag> HamtaAllaHelgdagar();

        IEnumerable<AdmSpecialdag> HamtaAllaSpecialdagar();

        AdmFAQ HamtaFAQ(int faqId);

        AdmFAQKategori HamtaFAQKategori(int faqCatId);

        IEnumerable<AdmInformation> HamtaInformationstexter();

        OpeningHoursInfoDTO HamtaOppettider();

        AdmInformation HamtaInfoText(string infoTyp);

        AdmInformation HamtaInfo(int infoId);

        IEnumerable<AdmRegister> HamtaRegister();

        AdmRegister HamtaRegisterMedKortnamn(string regKortNamn);

        AdmRegister HamtaRegisterMedId(int regId);

        IEnumerable<AdmDelregister> HamtaDelRegister();

        IEnumerable<AdmDelregister> HamtaDelRegisterForRegister(int regId);

        AdmDelregister HamtaDelRegisterForUppgiftsskyldighet(int uppgSkId);
        AdmDelregister HamtaDelRegisterForKortnamn(string shortName);

        IEnumerable<RegisterInfo> HamtaDelregisterOchFilkrav();
        IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranser();

        IEnumerable<AdmForvantadfil> HamtaAllaForvantadeFiler();

        IEnumerable<AdmFilkrav> HamtaAllaFilkrav();

        IEnumerable<AdmForvantadfil> HamtaForvantadeFilerForRegister(int regId);
        IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranserForRegister(int regId);

        IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranserForDelregister(int delregId);

        IEnumerable<AdmFilkrav> HamtaFilkravForRegister(int regId);
        IEnumerable<AdmRegister> HamtaAllaRegister();
        IEnumerable<AdmRegister> HamtaAllaRegisterForPortalen();

        IEnumerable<AdmDelregister> HamtaAllaDelregisterForPortalen();

        IEnumerable<AdmInsamlingsfrekvens> HamtaAllaInsamlingsfrekvenser();

        IEnumerable<Organisation> HamtaAllaOrganisationer();

        string HamtaKortnamnForDelregisterMedFilkravsId(int filkravsId);

        string HamtaKortnamnForDelregister(int delregId);

        AdmInsamlingsfrekvens HamtaInsamlingsfrekvens(int insamlingsid);

        string HamtaKortnamnForRegister(int regId);

        string HamtaNamnForFilkrav(int filkravId);

        AdmForeskrift HamtaForeskriftByFilkrav(int filkravId);

        IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisation(int orgId);

        IEnumerable<AppUserAdmin> HamtaAdminUsers();

        IEnumerable<RapporteringsresultatDTO> HamtaRapporteringsresultatForDelregOchPeriod(int delRegId, string period);

        IEnumerable<RapporteringsresultatDTO> HamtaRapporteringsresultatForRegOchPeriod(int regId, string period);

        IEnumerable<AdmRegister> HamtaRegisterForOrg(int orgId);

        IEnumerable<string> HamtaDelregistersPerioderForAr(int delregId, int ar);

        List<int> HamtaValbaraAr(int delregId);
        DateTime HamtaRapporteringsstartForRegisterOchPeriod(int regId, string period);

        DateTime HamtaRapporteringsstartForRegisterOchPeriodSpecial(int regId, string period);
        DateTime HamtaSenasteRapporteringForRegisterOchPeriodSpecial(int regId, string period);

        DateTime HamtaSenasteRapporteringForRegisterOchPeriod(int regId, string period);

        IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisationRegisterPeriod(int orgId, int regId, string periodForReg);

        string HamtaSammanlagdStatusForPeriod(IEnumerable<FilloggDetaljDTO> historikLista);

        IEnumerable<RegisterInfo> HamtaValdaRegistersForAnvandare(string userId, int orgId);


        int SkapaOrganisation(Organisation org, string userName);

        void SkapaOrganisationsenhet(Organisationsenhet orgUnit, string userName);

        void SkapaFAQKategori(AdmFAQKategori faqKategori, string userName);

        void SkapaFAQ(AdmFAQ faq, string userName);

        void SkapaHelgdag(AdmHelgdag helgdag, string userName);

        void SkapaSpecialdag(AdmSpecialdag specialdag, string userName);

        void SkapaInformationsText(AdmInformation infoText, string userName);

        void SkapaUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSk, string userName);

        void SkapaEnhetsUppgiftsskyldighet(AdmEnhetsUppgiftsskyldighet enhetsUppgSk, string userName);

        void SkapaRegister(AdmRegister reg, string userName);

        void SkapaDelregister(AdmDelregister delReg, string userName);

        void SkapaForvantadLeverans(AdmForvantadleverans forvLev, string userName);
        void SkapaForvantadLeveranser(IEnumerable<AdmForvantadleverans> forvLevList, string userName);
        IEnumerable<ForvantadLeveransDTO> SkapaForvantadeLeveranserUtkast(int selectedYear, int selectedDelRegId, int selectedFilkravId);

        void SkapaForvantadFil(AdmForvantadfil forvFil, string userName);

        void SkapaFilkrav(AdmFilkrav filkrav, string userName);

        void SkapaInsamlingsfrekvens(AdmInsamlingsfrekvens insamlingsfrekvens, string userName);

        void UppdateraOrganisation(Organisation org, string userName);

        void UppdateraKontaktperson(ApplicationUser user, string userName);

        void UppdateraAdminAnvandare(AppUserAdmin user, string userName);

        void UppdateraOrganisationsenhet(Organisationsenhet orgUnit, string userName);

        void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet, string userName);

        void UppdateraEnhetsUppgiftsskyldighet(AdmEnhetsUppgiftsskyldighet enhetsUppgSkyldighet, string userName);

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

        void UppdateraInsamlingsfrekvens(AdmInsamlingsfrekvens insamlingsfrekvens, string userName);

        void UppdateraAnvandarInfo(AppUserAdmin user, string userName);

        void SparaOppettider(OpeningHoursInfoDTO oppetTider, string userName);

        void TaBortFAQKategori(int faqKategoriId);

        void TaBortFAQ(int faqId);

        void TaBortHelgdag(int holidayId);

        void TaBortSpecialdag(int specialDayId);

        void TaBortKontaktperson(string contactId);

        void TaBortAdminAnvandare(string userId);

        List<OpeningDay> MarkeraStangdaDagar(List<string> closedDays);

        void SkickaPaminnelse(IEnumerable<RapporteringsresultatDTO> rappResList, string userId);

    }
}
