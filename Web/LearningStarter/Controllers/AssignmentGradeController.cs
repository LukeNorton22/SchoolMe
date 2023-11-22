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
[Route("api/assignmentGrade")]

public class AssignmentGradeController : ControllerBase
{
    private readonly DataContext _dataContext;
    public AssignmentGradeController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost("{AssignmentId}")]
    public IActionResult Create(int AssignmentId, [FromBody] AssignmentGradeCreateDto createDto)
    {
        var response = new Response();

        var assignment = _dataContext.Set<Tests>().FirstOrDefault(x => x.Id == AssignmentId);

        if (assignment == null)
        {
            return BadRequest("Assignment can not be found.");
        }
        if (createDto.Grades > 100 || createDto.Grades < 0)
        {
            return BadRequest("Grade must be between 0-100.");
        }

        var AssignmentGradesToCreate = new AssignmentGrade
        {
            AssignmentId = AssignmentId,
            Grades = createDto.Grades,
            userId = createDto.userId,
            userName=createDto.userName// Assign UserId from the request payload
                                        // You can also assign other properties as needed
        };

        _dataContext.Set<AssignmentGrade>().Add(AssignmentGradesToCreate);
        _dataContext.SaveChanges();

        var TestQuestionsToReturn = new AssignmentGradeGetDto
        {
            Id = AssignmentGradesToCreate.Id,
            AssignmentId = AssignmentId,
            Grades = createDto.Grades,
            userId = AssignmentGradesToCreate.userId,
            userName= AssignmentGradesToCreate.userName// Include UserId in the response DTO
                                                       // You can also include other properties as needed
        };

        response.Data = TestQuestionsToReturn;
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
                AssignmentId=AssignmentGrade.AssignmentId,
                Grades = AssignmentGrade.Grades,               
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
            .Set<AssignmentGrade>()
            .Select(assignmentGrade => new AssignmentGradeGetDto
            {
                Id = assignmentGrade.Id,    
                AssignmentId= assignmentGrade.AssignmentId,
                Grades = assignmentGrade.Grades,
                userId= assignmentGrade.userId,
                userName=assignmentGrade.userName,
              

            })
            .FirstOrDefault(AssignmentGrade => AssignmentGrade.Id == id);

        response.Data = data;
        if (data == null)
        {
            response.AddError("AssignmentId", "AssignmentGrade not found.");
        }
        return Ok(response);
    }

    [HttpPut("{id}")]
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

        
        AssignmentGradeToUpdate.Grades = updateDto.Grades;
        

        _dataContext.SaveChanges();

        var AssignmentGradeToReturn = new AssignmentGradeGetDto
        {
            Id = AssignmentGradeToUpdate.Id,
            AssignmentId = AssignmentGradeToUpdate.AssignmentId,
            Grades = AssignmentGradeToUpdate.Grades,
            
        };

        response.Data = AssignmentGradeToReturn;
        return Ok(response);
    }

    [HttpDelete("{id}")]
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