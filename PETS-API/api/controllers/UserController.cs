using api.Data;
using api.Dtos.User;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.Include(user => user.Pets).ToListAsync();
            var usersDto = users.Select(users => users.ToDto());
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _context.Users.Include(user => user.Pets).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto userDto)
        {
            var userModel = userDto.ToUserFromCreateDto();
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequestDto userDto)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }
            userModel.Age = userDto.Age;
            userModel.FirstName = userDto.FirstName;
            userModel.LastName = userDto.LastName;

            await _context.SaveChangesAsync();

            return Ok(userModel.ToDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }
            _context.Users.Remove(userModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("{userId}/assign-pet/{petId}")]
        public async Task<IActionResult> AssignPetToUser([FromRoute] int userId, [FromRoute] int petId)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (userModel == null)
            {
                return NotFound();
            }

            var petModel = await _context.Pets.FirstOrDefaultAsync(pet => pet.Id == petId);
            if (petModel == null)
            {
                return NotFound();
            }

            userModel.Pets.Add(petModel);
            await _context.SaveChangesAsync();

            return Ok(userModel.ToDto());
        }

        [HttpPost("create-user-with-pets")]
        public async Task<IActionResult> CreateUserWithPets([FromBody] CreateUserWithPetsRequestDto userWithPetsDto)
        {
            var userModel = userWithPetsDto.ToUserWithPetsFromCreateDto();

            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToDto());
        }

    }
}