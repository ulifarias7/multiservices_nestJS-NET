namespace Auth.API.Models.Dto
{
    public class CreateSubGroupDto
    {
        public string Realm { get; set; } = default!;
        public string ParentGroupId { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
