namespace Rommie.Infrastructure.Identity.Representations.Requests;

internal sealed record CredentialRepresentation(string Type, string Value, bool Temporary);
