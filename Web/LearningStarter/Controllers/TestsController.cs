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
    [Route("api/Tests")]

    public class TestsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public TestsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

     
        [HttpPost("{groupId}/{userId}")]
        public IActionResult Create(int groupId, int userId, [FromBody] TestsCreateDto createDto)
        {
            var response = new Response();

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            var user = _dataContext.Set<User>().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                response.AddError("UserId", "User not found.");
                return UnprocessableEntity(response);
            }

            if (string.IsNullOrEmpty(createDto.TestName))
            {
                response.AddError(nameof(createDto.TestName), "TestName can not be empty");
                return UnprocessableEntity(response);
            }

            var testToCreate = new Tests
            {
                GroupId = group.Id,
                UserId = user.Id,
                TestName = createDto.TestName,
            };

            _dataContext.Set<Tests>().Add(testToCreate);
            _dataContext.SaveChanges();

            // Include associated flashcards in the response
            var questions = _dataContext.Set<TestQuestions>()
                .Where(x => x.TestId == testToCreate.Id)
                .Select(x => new TestQuestionsGetDto
                {
                    Id = x.Id,
                    TestId = x.TestId,
                    Question = x.Question,
                    Answer = x.Answer,
                })
                .ToList();

            var testToReturn = new TestsGetDto
            {
                Id = testToCreate.Id,
                GroupId = testToCreate.GroupId,
                TestName = testToCreate.TestName,
                Questions = questions,
                UserId = user.Id,
            };

            response.Data = testToReturn;
            return Created("", response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext
                .Set<Tests>()
                .Include(x => x.Questions)
                .Select(Tests => new TestsGetDto
                {
                    Id = Tests.Id,
                    GroupId = Tests.GroupId,
                    TestName = Tests.TestName,
                    UserId = Tests.UserId,
                    Questions = Tests.Questions.Select(x => new TestQuestionsGetDto
                    {
                        Id = x.Id,
                        TestId = x.TestId,
                        Question = x.Question,
                        Answer = x.Answer,

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
                .Set<Tests>()
                .Where(test => test.Id == id) // Add a filter condition here
                .Select(test => new TestsGetDto
                {
                    Id = test.Id,
                    GroupId = test.GroupId,
                    TestName = test.TestName,
                    UserId = test.UserId,
                    Questions = test.Questions.Select(x => new TestQuestionsGetDto
                    {
                        Id = x.Id,
                        TestId = x.TestId,
                        Question = x.Question,
                        Answer = x.Answer,
                    }).ToList(),
                })
                .SingleOrDefault(); // Use SingleOrDefault to fetch a single test or null

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
        public IActionResult Update([FromBody] TestsUpdateDto updateDto, int id)
        {
            var response = new Response();

            var testToUpdate = _dataContext.Set<Tests>()
                .FirstOrDefault(group => group.Id == id);

            if (testToUpdate == null)
            {
                response.AddError("id", "Test not found");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            testToUpdate.TestName = updateDto.TestName;

            _dataContext.SaveChanges();

            var testToReturn = new TestsGetDto
            {
                Id = testToUpdate.Id,
                GroupId = testToUpdate.GroupId,
                TestName = testToUpdate.TestName,
            };

            response.Data = testToReturn;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var TestsToDelete = _dataContext.Set<Tests>()
                .FirstOrDefault(Tests => Tests.Id == id);


            if (TestsToDelete == null)
            {
                response.AddError("id", "Test not found.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }
            _dataContext.Set<Tests>().Remove(TestsToDelete);
            _dataContext.SaveChanges();
            response.Data = true;
            return Ok(response);
        }
    }

}

