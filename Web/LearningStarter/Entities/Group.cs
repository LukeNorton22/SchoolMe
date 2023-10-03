using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
namespace LearningStarter.Entities;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }    
    public string Description { get; set; }
    public List<GroupUser> Users { get; set; }
    

}

public class GroupCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }

}

public class GroupUpdateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class GroupGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<GroupUserGetDto>Users { get; set; }
}


public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");
    }
}

