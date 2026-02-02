using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Models.Dto
{
    public class UserKeycloackDto
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
