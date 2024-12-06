using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace ITCCLMBSSA_API.Models.PostEvent{
    public class APIPost{
        [DefaultValue("Onderwerp")]
        public string subject{get;set;}
        [DefaultValue("Bericht")]
        public string content{get;set;}
        [DefaultValue("2024-12-06T13:00:00.000Z")]
        public DateTime startTime{get;set;}
        [DefaultValue("2024-12-06T14:00:00.000Z")]
        public DateTime endTime{get;set;}
        [DefaultValue("Locatie")]
        public string Location{get;set;}
        public List<string> attendees{get;set;}
        [DefaultValue(false)]
        public bool isOnlineMeeting{get;set;}
        [DefaultValue("00:00:00")]
        public TimeSpan travelTime{get;set;}
    }
}