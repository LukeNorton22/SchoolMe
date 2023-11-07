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
[Route("api/Groups")]

public class GroupController : ControllerBase
{
    private readonly DataContext _dataContext;
    public GroupController(DataContext dataContext)
    {
        _dataContext = dataContext;
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
            })
            .FirstOrDefault();

        response.Data = groupToReturn;

        return Created("", response);
    }

    [HttpPost("groupId/user/userId")]
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

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<Group>()          
            .Select(group => new GroupGetDto
            {
                Id = group.Id,
                GroupName = group.GroupName,
                Description = group.Description,
                Users = group.Users.Select(x => new GroupUserGetDto
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,
                    

                }).ToList(),
                Tests = group.Test.Select(test => new TestsGetDto
                {
                    Id = test.Id,
                    GroupId = test.GroupId,
                    TestName = test.TestName,
                    // Add other test properties as needed
                }).ToList(),
                Messages = group.Messages.Select(Messages => new MessagesGetDto
                {
                    Id = Messages.Id,
                    GroupId= Messages.GroupId,
                    Content = Messages.Content,
                    CreatedAt = Messages.CreatedAt

                }).ToList(),
                FlashCardSets = group.FlashCardSets.Select(flashcardset => new FlashCardSetsGetDto
                {
                    Id = flashcardset.Id,
                    GroupId = flashcardset.GroupId,
                    SetName = flashcardset.SetName,

                }).ToList(),
                Assignments = group.Assignments.Select(assignments => new AssignmentsGetDto
                {
                    Id = assignments.Id,
                    GroupId = assignments.GroupId,
                    AssignmentName = assignments.AssignmentName

                }).ToList()

            }).ToList();

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


                }).ToList(),
                Tests = group.Test.Select(test => new TestsGetDto
                {
                    Id = test.Id,
                    GroupId = test.Group.Id,
                    TestName = test.TestName,

                }).ToList(),
                Messages = group.Messages.Select(Messages => new MessagesGetDto
                {
                    Id = Messages.Id,
                    Content = Messages.Content,
                    GroupId = Messages.Group.Id,
                    CreatedAt = Messages.CreatedAt

                }).ToList(),
                FlashCardSets = group.FlashCardSets.Select(flashcardset => new FlashCardSetsGetDto
                { 
                    Id = flashcardset.Id,
                    GroupId =flashcardset.Group.Id,
                    SetName = flashcardset.SetName,

                }).ToList(),
                Assignments = group.Assignments.Select(assignments => new AssignmentsGetDto 
                { 
                    Id =assignments.Id,
                    GroupId = assignments.Group.Id,
                    AssignmentName = assignments.AssignmentName

                }).ToList()

            })
            .FirstOrDefault(group  => group.Id == id);

        response.Data = data;

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

    [HttpDelete("id")]
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
