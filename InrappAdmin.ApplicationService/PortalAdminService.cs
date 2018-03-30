﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.ApplicationService.DTOModel;
using InrappAdmin.ApplicationService.Interface;
using InrappAdmin.ApplicationService.Helpers;
using InrappAdmin.DataAccess;
using InrappAdmin.DomainModel;

namespace InrappAdmin.ApplicationService
{
    public class PortalAdminService : IPortalAdminService
    {

        private readonly IPortalAdminRepository _portalAdminRepository;

        public PortalAdminService(IPortalAdminRepository portalRepository)
        {
            _portalAdminRepository = portalRepository;
        }

        public string HamtaHistorikForOrganisation(int orgId)
        {
            var test  = _portalAdminRepository.GetLeveranserForOrganisation(orgId);
            return test;
        }

        public Organisation HamtaOrganisation(int orgId)
        {
            var org = _portalAdminRepository.GetOrganisation(orgId);
            return org;
        }

        public string HamtaKommunkodForOrg(int orgId)
        {
            var kommunkod = _portalAdminRepository.GetKommunkodForOrg(orgId);
            return kommunkod;
        }

        public IEnumerable<ApplicationUser> HamtaKontaktpersonerForOrg(int orgId)
        {
            var contacts = _portalAdminRepository.GetContactPersonsForOrg(orgId);
            return contacts;
        }

        public Organisation HamtaOrganisationForKommunkod(string kommunkod)
        {
            var org = _portalAdminRepository.GetOrganisationFromKommunkod(kommunkod);
            return org;
        }

        public IEnumerable<Organisationsenhet> HamtaOrgEnheterForOrg(int orgId)
        {
            var orgEnheter = _portalAdminRepository.GetOrgUnitsForOrg(orgId);
            return orgEnheter;
        }

        public IEnumerable<AdmUppgiftsskyldighet> HamtaUppgiftsskyldighetForOrg(int orgId)
        {
            var uppgiftsskyldigheter = _portalAdminRepository.GetReportObligationInformationForOrg(orgId);
            return uppgiftsskyldigheter;
        }
        public Organisation HamtaOrgForAnvandare(string userId)
        {
            var org = _portalAdminRepository.GetOrgForUser(userId);
            return org;
        }

        public Organisation HamtaOrgForOrganisationsenhet(int orgUnitId)
        {
            var org = _portalAdminRepository.GetOrgForOrgUnit(orgUnitId);
            return org;
        }

        public Organisation HamtaOrgForUppgiftsskyldighet(int uppgSkId)
        {
            var org = _portalAdminRepository.GetOrgForReportObligation(uppgSkId);
            return org;
        }

        public IEnumerable<AdmFAQKategori> HamtaFAQkategorier()
        {
            var faqCats = _portalAdminRepository.GetFAQCategories();
            return faqCats;
        }

        public IEnumerable<AdmFAQ> HamtaFAQs(int faqCatId)
        {
            var faqs = _portalAdminRepository.GetFAQs(faqCatId);
            return faqs;
        }

        public IEnumerable<AdmInformation> HamtaInformationstexter()
        {
            var infoTexts = _portalAdminRepository.GetInformationTexts();
            return infoTexts;
        }

        public OpeningHoursInfoDTO HamtaOppettider()
        {
            var configInfo = _portalAdminRepository.GetAdmConfiguration();
            var oppettiderObj = new OpeningHoursInfoDTO();

            foreach (var item in configInfo)
            {
                switch (item.Typ)
                {
                    case "ClosedFromHour":
                        oppettiderObj.ClosedFromHour = item.Varde;
                        break;
                    case "ClosedFromMin":
                        oppettiderObj.ClosedFromMin = item.Varde;
                        break;
                    case "ClosedToHour":
                        oppettiderObj.ClosedToHour = item.Varde;
                        break;
                    case "ClosedToMin":
                        oppettiderObj.ClosedToMin = item.Varde;
                        break;
                    case "ClosedAnyway":
                        oppettiderObj.ClosedAnyway = Convert.ToBoolean(item.Varde);
                        break;
                    case "ClosedDays":
                        oppettiderObj.ClosedDays= item.Varde.Split(new char[] { ',' }).ToList();
                        break;
                }
            }

            return oppettiderObj;
        }

        public string HamtaInfoText(string infoTyp)
        {
            var info = _portalAdminRepository.GetInfoText(infoTyp);
            return info.Text;
        }

        public IEnumerable<AdmRegister> HamtaRegister()
        {
            var registerList = _portalAdminRepository.GetDirectories();
            return registerList;
        }

        public AdmRegister HamtaRegisterMedKortnamn(string regKortNamn)
        {
            var register = _portalAdminRepository.GetDirectoryByShortName(regKortNamn);
            return register;
        }

        public AdmRegister HamtaRegisterMedId(int regId)
        {
            var register = _portalAdminRepository.GetDirectoryById(regId);
            return register;
        }

