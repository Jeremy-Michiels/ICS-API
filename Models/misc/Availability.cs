namespace ITCCLMBSSA_API.Models.misc{
    public class Availability{
        public int id {get;set;} = 0;
        public DateTime datum{get;set;}
        public TimeSpan startTijd{get;set;}
        public TimeSpan eindTijd{get;set;}
        public List<string> attendees{get;set;}
        public bool allAvailable{get;set;} = false;
        public bool genoegTijd{get;set;}
        public bool genoegMetReistijd{get;set;}
    }
}