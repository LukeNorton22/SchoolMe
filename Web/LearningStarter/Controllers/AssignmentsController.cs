using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/assignments")]

    public class AssignmentsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public AssignmentsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpPost("{groupId}/{userId}")]
        public IActionResult Create(int groupId, int userId, [FromBody] AssignmentsCreateDto createDto)
        {
            var response = new Response();

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            var user = _dataContext.Set<User>().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                response.AddError("UserId", "User not found.");
                return UnprocessableEntity(response);
            }

            if (string.IsNullOrEmpty(createDto.AssignmentName))
            {
                response.AddError(nameof(createDto.AssignmentName), "SetName can not be empty");
                return UnprocessableEntity(response);
            }

            var AssignmentToCreate = new Assignments
            {
                GroupId = group.Id,
                UserId = user.Id,
                AssignmentName= createDto.AssignmentName,
            };

            _dataContext.Set<Assignments>().Add(AssignmentToCreate);
            _dataContext.SaveChanges();

            var grades = _dataContext.Set<AssignmentGrade>()
                .Where(x => x.AssignmentId == AssignmentToCreate.Id)
                .Select(x => new AssignmentGradeGetDto
                {
                    Id = x.Id,
                    AssignmentId = x.AssignmentId,
                    Grades = x.Grades,
                    userId = x.userId,
                    userName = x.userName,
                })
                .ToList();

            var AssignmentToReturn = new AssignmentsGetDto
            {
                Id = AssignmentToCreate.Id,
                GroupId = AssignmentToCreate.GroupId,
                AssignmentName = AssignmentToCreate.AssignmentName,
                Grades = grades,
               
                UserId = user.Id,
            };

            response.Data = AssignmentToReturn;
            return Created("", response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext
                .Set<Assignments>()
                .Include(x => x.Grades)
                .Select(assignment => new AssignmentsGetDto
                {
                    Id = assignment.Id,
                    GroupId =assignment.GroupId,
                    AssignmentName = assignment.AssignmentName,
                    Grades = assignment.Grades.Select(x => new AssignmentGradeGetDto
                    {
                        Id=x.Id,
                        AssignmentId=x.Id,
                        Grades = x.Grades , 
                        userId=x.userId,
                        userName=x.userName,

                    }).ToList(),
                })
                    .ToList();
            response.Data = data;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = new Response();

            var data = _dataContext
                .Set<Assignments>()
                .Where(assignment => assignment.Id == id) 
                .Select(assignment => new AssignmentsGetDto
                {
                    Id = assignment.Id,
                    GroupId = assignment.GroupId,
                    AssignmentName = assignment.AssignmentName,
                    Grades = assignment.Grades.Select(x => new AssignmentGradeGetDto
                    {
                        Id = x.Id,
                        AssignmentId = x.AssignmentId,
                        Grades = x.Grades,
                        userId = x.userId,
                        userName = x.userName,


                    }).ToList(),
                })
                .SingleOrDefault(); 

            if (data == null)
            {
                response.AddError("Id", "No test found for the specified Id.");
            }
            else
            {
                response.Data = data;
            }

            return Ok(response);
        }


        [HttpPut("{id}")]
        public IActionResult Update([FromBody] AssignmentsUpdateDto updateDto, int id)
        {
            var response = new Response();

            var assignmentToUpdate = _dataContext.Set<Assignments>()
                .FirstOrDefault(group => group.Id == id);

            if (assignmentToUpdate == null)
            {
                response.AddError("id", "Assignment not found");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            assignmentToUpdate.AssignmentName = updateDto.AssignmentName;

            _dataContext.SaveChanges();

            var assignmentToReturn = new AssignmentsGetDto
            {
                Id = assignmentToUpdate.Id,
                GroupId = assignmentToUpdate.GroupId,
                AssignmentName = assignmentToUpdate.AssignmentName,
            };

            response.Data = assignmentToReturn;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var AssignmentToDelete = _dataContext.Set<Assignments>()
                .FirstOrDefault(assignmentGrade => assignmentGrade.Id == id);

            if (AssignmentToDelete == null)
            {
                response.AddError("id", "Assignment not found");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            _dataContext.Set<Assignments>().Remove(AssignmentToDelete);
            _dataContext.SaveChanges();
            response.Data = true;

            return Ok(response);

        }
    }
}
