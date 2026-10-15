using CadenceXP.Api.Data;
using CadenceXP.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestsController : ControllerBase
{
    private readonly CadenceXpDbContext _database;

    public QuestsController(CadenceXpDbContext database)
    {
        _database = database;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuestResponse>>> GetQuests()
    {
        var quests = await _database.Quests
        .Select(quest => new QuestResponse
        {
            Id = quest.Id,
            Name = quest.Name,
            Description = quest.Description,
            Type = quest.Type,
            TargetValue = quest.TargetValue,
            XpReward = quest.XpReward
        })
        .ToListAsync();
        return Ok(quests);
    }
}