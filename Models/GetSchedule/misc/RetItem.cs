
using ITCCLMBSSA_API.Models.misc;

namespace ITCCLMBSSA_API.Models.GetSchedule{
    public class RetItem{
        public string scheduleId{get;set;}
        public string availabilityView{get;set;}
        public List<ScheduleItem> scheduleItems{get;set;}
        public WorkingHours workingHours{get;set;}
    }
}