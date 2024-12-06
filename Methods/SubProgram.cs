using ITCCLMBSSA_API.Controllers;
using ITCCLMBSSA_API.Models;
using ITCCLMBSSA_API.Models.misc;


//In deze class vind je alle methode's die het werk overzichtelijker maken, door alle methodes maar tot een doel te laten dienen.
public class SubProgramma{


    //Methode die alle beschikbare tijden van de verschillende mensen opstelt
    public List<Compare> FreeFromPlanning(ITCCLMBSSA_API.Models.GetSchedule.Return ret, DateTime startDate, DateTime endDate){
        var compareList = new List<Compare>();
        var disDates = new List<DateTime>();
        disDates.Add(startDate);
        var btw = startDate.AddDays(1);
        while(btw.Date > endDate.Date){
            disDates.Add(btw);
            btw = btw.AddDays(1);
        }
        if(startDate.Date != endDate.Date){
            disDates.Add(endDate);
        }
        // var disDates = ret.value.Select(x => x.scheduleItems.Select(x => x.start.dateTime.Date).Distinct().ToList()).OrderByDescending(y => y.Count()).FirstOrDefault();
        foreach(var item in ret.value){
            if(item.scheduleItems.Count() < 1){
                foreach(var date in disDates){
                    compareList.Add(CompareAdd(item.scheduleId, date,item.workingHours.startTijd,item.workingHours.eindTijd));
                }
            }
            else{
                int i = 1;
                ITCCLMBSSA_API.Models.GetSchedule.ScheduleItem prev = null;
                foreach(var sched in item.scheduleItems){
                    if(sched.end.dateTime.TimeOfDay.Hours == 16){
                    }
                    if(prev == null && i == 1){
                        if(sched.start.dateTime.TimeOfDay > item.workingHours.startTijd){
                            compareList.Add(CompareAdd(item.scheduleId, sched.start.dateTime.Date,item.workingHours.startTijd,sched.start.dateTime.TimeOfDay));
                        }
                    }
                    
                    else {
                        if(sched.start.dateTime.Date == prev.end.dateTime.Date){
                            if(sched.start.dateTime.TimeOfDay > prev.end.dateTime.TimeOfDay){
                                compareList.Add(CompareAdd(item.scheduleId, prev.end.dateTime.Date,prev.end.dateTime.TimeOfDay,sched.start.dateTime.TimeOfDay));                                
                                if(i == item.scheduleItems.Count() && sched.end.dateTime.TimeOfDay < item.workingHours.eindTijd){
                                    compareList.Add(CompareAdd(item.scheduleId, sched.start.dateTime.Date,sched.end.dateTime.TimeOfDay,item.workingHours.eindTijd));                                    
                                }
                            }
                        }
                        else{
                            if(prev.end.dateTime.TimeOfDay < item.workingHours.eindTijd){
                                compareList.Add(CompareAdd(item.scheduleId, prev.end.dateTime.Date,prev.end.dateTime.TimeOfDay,item.workingHours.eindTijd));                                
                            }
                            if(sched.start.dateTime.TimeOfDay > item.workingHours.startTijd){
                                compareList.Add(CompareAdd(item.scheduleId, sched.start.dateTime.Date,item.workingHours.eindTijd,sched.start.dateTime.TimeOfDay));                                
                            }
                        }
                    }


                    prev = sched;
                    i++;
                }
            }
        }
        return compareList;
    }


    
    
    
    
    
    //Methode die kijkt of er overlappende beschikbaarheden zijn
    public  List<Availability> Availabilities(ITCCLMBSSA_API.Models.GetSchedule.Return ret, List<Compare> compareList, TimeSpan time, TimeSpan tijdWeg){
        var users = ret.value.Select(x => x.scheduleId).Distinct().ToList();
        var availability = new List<Availability>();
        foreach(var item in compareList){
            if(availability.Where(x => x.startTijd == item.startTijd && x.eindTijd == item.eindTijd).Count() == 0){
            var sub = new Availability(){
                datum = item.datum,
                startTijd = item.startTijd,
                eindTijd = item.eindTijd,
                attendees = new List<string>(){
                    item.email
                },
                genoegTijd = item.eindTijd - item.startTijd >= time,
                genoegMetReistijd = item.eindTijd - item.startTijd >= time + tijdWeg
            };
            foreach(var user in users.Where(x => x != item.email)){

                int i = 0;
                var planned = ret.value.SingleOrDefault(x => x.scheduleId == user).scheduleItems;

                foreach (var busyTimes in planned){
                    //Checken of de tijden van de meeting overlappen met deze vrijgeroosterde tijd
                    if((busyTimes.start.dateTime.TimeOfDay < item.startTijd && busyTimes.end.dateTime.TimeOfDay > item.startTijd) || (busyTimes.start.dateTime.TimeOfDay < item.eindTijd && busyTimes.end.dateTime.TimeOfDay > item.eindTijd) || (busyTimes.start.dateTime.TimeOfDay > item.startTijd && busyTimes.end.dateTime.TimeOfDay < item.eindTijd) || (busyTimes.start.dateTime.TimeOfDay < item.startTijd && busyTimes.end.dateTime.TimeOfDay > item.eindTijd)){
                                    i = 0;
                                    break;

                        
                    }
                    else{
                        i++;

                    }
                }
                if(i == planned.Count()){
                    sub.attendees.Add(user);
                    if(sub.attendees.Count() == users.Count()){
                        sub.allAvailable = true;
                    }
                }
                else{
                }
            }
            availability.Add(sub);
            }
            
                
        }
        return availability;
    }



    
    
    
    
    
    //Methode die alle beschikbare en gedeeltelijk beschikbare tijden oplevert.
    // public  Availability PrintAvailability(List<Availability> availability){
    //     int i = 1;
    //     if(availability.Where(x => x.allAvailable == false).Count() > 0){
    //     foreach(var ava in availability.Where(x => x.allAvailable == false)){
    //         Console.Write(i + ". " + ava.datum.ToString("d") + "   " + ava.startTijd + "-" + ava.eindTijd + "   Beschikbaar: " );
    //         foreach(var avaUser in ava.attendees){
    //             Console.Write(avaUser);
    //             if(ava.attendees.Last() != avaUser){
    //                 Console.Write(" + ");
    //             }
    //         }
    //         ava.id = i;
    //         i++;
    //     }
    //     }
    //     if(availability.Where(x => x.allAvailable && x.genoegTijd == false).Count() > 0){
    //     foreach(var ava in availability.Where(x => x.allAvailable && x.genoegTijd == false)){
    //         ava.id = i;
    //         i++;
    //     }
    //     }

