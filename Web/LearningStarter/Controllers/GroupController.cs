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
            .Select(product => new GroupGetDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Users = product.Users

            })
            .ToList();

        response.Data = data;

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] GroupCreateDto createDto)
    {
        var response = new Response();

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
            Users = groupToCreate.Users
        };

        response.Data = groupToReturn;

        return Created("", response);
    }

}
