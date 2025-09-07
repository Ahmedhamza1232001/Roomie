using Microsoft.AspNetCore.Http;
using Rommie.Domain.ValueObjects;

namespace Rommie.Application.Dtos.Requests
{
    public class CreateDocumentRequestDto
    {
        public string? Description { get; set; } = string.Empty;
        public DocumentType DocumentType { get; set; }
        public List<IFormFile> Documents { get; set; } = [];
    }
}