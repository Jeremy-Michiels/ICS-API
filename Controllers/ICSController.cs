using System.Text.RegularExpressions;
using ITCCLMBSSA_API.Models;
using ITCCLMBSSA_API.Models.misc;
using Microsoft.AspNetCore.Mvc;

namespace ITCCLMBSSA_API.Controllers{

    [ApiController]
    [Route("[controller]")]
    public class ICSController : ControllerBase{

        [HttpPost]
        [Route("GetAvailability")]
        public async Task<ActionResult<List<Availability>>> GetAvailability(APIAvailability availability){
            try{
            //Checkt of emails correct format zijn
            foreach(var ma in availability.emails){
                if(!RegexExamples.RegexUtilities.IsValidEmail(ma)){
                    return BadRequest("Vul een geldige email in: " + ma);
                }
            }

            var XOAcontr = new XOutlookApiController();
            var SPcontr = new SubProgramma();

            //Haalt Bearer token op vanaf Microsoft
            var Bearer = await XOAcontr.GetBearerToken();
            if(!availability.emails.Contains(Bearer.userName)){
                availability.emails.Add(Bearer.userName);
            }

            //Formateert alle input voor API call
            var postItem = SPcontr.EmailPost(Bearer.userName, availability);

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
            if(postItem.attendees.Contains(Bearer.userName)){
                postItem.attendees.Add(Bearer.userName);
            }
            var availability =  new Availability{
                datum = postItem.startTime.Date,
                startTijd = postItem.startTime.TimeOfDay,
                eindTijd = postItem.endTime.TimeOfDay,
                attendees = postItem.attendees,
            };
            var list = await SPcontr.PostEvent(postItem.subject, postItem.content, availability, postItem.isOnlineMeeting, postItem.Location, postItem.travelTime, XOAcontr, Bearer);
            

            return Ok(list);
        }
        catch(Exception e){
            return StatusCode(500, e.Message);
        }
        }
    }
}