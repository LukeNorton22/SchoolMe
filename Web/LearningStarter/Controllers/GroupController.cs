using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/groups")]

public class GroupController : ControllerBase
{
    private readonly DataContext _dataContext;
    public GroupController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost("{groupId}/tests")]
    public IActionResult CreateTestInGroup(int groupId, [FromBody] TestsCreateDto testCreateDto)
    {
        var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);

        if (group == null)
        {
            return NotFound("Group not found.");
        }

        if (testCreateDto == null)
        {
            return BadRequest("Invalid test data.");
        }

        // Create a new test entity and associate it with the group
        var newTest = new Tests
        {
            GroupId = groupId,
            TestName = testCreateDto.TestName,
            // Set other properties as needed
        };

        _dataContext.Set<Tests>().Add(newTest); 
        _dataContext.SaveChanges();

        // Return a response, e.g., the newly created test
        return Ok(newTest);
    }
    [HttpPost("{groupId}/FlashCardSets")]
    public IActionResult CreateFlashCardSetInGroup(int groupId, [FromBody] FlashCardSetsCreateDto flashcardSetCreateDto)
    {
        var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);

        if (group == null)
        {
            return NotFound("Group not found.");
        }

        if (flashcardSetCreateDto == null)
        {
            return BadRequest("Invalid test data.");
        }

        // Create a new test entity and associate it with the group
        var newFlashCardSet = new FlashCardSets
        {
            GroupId = groupId,
            SetName = flashcardSetCreateDto.SetName,
            // Set other properties as needed
        };

        _dataContext.Set<FlashCardSets>().Add(newFlashCardSet);
        _dataContext.SaveChanges();

        // Return a response, e.g., the newly created test
        return Ok(newFlashCardSet);
    }

    [HttpPost("{groupId}/messages")]
    public IActionResult CreateMessageInGroup(int groupId, [FromBody] MessagesCreateDto messagesCreateDto)
    {
        var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);

        if (group == null)
        {
            return NotFound("Group not found.");
        }

        if (messagesCreateDto == null)
        {
            return BadRequest("Invalid message data.");
        }

        var newMessage = new Messages
        {
            GroupId= groupId,
            Content = messagesCreateDto.Content,
        };

        _dataContext.Set<Messages>().Add(newMessage);
        _dataContext.SaveChanges();

        // Return a response, e.g., the newly created test
        return Ok(newMessage);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<Group>()
            .Include(x => x.Test)
            .Include(x => x.Users)
            .Include(x => x.Messages)
            .Include(x=> x.FlashCardSets)
            
            .Select(group => new GroupGetDto
            {
                Id = group.Id,
                GroupName = group.GroupName,
                Description = group.Description,
                Messages = group.Messages.Select(x => new MessagesGetDto
                {
                    Id = x.Id,
                    Content = x.Content,

                }).ToList(),
                Users = group.Users.Select(x => new GroupUserGetDto
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,

                }).ToList(),
                Tests = group.Test.Select(x => new TestsGetDto
                {
                    Id=x.Id,
                    TestName = x.TestName,

                }).ToList(),
                FlashCardSets = group.FlashCardSets.Select(x => new FlashCardSetsGetDto
                {
                    Id=x.Id,
                    SetName = x.SetName,
                  

                }).ToList(),
            })

            .ToList();

        response.Data = data;

        return Ok(response);
    }


    [HttpGet ("{id}")]
    public IActionResult GetById(int id)
    {
        var response = new Response();
        var data = _dataContext
            .Set<Group>()
            .Select(group => new GroupGetDto
            {
                Id = group.Id,
                GroupName = group.GroupName,
                Description = group.Description,
                Users = group.Users.Select(x=> new GroupUserGetDto
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,


                }).ToList()

            
            })
            .FirstOrDefault(group  => group.Id == id);

        response.Data = data;

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] GroupCreateDto createDto)
    {
        var response = new Response();

        if (string.IsNullOrEmpty(createDto.GroupName))
        {
            response.AddError(nameof(createDto.GroupName), "Group name can't be empty");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        var groupToCreate = new Group
        {
            GroupName = createDto.GroupName,
            Description = createDto.Description,
        };

        _dataContext.Set<Group>().Add(groupToCreate);
        _dataContext.SaveChanges();

        var groupToReturn = _dataContext
            .Set<Group>()
            .Select(g => new Group
            {
                Id=g.Id,
                GroupName = g.GroupName,
                Description = g.Description,
                // Add more properties here as needed
            })
            .FirstOrDefault();

        response.Data = groupToReturn;

        return Created("", response);
    }


    [HttpPost("{groupId}/user/{userId}")]
    public IActionResult AddUserToGroup(int groupId, int userId)
    {
        var response = new Response();
        var group = _dataContext.Set<Group>()
            .FirstOrDefault(x=> x.Id == groupId);
        var user = _dataContext.Set<User>()
            .FirstOrDefault(x=> x.Id == userId);
        if(group == null)
        {
            response.AddError("id", "Group not found.");
        }
        if (user == null)
        {
            response.AddError("id", "User not found.");
        }
        var groupUser = new GroupUser
        {
            Group = group,
            User = user,

        };
        if (response.HasErrors)
        {
            return BadRequest(response);
        }
        _dataContext.Set<GroupUser>().Add(groupUser);
        _dataContext.SaveChanges();

        response.Data = new GroupGetDto
        {
            Id = groupId,
            GroupName = group.GroupName,
            Description = group.Description,
            Users = group.Users.Select(x => new GroupUserGetDto
            {
                Id = x.User.Id,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                UserName = x.User.UserName,
            }).ToList()
        };
        return Ok(response);
    }
 

    [HttpPut("{id}")]
    public IActionResult Update([FromBody] GroupUpdateDto updateDto, int id)
    {
        var response = new Response();

        var groupToUpdate = _dataContext.Set<Group>()
            .FirstOrDefault(group => group.Id == id);

        if (groupToUpdate == null)
        {
            response.AddError("id", "Group not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        } 

        groupToUpdate.GroupName = updateDto.GroupName;
        groupToUpdate.Description = updateDto.Description;

        _dataContext.SaveChanges();

        var groupToReturn = new GroupGetDto
        {
            Id = groupToUpdate.Id,
            GroupName = groupToUpdate.GroupName,
            Description = groupToUpdate.Description,
        };

        response.Data = groupToReturn;
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var groupToDelete = _dataContext.Set<Group>()
            .FirstOrDefault(group => group.Id == id);

        if(groupToDelete == null)
        {
            response.AddError("id", "Group not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        _dataContext.Set<Group>().Remove(groupToDelete);
        _dataContext.SaveChanges();
        response.Data = true;

        return Ok(response);

    }

    

}
