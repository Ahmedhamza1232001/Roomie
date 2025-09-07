using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Rommie.Domain.Abstractions;

namespace Rommie.Domain.Exceptions
{
    public class BadRequestException : LocalizedHttpException
    {
        public BadRequestException(string errorCode, params object[] args) : base(errorCode, 400, args)
        {
        }
    }
}