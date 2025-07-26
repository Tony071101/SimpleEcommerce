using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce_BackEnd.Models.Dtos.Talent;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TalentsController : ControllerBase
    {
        private readonly ITalentService talentService;
        private readonly IMapper mapper;

        public TalentsController(ITalentService talentService, IMapper mapper)
        {
            this.talentService = talentService;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TalentResponseDto>>> GetAllTalents()
        {
            var talents = await talentService.GetAllTalentsAsync();
            var talentDtos = mapper.Map<IEnumerable<TalentResponseDto>>(talents);
            return Ok(talentDtos);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TalentResponseDto>> GetTalentById(Guid id)
        {
            var talent = await talentService.GetTalentByIdAsync(id);
            if (talent == null)
            {
                return NotFound();
            }
            var talentDto = mapper.Map<TalentResponseDto>(talent);
            return Ok(talentDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TalentResponseDto>> AddTalent([FromBody] CreateTalentDto createTalentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var talentToAdd = mapper.Map<Talent>(createTalentDto); // Ánh xạ từ DTO input sang Entity

            var newTalent = await talentService.AddTalentAsync(talentToAdd);
            var newTalentDto = mapper.Map<TalentResponseDto>(newTalent); // Ánh xạ từ Entity sang DTO output

            return CreatedAtAction(nameof(GetTalentById), new { id = newTalent.TalentID }, newTalentDto);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTalent(Guid id, [FromBody] CreateTalentDto updateTalentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var talentToUpdate = mapper.Map<Talent>(updateTalentDto);
            talentToUpdate.TalentID = id; // Đảm bảo ID được giữ nguyên

            var updatedTalent = await talentService.UpdateTalentAsync(id, talentToUpdate);

            if (updatedTalent == null)
            {
                return NotFound();
            }

            var updatedTalentDto = mapper.Map<TalentResponseDto>(updatedTalent);
            return Ok(updatedTalentDto);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTalent(Guid id)
        {
            var success = await talentService.DeleteTalentAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}