using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/UserMessages")]

    public class UserMessageController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public UserMessageController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext
                .Set<UserMessages>()
                .Select(product => new UserMessagesGetDto
                {
                    Id = product.Id,
                    Content = product.Content,
                    Group = product.Group,
                    Sender = product.Sender

                })
                .ToList();

            response.Data = data;

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserMessagesCreateDto createDto)
        {
            var response = new Response();

            var messageToCreate = new UserMessages
            {
                Content = createDto.Content,
            };

            _dataContext.Set<UserMessages>().Add(messageToCreate);
            _dataContext.SaveChanges(); 

            var messageToReturn = new UserMessagesGetDto
            {
      
                Content = messageToCreate.Content
             
            };

            response.Data = messageToReturn;

            return Created("", response);
        }
    }
}
