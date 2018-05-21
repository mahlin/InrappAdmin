using System;
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

        public IEnumerable<AppUserAdmin> HamtaAdminUsers()
        {
            var adminUsers = _portalAdminRepository.GetAdminUsers();
            return adminUsers;
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

        public IEnumerable<AdmHelgdag> HamtaAllaHelgdagar()
        {
            var helgdagar = _portalAdminRepository.GetAllHolidays();
            return helgdagar;
        }

        public IEnumerable<AdmSpecialdag> HamtaAllaSpecialdagar()
        {
            var specialdagar = _portalAdminRepository.GetAllSpecialDays();
            return specialdagar;
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

        public AdmInformation HamtaInfoText(string infoTyp)
        {
            var info = _portalAdminRepository.GetInfoText(infoTyp);
            return info;
        }

        public AdmInformation HamtaInfo(int infoId)
        {
            var info = _portalAdminRepository.GetInfoText(infoId);
            return info;
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

        public AdmDelregister HamtaDelRegisterForKortnamn(string shortName)
        {
            var subDirectory = _portalAdminRepository.GetSubDirectoryByShortName(shortName);
            return subDirectory;
        }

        public IEnumerable<RegisterInfo> HamtaDelregisterOchFilkrav()
        {
            var delregMedFilkravList = new List<RegisterInfo>();

            var delregList = _portalAdminRepository.GetAllSubDirectoriesForPortal();
            foreach (var delreg in delregList)
            {
                var regFilkravList = new List<RegisterFilkrav>();
                var regInfo = new RegisterInfo
                {
                    Id = delreg.Id,
                    Kortnamn = delreg.Kortnamn
                };

                //Hämta varje delregisters filkrav
                var filkravList = _portalAdminRepository.GetFileRequirementsForSubDirectory(delreg.Id).ToList();
                foreach (var filkrav in filkravList)
                {
                    var regFilkrav = new RegisterFilkrav
                    {
                        Id = filkrav.Id
                    };
                    var namn = delreg.Kortnamn;
                    if (filkrav.Namn != null)
                    {
                        namn = namn + " - " + filkrav.Namn;
                    }
                    regFilkrav.Namn = namn;
                    regFilkravList.Add(regFilkrav);
                }

                regInfo.Filkrav = regFilkravList;
                delregMedFilkravList.Add(regInfo);
            }
            return delregMedFilkravList;
        }


        public IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranser()
        {
            var forvlevList = _portalAdminRepository.GetExpectedDeliveries();
            return forvlevList;
        }

        public IEnumerable<AdmForvantadfil> HamtaAllaForvantadeFiler()
        {
            var forvFilList = _portalAdminRepository.GetAllExpectedFiles();
            return forvFilList;
        }

        public IEnumerable<AdmFilkrav> HamtaAllaFilkrav()
        {
            var filkravList = _portalAdminRepository.GetAllFileRequirements();
            return filkravList;
        }

        public string HamtaKortnamnForDelregisterMedFilkravsId(int filkravId)
        {
            var delRegKortnamn = _portalAdminRepository.GetSubDirectoryShortNameForExpectedFile(filkravId);
            return delRegKortnamn;
        }

        public string HamtaKortnamnForRegister(int regId)
        {
            var regKortnamn = _portalAdminRepository.GetDirectoryShortName(regId);
            return regKortnamn;
        }

        public string HamtaNamnForFilkrav(int filkravId)
        {
            var filkravNamn = _portalAdminRepository.GetFileRequirementName(filkravId);
            return filkravNamn;
        }

        public string HamtaKortnamnForDelregister(int delregId)
        {
            var delRegKortnamn = _portalAdminRepository.GetSubDirectoryShortName(delregId);
            return delRegKortnamn;
        }

        public IEnumerable<AdmForvantadfil> HamtaForvantadeFilerForRegister(int regId)
        {
            var forvantadeFiler = _portalAdminRepository.GetExpectedFilesForDirectory(regId);
            return forvantadeFiler;
        }

        public IEnumerable<AdmRegister> HamtaAllaRegister()
        {
            var registersList = _portalAdminRepository.GetAllRegisters();
            return registersList;
        }

        public IEnumerable<AdmRegister> HamtaAllaRegisterForPortalen()
        {
            var registersList = _portalAdminRepository.GetAllRegistersForPortal();
            return registersList;
        }

        public IEnumerable<AdmDelregister> HamtaAllaDelregisterForPortalen()
        {
            var delregistersList = _portalAdminRepository.GetAllSubDirectoriesForPortal();
            return delregistersList;
        }

        public IEnumerable<AdmForvantadleverans> HamtaForvantadeLeveranserForRegister(int regId)
        {
            var forvLeveranser = _portalAdminRepository.GetExpectedDeliveriesForDirectory(regId);
            return forvLeveranser;
        }

        public IEnumerable<AdmFilkrav> HamtaFilkravForRegister(int regId)
        {
            var filkrav= _portalAdminRepository.GetFileRequirementsForDirectory(regId); 
            return filkrav;
        }

        public AdmFAQ HamtaFAQ(int faqId)
        {
            var faq = _portalAdminRepository.GetFAQ(faqId);
            return faq;
        }

        public IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisation(int orgId)
        {
            var historikLista = new List<FilloggDetaljDTO>();
            //TODO - tidsintervall?
            //var leveransIdList = _portalRepository.GetLeveransIdnForOrganisation(orgId).OrderByDescending(x => x);
            var leveransList = _portalAdminRepository.GetLeveranserForOrganisation(orgId);
            foreach (var leverans in leveransList)
            {
                var filloggDetalj = new FilloggDetaljDTO();
                //Kolla om återkopplingsfil finns för aktuell leverans
                var aterkoppling = _portalAdminRepository.GetAterkopplingForLeverans(leverans.Id);

                //Kolla om enhetskod finns för aktuell leverans (stadsdelsleverans)
                var enhetskod = String.Empty;
                if (leverans.OrganisationsenhetsId != null)
                {
                    var orgenhetid = Convert.ToInt32(leverans.OrganisationsenhetsId);
                    enhetskod = _portalAdminRepository.GetEnhetskodForLeverans(orgenhetid);
                }

                //Hämta period för aktuell leverans
                var period = _portalAdminRepository.GetPeriodForAktuellLeverans(leverans.ForvantadleveransId);


                var filer = _portalAdminRepository.GetFilerForLeveransId(leverans.Id);
                var registerKortnamn = _portalAdminRepository.GetSubDirectoryShortName(leverans.DelregisterId);
                foreach (var fil in filer)
                {
                    filloggDetalj = (FilloggDetaljDTO.FromFillogg(fil));
                    filloggDetalj.Kontaktperson = leverans.ApplicationUserId;
                    filloggDetalj.Leveransstatus = leverans.Leveransstatus;
                    filloggDetalj.Leveranstidpunkt = leverans.Leveranstidpunkt;
                    filloggDetalj.RegisterKortnamn = registerKortnamn;
                    filloggDetalj.Resultatfil = "Ej kontrollerad";
                    filloggDetalj.Enhetskod = enhetskod;
                    filloggDetalj.Period = period;
                    if (aterkoppling != null)
                    {
                        filloggDetalj.Leveransstatus = aterkoppling.Leveransstatus;
                        filloggDetalj.Resultatfil = aterkoppling.Resultatfil;
                    }
                    historikLista.Add(filloggDetalj);
                }
            }
            var sorteradHistorikLista = historikLista.OrderByDescending(x => x.Leveranstidpunkt).ToList();

            return sorteradHistorikLista;
        }

        public AdmForeskrift HamtaForeskriftByFilkrav(int filkravId)
        {
            var foreskrift = _portalAdminRepository.GetForeskriftByFileReq(filkravId);
            return foreskrift;
        }

        public void SkapaOrganisationsenhet(Organisationsenhet orgUnit, string userName)
        {
            //Sätt datum och användare
            orgUnit.SkapadDatum = DateTime.Now;
            orgUnit.SkapadAv = userName;
            orgUnit.AndradDatum = DateTime.Now;
            orgUnit.AndradAv = userName;

            _portalAdminRepository.CreateOrgUnit(orgUnit);
        }

        public void SkapaFAQKategori(AdmFAQKategori faqKategori, string userName)
        {
            //Sätt datum och användare
            faqKategori.SkapadDatum = DateTime.Now;
            faqKategori.SkapadAv = userName;
            faqKategori.AndradDatum = DateTime.Now;
            faqKategori.AndradAv = userName;

            _portalAdminRepository.CreateFAQCategory(faqKategori);
        }

        public void SkapaFAQ(AdmFAQ faq, string userName)
        {
            //Sätt datum och användare
            faq.SkapadDatum = DateTime.Now;
            faq.SkapadAv = userName;
            faq.AndradDatum = DateTime.Now;
            faq.AndradAv = userName;

            _portalAdminRepository.CreateFAQ(faq);
        }

        public void SkapaHelgdag(AdmHelgdag helgdag, string userName)
        {
            //Sätt datum och användare
            helgdag.SkapadDatum = DateTime.Now;
            helgdag.SkapadAv = userName;
            helgdag.AndradDatum = DateTime.Now;
            helgdag.AndradAv = userName;

            _portalAdminRepository.CreateHoliday(helgdag);
        }

        public void SkapaSpecialdag(AdmSpecialdag specialdag, string userName)
        {
            //Sätt datum och användare
            specialdag.SkapadDatum = DateTime.Now;
            specialdag.SkapadAv = userName;
            specialdag.AndradDatum = DateTime.Now;
            specialdag.AndradAv = userName;

            _portalAdminRepository.CreateSpecialDay(specialdag);
        }

        public void SkapaInformationsText(AdmInformation infoText, string userName)
        {
            //Sätt datum och användare
            infoText.SkapadDatum = DateTime.Now;
            infoText.SkapadAv = userName;
            infoText.AndradDatum = DateTime.Now;
            infoText.AndradAv = userName;
            _portalAdminRepository.CreateInformationText(infoText);
        }

        public void SkapaUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSk, string userName)
        {
            //Sätt datum och användare
            uppgSk.SkapadDatum = DateTime.Now;
            uppgSk.SkapadAv = userName;
            uppgSk.AndradDatum = DateTime.Now;
            uppgSk.AndradAv = userName;
            _portalAdminRepository.CreateReportObligation(uppgSk);
        }

        public void SkapaRegister(AdmRegister reg, string userName)
        {
            //Sätt datum och användare
            reg.SkapadDatum = DateTime.Now;
            reg.SkapadAv = userName;
            reg.AndradDatum = DateTime.Now;
            reg.AndradAv = userName; 
            _portalAdminRepository.CreateDirectory(reg);
        }

        public void SkapaDelregister(AdmDelregister delReg, string userName)
        {
            //Sätt datum och användare
            delReg.SkapadDatum = DateTime.Now;
            delReg.SkapadAv = userName;
            delReg.AndradDatum = DateTime.Now;
            delReg.AndradAv = userName;
            _portalAdminRepository.CreateSubDirectory(delReg);
        }

        public void SkapaForvantadLeverans(AdmForvantadleverans forvLev, string userName)
        {
            //Sätt datum och användare
            forvLev.SkapadDatum = DateTime.Now;
            forvLev.SkapadAv = userName;
            forvLev.AndradDatum = DateTime.Now;
            forvLev.AndradAv = userName;
            _portalAdminRepository.CreateExpectedDelivery(forvLev);
        }

        public void SkapaForvantadFil(AdmForvantadfil forvFil, string userName)
        {
            //Sätt datum och användare
            forvFil.SkapadDatum = DateTime.Now;
            forvFil.SkapadAv = userName;
            forvFil.AndradDatum = DateTime.Now;
            forvFil.AndradAv = userName;
            _portalAdminRepository.CreateExpectedFile(forvFil);
        }

        public void SkapaFilkrav(AdmFilkrav filkrav, string userName)
        {
            //Sätt datum och användare
            filkrav.SkapadDatum = DateTime.Now;
            filkrav.SkapadAv = userName;
            filkrav.AndradDatum = DateTime.Now;
            filkrav.AndradAv = userName;
            _portalAdminRepository.CreateFileRequirement(filkrav);
        }

        public void UppdateraOrganisation(Organisation org, string userName)
        {
            //Sätt datum och användare
            org.AndradDatum = DateTime.Now;
            org.AndradAv = userName;
            _portalAdminRepository.UpdateOrganisation(org);
        }

        public void UppdateraKontaktperson(ApplicationUser user, string userName)
        {
            user.AndradDatum = DateTime.Now;
            user.AndradAv = userName;
            _portalAdminRepository.UpdateContactPerson(user);
        }

        public void UppdateraAdminAnvandare(AppUserAdmin user, string userName)
        {
            user.AndradDatum = DateTime.Now;
            user.AndradAv = userName;
            _portalAdminRepository.UpdateAdminUser(user);
        }

        public void UppdateraOrganisationsenhet(Organisationsenhet orgUnit, string userName)
        {
            //Sätt datum och användare
            orgUnit.AndradDatum = DateTime.Now;
            orgUnit.AndradAv = userName;
            _portalAdminRepository.UpdateOrgUnit(orgUnit);
        }

        public void UppdateraUppgiftsskyldighet(AdmUppgiftsskyldighet uppgSkyldighet, string userName)
        {
            //Sätt datum och användare
            uppgSkyldighet.AndradDatum = DateTime.Now;
            uppgSkyldighet.AndradAv = userName;
            _portalAdminRepository.UpdateReportObligation(uppgSkyldighet);
        }

        public void UppdateraFAQKategori(AdmFAQKategori faqKategori, string userName)
        {
            faqKategori.AndradDatum = DateTime.Now;
            faqKategori.AndradAv = userName;
            _portalAdminRepository.UpdateFAQCategory(faqKategori);
        }

        public void UppdateraFAQ(AdmFAQ faq, string userName)
        {
            faq.AndradDatum = DateTime.Now;
            faq.AndradAv = userName;
            _portalAdminRepository.UpdateFAQ(faq);
        }

        public void UppdateraHelgdag(AdmHelgdag holiday, string userName)
        {
            holiday.AndradDatum = DateTime.Now;
            holiday.AndradAv = userName;
            _portalAdminRepository.UpdateHoliday(holiday);
        }

        public void UppdateraSpecialdag(AdmSpecialdag specialday, string userName)
        {
            specialday.AndradDatum = DateTime.Now;
            specialday.AndradAv = userName;
            _portalAdminRepository.UpdateSpecialDay(specialday);
        }


        public void UppdateraInformationstext(AdmInformation infoText, string userName)
        {
            infoText.AndradDatum = DateTime.Now;
            infoText.AndradAv = userName;
            _portalAdminRepository.UpdateInfoText(infoText);
        }

        public void UppdateraRegister(AdmRegister register, string userName)
        {
            register.AndradAv = userName;
            register.AndradDatum = DateTime.Now;
            _portalAdminRepository.UpdateDirectory(register);
        }

        public void UppdateraDelregister(AdmDelregister delregister, string userName)
        {
            delregister.AndradAv = userName;
            delregister.AndradDatum = DateTime.Now;
            _portalAdminRepository.UpdateSubDirectory(delregister);
        }

        public void UppdateraForvantadLeverans(AdmForvantadleverans forvLev, string userName)
        {
            forvLev.AndradAv = userName;
            forvLev.AndradDatum = DateTime.Now;
            _portalAdminRepository.UpdateExpectedDelivery(forvLev);
        }

        public void UppdateraForvantadFil(AdmForvantadfil forvFil, string userName)
        {
            forvFil.AndradAv = userName;
            forvFil.AndradDatum = DateTime.Now;
            _portalAdminRepository.UpdateExpectedFile(forvFil);
        }

        public void UppdateraFilkrav(AdmFilkrav filkrav, string userName)
        {
            filkrav.AndradAv = userName;
            filkrav.AndradDatum = DateTime.Now;
            _portalAdminRepository.UpdateFileRequirement(filkrav);
        }

        public void UppdateraAnvandarInfo(AppUserAdmin user, string userName)
        {
            user.AndradAv = userName;
            user.AndradDatum = DateTime.Now;
            _portalAdminRepository.UpdateUserInfo(user);
        }

        public void SparaOppettider(OpeningHoursInfoDTO oppetTider, string userName)
        {
            //Dela upp informationen i konf-objekt och spara till databasen
            AdmKonfiguration admKonfClosedAnayway = new AdmKonfiguration();
            admKonfClosedAnayway.AndradAv = userName;
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
            admKonfClosedDays.AndradAv = userName;
            admKonfClosedDays.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedDays);

            //Closed from hour
            AdmKonfiguration admKonfClosedFromHour = new AdmKonfiguration
            {
                Typ = "ClosedFromHour",
                Varde = oppetTider.ClosedFromHour.ToString()
            };
            admKonfClosedFromHour.AndradAv = userName;
            admKonfClosedFromHour.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedFromHour);

            //Closed from minute
            AdmKonfiguration admKonfClosedFromMin = new AdmKonfiguration
            {
                Typ = "ClosedFromMin",
                Varde = oppetTider.ClosedFromMin.ToString()
            };
            admKonfClosedFromMin.AndradAv = userName;
            admKonfClosedFromMin.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedFromMin);

            //Closed to hour
            AdmKonfiguration admKonfClosedToHour = new AdmKonfiguration
            {
                Typ = "ClosedToHour",
                Varde = oppetTider.ClosedToHour.ToString()
            };
            admKonfClosedToHour.AndradAv = userName;
            admKonfClosedToHour.AndradDatum = DateTime.Now;
            _portalAdminRepository.SaveOpeningHours(admKonfClosedToHour);

            //Closed to minute
            AdmKonfiguration admKonfClosedToMin = new AdmKonfiguration
            {
                Typ = "ClosedFromMin",
                Varde = oppetTider.ClosedToMin.ToString()
            };
            admKonfClosedToMin.AndradAv = userName;
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
            infoTextClosedpage.AndradAv = userName;
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
            _portalAdminRepository.DeleteFAQ(faqId);
        }

        public void TaBortHelgdag(int holidayId)
        {
            _portalAdminRepository.DeleteHoliday(holidayId);
        }

        public void TaBortSpecialdag(int specialDayId)
        {
            _portalAdminRepository.DeleteSpecialDay(specialDayId);
        }

        public void TaBortKontaktperson(string contactId)
        {
            _portalAdminRepository.DeleteContact(contactId);
        }

        public void TaBortAdminAnvandare(string userId)
        {
            _portalAdminRepository.DeleteAdminUser(userId);
        }

    }
}
