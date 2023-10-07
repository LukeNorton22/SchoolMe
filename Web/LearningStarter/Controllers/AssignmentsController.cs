using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext
                .Set<Assignments>()
                .Select(Assignments => new AssignmentsGetDto
                {
                    Id = Assignments.Id,
                    GroupId =Assignments.GroupId,
                    Name = Assignments.Name,
                    Grade = Assignments.Grade.Select(x => new AssignmentGradeGetDto
                    {
                        Grade = x.Grade

                    }).ToList()
                })
                    .ToList();
            response.Data = data;
            return Ok(response);
        }
        [HttpGet("({id}")]
        public IActionResult GetById(int id)
        {
            var response = new Response();


            var data = _dataContext
                .Set<Assignments>()
                .Select(Assignments => new AssignmentsGetDto
                {
                    Id = Assignments.Id,
                    Name = Assignments.Name,
                    Grade = Assignments.Grade.Select(x => new AssignmentGradeGetDto
                    {
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

        [HttpPost]
        public IActionResult Create(int groupId, [FromBody] AssignmentsCreateDto createDto)
        {
            var response = new Response();

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
            if (createDto.Name == null)
            {
                response.AddError(nameof(createDto.Name), "SetName can not be empty");
            }



            var AssignmentsToCreate = new Assignments
            {

                GroupId = group.Id,
                Name = createDto.Name,

            };


            if (AssignmentsToCreate == null)
            {
                return BadRequest("FlashCardSet can not be found.");
            }
            if (group == null)
            {
                return BadRequest("Group can not be found.");
            }


            _dataContext.Set<Assignments>().Add(AssignmentsToCreate);
            _dataContext.SaveChanges();

            var AssignmentsToReturn = new AssignmentsGetDto
            {
                Id = AssignmentsToCreate.Id,

                GroupId = group.Id,
                Name = AssignmentsToCreate.Name,
            };

            response.Data = AssignmentsToReturn;
            return Created("", response);

        }
        [HttpPut("{id}")]
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

            AssignmentsToUpdate.Name = updateDto.Name;

            _dataContext.SaveChanges();

            var AssignmentsToReturn = new AssignmentsGetDto
            {
                Id = AssignmentsToUpdate.Id,
                Name = AssignmentsToUpdate.Name,

            };
            response.Data = AssignmentsToReturn;
            return Ok(response);
        }

        [HttpPost("{AssignmentId}/grade/{AssignmentGradeId}")]
        public IActionResult AddAssignmentGradeToAssignment(int AssignmentId, int AssignmentGradeId)
        {
            var response = new Response();

            // Fetch the corresponding Assignment
            var assignment = _dataContext.Set<Assignments>().FirstOrDefault(x => x.Id == AssignmentId);

            // Fetch the corresponding Assignment AssignmentGrade
            var assignmentGrade = _dataContext.Set<AssignmentGrade>().FirstOrDefault(x => x.Id == AssignmentGradeId);

            if (assignment == null || assignmentGrade == null)
            {
                return BadRequest("Test or Test Question not found.");
            }


            // Associate the AssignmentGrade with the Assignment
            assignmentGrade.Assignments = assignment;

            // Add the AssignmentGrade to the data context (if not already added)
            if (!_dataContext.Set<AssignmentGrade>().Local.Contains(assignmentGrade))
            {
                _dataContext.Set<AssignmentGrade>().Add(assignmentGrade);
            }
            response.Data = new AssignmentsGetDto
            {
                Id = AssignmentId,
                Name = assignment.Name,
                
                Grade = assignment.Grade.Select(x => new AssignmentGradeGetDto
                {
                    Id = assignmentGrade.Id,
                    Grade = assignmentGrade.Grade,
                 

                }).ToList()
            };
            // Save changes to the database
            _dataContext.SaveChanges();

            // Your response logic here
            return Ok(response);
        }
    }
}
