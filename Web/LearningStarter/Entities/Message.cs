using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Group Group { get; set; }
        public User Sender {  get; set; }
    }

    public class MessageGetDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Group Group { get; set; }
        public User Sender { get; set; }

    }

    public class MessageCreateDto
    {
        public string Content { get; set; }

    }

    public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");
        }

        
    }
}
