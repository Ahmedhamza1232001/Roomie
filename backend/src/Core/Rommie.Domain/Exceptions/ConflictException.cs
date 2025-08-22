using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Rommie.Domain.Abstractions;

namespace Rommie.Domain.Exceptions
{
    public class ConflictException(string code, params Object[] args) : LocalizedHttpException(code, 409, args)
    {
        public string Code { get; } = code;
    }
}
