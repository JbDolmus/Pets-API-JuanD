using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Pet;

namespace api.Dtos.User
{
    public class CreateUserWithPetsRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public List<CreatePetRequestDto> Pets { get; set; }
    }
}