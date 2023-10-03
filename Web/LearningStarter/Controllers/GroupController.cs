using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

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

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<Group>()
            .Select(group => new GroupGetDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Users = group.Users.Select(x => new GroupUserGetDto
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,


                }).ToList()

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
                Name = group.Name,
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

        if (string.IsNullOrEmpty(createDto.Name))
        {
            response.AddError(nameof(createDto.Name), "Group name can't be empty");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        var groupToCreate = new Group
        {
            Name = createDto.Name,
            Description = createDto.Description,
        };

        _dataContext.Set<Group>().Add(groupToCreate);
        _dataContext.SaveChanges();

        var groupToReturn = new GroupGetDto
        {
            Id = groupToCreate.Id,
            Name = groupToCreate.Name,
            Description = groupToCreate.Description,
        };

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
            Name = group.Name,
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

        groupToUpdate.Name = updateDto.Name;
        groupToUpdate.Description = updateDto.Description;

        _dataContext.SaveChanges();

        var groupToReturn = new GroupGetDto
        {
            Id = groupToUpdate.Id,
            Name = groupToUpdate.Name,
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
