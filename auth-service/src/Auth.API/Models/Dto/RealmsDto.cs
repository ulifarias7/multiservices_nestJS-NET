namespace Auth.API.Models.Dto
{
    public class RealmsDto
    {
        public string Id { get; set; } = null!;
        public string Realm { get; set; } = null!;
        public bool Enabled { get; set; }
    }
}
