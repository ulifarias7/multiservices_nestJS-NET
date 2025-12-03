namespace Document.API.Models.Request
{
    public class CreateObjectDto
    {
        public string? Bucket { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? File { get; set; }
    }
}
