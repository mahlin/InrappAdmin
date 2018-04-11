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

        IEnumerable<AdmFAQKategori> HamtaFAQkategorier();

        IEnumerable<AdmFAQ> HamtaFAQs(int faqCatId);

        AdmFAQ HamtaFAQ(int faqId);

        IEnumerable<AdmInformation> HamtaInformationstexter();

        OpeningHoursInfoDTO HamtaOppettider();

        string HamtaInfoText(string infoTyp);

        IEnumerable<AdmRegister> HamtaRegister();

        AdmRegister HamtaRegisterMedKortnamn(string regKortNamn);

        AdmRegister HamtaRegisterMedId(int regId);

        IEnumerable<AdmDelregister> HamtaDelRegister();

        IEnumerable<AdmDelregister> HamtaDelRegisterForRegister(int regId);

        IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranser();

        IEnumerable<AdmForvantadfil> HamtaAllaForvantadeFiler();

        IEnumerable<AdmForvantadfil> HamtaForvantadeFilerForRegister(int regId);
        IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranserForRegister(int regId);
        IEnumerable<AdmRegister> HamtaAllaRegister();
        IEnumerable<AdmRegister> HamtaAllaRegisterForPortalen();

        string HamtaKortnamnForDelregisterMedForeskriftsId(int foreskriftsId);

        string HamtaKortnamnForDelregister(int delregId);

        IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisation(int orgId);

        void SkapaOrganisationsenhet(Organisationsenhet orgUnit, string userName);

        void SkapaFAQKategori(AdmFAQKategori faqKategori, string userName);

        void SkapaFAQ(AdmFAQ faq, string userName);

        void SkapaInformationsText(AdmInformation infoText, string userName);

        void SkapaUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSk, string userName);

        void SkapaRegister(AdmRegister reg, string userName);

        void SkapaDelregister(AdmDelregister delReg, string userName);

        void SkapaForvantadLeverans(AdmForvantadleverans forvLev, string userName);

        void SkapaForvantadFil(AdmForvantadfil forvFil, string userName);

        void UppdateraOrganisation(Organisation org, string userName);

        void UppdateraKontaktperson(ApplicationUser user, string userName);

        void UppdateraOrganisationsenhet(Organisationsenhet orgUnit, string userName);

        void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet, string userName);

        void UppdateraFAQKategori(AdmFAQKategori faqKategori, string userName);

        void UppdateraFAQ(AdmFAQ faq, string userName);

        void UppdateraInformationstext(AdmInformation infoText, string userName);

        void UppdateraRegister(AdmRegister register, string userName);

        void UppdateraDelregister(AdmDelregister delregister, string userName);

        void UppdateraForvantadLeverans(AdmForvantadleverans forvLev, string userName);

        void UppdateraForvantadFil(AdmForvantadfil forvFil, string userName);
        void UppdateraAnvandarInfo(AppUserAdmin user);

        void SparaOppettider(OpeningHoursInfoDTO oppetTider, string userName);

        void TaBortFAQKategori(int faqKategoriId);

        void TaBortFAQ(int faqId);

        void TaBortKontaktperson(string contactId);

        List<OpeningDay> MarkeraStangdaDagar(List<string> closedDays);

    }
}
