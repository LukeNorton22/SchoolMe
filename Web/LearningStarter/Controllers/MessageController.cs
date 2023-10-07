using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/Messages")]


public class MessageController : ControllerBase
{
    private readonly DataContext _dataContext;
    public MessageController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<Messages>()
            .Select(usermessage => new MessagesGetDto
            {
                Id = usermessage.Id,
                Content = usermessage.Content,
                ImageUrl = usermessage.ImageUrl,
                CreatedAt = usermessage.CreatedAt,

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
            .Set<Messages>()
            .Select(usermessage => new MessagesGetDto
            {
                Id = usermessage.Id,
                Content = usermessage.Content,
                ImageUrl = usermessage.ImageUrl,
                CreatedAt = usermessage.CreatedAt,


            })
            .FirstOrDefault(usermessage => usermessage.Id == id);

        response.Data = data;

        return Ok(response);
    }

    
    [HttpPost]
    public IActionResult Create(int groupId, [FromBody] MessagesCreateDto createDto)
    {
        var response = new Response();

        var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
        if (createDto.Content == null)
        {
            response.AddError(nameof(createDto.Content), "Content can not be empty");
        }



        var MessagesToCreate = new Messages
        {
            GroupId = group.Id,
            Content = createDto.Content,
            ImageUrl = createDto.ImageUrl,

        };


        if (MessagesToCreate == null)
        {
            return BadRequest("FlashCardSet can not be found.");
        }
        if (group == null)
        {
            return BadRequest("Group can not be found.");
        }

        if (group == null)
        {
            return BadRequest("Group can not be found.");
        }
        _dataContext.Set<Messages>().Add(MessagesToCreate);
        _dataContext.SaveChanges();

        var MessagesToReturn = new MessagesGetDto
        {
            Id = MessagesToCreate.Id,
            GroupId = MessagesToCreate.GroupId,
            Content =MessagesToCreate.Content,
            ImageUrl = MessagesToCreate.ImageUrl,
            CreatedAt = MessagesToCreate.CreatedAt,
        };

        response.Data = MessagesToReturn;
        return Created("", response);

    }

    [HttpPut("{id}")]
    public IActionResult Update([FromBody] MessagesUpdateDto updateDto, int id)
    {
        var response = new Response();

        var UserMessageToUpdate = _dataContext.Set<Messages>()
            .FirstOrDefault(group => group.Id == id);

        if (UserMessageToUpdate == null)
        {
            response.AddError("id", "Message not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        UserMessageToUpdate.Content = updateDto.Content;
        UserMessageToUpdate.ImageUrl = updateDto.ImageUrl;


        _dataContext.SaveChanges();

        var UserMessageToReturn = new MessagesGetDto
        {
            Id = UserMessageToUpdate.Id,
            Content = UserMessageToUpdate.Content,
            ImageUrl = UserMessageToUpdate.ImageUrl,
        };

        response.Data = UserMessageToReturn;

        return Ok(response);
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var UserMessageToDelete = _dataContext.Set<Messages>()
            .FirstOrDefault(usermessage => usermessage.Id == id);

        if (UserMessageToDelete == null)
        {
            response.AddError("id", "Message not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        _dataContext.Set<Messages>().Remove(UserMessageToDelete);
        _dataContext.SaveChanges();
        response.Data = true;

        return Ok(response);

    }
}
