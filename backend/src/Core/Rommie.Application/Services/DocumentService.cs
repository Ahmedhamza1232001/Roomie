using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rommie.Application.Abstractions;
using Rommie.Application.Dtos.Requests;
using Rommie.Application.Interfaces;
using Rommie.Domain.Entities;
using Rommie.Domain.Exceptions;

namespace Rommie.Application.Services
{
    public class DocumentService(IGenericRepository<UserVerificationRequest, Guid> UserVerificationRepo, IGenericRepository<User, Guid> UserRepo, IUnitOfWork unitOfWork, ILogger<DocumentService> logger, IWebHostEnvironment env) : IDocumentService
    {
        public async Task<ICollection<string>> CreateDocumentVerificationRequestAsync(Guid UserId, CreateDocumentRequestDto request, CancellationToken token)
        {
            var user = await UserRepo.GetById(UserId);
            logger.LogInformation("Creating document verification request for user ID {UserId}", UserId);
            if (user == null)
            {
                logger.LogError("User with ID {UserId} not found.", UserId);
                throw new NotFoundException("User.NotFound");
            }
            List<string> imageUrls = [];
            foreach (var doc in request.Documents)
            {
                logger.LogInformation("Saving document image for user ID {UserId}", UserId);
                var imageUrl = await SaveImageAsync(doc);
                imageUrls.Add(imageUrl);
            }
            var userVerificationRequest = UserVerificationRequest.Create(user.Id, request.DocumentType, imageUrls);
            UserVerificationRepo.Add(userVerificationRequest);
            await unitOfWork.SaveChangesAsync(token);
            return imageUrls;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(extension))
                throw new BadRequestException("Invalid file type.");

            var uploadPath = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }
    }
}