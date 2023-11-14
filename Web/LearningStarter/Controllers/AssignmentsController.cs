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

        [HttpPost("{groupId}")]
        public IActionResult Create(int groupId, [FromBody] AssignmentsCreateDto createDto)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(createDto.AssignmentName))
            {
                response.AddError(nameof(createDto.AssignmentName), "Assignment Name cannot be empty");
                return BadRequest(response); // Return a 400 response with validation errors
            }

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);

            if (group == null)
            {
                response.AddError("GroupId", "Group not found.");
                return BadRequest(response);
            }

            var AssignmentsToCreate = new Assignments
            {
                GroupId = group.Id,
                AssignmentName = createDto.AssignmentName,
                
            };

            _dataContext.Set<Assignments>().Add(AssignmentsToCreate);
            _dataContext.SaveChanges();

            var AssignmentsToReturn = new AssignmentsGetDto
            {
                Id = AssignmentsToCreate.Id,
                GroupId = AssignmentsToCreate.GroupId,
                AssignmentName = AssignmentsToCreate.AssignmentName,
            };

            response.Data = AssignmentsToReturn;
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
                        Grades = x.Grades   

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
