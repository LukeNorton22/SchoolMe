using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace LearningStarter.Entities
{
    public class GroupMessages
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int MessageId { get; set; }
        public Messages Message { get; set; }
    }

    public class GroupMessagesGetDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class GroupMessagesEntityTypeConfiguration : IEntityTypeConfiguration<GroupMessages>
    {
        public void Configure(EntityTypeBuilder<GroupMessages> builder)
        {
            builder.ToTable("GroupMessages");

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Messages);

            builder.HasOne(x => x.Message)
                .WithMany();
        }
    }
}
