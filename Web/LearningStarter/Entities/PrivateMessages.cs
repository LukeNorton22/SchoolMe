/*using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;


namespace LearningStarter.Entities;

public class PrivateMessages
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }

}

public class PrivateMessagesGetDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }

}

public class PrivateMessagesCreateDto
{
    public string Content { get; set; }
    public string ImageUrl { get; set; }

}

public class PrivateMessagesUpdateDto
{

    public string Content { get; set; }
    public string ImageUrl { get; set; }

}

public class PrivateMessagesEntityTypeConfiguration : IEntityTypeConfiguration<PrivateMessages>
{
    public void Configure(EntityTypeBuilder<PrivateMessages> builder)
    {
        builder.ToTable("PrivateMessages");
    }

}
*/