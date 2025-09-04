using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rommie.Application.Dtos.Requests
{
    public class LoginUserRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}