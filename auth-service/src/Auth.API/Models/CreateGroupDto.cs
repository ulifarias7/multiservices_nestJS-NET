namespace Auth.API.Models
{
    public class CreateGroupDto
    {
        public string Name { get; set; } = default!;

        public string Realms { get; set; } = default!;
    }
}
