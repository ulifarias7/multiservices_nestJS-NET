namespace Auth.API.Models.Dto
{
    public class ResetPasswordDto
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public bool Temporary { get; set; } = false;
    }
}
