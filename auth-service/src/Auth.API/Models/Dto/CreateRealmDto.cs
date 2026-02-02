namespace Auth.API.Models.Dto
{
    public class CreateRealmDto
    {
        public string Realm { get; set; } = null!;
        public bool Enabled { get; set; } = true;
    }
}
