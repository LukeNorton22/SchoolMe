using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;


namespace LearningStarter.Entities;

public class UserMessages
{
    public int Id { get; set; }
    public string Content { get; set; }
    public Group Group { get; set; }
    public User Sender {  get; set; }
}

public class UserMessagesGetDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public Group Group { get; set; }
    public User Sender { get; set; }

}

public class UserMessagesCreateDto
{
    public string Content { get; set; }
  

}

public class UserMessagesEntityTypeConfiguration : IEntityTypeConfiguration<UserMessages>
{
    public void Configure(EntityTypeBuilder<UserMessages> builder)
    {
        builder.ToTable("UserMessages");
    }

}
