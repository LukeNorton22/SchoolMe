using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;

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

    [HttpPost("{TestId}")]
    public IActionResult Create(int TestId, [FromBody] TestQuestionsCreateDto createDto)
    {
        var response = new Response();

        var test = _dataContext.Set<Tests>().FirstOrDefault(x => x.Id == TestId);

        if (test == null)
        {
            return BadRequest("Test can not be found.");
        }

        if (createDto.Question == null)
        {
            response.AddError(nameof(createDto.Question), "Question can not be empty");
            return BadRequest(response);
        }

        var TestQuestionsToCreate = new TestQuestions
        {
            TestId = TestId,
            Question = createDto.Question,
            Answer = createDto.Answer,
        };

        _dataContext.Set<TestQuestions>().Add(TestQuestionsToCreate);
        _dataContext.SaveChanges();

        var TestQuestionsToReturn = new TestQuestionsGetDto
        {
            Id = TestQuestionsToCreate.Id,
            TestId = TestId,
            Question = createDto.Question,
            Answer = createDto.Answer,
        };

        response.Data = TestQuestionsToReturn;
        return Created("", response);
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
                TestId = TestQuestions.TestId,
                Question = TestQuestions.Question,
                Answer = TestQuestions.Answer,
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
            .Set<TestQuestions>()
            .Select(TestQuestions => new TestQuestionsGetDto
            {
                Id = TestQuestions.Id,              
                Question = TestQuestions.Question,
                Answer = TestQuestions.Answer,
            })
            .FirstOrDefault(TestQuestions => TestQuestions.Id == id);

        response.Data = data;
        if (data == null)
        {
            response.AddError("id", "Test questions not found.");
        }
        return Ok(response);

    }

    [HttpPut("id")]
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
            Answer = TestQuestionsToUpdate.Answer,

        };
        response.Data = TestQuestionsToReturn;
        return Ok(response);
    }

    [HttpDelete("id")]
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
