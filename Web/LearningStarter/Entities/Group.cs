using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Text.Json.Serialization;

namespace LearningStarter.Entities;

public class Group
{
    public int Id { get; set; }
    public string GroupName { get; set; }    
    public string Description { get; set; }

    [JsonIgnore]
    public List<GroupUser> Users { get; set; }
 
    public List<Messages> Messages { get; set; }
    [JsonIgnore]
    public List<Assignments> Assignments { get; set; }
    [JsonIgnore]
    public List<FlashCardSets> FlashCardSets { get; set; }
    
    public List<Tests> Test { get; set; }
}

public class GroupCreateDto
{
    
    public string GroupName { get; set; }
    public string Description { get; set; }

}

public class GroupUpdateDto
{
   
    public string GroupName { get; set; }
    public string Description { get; set; }
}

public class GroupGetDto
{
    public int Id { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }

    public List<GroupUserGetDto> Users { get; set; }

    public List<MessagesGetDto> Messages { get; set; }
   
    public List <TestsGetDto> Tests { get; set; }

    public List<FlashCardSetsGetDto>  FlashCardSets { get; set; }

    public List<AssignmentsGetDto> Assignments { get; set; }
}


public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");

        builder.HasMany(x => x.Test).WithOne(x => x.Group);
        builder.HasMany(x => x.Messages);
        builder.HasMany(x => x.FlashCardSets).WithOne(x => x.Group);
        builder.HasMany(x=>x.Assignments).WithOne(x => x.Group);

    }
}

