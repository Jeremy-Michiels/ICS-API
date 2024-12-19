using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ITCCLMBSSA_API.Models.misc{
    public class Availability{
        public DateTime datum{get;set;}
        public TimeSpan startTijd{get;set;}
        public TimeSpan eindTijd{get;set;}
        public List<string> attendees{get;set;}
        public bool allAvailable{get;set;} = false;
        public bool genoegTijd{get;set;}
        public bool genoegMetReistijd{get;set;}
        public bool isSub{get;set;} = false;
    }
}