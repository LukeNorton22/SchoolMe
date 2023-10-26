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
    [Route("api/Assignments")]

    public class AssignmentsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public AssignmentsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public IActionResult Create(int groupId, [FromBody] AssignmentsCreateDto createDto)
        {
            var response = new Response();

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
            if (createDto.AssignmentName == null)
            {
                response.AddError(nameof(createDto.AssignmentName), "SetName can not be empty");
            }

            if (group == null)
            {
                return BadRequest("Group can not be found.");
            }


            var AssignmentsToCreate = new Assignments
            {

                GroupId = group.Id,
                AssignmentName = createDto.AssignmentName,

            };


            if (AssignmentsToCreate == null)
            {
                return BadRequest("FlashCardSet can not be found.");
            }
          


            _dataContext.Set<Assignments>().Add(AssignmentsToCreate);
            _dataContext.SaveChanges();

            var AssignmentsToReturn = new AssignmentsGetDto
            {
                Id = AssignmentsToCreate.Id,

                GroupId = group.Id,
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
                .Include(x => x.Grade)
                .Select(assignment => new AssignmentsGetDto
                {
                    Id = assignment.Id,
                    GroupId =assignment.GroupId,
                    AssignmentName = assignment.AssignmentName,
                    AverageGrade=assignment.AverageGrade,
                    Grade = assignment.Grade.Select(x => new AssignmentGradeGetDto
                    {
                        Id=x.Id,
                        CreatorId = x.CreatorId,
                        AssignmentId=assignment.Id,
                        Grade = x.Grade

                    }).ToList(),
                })
                    .ToList();
            response.Data = data;
            return Ok(response);
        }

        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            var response = new Response();


            var data = _dataContext
                .Set<Assignments>()
                 .Include(x => x.Grade)
                .Select(Assignments => new AssignmentsGetDto
                {
                    Id = Assignments.Id,
                    GroupId=Assignments.GroupId,
                    AverageGrade=Assignments.AverageGrade,
                    AssignmentName = Assignments.AssignmentName,
                    Grade = Assignments.Grade.Select(x => new AssignmentGradeGetDto
                    {
                        Id = x.Id,
                        AssignmentId = x.AssignmentId,
                        Grade = x.Grade

                    }).ToList()
                })
                .FirstOrDefault(Assignments => Assignments.Id == id);

            response.Data = data;
            if (data == null)
            {
                response.AddError("id", "Assignment not found.");
            }
            return Ok(response);

        }

        [HttpPut("id")]
        public IActionResult Update([FromBody] AssignmentsUpdateDto updateDto, int id)
        {
            var response = new Response();
          
            var AssignmentsToUpdate = _dataContext.Set<Assignments>()
                .FirstOrDefault(Assignments => Assignments.Id == id);

            if (AssignmentsToUpdate == null)
            {
                response.AddError("id", "Assignment not found.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            AssignmentsToUpdate.AssignmentName = updateDto.AssignmentName;

            _dataContext.SaveChanges();

            var AssignmentsToReturn = new AssignmentsGetDto
            {
                Id = AssignmentsToUpdate.Id,
                AssignmentName = AssignmentsToUpdate.AssignmentName,
                AverageGrade=AssignmentsToUpdate.AverageGrade,

            };
            response.Data = AssignmentsToReturn;
            return Ok(response);
        }

        [HttpDelete("id")]
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
