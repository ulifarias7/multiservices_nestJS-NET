namespace Auth.API.Models.Dto
{
    public class GroupsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<GroupsDto> SubGroups { get; set; } = new List<GroupsDto>();
    }
}
