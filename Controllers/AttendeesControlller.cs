using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendeesControlller : Controller
    {
        public StoreDBContext StoreDBContext { get; set; }
        public AttendeesControlller(StoreDBContext storeDBContext)
        {
            StoreDBContext = storeDBContext;
        }

        [HttpGet("GetAttendees")]
        public List<AttendeesDto>  GetAttendees()
        {
            return StoreDBContext.attendees.Select(x => new AttendeesDto
            {
                Id=x.Id ,
                MeetingId = x.MeetingId,
                IsPresent= x.IsPresent,
            }).ToList();
            
        }
        [HttpPost("AddAttendees")]
        [Authorize(Roles = "Admin,Employee")]
        public HttpStatusCode PostAttendees(AttendeesDto attendees) {
            var MeetingCreatedById = int.Parse(User.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.NameIdentifier).Value);
            var meeting = StoreDBContext.meetings.FirstOrDefault(m => m.Id == attendees.MeetingId);
            if(meeting == null) return HttpStatusCode.NotFound;
            if (meeting.CreatedBy != MeetingCreatedById)return HttpStatusCode.Unauthorized;
            
            var NewAttendee = new Attendees
                {
                    Id = attendees.Id,
                    MeetingId = attendees.MeetingId,
                    IsPresent = attendees.IsPresent,
                };
            StoreDBContext.attendees.Add(NewAttendee);
            StoreDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
        [HttpPut("UpdateAttendees")]
        [Authorize(Roles ="Admin,Employee")]
        public HttpStatusCode PutAttendees(int AttendeeId, AttendeesDto attendees)
        {
            var MeetingCreatedById=int.Parse(User.Claims.FirstOrDefault(c=>c.Type ==ClaimTypes.NameIdentifier).Value);
            var meeting=StoreDBContext.meetings.FirstOrDefault(m=> m.Id==attendees.MeetingId);
            if(meeting == null)return HttpStatusCode.NotFound;
            if(meeting.CreatedBy != MeetingCreatedById) return HttpStatusCode.Unauthorized;
            var attendee=StoreDBContext.attendees.FirstOrDefault(c=> c.Id == AttendeeId);
            if (attendee == null) return HttpStatusCode.NotFound;
            var UpdatedAttendee = new Attendees
            {
                Id = attendees.Id,
                MeetingId = attendees.MeetingId,
                IsPresent = attendees.IsPresent,

            };
            StoreDBContext.attendees.Update(UpdatedAttendee);
            StoreDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
        [HttpDelete("DeleteAttendee")]
        [Authorize(Roles = "Admin,Employee")]
        public HttpStatusCode DeleteAttendees(int AttendeeId)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
            var attendee= StoreDBContext.attendees.Include(a=>a.Meeting).FirstOrDefault(a=>a.Id==AttendeeId);
            if(attendee == null) return HttpStatusCode.NotFound;
            var createdById=attendee.Meeting.CreatedBy;
            if (createdById != UserId) return HttpStatusCode.Unauthorized;
            StoreDBContext.attendees.Remove(attendee);
            StoreDBContext.SaveChanges();
            return HttpStatusCode.OK;
            /*fine a3mil:
             var Attendee= storeDbContext.attendees.firstOrDefault(a => a.id==AttendeeId);
             var createdById= storeDbContext.Meeting.where(m=>m.id==Attendee.meetingId).select(m => m.CreatedBy).firstOrDefault();
            */
        }
    }

}
