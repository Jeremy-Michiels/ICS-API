using ITCCLMBSSA_API.Models.misc;

namespace ITCCLMBSSA_API.Models.GetSchedule{
    public class ScheduleItem{
        public string status{get;set;}
        public DateTimeTimeZone start{get;set;}
        public DateTimeTimeZone end{get;set;}
    }
}