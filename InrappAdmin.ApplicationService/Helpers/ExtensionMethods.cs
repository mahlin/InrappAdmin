﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.ApplicationService.DTOModel;
using InrappAdmin.DomainModel;

namespace InrappAdmin.ApplicationService.Helpers
{
    public static class ExtensionMethods
    {
        public static List<OpeningDay> GetDaysOfWeek()
        {
            CultureInfo swedish = new CultureInfo("sv-SE");
            List<OpeningDay> days = new List<OpeningDay>();
            OpeningDay monday = new OpeningDay
            {
                NameEnglish = DayOfWeek.Monday.ToString(),
                NameSwedish = swedish.DateTimeFormat.GetDayName(DayOfWeek.Monday).ToString()
            };
            OpeningDay tuesday = new OpeningDay
            {
                NameEnglish = DayOfWeek.Tuesday.ToString(),
                NameSwedish = swedish.DateTimeFormat.GetDayName(DayOfWeek.Tuesday).ToString()
            };
            OpeningDay wednesday = new OpeningDay
            {
                NameEnglish = DayOfWeek.Wednesday.ToString(),
                NameSwedish = swedish.DateTimeFormat.GetDayName(DayOfWeek.Wednesday).ToString()
            };
            OpeningDay thursday = new OpeningDay
            {
                NameEnglish = DayOfWeek.Thursday.ToString(),
                NameSwedish = swedish.DateTimeFormat.GetDayName(DayOfWeek.Thursday).ToString()
            };
            OpeningDay friday = new OpeningDay
            {
                NameEnglish = DayOfWeek.Friday.ToString(),
                NameSwedish = swedish.DateTimeFormat.GetDayName(DayOfWeek.Friday).ToString()
            };
            OpeningDay saturday = new OpeningDay
            {
                NameEnglish = DayOfWeek.Saturday.ToString(),
                NameSwedish = swedish.DateTimeFormat.GetDayName(DayOfWeek.Saturday).ToString()
            };
            OpeningDay sunday = new OpeningDay
            {
                NameEnglish = DayOfWeek.Sunday.ToString(),
                NameSwedish = swedish.DateTimeFormat.GetDayName(DayOfWeek.Sunday).ToString()
            };



            days.Add(monday);
            days.Add(tuesday);
            days.Add(wednesday);
            days.Add(thursday);
            days.Add(friday);
            days.Add(saturday);
            days.Add(sunday);

            return days;
        }

        //public static T SetAndradAv<T>(T inputObj, string userName)
        //{
        //    inputObj.SkapadAv = username


        //}

  
    }
}
