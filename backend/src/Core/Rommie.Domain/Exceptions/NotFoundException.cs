using Rommie.Domain.Abstractions;

namespace Rommie.Domain.Exceptions
{
    public class NotFoundException(string code, params Object[] args) : LocalizedHttpException(code, 404, args)
    {
        public string Code { get; init; } = code;
    }
}
