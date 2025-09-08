using System.Net;
using System.Security.Claims;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MeetingRoomManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : Controller
    {
        public readonly StoreDBContext _storeDBContext;
        public BookingsController(StoreDBContext storeDBContext)
        {
            _storeDBContext = storeDBContext;
        }
        [HttpGet("GetBookings")]
        public ActionResult getMeetings()
        {
            var meetings = _storeDBContext.meetings.Include(m => m.Attendee).Select(m => new GetMeetings
            {
                Id = m.Id,
                Title = m.Title,
                Agenda = m.Agenda,
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Status = m.Status,
                RoomId = m.RoomId,
                CreatedBy = m.CreatedBy,
                Attendees = m.Attendee.Select(a => new AttendeesDto
                {
                    Id = a.UserId,
                    Email = a.Email,
                }).ToList()
            });
           /* var MeetingDto = meetings.Select(b => new GetMeetings
            {
                Title = b.Title,
                Agenda = b.Agenda,
                Attendees = b.Attendee != null
            ? b.Attendee.Select(a => new AttendeesDto // Select for each attendee in the collection
            {
                Id = a.Id,
                Email = a.Email ?? "", // Access Email property of the individual Attendee
                MeetingId = a.MeetingId,
                IsPresent = a.IsPresent
            }).ToList()
            : new List<AttendeesDto>(),
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                RoomId = b.RoomId,
                CreatedBy = b.CreatedBy,
            }).ToList();*/
     
            return Ok(new
            {
                success = true,
                meetings,
            });
        }
        [HttpGet("Total-Meeting")]
        public async Task<IActionResult> GetTotalMeeting()
        {
            var count= await _storeDBContext.meetings.Where(m=>m.EndTime> DateTime.Today).CountAsync();
            return Ok(count);
        }//countAsync():3achen ma y3tine kel l m3lumet y3tine bas l count
        [HttpGet("Todays-Meeting")]
        public async Task<ActionResult<int>> GetTodaysMeeting()
        {
            var today = DateTime.Today;
            var start = DateTime.Today;
            var end =start.AddDays(1);
            //men 12 duhur la 12 duhur tene yom
            var count = await _storeDBContext.meetings
                            .Where(m => m.StartTime >= start && m.EndTime < end).CountAsync();
            return Ok(count);
        }
        [HttpGet("Upcoming-Meetings")]
        public async Task<IActionResult> GetUpcomingMeeting()
        {
            var UpcomingMeeting = await _storeDBContext.meetings.Where(m => m.StartTime > DateTime.Now)
                                .Select(m => new
                                {  
                                    m.Id,
                                    m.Title,
                                    m.StartTime,
                                    m.EndTime,
                                   // m.Date,
                                    m.RoomId
                                }).ToListAsync();
            return Ok(new
            {
                success = true,
                UpcomingMeeting
            });
        }
        [HttpPost("AddBookings")]
        [Authorize(Roles = "Admin,Employee")]
        public HttpStatusCode postMeeting(AddMeeting meet)
        {
            var room = _storeDBContext.rooms.FirstOrDefault(r => r.Id == meet.RoomId);
            if (room == null) return HttpStatusCode.NotFound;
            if (room.RoomStatus == "Booked")
            {
                var Meeting=_storeDBContext.meetings.Where(m=>m.RoomId==room.Id && m.StartTime<=DateTime.Now && m.EndTime>=DateTime.Now).FirstOrDefault();
                if(Meeting !=null)return HttpStatusCode.BadRequest;

            }
            var attendees=_storeDBContext.attendees.Where(a=>meet.AttendeesEmail.Contains(a.Email)).ToList();
            var meeting = new Meetings
            {
                Title = meet.Title,
                Agenda = meet.Agenda,
                StartTime = meet.StartTime,
                EndTime = meet.EndTime,
                Date=meet.DATE,
                CreatedBy = meet.CreatedBy,
                RoomId = meet.RoomId,
                Attendee=attendees
            };
            _storeDBContext.attendees.AddRange(attendees);
            _storeDBContext.meetings.Add(meeting);
            room.RoomStatus = "Booked";
            _storeDBContext.rooms.Update(room);
            _storeDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
        [HttpPut("UpdateBookings")]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult putMeeting(int Meetingid, UpdateMeeting meet)
        {
            //bde jib l meeting l ma7juze men ebal l parameter id
            var booking = _storeDBContext.meetings.Include(m => m.Attendee).FirstOrDefaultAsync(m => m.Id == Meetingid);
            if (booking == null) return NotFound();
            //jib id l user l 3m y3mil l request
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (booking.Id.ToString() == currentUserID)
            {
                var newAttendees= _storeDBContext.attendees.Where(a=>meet.AttendeesEmail.Contains(a.Email)).ToList();
                var meeting = new Meetings
                {
                    Id=Meetingid,
                    Title = meet.Title,
                    Agenda = meet.Agenda,
                    StartTime = meet.StartTime,
                    EndTime = meet.EndTime,
                    RoomId = meet.RoomId,
                };
                meeting.Attendee.Clear();
                foreach(var a in newAttendees)meeting.Attendee.Add(a);
                _storeDBContext.SaveChanges();
                return Ok("Booking Updated");

            }
            return NotFound();


        }
        [HttpDelete("DeleteBooking")]
        [Authorize(Roles = "Admin,Employee")]
        public HttpStatusCode RemoveBooking(int id)
        {
            var booking = _storeDBContext.meetings.Find(id);
            if (booking == null) return HttpStatusCode.NotFound;
            var room = _storeDBContext.rooms.FirstOrDefault(r => r.Id == booking.RoomId);
            //jib id l user l 3m y3mil l request
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (booking.CreatedBy.ToString() == currentUserID)
            {   
                _storeDBContext.meetings.Remove(booking);
                 room.RoomStatus = "Available";
                _storeDBContext.rooms.Update(room);
                _storeDBContext.SaveChanges();
                return HttpStatusCode.OK;
            }
            else return HttpStatusCode.NotFound;
            
        }
    }
}