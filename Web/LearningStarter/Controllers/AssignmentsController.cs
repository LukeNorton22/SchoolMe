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

                    Name = Assignments.Name,
                    AverageGrade = Assignments.AverageGrade,
                    GroupId = Assignments.GroupId,
                    Grades = Assignments.Grades.Select(x => new AssignmentGradeGetDto
                    {


                        Grade = Assignments.Grade,



                    }).ToList()
                });
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

                    CreatedById = Assignments.CreatedById,
                    SetName = Assignments.SetName,
                    AssignmentGrade = Assignments.AssignmentGrade.Select(x => new AssignmentsFlashCardGetDto
                    {
                        Id = x.AssignmentGrade.Id,
                        AssignmentId = x.AssignmentGrade.AssignmentId,
                        Grade = x.AssignmentGrade.Grade,



                    }).ToList()
                })
                .FirstOrDefault(Assignments => Assignments.Id == id);

            response.Data = data;
            if (data == null)
            {



                response.AddError("id", "FlashCard not found.");
            }
            return Ok(response);

        }

        [HttpPost]
        public IActionResult Create([FromBody] AssignmentsCreateDto createDto)
        {
            var response = new Response();

            if (createDto.CreatedById < 0)
            {
                response.AddError(nameof(createDto.CreatedById), "CreatedById must be valid");
            }
            if (createDto.SetName == null)
            {
                response.AddError(nameof(createDto.SetName), "SetName can not be empty");
            }



            var AssignmentsToCreate = new Assignments
            {

                CreatedById = createDto.CreatedById,
                SetName = createDto.SetName,
            };

            _dataContext.Set<Assignments>().Add(AssignmentsToCreate);
            _dataContext.SaveChanges();

            var AssignmentsToReturn = new AssignmentsGetDto
            {
                Id = AssignmentsToCreate.Id,

                CreatedById = AssignmentsToCreate.CreatedById,
                SetName = AssignmentsToCreate.SetName,
            };

            response.Data = AssignmentsToReturn;
            return Created("", response);

        }
        [HttpPut("{id}")]
        public IActionResult Update([FromBody] AssignmentsUpdateDto updateDto, int id)
        {
            var response = new Response();

            if (updateDto.CreatedById < 0)
            {
                response.AddError(nameof(updateDto.CreatedById), "CreatedById must be valid");
            }
            if (updateDto.SetName == null)
            {
                response.AddError(nameof(updateDto.SetName), "SetName can not be empty");
            }


            var AssignmentsToUpdate = _dataContext.Set<Assignments>()
                .FirstOrDefault(Assignments => Assignments.Id == id);

            if (AssignmentsToUpdate == null)
            {
                response.AddError("id", "FlashCard not found.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            AssignmentsToUpdate.SetName = updateDto.SetName;
            AssignmentsToUpdate.CreatedById = updateDto.CreatedById;

            _dataContext.SaveChanges();

            var AssignmentsToReturn = new AssignmentsGetDto
            {
                Id = AssignmentsToUpdate.Id,

                SetName = AssignmentsToUpdate.SetName,
                CreatedById = AssignmentsToUpdate.CreatedById,

            };
            response.Data = AssignmentsToReturn;
            return Ok(response);
        }

        [HttpPost("{AssignmentId}/AssignmentGrade/{AssignmentAssignmentGradeId}")]
        public IActionResult AddAssignmentGradeToAssignment(int AssignmentId, int AssignmentGradeId)
        {
            var response = new Response();

            // Fetch the corresponding Assignment
            var Assignment = _dataContext.Set<Assignments>().FirstOrDefault(x => x.Id == AssignmentId);

            // Fetch the corresponding Assignment AssignmentGrade
            var asgnmentGrade = _dataContext.Set<AssignmentGrade>().FirstOrDefault(x => x.Id == AssignmentGradeId);
            
            // Check if the Assignment and Assignment AssignmentGrade exist
            if (Assignment == null || AssignmentGrade == null)
            {
                return BadRequest("Assignment or Assignment AssignmentGrade not found.");
            }

            // Associate the Assignment AssignmentGrade with the Assignment
            AssignmentGrade.Assignments =Assignment;

            // Add the Assignment AssignmentGrade to the data context (if not already added)
            if (!_dataContext.Set<AssignmentGrade>().Local.Contains(AssignmentGrade))
            {
                _dataContext.Set<AssignmentGrades>().Add(AssignmentGrade);
            }

            // Save changes to the database
            _dataContext.SaveChanges();

            // Your response logic here

            return Ok(response);
        }
    }
}
