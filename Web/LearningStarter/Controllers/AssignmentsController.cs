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
                    Name = assignment.Name,
                    AverageGrade=assignment.AverageGrade,
                    Grade = assignment.Grade.Select(x => new AssignmentGradeGetDto
                    {
                        Id=x.Id,
                        AssignmentId=assignment.Id,
                        Grade = x.Grade

                    }).ToList(),
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
                 .Include(x => x.Grade)
                .Select(Assignments => new AssignmentsGetDto
                {
                    Id = Assignments.Id,
                    GroupId=Assignments.GroupId,
                    AverageGrade=Assignments.AverageGrade,
                    Name = Assignments.Name,
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

        [HttpPost]
        public IActionResult Create(int groupId, [FromBody] AssignmentsCreateDto createDto)
        {
            var response = new Response();

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
            if (createDto.Name == null)
            {
                response.AddError(nameof(createDto.Name), "SetName can not be empty");
            }

            if (group == null)
            {
                return BadRequest("Group can not be found.");
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
                AverageGrade=AssignmentsToUpdate.AverageGrade,

            };
            response.Data = AssignmentsToReturn;
            return Ok(response);
        }

       
    }
}
