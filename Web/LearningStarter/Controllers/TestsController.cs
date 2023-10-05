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
    [Route("api/Tests")]

    public class TestsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public TestsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext
                .Set<Tests>()
                .Select(Tests => new TestsGetDto
                {
                   
                    TestName = Tests.TestName,
                    Questions = Tests.Questions.Select(Tests => new TestQuestionsGetDto
                    {
                        Question = Tests.Question,

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
                .Set<Tests>()
                .Select(Tests => new TestsGetDto
                {
                    Id = Tests.Id,
                    TestName = Tests.TestName,
                    Questions = Tests.Questions.Select(Tests => new TestQuestionsGetDto
                    {

                        Question = Tests.Question,

                    }).ToList(),
                })
                .FirstOrDefault(Tests => Tests.Id == id);

            response.Data = data;
            if (data == null)
            {



                response.AddError("id", "Test not found.");
            }
            return Ok(response);

        }
        [HttpPost]
        public IActionResult Create([FromBody] TestsCreateDto createDto)
        {
            var response = new Response();

          
           
            if (createDto.TestName == null)
            {
                response.AddError(nameof(createDto.TestName), "Must enter a valid name");
            }

            var TestsToCreate = new Tests
            {
                TestName = createDto.TestName,
            };

            _dataContext.Set<Tests>().Add(TestsToCreate);
            _dataContext.SaveChanges();

            var TestsToReturn = new TestsGetDto
            {
                TestName = TestsToCreate.TestName,
            };

            response.Data = TestsToReturn;
            return Created("", response);

        }
       
        [HttpPost("{testId}/question/{testquestionId}")]
        public IActionResult AddQuestionToTest(int testId, int testquestionId)
        {
            var response = new Response();

            // Fetch the corresponding test
            var test = _dataContext.Set<Tests>().FirstOrDefault(x => x.Id == testId);

            // Fetch the corresponding test question
            var testQuestion = _dataContext.Set<TestQuestions>().FirstOrDefault(x => x.Id == testquestionId);

            // Check if the test and test question exist
            if (test == null || testQuestion == null)
            {
                return BadRequest("Test or Test Question not found.");
            }

            // Associate the test question with the test
            testQuestion.Tests = test;

            // Add the test question to the data context (if not already added)
            if (!_dataContext.Set<TestQuestions>().Local.Contains(testQuestion))
            {
                _dataContext.Set<TestQuestions>().Add(testQuestion);
            }

            // Save changes to the database
            _dataContext.SaveChanges();

            // Your response logic here

            return Ok(response);
        }



        [HttpPut("{id}")]
        public IActionResult Update([FromBody] TestsUpdateDto updateDto, int id)
        {
            var response = new Response();

           
           
            if (updateDto.TestName == null)
            {
                response.AddError(nameof(updateDto.TestName), "Must enter a valid Name");
            }


            var TestsToUpdate = _dataContext.Set<Tests>()
                .FirstOrDefault(Tests => Tests.Id == id);

            if (TestsToUpdate == null)
            {
                response.AddError("id", "Test not found.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }


            _dataContext.SaveChanges();

            var TestsToReturn = new TestsGetDto
            {
               
                TestName = TestsToUpdate.TestName,

            };
            response.Data = TestsToReturn;
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

