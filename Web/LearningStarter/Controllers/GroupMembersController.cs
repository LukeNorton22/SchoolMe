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

        [HttpPost]
        public IActionResult Create([FromBody] GroupMembersCreateDto createDto)
        {
            var response = new Response();
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
            }
    

