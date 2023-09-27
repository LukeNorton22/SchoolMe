using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace LearningStarter.Controllers;
[ApiController]
[Route("api/GroupMembers")]

public class GroupMembersController : ControllerBase
{
    private readonly DataContext _dataContext;
    public GroupMembersController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<GroupMembers>()
            .Select(GroupMembers => new GroupMembersGetDto
            {
                Id = GroupMembers.Id,
                GroupId = GroupMembers.GroupId,
                UserId = GroupMembers.UserId,
            })
            .ToList();
        response.Data = data;
        return Ok(response);
    }

    [HttpGet("(id")]
    public IActionResult GetById(int id)
    {
        var response = new Response();
        
        var data = _dataContext
            .Set<GroupMembers>()
            .Select(GroupMembers => new GroupMembersGetDto
            {
                Id = GroupMembers.Id,
                GroupId = GroupMembers.GroupId,
                UserId = GroupMembers.UserId,
            })
            .FirstOrDefault(GroupMembers => GroupMembers.Id == id);
        
        response.Data = data;
        if (data == null)
        {



           response.AddError("id", "GroupMembers not found.");
        }
        return Ok(response);

    }
    [HttpPost]
    public IActionResult Create([FromBody] GroupMembersCreateDto createDto)
    {
        var response = new Response();

        if(createDto.UserId <0)
        {
            response.AddError(nameof(createDto.UserId), "Must enter a valid User Id");
        }
        if(createDto.GroupId <0)
        {
            response.AddError(nameof(createDto.GroupId), "Must enter a valid Group Id");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }
        
        var GroupMembersToCreate = new GroupMembers
        {
            GroupId = createDto.GroupId,
            UserId = createDto.UserId,
        };


        _dataContext.Set<GroupMembers>().Add(GroupMembersToCreate);
        _dataContext.SaveChanges();

        var GroupMembersToReturn = new GroupMembersGetDto
        {
            Id = GroupMembersToCreate.Id,
            GroupId = GroupMembersToCreate.GroupId,
            UserId = GroupMembersToCreate.UserId,
        };

        response.Data = GroupMembersToReturn;
        return Created("", response);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromBody] GroupMembersUpdateDto updateDto, int id)
    {
        var response = new Response();

        if (updateDto.UserId < 0)
        {
            response.AddError(nameof(updateDto.UserId), "Must enter a valid User Id");
        }
        if (updateDto.GroupId < 0)
        {
            response.AddError(nameof(updateDto.GroupId), "Must enter a valid Group Id");
        }

        
        var GroupMembersToUpdate = _dataContext.Set<GroupMembers>()
            .FirstOrDefault(GroupMembers => GroupMembers.Id == id);

        if (GroupMembersToUpdate == null)
        {
            response.AddError("id", "GroupMembers not found.");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        GroupMembersToUpdate.GroupId = updateDto.GroupId;
        GroupMembersToUpdate.UserId = updateDto.UserId;

        _dataContext.SaveChanges();

        var GroupMembersToReturn = new GroupMembersGetDto
        {
            Id = GroupMembersToUpdate.Id,
            GroupId = GroupMembersToUpdate.GroupId,
            UserId = GroupMembersToUpdate.UserId,

        };
        response.Data= GroupMembersToReturn;
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var GroupMembersToDelete = _dataContext.Set<GroupMembers>()
            .FirstOrDefault(GroupMembers => GroupMembers.Id == id);


        if (GroupMembersToDelete== null)
        {
            response.AddError("id", "GroupMembers not found.");
        }

        if(response.HasErrors)
        {
            return BadRequest(response);
        }
        _dataContext.Set<GroupMembers>().Remove(GroupMembersToDelete);
        _dataContext.SaveChanges();
        response.Data = true;
        return Ok(response);
    }
            }
    

