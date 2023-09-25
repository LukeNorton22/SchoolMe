using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;

namespace LearningStarter.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; } 
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }

    }

    public class GroupCreateDto
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public string Description { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
    }

    public class GroupEntityConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired();

        }
    }

}
