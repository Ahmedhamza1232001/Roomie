using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rommie.Application.Dtos.Requests
{
    public class UpdateUserRequestDto
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
    }
}