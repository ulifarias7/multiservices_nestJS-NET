namespace Auth.API.Models.Dto
{
    public class CreateGroupDto
    {
        public string Name { get; set; } = default!;
        public string Realms { get; set; } = default!;
        public string? ParentGroupId { get; set; }
    }
}
