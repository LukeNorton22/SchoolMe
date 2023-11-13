using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/Message")]


public class MessageController : ControllerBase
{
    private readonly DataContext _dataContext;
    public MessageController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost("{groupId}")]
    public IActionResult Create(int groupId, [FromBody] MessagesCreateDto createDto)
    {
        var response = new Response();

        if (string.IsNullOrEmpty(createDto.Content))
        {
            response.AddError(nameof(createDto.Content), "Test Name cannot be empty");
            return BadRequest(response); // Return a 400 response with validation errors
        }

        var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);

        if (group == null)
        {
            response.AddError("GroupId", "Group not found.");
            return BadRequest(response);
        }

        var MessageToCreate = new Messages
        {
            GroupId = group.Id,
            Content = createDto.Content
        };

        _dataContext.Set<Messages>().Add(MessageToCreate);
        _dataContext.SaveChanges();

        var MessagesToReturn = new MessagesGetDto
        {
            Id = MessageToCreate.Id,
            GroupId = MessageToCreate.GroupId,
            Content = MessageToCreate.Content,
            CreatedAt = MessageToCreate.CreatedAt
        };

        response.Data = MessagesToReturn;
        return Created("", response);
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
                GroupId=usermessage.GroupId,
                Content = usermessage.Content,
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
                GroupId=usermessage.GroupId,
                Content = usermessage.Content,
                CreatedAt = usermessage.CreatedAt,


            })
            .FirstOrDefault(usermessage => usermessage.Id == id);

        response.Data = data;

        return Ok(response);
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


        _dataContext.SaveChanges();

        var UserMessageToReturn = new MessagesGetDto
        {
            Id = UserMessageToUpdate.Id,
            Content = UserMessageToUpdate.Content,
        };

        response.Data = UserMessageToReturn;

        return Ok(response);
    }

    [HttpDelete("id")]
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