    //     if(availability.Where(x => x.allAvailable && x.genoegMetReistijd == false && x.genoegTijd == true).Count() > 0){
    //     foreach(var ava in availability.Where(x => x.allAvailable && x.genoegMetReistijd == false && x.genoegTijd == true)){
    //         ava.id = i;
    //         i++;
    //     }
    //     }





    //     if(availability.Where(x => x.allAvailable && x.genoegTijd && x.genoegMetReistijd).Count() > 0){
        
        
    //     foreach(var ava in availability.Where(x => x.allAvailable && x.genoegTijd && x.genoegMetReistijd)){
    //         ava.id = i;
    //         i++;
    //     }
    //     }
        

    //     while(true){   
    //         var selectedTime = Console.ReadLine();
    //         try{
    //             int nummer = int.Parse(selectedTime);
    //             if(nummer > 0 && nummer <= availability.OrderBy(x => x.id).Count()){
    //                 var gekozen = availability.OrderBy(x => x.id).Skip(nummer - 1).First();
    //                 return gekozen;
    //             }
    //             else{
    //                 throw new Exception();
    //             }
    //         }
    //         catch{
    //         }
    //     }

    // }


    //Vraagt om alle emails van mensen die tot de meeting behoren, en de tijden van de meeting, en formateert dit voor de Outlook API call
    public  ITCCLMBSSA_API.Models.GetSchedule.Post EmailPost(string activeEmail, APIAvailability availability){
        var emails = new List<string>
        {
            activeEmail
        };
        foreach(var e in availability.emails){
            emails.Add(e);
        }

        var postItem = new ITCCLMBSSA_API.Models.GetSchedule.Post()
        {
            Schedules = emails,
            StartTime = new DateTimeTimeZone
            {
                dateTime = availability.StartTime,
                timeZone = "W. Europe Standard Zone"
            },
            EndTime = new DateTimeTimeZone
            {
                dateTime = availability.EndTime,
                timeZone = "W. Europe Standard Zone"
            },
            availabilityViewInterval = "15",
        };
        return postItem;
    }


    public  ITCCLMBSSA_API.Models.PostEvent.Post PItem(Availability meeting, string subject, string body, bool online, string location, string showAs){
        var item = new ITCCLMBSSA_API.Models.PostEvent.Post(){
            subject = subject,
            body = new ITCCLMBSSA_API.Models.PostEvent.BodyType(){
                contentType = "html",
                content = body,
            },
            start = new DateTimeTimeZone(){
                dateTime = DateTime.Parse(meeting.datum.ToString("d") + " " + meeting.startTijd),
                timeZone = "W. Europe Standard Time",
            },
            end = new DateTimeTimeZone(){
                dateTime = DateTime.Parse(meeting.datum.ToString("d") + " " + meeting.eindTijd),
                timeZone = "W. Europe Standard Time",
            },
            location = new ITCCLMBSSA_API.Models.PostEvent.Location(){
                displayName = location
            },
            attendees = new List<ITCCLMBSSA_API.Models.PostEvent.Attendees>(),
            allowNewTimeProposals = true,
            isOnlineMeeting = online,
            onlineMeetingProvider = online ? "teamsForBusiness" : null,
            showAs = showAs
        };

        foreach(var at in meeting.attendees){
        
            item.attendees.Add(new ITCCLMBSSA_API.Models.PostEvent.Attendees(){
                type = "required",
                emailAddress = new ITCCLMBSSA_API.Models.PostEvent.MailName(){
                    address = at,
                    name = "naam",
                }
            });
        }

        return item;
    }


