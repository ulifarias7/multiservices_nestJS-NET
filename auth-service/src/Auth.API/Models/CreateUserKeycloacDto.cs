using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Auth.API.Models
{
    public class CreateUserKeycloacDto
    {
        [FromForm(Name = "username")]
        public string? UserName { get; set; }

        [FromForm(Name ="firstname")]
        public string? FirstName { get; set; }

        [FromForm(Name = "lastname")]
        public string? LastName { get; set; }

        [FromForm(Name = "email")]
        public string? Email { get; set; }

        [FromForm(Name = "emailVerified")]
        public bool? EmailVerified { get; set; }

        [FromForm(Name = "enabled")]
        public bool? Enabled { get; set; }

        [FromForm(Name = "groups")]
        public List<string> GroupIds { get; set; } = new();

        [FromForm(Name = "credentials")]
        public CredentialDto CredentialDto { get; set; }

    }
    public class CredentialDto
    {
        [FromForm(Name = "type")]
        public string? Type { get; set; } = "password";

        [FromForm(Name = "value")]
        public string Value { get; set; }

        [FromForm(Name = "temporary")]
        public bool? Temporary { get; set; } = false;
    }
}
