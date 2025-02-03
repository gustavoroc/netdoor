using AuthService.Database;
using AuthService.DTOs;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class GroupService(ApplicationDbContext context)
    {
        public ApplicationDbContext Context { get; set; } = context;
        public DbSet<Group> EntityContext = context.Set<Group>();

        public async Task CreateGroup(GroupDto dto)
        {
            var group = new Group
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
            };

            await EntityContext.AddAsync(group);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveGroup(int groupId)
        {
            var entity = await EntityContext.FirstAsync(x => x.Id == groupId);
            EntityContext.Remove(entity);

            await Context.SaveChangesAsync();
        }

        public async Task AddUserToGroup(int groupId, string userId)
        {
            var group = await EntityContext
                .Include(y => y.UserGroups)
                .FirstAsync(x => x.Id == groupId);

            group.AddUser(userId);

            await Context.SaveChangesAsync();
        }
    }
}
