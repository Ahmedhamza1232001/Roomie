using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rommie.Domain.ValueObjects;

namespace Rommie.Domain.Entities
{
    public class UserVerificationRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DocumentType DocumentType { get; set; }
        public List<string> DocumentUrl { get; set; } = [];
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public VerificationRequestStatus Status { get; set; } = VerificationRequestStatus.Pending;
        public virtual User User { get; set; } = null!;
        public static UserVerificationRequest Create(Guid UserId, DocumentType documentType, List<string> documentUrl)
        {
            return new UserVerificationRequest
            {
                DocumentType = documentType,
                Id = Guid.NewGuid(),
                UserID = UserId,
                CreatedAtUtc = DateTime.UtcNow,
                Status = VerificationRequestStatus.Pending,
                DocumentUrl = documentUrl
            };
        }
    }
}