    public async Task<List<ITCCLMBSSA_API.Models.PostEvent.Return>> PostEvent(string onderwerp, string body, ITCCLMBSSA_API.Models.misc.Availability gekozenTijdstip, bool online, string locatie, TimeSpan tijdWeg, XOutlookApiController contr, AccessTokenReturn Bearer){
                    //Data formatteren voor Outlook API
                    var pEvent = PItem(gekozenTijdstip, onderwerp, body, online, locatie, "busy");
                    var i = await PostTravelTime(gekozenTijdstip, tijdWeg , onderwerp, body, contr, Bearer);
                    var postEvent = await contr.PostEvent(pEvent, Bearer.AccessToken);
                    if(postEvent == null){
                        throw new Exception("Onbekende foutmelding");
                    }
                    else{
                        i.Add(postEvent);
                        return i;
                    }



    }
    public async Task<List<ITCCLMBSSA_API.Models.PostEvent.Return>> PostTravelTime(ITCCLMBSSA_API.Models.misc.Availability gekozenTijdstip, TimeSpan tijdWeg, string onderwerp, string body, XOutlookApiController contr, AccessTokenReturn Bearer){
        if(tijdWeg.Ticks > 0){
        
        var voortijd = new Availability(){
                                        datum = gekozenTijdstip.datum,
                                        startTijd = gekozenTijdstip.startTijd - tijdWeg,
                                        eindTijd = gekozenTijdstip.startTijd,
                                        attendees = gekozenTijdstip.attendees,
                                        allAvailable = gekozenTijdstip.allAvailable,
                                        genoegTijd = gekozenTijdstip.genoegTijd,
                                    };
                                    var natijd = new Availability(){
                                        datum = gekozenTijdstip.datum,
                                        startTijd = gekozenTijdstip.eindTijd,
                                        eindTijd = gekozenTijdstip.eindTijd + tijdWeg,
                                        attendees = gekozenTijdstip.attendees,
                                        allAvailable = gekozenTijdstip.allAvailable,
                                        genoegTijd = gekozenTijdstip.genoegTijd,
                                    };
                                    var ond = "Reistijd" + onderwerp;
                                    var bod = "Reistijd" + body;
                                    var voor = PItem(voortijd, ond, bod, false, "", "workingElsewhere");
                                    var na = PItem(natijd, ond, bod, false, "", "workingElsewhere");

                                    var pvoor = await contr.PostEvent(voor, Bearer.AccessToken);
                                    if(pvoor != null){
                                    }
                                    var pna = await contr.PostEvent(na, Bearer.AccessToken);
                                    if(pna != null){
                                    }
                                    return new List<ITCCLMBSSA_API.Models.PostEvent.Return>(){
                                        pvoor, 
                                        pna
                                    };
        }
        else{
            return new List<ITCCLMBSSA_API.Models.PostEvent.Return>();
        }
    }

    // public  ITCCLMBSSA_API.Models.misc.Availability LongerThanPlanned(ITCCLMBSSA_API.Models.misc.Availability gekozenTijdstip, TimeSpan tijdWeg, TimeSpan time){
    //     var maxEndTime = gekozenTijdstip.eindTijd - tijdWeg;
    //     var minStartTime = gekozenTijdstip.startTijd + tijdWeg;
    //     var MaxStartTime = maxEndTime - time;
    //     while(true){
    //         var tijdstip = Console.ReadLine();
    //         try{
    //             var tParse = TimeSpan.Parse(tijdstip);
    //             if(tParse >= minStartTime && tParse <= MaxStartTime){
    //                 gekozenTijdstip.startTijd = tParse;
    //                 gekozenTijdstip.eindTijd = gekozenTijdstip.startTijd + time;
    //                 return gekozenTijdstip;
                    
    //             }
    //             else{
    //                 throw new Exception();
    //             }
    //         }
    //         catch{
    //         }
    //     }
    // }
    public static Compare CompareAdd(string email, DateTime datum, TimeSpan starttijd, TimeSpan eindtijd){
            Console.WriteLine("Beschikbaar op " + datum.ToString("d")+ " van " + starttijd + " tot " + eindtijd);
            return new Compare{
                            email = email,
                            datum = datum,
                            startTijd = starttijd,
                            eindTijd = eindtijd,
                        };
        }
}