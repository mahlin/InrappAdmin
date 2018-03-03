using System;
using System.Collections.Generic;

namespace InrappAdmin.DomainModel
{
    public class AdmFilkrav
    {
        public int Id { get; set; }
        public int DelregisterId { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
        public virtual AdmDelregister AdmDelregister { get; set; }
        public virtual ICollection<AdmForvantadfil> AdmForvantadfil { get; set; }
        public virtual ICollection<AdmForvantadleverans> AdmForvantadleverans { get; set; }

    }
}