namespace ITCCLMBSSA_API.Models.misc{
    public class WorkingHours{
        public List<string> daysOfWeek{get;set;}
        public TimeSpan startTijd{get;set;}
        public TimeSpan eindTijd{get;set;}
        public TimeZone timeZone{get;set;}
    }
}