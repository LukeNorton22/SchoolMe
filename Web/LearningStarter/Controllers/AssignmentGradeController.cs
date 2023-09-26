using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

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

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<AssignmentGrade>()
            .Select(product => new AssignmentGradeGetDto
            {
                Id = product.Id,
                Name = product.Name,
                Grade = product.Grade,
                AverageGrade = product.AverageGrade

            })
            .ToList();

        response.Data = data;

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] AssignmentGradeCreateDto createDto)
    {
        var response = new Response();

        var AssignmentGradeToCreate = new AssignmentGrade
        {
            Name = createDto.Name,
            Grade = createDto.Grade,
            AverageGrade = createDto.AverageGrade
        };

        _dataContext.Set<AssignmentGrade>().Add(AssignmentGradeToCreate);
        _dataContext.SaveChanges();

        var AssignmentGradeToReturn = new AssignmentGradeGetDto
        {
           Name = createDto.Name,
           Grade = createDto.Grade,
           AverageGrade = createDto.AverageGrade
        };

        response.Data = AssignmentGradeToReturn;

        return Created("", response);
    }

}