        public IEnumerable<AdmDelregister> HamtaDelRegister()
        {
            var subDirectories = _portalAdminRepository.GetSubDirectories();
            return subDirectories;
        }

        public IEnumerable<AdmDelregister> HamtaDelRegisterForRegister(int regId)
        {
            var subDirectories = _portalAdminRepository.GetSubDirectoriesForDirectory(regId);
            return subDirectories;
        }

        public IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranser()
        {
            var forvlevList = _portalAdminRepository.GetExpectedDeliveries();
            return forvlevList;
        }

        public void SkapaOrganisationsenhet(Organisationsenhet orgUnit)
        {
            //Sätt datum och användare
            orgUnit.SkapadDatum = DateTime.Now;
            orgUnit.SkapadAv = "InrappAdmin";
            orgUnit.AndradDatum = DateTime.Now;
            orgUnit.AndradAv = "InrappAdmin";

            _portalAdminRepository.CreateOrgUnit(orgUnit);
        }

        public void SkapaFAQKategori(AdmFAQKategori faqKategori)
        {
            //Sätt datum och användare
            faqKategori.SkapadDatum = DateTime.Now;
            faqKategori.SkapadAv = "InrappAdmin";
            faqKategori.AndradDatum = DateTime.Now;
            faqKategori.AndradAv = "InrappAdmin";

            _portalAdminRepository.CreateFAQCategory(faqKategori);
        }

        public void SkapaFAQ(AdmFAQ faq)
        {
            //Sätt datum och användare
            faq.SkapadDatum = DateTime.Now;
            faq.SkapadAv = "InrappAdmin";
            faq.AndradDatum = DateTime.Now;
            faq.AndradAv = "InrappAdmin";

            _portalAdminRepository.CreateFAQ(faq);
        }

        public void SkapaInformationsText(AdmInformation infoText)
        {
            //Sätt datum och användare
            infoText.SkapadDatum = DateTime.Now;
            infoText.SkapadAv = "InrappAdmin";
            infoText.AndradDatum = DateTime.Now;
            infoText.AndradAv = "InrappAdmin";
            _portalAdminRepository.CreateInformationText(infoText);
        }

        public void SkapaUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSk)
        {
            //Sätt datum och användare
            uppgSk.SkapadDatum = DateTime.Now;
            uppgSk.SkapadAv = "InrappAdmin";
            uppgSk.AndradDatum = DateTime.Now;
            uppgSk.AndradAv = "InrappAdmin";
            _portalAdminRepository.CreateReportObligation(uppgSk);
        }

        public void SkapaRegister(AdmRegister reg)
        {
            //Sätt datum och användare
            reg.SkapadDatum = DateTime.Now;
            reg.SkapadAv = "InrappAdmin";
            reg.AndradDatum = DateTime.Now;
            reg.AndradAv = "InrappAdmin"; 
            _portalAdminRepository.CreateDirectory(reg);
        }

        public void SkapaDelregister(AdmDelregister delReg)
        {
            //Sätt datum och användare
            delReg.SkapadDatum = DateTime.Now;
            delReg.SkapadAv = "InrappAdmin";
            delReg.AndradDatum = DateTime.Now;
            delReg.AndradAv = "InrappAdmin";
            _portalAdminRepository.CreateSubDirectory(delReg);
        }

        public void SkapaForvantadLeverans(AdmForvantadleverans forvLev)
        {
            //Sätt datum och användare
            forvLev.SkapadDatum = DateTime.Now;
            forvLev.SkapadAv = "InrappAdmin";
            forvLev.AndradDatum = DateTime.Now;
            forvLev.AndradAv = "InrappAdmin";
            _portalAdminRepository.CreateExpectedDelivery(forvLev);
        }

        public void UppdateraOrganisation(Organisation org)
        {
            //Sätt datum och användare
            org.AndradDatum = DateTime.Now;
            org.AndradAv = "InrappAdmin";
            _portalAdminRepository.UpdateOrganisation(org);
        }

        public void UppdateraKontaktperson(ApplicationUser user)
        {
            _portalAdminRepository.UpdateContactPerson(user);
        }

        public void UppdateraOrganisationsenhet(Organisationsenhet orgUnit)
        {
            //Sätt datum och användare
            orgUnit.AndradDatum = DateTime.Now;
            orgUnit.AndradAv = "InrappAdmin";
            _portalAdminRepository.UpdateOrgUnit(orgUnit);
        }

