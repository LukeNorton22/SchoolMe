using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
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

    [HttpPost("{groupId}")]
    public IActionResult AddUserToGroup(int groupId, [FromBody] GroupUserUpdateDto userData)
    {
        var response = new Response();
        Console.WriteLine("Received userName:", userData.userName);
        try
        {
            var group = _dataContext.Set<Group>()
                .Include(g => g.Users)
                .FirstOrDefault(x => x.Id == groupId);

            if (group == null)
            {
                response.AddError("groupId", "Group not found.");
                return BadRequest(response);
            }

            var user = _dataContext.Set<User>()
                .FirstOrDefault(x => x.UserName == userData.userName);

            if (user == null)
            {
                response.AddError("userName", "User not found.");
                return BadRequest(response);
            }

            if (group == null)
            {
                response.AddError("groupId", "Group not found.");
                return BadRequest(response);
            }

            // Check if Users collection is null
            if (group.Users == null)
            {
                response.AddError("groupId", "Group users not found.");
                return BadRequest(response);
            }

            // Check if the user is already in the group
            if (group.Users.Any(u => u.User != null && u.User.UserName == userData.userName))
            {
                response.AddError("userName", "User is already in the group.");
                return BadRequest(response);
            }
            var groupUser = new GroupUser
            {
                Group = group,
                User = user
            };

            _dataContext.Set<GroupUser>().Add(groupUser);
            _dataContext.SaveChanges();

            response.Data = new GroupGetDto
            {
                Id = groupId,
                GroupName = group.GroupName,
                Description = group.Description,
                Users = group.Users.Where(u => u.User != null).Select(x => new GroupUserGetDto
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,
                }).ToList()
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.AddError("error", $"An error occurred: {ex.Message}");
            return BadRequest(response);
        }
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
                    UserId = test.UserId,
                    // Add other test properties as needed
                }).ToList(),

                Messages = group.Messages.Select(Messages => new MessagesGetDto
                {
                    Id = Messages.Id,
                    GroupId= Messages.GroupId,
                    Content = Messages.Content,
                    CreatedAt = Messages.CreatedAt,
                    UserId = Messages.UserId,
                    UserName = Messages.User.UserName,

                }).ToList(),

                FlashCardSets = group.FlashCardSets.Select(flashcardset => new FlashCardSetsGetDto
                {
                    Id = flashcardset.Id,
                    GroupId = flashcardset.GroupId,
                    SetName = flashcardset.SetName,
                    UserId= flashcardset.UserId,

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

    [HttpPost("CreateAndAddUser")]
    public IActionResult CreateAndAddUser([FromBody] GroupCreateDto createDto, [FromQuery] int userId)
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

        // Get the created group with the added user
        var groupWithUser = _dataContext
            .Set<Group>()
            .Include(g => g.Users)
            .FirstOrDefault(g => g.Id == groupToCreate.Id);

        var user = _dataContext.Set<User>().FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            response.AddError("userId", "User not found.");
            return BadRequest(response);
        }

        if (groupWithUser == null)
        {
            response.AddError("groupId", "Group not found.");
            return BadRequest(response);
        }

        var groupUser = new GroupUser
        {
            Group = groupWithUser,
            User = user
        };

        _dataContext.Set<GroupUser>().Add(groupUser);
        _dataContext.SaveChanges();

        var groupToReturn = new GroupGetDto
        {
            Id = groupWithUser.Id,
            GroupName = groupWithUser.GroupName,
            Description = groupWithUser.Description,
            Users = groupWithUser.Users.Where(u => u.User != null).Select(x => new GroupUserGetDto
            {
                Id = x.User.Id,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                UserName = x.User.UserName,
            }).ToList()
        };

        response.Data = groupToReturn;

        return Created("", response);
    }


    [HttpGet("ByUserId/{userId}")]
    public IActionResult GetGroupsByUserId(int userId)
    {
        var response = new Response();

        var groups = _dataContext.Set<Group>()
            .Where(g => g.Users.Any(u => u.User != null && u.User.Id == userId))
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
                    GroupId = test.Group.Id,
                    TestName = test.TestName,
                    UserId = test.User.Id,
                }).ToList(),
                Messages = group.Messages.Select(Messages => new MessagesGetDto
                {
                    Id = Messages.Id,
                    Content = Messages.Content,
                    GroupId = Messages.Group.Id,
                    CreatedAt = Messages.CreatedAt,
                    UserName = Messages.User.UserName,
                    UserId = Messages.User.Id,
                }).ToList(),
                FlashCardSets = group.FlashCardSets.Select(flashcardset => new FlashCardSetsGetDto
                {
                    Id = flashcardset.Id,
                    GroupId = flashcardset.Group.Id,
                    SetName = flashcardset.SetName,
                    UserId = flashcardset.UserId,
                }).ToList(),
                Assignments = group.Assignments.Select(assignments => new AssignmentsGetDto
                {
                    Id = assignments.Id,
                    GroupId = assignments.Group.Id,
                    AssignmentName = assignments.AssignmentName,
                }).ToList()
            })
            .ToList();

        response.Data = groups;

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
                    UserId = test.User.Id,

                }).ToList(),
                Messages = group.Messages.Select(Messages => new MessagesGetDto
                {
                    Id = Messages.Id,
                    Content = Messages.Content,
                    GroupId = Messages.Group.Id,
                    CreatedAt = Messages.CreatedAt,
                    UserName = Messages.User.UserName,
                    UserId = Messages.User.Id,
                }).ToList(),
                FlashCardSets = group.FlashCardSets.Select(flashcardset => new FlashCardSetsGetDto
                { 
                    Id = flashcardset.Id,
                    GroupId =flashcardset.Group.Id,
                    SetName = flashcardset.SetName,
                    UserId =flashcardset.User.Id,

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
