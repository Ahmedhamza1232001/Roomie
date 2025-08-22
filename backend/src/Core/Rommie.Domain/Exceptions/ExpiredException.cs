using Rommie.Domain.Abstractions;

namespace Rommie.Domain.Exceptions;

public class ExpiredException(string code, params Object[] args) : LocalizedHttpException(code, 410, args)
{
    public string Code { get; } = code;
}
