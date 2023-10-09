/*using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/PrivateMessages")]

public class PrivateMessageController : ControllerBase
{
    private readonly DataContext _dataContext;
    public PrivateMessageController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<PrivateMessages>()
            .Select(privatemessage => new PrivateMessagesGetDto
            {
                Id = privatemessage.Id,
                Content = privatemessage.Content,
                ImageUrl = privatemessage.ImageUrl,
                CreatedAt = privatemessage.CreatedAt,

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
            .Set<PrivateMessages>()
            .Select(privatemessage => new PrivateMessagesGetDto
            {
                Id = privatemessage.Id,
                Content = privatemessage.Content,
                ImageUrl = privatemessage.ImageUrl,
                CreatedAt = privatemessage.CreatedAt,


            })
            .FirstOrDefault(privatemessage => privatemessage.Id == id);

        response.Data = data;

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] PrivateMessagesCreateDto createDto)
    {
        var response = new Response();

        var PrivateMessageToCreate = new PrivateMessages
        {
            Content = createDto.Content,
            ImageUrl = createDto.ImageUrl,

        };


        _dataContext.Set<PrivateMessages>().Add(PrivateMessageToCreate);

        _dataContext.SaveChanges();

        var PrivateMessageToReturn = new PrivateMessagesGetDto
        {
            Id = PrivateMessageToCreate.Id,
            Content = PrivateMessageToCreate.Content,
            ImageUrl = PrivateMessageToCreate.ImageUrl,
            CreatedAt = PrivateMessageToCreate.CreatedAt,

        };

        response.Data = PrivateMessageToReturn;

        return Created("", response);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromBody] PrivateMessagesUpdateDto updateDto, int id)
    {
        var response = new Response();

        var PrivateMessageToUpdate = _dataContext.Set<PrivateMessages>()
            .FirstOrDefault(privatemessage => privatemessage.Id == id);

        if (PrivateMessageToUpdate == null)
        {
            response.AddError("id", "Message not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        PrivateMessageToUpdate.Content = updateDto.Content;
        PrivateMessageToUpdate.ImageUrl = updateDto.ImageUrl;


        _dataContext.SaveChanges();

        var PrivateMessageToReturn = new PrivateMessagesGetDto
        {
            Id = PrivateMessageToUpdate.Id,
            Content = PrivateMessageToUpdate.Content,
            ImageUrl = PrivateMessageToUpdate.ImageUrl,
        };

        response.Data = PrivateMessageToReturn;

        return Ok(response);
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var PrivateMessageToDelete = _dataContext.Set<PrivateMessages>()
            .FirstOrDefault(privatemessage => privatemessage.Id == id);

        if (PrivateMessageToDelete == null)
        {
            response.AddError("id", "Message not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        _dataContext.Set<PrivateMessages>().Remove(PrivateMessageToDelete);
        _dataContext.SaveChanges();
        response.Data = true;

        return Ok(response);

    }
}
*/