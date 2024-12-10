using System.Text.RegularExpressions;
using ITCCLMBSSA_API.Models;
using ITCCLMBSSA_API.Models.misc;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ITCCLMBSSA_API.Controllers{

    [ApiController]
    [Route("[controller]")]
    public class ICSController : ControllerBase{

        [HttpPost]
        [Route("GetAvailability")]
        public async Task<ActionResult<List<Availability>>> GetAvailability(APIAvailability availability, [FromQuery] string? bearerToken){
            try{
            //Checkt of emails correct format zijn
            foreach(var ma in availability.emails){
                if(!RegexExamples.RegexUtilities.IsValidEmail(ma)){
                    return BadRequest("Vul een geldige email in: " + ma);
                }
            }

            var XOAcontr = new XOutlookApiController();
            var SPcontr = new SubProgramma();

            AccessTokenReturn Bearer = null;
            if(bearerToken == null){
                Bearer = await XOAcontr.GetBearerToken();
                
            }
            else{
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(bearerToken);
                try{
                Bearer = new AccessTokenReturn(){
                    AccessToken = bearerToken,
                    userName = jwtSecurityToken.Claims.First(x => x.Type == "upn").Value,
                    expiresOn = jwtSecurityToken.ValidTo
                };
                if(Bearer.expiresOn < DateTime.Now){
                    throw new Exception();
                }
                }
                catch{
                    Bearer = await XOAcontr.GetBearerToken();
                }
            }
            if(!availability.emails.Contains(Bearer.userName)){
                    availability.emails.Add(Bearer.userName);
                }

            //Haalt Bearer token op vanaf Microsoft
            

            //Formateert alle input voor API call
            var postItem = SPcontr.EmailPost(availability);

            //API call naar Outlook API voor data wie wanneer bezet is
            var ret = await XOAcontr.GetSchedule(postItem, Bearer.AccessToken);

            //Wie is wanneer beschikbaar
            var free = SPcontr.FreeFromPlanning(ret, availability.StartTime, availability.EndTime);

            //Wie zijn op dezelfde tijden beschikbaar
            var ava = SPcontr.Availabilities(ret, free, availability.MeetingTime, availability.tijdWeg);
            return ava;
            }
            catch(Exception e){
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("PostEvent")]
        public async Task<ActionResult<List<Models.PostEvent.Return>>> GetPostEvent(Models.PostEvent.APIPost postItem){
            try{
            //Maakt controller aan
            var XOAcontr = new XOutlookApiController();
            var SPcontr = new SubProgramma();
            //Haalt bearer token op
            var Bearer = await XOAcontr.GetBearerToken();
            if(postItem.attendees.Select(x => x.email).Contains(Bearer.userName)){
                postItem.attendees.Add(new PostAttendee{
                    email = Bearer.userName,
                    needTravelTime = true});
            }
            var availability =  new Availability{
                datum = postItem.startTime.Date,
                startTijd = postItem.startTime.TimeOfDay,
                eindTijd = postItem.endTime.TimeOfDay,
                attendees = postItem.attendees.Select(x => x.email).ToList(),
            };
            var travelList = new List<string>();
            foreach(var item in postItem.attendees){
                if(item.needTravelTime){
                    travelList.Add(item.email);
                }
            }
            var list = await SPcontr.PostEvent(postItem.subject, postItem.content, availability, postItem.isOnlineMeeting, postItem.Location, postItem.travelTime, XOAcontr, Bearer, travelList);
            

            return Ok(list);
        }
        catch(Exception e){
            return StatusCode(500, e.Message);
        }
        }
    }
}