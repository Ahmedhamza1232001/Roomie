using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rommie.Application.Dtos.Requests;
using Rommie.Domain.ValueObjects;

namespace Rommie.Application.Interfaces
{
    public interface IDocumentService
    {
        public Task<ICollection<string>> CreateDocumentVerificationRequestAsync(Guid UserId, CreateDocumentRequestDto request, CancellationToken cancellationToken = default);
        public Task<string> SaveImageAsync(IFormFile file);
    }
}