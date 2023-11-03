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

        [HttpPost]
        public IActionResult Create(int groupId, [FromBody] TestsCreateDto createDto)
        {
            var response = new Response();

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
            if (createDto.TestName == null)
            {
                response.AddError(nameof(createDto.TestName), "Test Name can not be empty");
            }

            if (group == null)
            {
                return BadRequest("Group can not be found.");
            }

            var TestsToCreate = new Tests
            {
                GroupId = group.Id,
                TestName = createDto.TestName      
            };


            if (TestsToCreate == null)
            {
                return BadRequest("Test can not be found.");
            }



            _dataContext.Set<Tests>().Add(TestsToCreate);
            _dataContext.SaveChanges();

            var TestsToReturn = new TestsGetDto
            {
                Id = TestsToCreate.Id,
                GroupId = TestsToCreate.GroupId,
                TestName = TestsToCreate.TestName,
            };

            response.Data = TestsToReturn;
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

        [HttpGet("{GroupId}")]
        public IActionResult GetByGroupId(int GroupId)
        {
            var response = new Response();

            var data = _dataContext
                .Set<Tests>()
                .Where(test => test.GroupId == GroupId) // Filter by GroupId
                .Select(test => new TestsGetDto
                {
                    Id = test.Id,
                    GroupId = test.GroupId,
                    TestName = test.TestName,
                    Questions = test.Questions.Select(x => new TestQuestionsGetDto
                    {
                        Id = x.Id,
                        TestId = x.TestId,
                        Question = x.Question,
                        Answer = x.Answer,
                    }).ToList(),
                })
                .ToList();

            if (data.Count == 0)
            {
                response.AddError("GroupId", "No tests found for the specified GroupId.");
            }
            else
            {
                response.Data = data;
            }

            return Ok(response);
        }


        [HttpPut("id")]
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

        [HttpDelete("id")]
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

