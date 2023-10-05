using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Microsoft.VisualBasic;

namespace LearningStarter.Entities;

public class Messages
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public Messages Message { get; set; }



}

public class MessagesGetDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}

public class MessagesCreateDto
{
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set;} = DateTime.Now;

}

public class MessagesUpdateDto
{
   
    public string Content { get; set; }
    public string ImageUrl { get; set; }

}

public class MessagesEntityTypeConfiguration : IEntityTypeConfiguration<Messages>
{
    public void Configure(EntityTypeBuilder<Messages> builder)
    {
        builder.ToTable("Messages");

        builder.HasOne(x => x.Group).WithMany(x => x.Messages);
        
    }

}
