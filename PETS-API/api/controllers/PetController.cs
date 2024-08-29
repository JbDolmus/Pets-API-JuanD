using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Pet;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/pet")]
    [ApiController]
    public class PetController : ControllerBase
    {

        private readonly ApplicationDBContext _context;
        public PetController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pets = await _context.Pets.ToListAsync();
            var petsDto = pets.Select(pets => pets.ToDto());
            return Ok(petsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePetRequestDto petDto)
        {
            var petModel = petDto.ToPetFromCreateDto();
            await _context.Pets.AddAsync(petModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = petModel.Id }, petModel.ToDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePetRequestDto petDto)
        {
            var petModel = await _context.Pets.FirstOrDefaultAsync(pet => pet.Id == id);
            if (petModel == null)
            {
                return NotFound();
            }
            petModel.Name = petDto.Name;
            petModel.Animal = petDto.Animal;

            await _context.SaveChangesAsync();

            return Ok(petModel.ToDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var petModel = await _context.Pets.FirstOrDefaultAsync(pet => pet.Id == id);
            if (petModel == null)
            {
                return NotFound();
            }
            _context.Pets.Remove(petModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}