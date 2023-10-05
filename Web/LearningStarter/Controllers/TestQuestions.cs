using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/TestQuestions")]

public class TestQuestionsController : ControllerBase
{
    private readonly DataContext _dataContext;
    public TestQuestionsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<TestQuestions>()
            .Select(TestQuestions => new TestQuestionsGetDto
            {
                Id = TestQuestions.Id,             
                Question = TestQuestions.Question,
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
            .Set<TestQuestions>()
            .Select(TestQuestions => new TestQuestionsGetDto
            {
                Id = TestQuestions.Id,
               
                Question = TestQuestions.Question,
            })
            .FirstOrDefault(TestQuestions => TestQuestions.Id == id);

        response.Data = data;
        if (data == null)
        {
            response.AddError("id", "Test questions not found.");
        }
        return Ok(response);

    }
    [HttpPost]
    public IActionResult Create([FromBody] TestQuestionsCreateDto createDto)
    {
        var response = new Response();


        var TestQuestionsToCreate = new TestQuestions
        {
            Question = createDto.Question,
             
        };

        _dataContext.Set<TestQuestions>().Add(TestQuestionsToCreate);
        _dataContext.SaveChanges(); 

        var TestQuestionsToReturn = new TestQuestionsGetDto
        {
            Id = TestQuestionsToCreate.Id,
              
                Question = TestQuestionsToCreate.Question,
        };

        response.Data = TestQuestionsToReturn;
        return Created("", response);

    }
    [HttpPut("{id}")]
    public IActionResult Update([FromBody] TestQuestionsUpdateDto updateDto, int id)
    {
        var response = new Response();


        var TestQuestionsToUpdate = _dataContext.Set<TestQuestions>()
            .FirstOrDefault(TestQuestions => TestQuestions.Id == id);

        if (TestQuestionsToUpdate == null)
        {
            response.AddError("id", "Test question not found.");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        TestQuestionsToUpdate.Question = updateDto.Question;

        _dataContext.SaveChanges();

        var TestQuestionsToReturn = new TestQuestionsGetDto
        {
            Id = TestQuestionsToUpdate.Id,
            Question = TestQuestionsToUpdate.Question,

        };
        response.Data = TestQuestionsToReturn;
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var TestQuestionsToDelete = _dataContext.Set<TestQuestions>()
            .FirstOrDefault(TestQuestions => TestQuestions.Id == id);


        if (TestQuestionsToDelete == null)
        {
            response.AddError("id", "Test question not found.");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }
        _dataContext.Set<TestQuestions>().Remove(TestQuestionsToDelete);
        _dataContext.SaveChanges();
        response.Data = true;
        return Ok(response);
    }
}
