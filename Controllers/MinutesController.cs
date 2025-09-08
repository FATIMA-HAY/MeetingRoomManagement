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
    public class MinutesController : Controller
    {
        public readonly StoreDBContext storeDBContext;
        public MinutesController(StoreDBContext storeDBContext)
        {
            this.storeDBContext = storeDBContext;
        }

        [HttpPost("AddMinutes")]
        public async Task<IActionResult> CreateMinutes([FromBody] AddMinutesDto Minutes)
        {
            var minutes = new Minutes
            {
                MeetingId = Minutes.MeetingId,
                PointOfDisc = Minutes.PointOfDisc,
                Summary = Minutes.Summary,
                Assignments = new List<Assignements>(),
                Attendees = new List<Attendees>()
            };
            storeDBContext.minutes.Add(minutes);
            await storeDBContext.SaveChangesAsync();
            foreach (var Id in Minutes.PresentUserIds)
            {
                var user = await storeDBContext.user.FindAsync(Id);
                if (user == null)
                {
                    // ممكن تتخطى أو ترجع خطأ إذا المستخدم مش موجود
                    return NotFound();
                }
                {
                    var attendee = new Attendees
                    {
                        UserId = Id,
                        Email = user.EMAIL,
                        MeetingId = Minutes.MeetingId,
                        IsPresent = true
                    };
                    minutes.Attendees.Add(attendee);

                }


                foreach (var item in Minutes.Assignments)
                {
                    // Find the attendee assigned to this task by email (from current added attendees)
                    var assignedAttendee = minutes.Attendees.FirstOrDefault(a => a.UserId == item.AssignedTo);

                    var assignment = new Assignements
                    {
                        ActionItems = item.ActionItems,
                        DueDate = item.DueDate,
                        Attendee = assignedAttendee,   // link FK relationship
                        MomId = minutes.Id // Will be assigned after SaveChanges (see note below)
                    };

                    minutes.Assignments.Add(assignment);
                }

                storeDBContext.minutes.Add(minutes);
                await storeDBContext.SaveChangesAsync();
            }
            return Ok(minutes);
        }
    }
}
