using System.ComponentModel;

namespace ITCCLMBSSA_API.Models{
    public class APIAvailability{
        public List<string> emails{get;set;}

        [DefaultValue("2024-12-04T08:00:00.000Z")]
        public DateTime StartTime{get;set;}
        [DefaultValue("2024-12-04T18:00:00.000Z")]
        public DateTime EndTime{get;set;}
        [DefaultValue("00:00:00")]
        public TimeSpan MeetingTime{get;set;}
        [DefaultValue("00:00:00")]
        public TimeSpan tijdWeg{get;set;}
    }
}