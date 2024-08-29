using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.User;
using api.Models;

namespace api.Mappers
{
    public static class UserMapper
    {

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                PetList = user.Pets.Select(PetMapper.ToDto).ToList()
            };
        }

        public static User ToUserFromCreateDto(this CreateUserRequestDto createUserRequest)
        {
            return new User
            {
                FirstName = createUserRequest.FirstName,
                LastName = createUserRequest.LastName,
                Age = createUserRequest.Age
            };
        }

        public static User ToUserWithPetsFromCreateDto(this CreateUserWithPetsRequestDto createUserWithPetsRequest)
        {
            return new User
            {
                FirstName = createUserWithPetsRequest.FirstName,
                LastName = createUserWithPetsRequest.LastName,
                Age = createUserWithPetsRequest.Age,
                Pets = createUserWithPetsRequest.Pets.Select(petDto => new Pet
                {
                    Name = petDto.Name,
                    Animal = petDto.Animal
                }).ToList()
            };
        }
    }
}