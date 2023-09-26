using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Entities;


        public class GroupMembers
        {
            public int Id { get; set; }
            public int GroupId { get; set; }
            public int UserId { get; set; }
        }

        public class GroupMembersGetDto
        {
            public int Id { get; set; }
            public int GroupId { get; set; }
            public int UserId { get; set; }
        }
        public class GroupMembersCreateDto
        {

            public int GroupId { get; set; }
            public int UserId { get; set; }
        }
public class GroupMembersUpdateDto
{


    public int GroupId { get; set; }
    public int UserId { get; set; }
}
    public class GroupMembersEntityTypeConfiguration : IEntityTypeConfiguration<GroupMembers>
    {
        public void Configure(EntityTypeBuilder<GroupMembers> builder)
        {
            builder.ToTable("GroupMembers");
        }
    }


