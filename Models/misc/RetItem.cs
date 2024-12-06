
namespace ITCCLMBSSA_API.Models.misc{
    public class RetItem{
        public string scheduleId{get;set;}
        public string availabilityView{get;set;}
        public List<ScheduleItem> scheduleItems{get;set;}
        public WorkingHours workingHours{get;set;}
    }
}