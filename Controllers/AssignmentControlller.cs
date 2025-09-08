using System.Net;
using System.Security.Claims;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentControlller : Controller
    {
        public readonly StoreDBContext _dbContext;
        [HttpGet("GetAssignments")]
        public List<AssignmentDto> GetAssignments()
        {
            return _dbContext.assignements.Select(a => new AssignmentDto
            {
                ActionItems = a.ActionItems,
                DueDate = a.DueDate,
                //MomId = a.MomId,
                AssignedTo = a.AssignedTo,
            }).ToList();
        }
        [HttpPost("AddAssignment")]
        [Authorize(Roles = "Admin,Employee")]
      /*  public HttpStatusCode AddAssignment(AssignmentDto assignment)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
            var Assignment = _dbContext.assignements.Include(m => m.Minute).FirstOrDefault(m => m.Id == assignment.MomId);
            if (Assignment == null) return HttpStatusCode.NotFound;
            var minutes = _dbContext.minutes.Include(m => m.Meeting).FirstOrDefault(m => m.Id == Assignment.Minute.MeetingId);
            //l object "Assignment" no3 assignment mech no3 minute lahek lama bde usal la column juwet l minute bde ektub assignment.minute.meetingId
            var CreatedById = _dbContext.meetings.Where(m => m.Id == minutes.MeetingId).Select(c => c.CreatedBy).FirstOrDefault();
            //lzm 7at firstOrdefault la e7sal 3ala value aw by3tine boolen eza true aw false
            if (CreatedById == userId)
            {
                var Newassignment = new Assignements
                {
                    ActionItems = assignment.ActionItems,
                    DueDate = assignment.DueDate,
                    //MomId = assignment.MomId,
                    AssignedTo = assignment.AssignedTo,
                };
                _dbContext.assignements.Add(Newassignment);
                _dbContext.SaveChanges();
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.Unauthorized;
        }*/
        [HttpPut("UpdateAssignment")]
        [Authorize(Roles = "Admin,Employee")]
      /*  public HttpStatusCode UpdateAssignment(int AssignmentId, AssignmentDto assignementDto)
        {
            var UserId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var assignements = _dbContext.assignements.Include(m => m.Minute).FirstOrDefault(m => m.Id == assignementDto.MomId);
            var createdByID = _dbContext.meetings.Where(m => m.Id == assignements.Minute.MeetingId).Select(m => m.CreatedBy).FirstOrDefault();
            if (assignements == null) return HttpStatusCode.NotFound;
            if (createdByID == UserId)
            {
                var UpdatedAssignment = new Assignements
                {
                    Id = AssignmentId,
                    ActionItems = assignementDto.ActionItems,
                    DueDate = assignementDto.DueDate,
                    MomId = assignementDto.MomId,
                    AssignedTo = assignementDto.AssignedTo,
                };
                _dbContext.assignements.Update(UpdatedAssignment);
                _dbContext.SaveChanges();
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.Unauthorized;
        }*/

        [HttpDelete("DeleteAssignment")]
        [Authorize(Roles = "Admin,Employee")]
        public HttpStatusCode DeleteAssignment(int assignmentId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
            var assignment = _dbContext.assignements.Include(a => a.Minute).FirstOrDefault(m => m.Id == assignmentId);
            var createdbyid = _dbContext.meetings.Where(m => m.Id == assignment.Minute.MeetingId).Select(assignment => assignment.CreatedBy).FirstOrDefault();
            var as2 = _dbContext.assignements.Where(a => a.Id == assignmentId).FirstOrDefault();
            if (userId == createdbyid)
            {
                _dbContext.assignements.Remove(as2);
                _dbContext.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            return HttpStatusCode.NotFound;

        }
    }
}
