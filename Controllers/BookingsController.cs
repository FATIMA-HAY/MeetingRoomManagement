using System.Net;
using System.Security.Claims;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public List<GetMeetings> get()
        {
            return _storeDBContext.meetings.Select(m => new GetMeetings
            {
                Title = m.Title,
                Agenda = m.Agenda,
                AttendeesNumber = m.AttendeesNumber,
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Status = m.Status,
                RoomId = m.RoomId,
                CreatedBy = m.CreatedBy,
            }).ToList();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public HttpStatusCode post(AddMeeting meet)
        {
            var meeting = new Meetings
            {
                Title = meet.Title,
                Agenda = meet.Agenda,
                StartTime = meet.StartTime,
                EndTime = meet.EndTime,
                CreatedBy = meet.CreatedBy,
                RoomId = meet.RoomId
            };
            _storeDBContext.meetings.Add(meeting);
            _storeDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
        [HttpPut]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult put(int Meetingid, UpdateMeeting meet)
        {
            //bde jib l meeting l ma7juze men ebal l parameter id
            var booking = _storeDBContext.meetings.Find(Meetingid);
            if (booking == null) return NotFound();
            //jib id l user l 3m y3mil l request
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (booking.Id.ToString() == currentUserID)
            {
                var meeting = new Meetings
                {
                    Id=Meetingid,
                    Title = meet.Title,
                    Agenda = meet.Agenda,
                    StartTime = meet.StartTime,
                    EndTime = meet.EndTime,
                    RoomId = meet.RoomId,
                };
                _storeDBContext.meetings.Update(meeting);
                _storeDBContext.SaveChanges();
                return Ok("Booking Updated");

            }
            return NotFound();


        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public HttpStatusCode RemoveBooking(int id)
        {
            var booking = _storeDBContext.meetings.Find(id);
            if (booking == null) return HttpStatusCode.NotFound;
            //jib id l user l 3m y3mil l request
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (booking.Id.ToString() == currentUserID)
            {
                _storeDBContext.meetings.Remove(booking);
                _storeDBContext.SaveChanges();
                return HttpStatusCode.OK;
            };
            return HttpStatusCode.NotFound;
            
        }
    }
}