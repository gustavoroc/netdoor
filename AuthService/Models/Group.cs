namespace AuthService.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }

        public void AddUser(string userId)
        {
            var userGroupRelationship = UserGroups.FirstOrDefault(y => y.UserId == userId);

            if (userGroupRelationship is null)
            {
                var newRelationship = new UserGroup { UserId =  userId };
                UserGroups.Add(newRelationship);
            }
        }

    }
}
