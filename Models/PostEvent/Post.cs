

using ITCCLMBSSA_API.Models.misc;

namespace ITCCLMBSSA_API.Models.PostEvent{
    public class Post{
        public string subject{get;set;}
        public BodyType body{get;set;}
        public DateTimeTimeZone start{get;set;}
        public DateTimeTimeZone end{get;set;}
        public Location location{get;set;}
        public List<Attendees> attendees{get;set;}
        public bool allowNewTimeProposals{get;set;} = true;
        public bool isOnlineMeeting{get;set;}
        public string onlineMeetingProvider{get;set;}
        public string showAs{get;set;}
    }
}