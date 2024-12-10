namespace ITCCLMBSSA_API.Models.misc{
    public class WorkingHours{
        public List<string> daysOfWeek{get;set;}
        public TimeSpan startTime{get;set;}
        public TimeSpan endTime{get;set;}
        public TimeZone timeZone{get;set;}
    }
}