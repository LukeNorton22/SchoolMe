using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace LearningStarter.Controllers;
[ApiController]
[Route("api/FlashCards")]

public class FlashCardsController : ControllerBase
{
    private readonly DataContext _dataContext;
    public FlashCardsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();
        var data = _dataContext
            .Set<FlashCards>()
            .Select(FlashCards => new FlashCardsGetDto
            {
                Id = FlashCards.Id,
                FlashCardSetId = FlashCards.FlashCardSetId,
                Question = FlashCards.Question,
                Answer = FlashCards.Answer,
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
            .Set<FlashCards>()
            .Select(FlashCards => new FlashCardsGetDto
            {
                Id = FlashCards.Id,
                FlashCardSetId = FlashCards.FlashCardSetId,
                Question = FlashCards.Question,
                Answer = FlashCards.Answer,
            })
            .FirstOrDefault(FlashCards => FlashCards.Id == id);

        response.Data = data;
        if (data == null)
        {



            response.AddError("id", "FlashCard not found.");
        }
        return Ok(response);

    }

    [HttpPost]
    public IActionResult Create([FromBody] FlashCardsCreateDto createDto)
    {
        var response = new Response();

        if (createDto.Question == null)
        {
            response.AddError(nameof(createDto.Question), "Question can not be empty");
        }
        if (createDto.Answer == null)
        {
            response.AddError(nameof(createDto.Answer), "Answer can not be empty");
        }
        if (createDto.FlashCardSetId < 0)
        {
            response.AddError(nameof(createDto.FlashCardSetId), "Must enter a valid FlashCard Set Id");
        }

        var FlashCardsToCreate = new FlashCards
        {
            FlashCardSetId = createDto.FlashCardSetId,
            Question = createDto.Question,
            Answer = createDto.Answer,
        };

        _dataContext.Set<FlashCards>().Add(FlashCardsToCreate);
        _dataContext.SaveChanges();

        var FlashCardsToReturn = new FlashCardsGetDto
        {
            Id = FlashCardsToCreate.Id,
            FlashCardSetId = FlashCardsToCreate.FlashCardSetId,
            Question = FlashCardsToCreate.Question,
            Answer = FlashCardsToCreate.Answer,
        };

        response.Data = FlashCardsToReturn;
        return Created("", response);

    }
    [HttpPut("{id}")]
    public IActionResult Update([FromBody] FlashCardsUpdateDto updateDto, int id)
    {
        var response = new Response();

        if (updateDto.Question == null)
        {
            response.AddError(nameof(updateDto.Question), "Question can not be empty");
        }
        if (updateDto.Answer == null)
        {
            response.AddError(nameof(updateDto.Answer), "Answer can not be empty");
        }
        if(updateDto.FlashCardSetId < 0)
        {
            response.AddError(nameof(updateDto.FlashCardSetId), "Must enter a valid FlashCard Set Id");
        }


        var FlashCardsToUpdate = _dataContext.Set<FlashCards>()
            .FirstOrDefault(FlashCards => FlashCards.Id == id);

        if (FlashCardsToUpdate == null)
        {
            response.AddError("id", "FlashCard not found.");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        FlashCardsToUpdate.Answer = updateDto.Answer;
        FlashCardsToUpdate.Question = updateDto.Question;

        _dataContext.SaveChanges();

        var FlashCardsToReturn = new FlashCardsGetDto
        {
            Id = FlashCardsToUpdate.Id,
            FlashCardSetId = FlashCardsToUpdate.FlashCardSetId,
            Answer = FlashCardsToUpdate.Answer,
            Question = FlashCardsToUpdate.Question,

        };
        response.Data = FlashCardsToReturn;
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var FlashCardsToDelete = _dataContext.Set<FlashCards>()
            .FirstOrDefault(FlashCards => FlashCards.Id == id);


        if (FlashCardsToDelete == null)
        {
            response.AddError("id", "FlashCards not found.");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }
        _dataContext.Set<FlashCards>().Remove(FlashCardsToDelete);
        _dataContext.SaveChanges();
        response.Data = true;
        return Ok(response);
    }
}
