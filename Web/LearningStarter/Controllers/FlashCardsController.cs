using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace LearningStarter.Controllers;
[ApiController]
[Route("api/flashcards")]

public class FlashCardsController : ControllerBase
{
    private readonly DataContext _dataContext;
    public FlashCardsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public IActionResult Create(int flashcardsetId, [FromBody] FlashCardsCreateDto createDto)
    {
        var response = new Response();

        var flashcardset = _dataContext.Set<FlashCardSets>().FirstOrDefault(x => x.Id == flashcardsetId);
        if (createDto.Question == null)
        {
            response.AddError(nameof(createDto.Question), "SetName can not be empty");
        }
        if (createDto.Answer == null)
        {
            response.AddError(nameof(createDto.Answer), "SetName can not be empty");
        }
        if (flashcardset == null)
        {
            return BadRequest("FlashCardSet can not be found.");
        }


        var FlashCardsToCreate = new FlashCards
        {

            FlashCardSetId = flashcardset.Id,
            Question = createDto.Question,
            Answer = createDto.Answer,

        };

        _dataContext.Set<FlashCards>().Add(FlashCardsToCreate);
        _dataContext.SaveChanges();

        var FlashCardsToReturn = new FlashCardsGetDto
        {
            Id = FlashCardsToCreate.Id,
            FlashCardSetId = flashcardset.Id,
            Question = createDto.Question,
            Answer = createDto.Answer,
        };

        response.Data = FlashCardsToReturn;
        return Created("", response);

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
                FlashCardSetId=FlashCards.FlashCardSetId,
                Question = FlashCards.Question,
                Answer = FlashCards.Answer,
            })
            .ToList();
        response.Data = data;
        return Ok(response);
    }

    [HttpGet("id")]
    public IActionResult GetById(int id)
    {
        var response = new Response();

        
        var data = _dataContext
            .Set<FlashCards>()
            .Select(FlashCards => new FlashCardsGetDto
            {
                Id = FlashCards.Id,
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

    [HttpPut("id")]
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
            Answer = FlashCardsToUpdate.Answer,
            Question = FlashCardsToUpdate.Question,

        };
        response.Data = FlashCardsToReturn;
        return Ok(response);
    }

    [HttpDelete("id")]
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
