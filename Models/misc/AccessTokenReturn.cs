using Microsoft.Identity.Client;

namespace ITCCLMBSSA_API.Models.misc{
    public class AccessTokenReturn{

        public string userName{get;set;}
        public string AccessToken{get;set;}
        public DateTime expiresOn{get;set;}
    }
}