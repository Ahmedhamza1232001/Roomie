using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Rommie.Domain.Abstractions;

namespace Rommie.Domain.Exceptions;

public class NotAuthorizedException(string code, params Object[] args) : LocalizedHttpException(code, 401, args)
{
    public string Code { get; private init; } = code;
}
