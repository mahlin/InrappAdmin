﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InrappAdmin.DomainModel;

namespace InrappAdmin.Web.Models.ViewModels
{
    public class AdminViewModels
    {
        public class AdminViewModel
        {
            public IEnumerable<AppUserAdminViewModel> AdminUsers { get; set; }
            public string SelectedUser { get; set; }
        }

        public class AppUserAdminViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public bool PhoneNumberConfirmed { get; set; }
            public string StringOfRoles { get; set; }
            public IList<string> Roles { get; set; }
            public DateTime SkapadDatum { get; set; }
            public string SkapadAv { get; set; }
            public DateTime AndradDatum { get; set; }
            public string AndradAv { get; set; }
            

        }
    }
}