        public void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet)
        {
            //Sätt datum och användare
            uppgSkyldighet.AndradDatum = DateTime.Now;
            uppgSkyldighet.AndradAv = "InrappAdmin";
            _portalAdminRepository.UpdateReportObligation(uppgSkyldighet);
        }

        public void UppdateraFAQKategori(AdmFAQKategori faqKategori)
        {
            faqKategori.AndradDatum = DateTime.Now;
            faqKategori.AndradAv = "InrappAdmin";
            _portalAdminRepository.UpdateFAQCategory(faqKategori);
        }

        public void UppdateraFAQ(AdmFAQ faq)
        {
            faq.AndradDatum = DateTime.Now;
            faq.AndradAv = "InrappAdmin";
            _portalAdminRepository.UpdateFAQ(faq);
        }

        public void UppdateraInformationstext(AdmInformation infoText)
        {
            infoText.Id = _portalAdminRepository.GetPageInfoTextId(infoText.Informationstyp);
            infoText.AndradDatum = DateTime.Now;
            infoText.AndradAv = "InrappAdmin";
            _portalAdminRepository.UpdateInfoText(infoText);
        }

        public void UppdateraRegister(AdmRegister register)
        {
            _portalAdminRepository.UpdateDirectory(register);
        }

        public void UppdateraDelregister(AdmDelregister delregister)
        {
            _portalAdminRepository.UpdateSubDirectory(delregister);
        }

        public void UppdateraForvantadLeverans(AdmForvantadleverans forvLev)
        {
            _portalAdminRepository.UpdateExpectedDelivery(forvLev);
        }

        public void SparaOppettider(OpeningHoursInfoDTO oppetTider)
        {
            //Dela upp informationen i konf-objekt och spara till databasen
            AdmKonfiguration admKonfClosedAnayway = new AdmKonfiguration();
            admKonfClosedAnayway.AndradAv = "InrappAdmin";
            admKonfClosedAnayway.AndradDatum = DateTime.Now;
            admKonfClosedAnayway.Typ = "ClosedAnyway";

            //Closed anyway
            if (oppetTider.ClosedAnyway)
            { 
                admKonfClosedAnayway.Varde = "True";
            }
            else
            {
                admKonfClosedAnayway.Varde = "False";
            }
            _portalAdminRepository.SaveOpeningHours(admKonfClosedAnayway);

            //Closed days
            string daysJoined = string.Join(",", oppetTider.ClosedDays);
            AdmKonfiguration admKonfClosedDays = new AdmKonfiguration
            {
                Typ = "ClosedDays",
                Varde = daysJoined
            };
            admKonfClosedDays.AndradAv = "InrappAdmin";
            admKonfClosedDays.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedDays);

            //Closed from hour
            AdmKonfiguration admKonfClosedFromHour = new AdmKonfiguration
            {
                Typ = "ClosedFromHour",
                Varde = oppetTider.ClosedFromHour.ToString()
            };
            admKonfClosedFromHour.AndradAv = "InrappAdmin";
            admKonfClosedFromHour.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedFromHour);

            //Closed from minute
            AdmKonfiguration admKonfClosedFromMin = new AdmKonfiguration
            {
                Typ = "ClosedFromMin",
                Varde = oppetTider.ClosedFromMin.ToString()
            };
            admKonfClosedFromMin.AndradAv = "InrappAdmin";
            admKonfClosedFromMin.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedFromMin);

            //Closed to hour
            AdmKonfiguration admKonfClosedToHour = new AdmKonfiguration
            {
                Typ = "ClosedToHour",
                Varde = oppetTider.ClosedToHour.ToString()
            };
            admKonfClosedToHour.AndradAv = "InrappAdmin";
            admKonfClosedToHour.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedToHour);

            //Closed to minute
            AdmKonfiguration admKonfClosedToMin = new AdmKonfiguration
            {
                Typ = "ClosedFromMin",
                Varde = oppetTider.ClosedToMin.ToString()
            };
            admKonfClosedToMin.AndradAv = "InrappAdmin";
            admKonfClosedToMin.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedToMin);

            //Closed informationtext
            var infoPageId = _portalAdminRepository.GetPageInfoTextId("Stangtsida");
            AdmInformation infoTextClosedpage = new AdmInformation
            {
                Id = infoPageId,
                Informationstyp = "Stangtsida",
                Text = oppetTider.InfoTextForClosedPage
            };
            infoTextClosedpage.AndradAv = "InrappAdmin";
            infoTextClosedpage.AndradDatum = DateTime.Now;
            _portalAdminRepository.UpdateInfoText(infoTextClosedpage);
        }
        
        public List<OpeningDay> MarkeraStangdaDagar(List<string> closedDays)
        {
            var daysOfWeek = ExtensionMethods.GetDaysOfWeek();

            foreach (var day in daysOfWeek)
            {
                if (closedDays.Contains(day.NameEnglish))
                {
                    day.Selected = true;
                }
            }
            return daysOfWeek;
        }

        public void TaBortFAQKategori(int faqKategoriId)
        {
            _portalAdminRepository.DeleteFAQCategory(faqKategoriId);
        }



        public void TaBortFAQ(int faqId)
        {
            _portalAdminRepository.DeleteFAQ(faqId);}

        public void TaBortKontaktperson(string contactId)
        {
            _portalAdminRepository.DeleteContact(contactId);
        }


    }
}
