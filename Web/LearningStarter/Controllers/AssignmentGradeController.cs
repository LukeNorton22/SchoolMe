using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/AssignmentGrade")]

public class AssignmentGradeController : ControllerBase
{
    private readonly DataContext _dataContext;
    public AssignmentGradeController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public IActionResult Create(int assignmentId, int creatorId, [FromBody] AssignmentGradeCreateDto createDto)
    {
        var response = new Response();
        var assignment = _dataContext.Set<Assignments>().FirstOrDefault(x => x.Id == assignmentId);
        if (createDto.Grade <0)
        {
            response.AddError(nameof(createDto.Grade), "Must enter a valid Grade");
        }
        if (assignment == null)
        {
            return BadRequest("Assignment can not be found.");
        }
        var creator = _dataContext.Set<User>().FirstOrDefault(y => y.Id == creatorId);
        if (creator == null)
        {
            return BadRequest("Creator user not found.");
        }

        var AssignmentGradeToCreate = new AssignmentGrade 
        {

            AssignmentId = assignment.Id,
            CreatorId = creatorId,
            Grade = createDto.Grade,

        };
        if (AssignmentGradeToCreate == null)
        {
            return BadRequest("AssignmentGrade can not be found.");
        }
        _dataContext.Set<AssignmentGrade>().Add(AssignmentGradeToCreate);
        _dataContext.SaveChanges();
        var AssignmentGradeToReturn = new AssignmentGradeGetDto
        {
           Id = AssignmentGradeToCreate.Id,
           CreatorId=creator.Id,
           AssignmentId = assignment.Id,
           Grade = AssignmentGradeToCreate.Grade, 
        };

        response.Data = AssignmentGradeToReturn;

        return Created("", response);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<AssignmentGrade>()
            .Select(AssignmentGrade => new AssignmentGradeGetDto
            {
                Id = AssignmentGrade.Id,
                CreatorId = AssignmentGrade.CreatorId,
                AssignmentId=AssignmentGrade.AssignmentId,
                Grade = AssignmentGrade.Grade,               
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
            .Set<AssignmentGrade>()
            .Select(assignmentGrade => new AssignmentGradeGetDto
            {
                Id = assignmentGrade.Id,    
                CreatorId = assignmentGrade.CreatorId,
                AssignmentId= assignmentGrade.AssignmentId,
                Grade = assignmentGrade.Grade,
              

            })
            .FirstOrDefault(AssignmentGrade => AssignmentGrade.Id == id);

        response.Data = data;
        if (data == null)
        {
            response.AddError("id", "AssignmentGrade not found.");
        }
        return Ok(response);
    }

    [HttpPut("id")]
    public IActionResult Update([FromBody] AssignmentGradeUpdateDto updateDto, int id)
    {
        var response = new Response();

        var AssignmentGradeToUpdate = _dataContext.Set<AssignmentGrade>()
            .FirstOrDefault(assignmentGrade => assignmentGrade.Id == id);

        if (AssignmentGradeToUpdate == null)
        {
            response.AddError("id", "Assignment not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        
        AssignmentGradeToUpdate.Grade = updateDto.Grade;
        

        _dataContext.SaveChanges();

        var AssignmentGradeToReturn = new AssignmentGradeGetDto
        {
            Id = AssignmentGradeToUpdate.Id,
            CreatorId = AssignmentGradeToUpdate.CreatorId,
            AssignmentId = AssignmentGradeToUpdate.AssignmentId,
            Grade = AssignmentGradeToUpdate.Grade,
            
        };

        response.Data = AssignmentGradeToReturn;
        return Ok(response);
    }

    [HttpDelete("id")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var AssignmentGradeToDelete = _dataContext.Set<AssignmentGrade>()
            .FirstOrDefault(assignmentGrade => assignmentGrade.Id == id);

        if (AssignmentGradeToDelete == null)
        {
            response.AddError("id", "Assignment not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        _dataContext.Set<AssignmentGrade>().Remove(AssignmentGradeToDelete);
        _dataContext.SaveChanges();
        response.Data = true;

        return Ok(response);

    }


}