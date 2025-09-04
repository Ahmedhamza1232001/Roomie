using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rommie.Application.Dtos.Responses
{
    public class LoginUserResponse
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}