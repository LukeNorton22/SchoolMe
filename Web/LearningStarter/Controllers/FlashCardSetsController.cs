﻿using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/FlashCardSets")]

    public class FlashCardSetsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public FlashCardSetsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();
            var data = _dataContext
                .Set<FlashCardSets>()
                .Select(FlashCardSets => new FlashCardSetsGetDto
                {
                    Id = FlashCardSets.Id,
                    
                    CreatedById = FlashCardSets.CreatedById,
                    SetName = FlashCardSets.SetName,
                    FlashCards = FlashCardSets.FlashCards.Select(x => new FlashCardSetsFlashCardGetDto
                    {
                        Id = x.FlashCards.Id,
                        Question = x.FlashCards.Question,
                        Answer = x.FlashCards.Answer,
                      


                    }).ToList()
                })
                .ToList();
            response.Data = data;
            return Ok(response);
        }
        [HttpGet("({id}")]
        public IActionResult GetById(int id)
        {
            var response = new Response();


            var data = _dataContext
                .Set<FlashCardSets>()
                .Select(FlashCardSets => new FlashCardSetsGetDto
                {
                    Id = FlashCardSets.Id,
                  
                    CreatedById = FlashCardSets.CreatedById,
                    SetName = FlashCardSets.SetName,
                    FlashCards = FlashCardSets.FlashCards.Select(x => new FlashCardSetsFlashCardGetDto
                    {
                        Id = x.FlashCards.Id,
                        Question = x.FlashCards.Question,
                        Answer = x.FlashCards.Answer,
                       


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

        [HttpPost]
        public IActionResult Create([FromBody] FlashCardSetsCreateDto createDto)
        {
            var response = new Response();

            if (createDto.CreatedById < 0)
            {
                response.AddError(nameof(createDto.CreatedById), "CreatedById must be valid");
            }
            if (createDto.SetName == null)
            {
                response.AddError(nameof(createDto.SetName), "SetName can not be empty");
            }
          
           

            var FlashCardSetsToCreate = new FlashCardSets
            {
               
                CreatedById = createDto.CreatedById,
                SetName = createDto.SetName,
            };

            _dataContext.Set<FlashCardSets>().Add(FlashCardSetsToCreate);
            _dataContext.SaveChanges();

            var FlashCardSetsToReturn = new FlashCardSetsGetDto
            {
                Id = FlashCardSetsToCreate.Id,
              
                CreatedById = FlashCardSetsToCreate.CreatedById,
                SetName = FlashCardSetsToCreate.SetName,
            };

            response.Data = FlashCardSetsToReturn;
            return Created("", response);

        }
        [HttpPut("{id}")]
        public IActionResult Update([FromBody] FlashCardSetsUpdateDto updateDto, int id)
        {
            var response = new Response();

            if (updateDto.CreatedById < 0)
            {
                response.AddError(nameof(updateDto.CreatedById), "CreatedById must be valid");
            }
            if (updateDto.SetName == null)
            {
                response.AddError(nameof(updateDto.SetName), "SetName can not be empty");
            }
           

            var FlashCardSetsToUpdate = _dataContext.Set<FlashCardSets>()
                .FirstOrDefault(FlashCardSets => FlashCardSets.Id == id);

            if (FlashCardSetsToUpdate == null)
            {
                response.AddError("id", "FlashCard not found.");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            FlashCardSetsToUpdate.SetName = updateDto.SetName;
            FlashCardSetsToUpdate.CreatedById = updateDto.CreatedById;

            _dataContext.SaveChanges();

            var FlashCardSetsToReturn = new FlashCardSetsGetDto
            {
                Id = FlashCardSetsToUpdate.Id,
        
                SetName = FlashCardSetsToUpdate.SetName,
                CreatedById = FlashCardSetsToUpdate.CreatedById,

            };
            response.Data = FlashCardSetsToReturn;
            return Ok(response);
        }
        [HttpDelete("{id}")]
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
        [HttpPost("{FlashCardSetsId}/FlashCards/{FlashCardsId}")]
        public IActionResult AddFlashCardsToFlashCardSets(int FlashCardSetsId, int FlashCardsId)
        {
            var response = new Response();
          

                
            var FlashCardSets = _dataContext.Set<FlashCardSets>()
                .FirstOrDefault(x => x.Id == FlashCardSetsId);
            var FlashCards = _dataContext.Set<FlashCards>()
                .FirstOrDefault(x => x.Id == FlashCardsId);
            if (FlashCards == null)
            {
                response.AddError("id", "FlashCard not found.");
            }
            if (FlashCardSets == null)
            {
                response.AddError("id", "FlashCardSet not found.");
            }
            var FlashCardSetsFlashCard = new FlashCardSetsFlashCard
            {
                FlashCardSets = FlashCardSets,
                FlashCards = FlashCards,

            };
            if (response.HasErrors)
            {
                return BadRequest(response);
            }
            _dataContext.Set<FlashCardSetsFlashCard>().Add(FlashCardSetsFlashCard);
            _dataContext.SaveChanges();

            response.Data = new FlashCardSetsGetDto
            {
                Id = FlashCardSetsId,
                CreatedById = FlashCardSets.CreatedById,
                SetName = FlashCardSets.SetName,
                FlashCards = FlashCardSets.FlashCards.Select(x => new FlashCardSetsFlashCardGetDto
                {
                    Id = x.FlashCards.Id,
                    Question = x.FlashCards.Question,
                    Answer = x.FlashCards.Answer,
                  
                }).ToList()
            };
            return Ok(response);
        }
    }
}