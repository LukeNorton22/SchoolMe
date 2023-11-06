using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using Group = LearningStarter.Entities.Group;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/FCSets")]

    public class FlashCardSetsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public FlashCardSetsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("{groupId}")]
        public IActionResult Create(int groupId, [FromBody] FlashCardSetsCreateDto createDto)
        {
            var response = new Response();

            var group = _dataContext.Set<Group>().FirstOrDefault(x => x.Id == groupId);
            if (createDto.SetName == null)
            {
                response.AddError(nameof(createDto.SetName), "SetName can not be empty");
            }

            if (group == null)
            {
                return BadRequest("Group can not be found.");
            }

            var FlashCardSetsToCreate = new FlashCardSets
            {              
                GroupId = group.Id,
                SetName = createDto.SetName,               
            };
           

            if (FlashCardSetsToCreate== null) {
                return BadRequest("FlashCardSet can not be found.");
            }
           
            
           
            _dataContext.Set<FlashCardSets>().Add(FlashCardSetsToCreate);
            _dataContext.SaveChanges();

            var FlashCardSetsToReturn = new FlashCardSetsGetDto
            {
                Id = FlashCardSetsToCreate.Id, 
                GroupId = FlashCardSetsToCreate.GroupId,
                SetName = FlashCardSetsToCreate.SetName,
            };

            response.Data = FlashCardSetsToReturn;
            return Created("", response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext
                .Set<FlashCardSets>()
                .Include(x => x.FlashCards)
                .Select(FlashCardSets => new FlashCardSetsGetDto
                {
                    Id = FlashCardSets.Id,
                    GroupId = FlashCardSets.GroupId,                
                    SetName = FlashCardSets.SetName,
                    FlashCards = FlashCardSets.FlashCards.Select(x => new FlashCardsGetDto
                    {
                        Id = x.Id,
                        FlashCardSetId = x.FlashCardSetId,
                        Question = x.Question,
                        Answer= x.Answer,

                    }).ToList(),
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
                .Set<FlashCardSets>()
                .Select(FlashCardSets => new FlashCardSetsGetDto
                {
                  Id = FlashCardSets.Id,
                  GroupId= FlashCardSets.GroupId,
                    
                    SetName = FlashCardSets.SetName,
                    FlashCards = FlashCardSets.FlashCards.Select(x => new FlashCardsGetDto
                    {
                        Id = x.Id,
                        FlashCardSetId=x.FlashCardSetId,
                        Question = x.Question,
                        Answer = x.Answer,
                       
                    }).ToList()
                })
                .FirstOrDefault(FlashCardSets => FlashCardSets.Id == id);

            response.Data = data;
            if (data == null)
            {



                response.AddError("id", "FlashCard not found.");
            }
            return Ok(response);

        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] FlashCardSetsUpdateDto updateDto, int id)
        {
            var response = new Response();

            var setToUpdate = _dataContext.Set<FlashCardSets>()
                .FirstOrDefault(group => group.Id == id);

            if (setToUpdate == null)
            {
                response.AddError("id", "Set not found");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            setToUpdate.SetName = updateDto.SetName;

            _dataContext.SaveChanges();

            var setToReturn = new FlashCardSetsGetDto
            {
                Id = setToUpdate.Id,
                GroupId = setToUpdate.GroupId,
                SetName = setToUpdate.SetName,
            };

            response.Data = setToReturn;
            return Ok(response);
        }

        [HttpDelete("id")]
        public IActionResult Delete(int id)
        {
            var response = new Response();

            var FlashCardSetsToDelete = _dataContext.Set<FlashCardSets>()
                .FirstOrDefault(FlashCardSets => FlashCardSets.Id == id);


            if (FlashCardSetsToDelete == null)
            {
                response.AddError("id", "FlashCardSets not found.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }
            _dataContext.Set<FlashCardSets>().Remove(FlashCardSetsToDelete);
            _dataContext.SaveChanges();
            response.Data = true;
            return Ok(response);
        }
       
    }
}