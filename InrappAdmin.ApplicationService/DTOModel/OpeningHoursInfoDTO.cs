using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InrappAdmin.ApplicationService.DTOModel
{
    public class OpeningHoursInfoDTO
    {
        public int ClosedFromHour { get; set; }
        public int ClosedFromMin { get; set; }
        public int ClosedToHour { get; set; }
        public  int ClosedToMin { get; set; }
        [DisplayName("Stäng portalen")]
        public bool ClosedAnyway { get; set; }
        public List<string> ClosedDays { get; set; }
        public string InfoTextForClosedPage { get; set; }
    }
}
