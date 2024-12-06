using ITCCLMBSSA_API.Models.misc;

namespace ITCCLMBSSA_API.Models.PostEvent{
    public class Return{
        public string odataContext{get;set;}
        public string odataEtag{get;set;}
        public string id{get;set;}
        public DateTime createdDateTime{get;set;}
        public DateTime lastModifiedDateTime{get;set;}
        public string changeKey{get;set;}
        public List<string> categories{get;set;}
        public string originalStartTimeZone{get;set;}
        public string originalEndTimeZone{get; set;}
        public string iCalUId{get;set;}
        public int reminderMinutesBeforeStart{get;set;}
        public bool isReminderOn{get;set;}
        public bool hasAttachments{get;set;}
        public string subject{get;set;}
        public string bodyPreview{get;set;}
        public string importance{get;set;}
        public string sensitivity{get;set;}
        public bool isAllDay{get;set;}
        public bool isCancelled{get;set;}
        public bool isOrganizer{get;set;}
        public bool responseRequested{get;set;}
        public string seriesMasterId{get;set;}
        public string showAs{get;set;}
        public string type{get;set;}
        public string webLink{get;set;}    
        public string onlineMeetingUrl{get;set;}
        public bool isOnlineMeeting{get;set;}
        public string onlineMeetingProvider{get;set;}
        public bool allowNewTimeProposals{get;set;}
        public string recurrence{get;set;}
        public ResStatus responseStatus{get;set;}
        public BodyType body{get;set;}
        public DateTimeTimeZone start{get;set;}
        public DateTimeTimeZone end{get;set;}
        public LocationType location{get;set;}
        public List<LocationType> locations{get;set;}
        public List<Attendee> attendees{get;set;}
        public OrgMail organizer{get;set;}
        public MeetingOnl onlineMeeting{get;set;}

    }
}