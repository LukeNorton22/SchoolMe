using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
namespace LearningStarter.Entities
{
    public class FlashCardSets
    {
        public int Id { get; set; }
      
        public int CreatedById { get; set; }

        public string SetName { get; set; }

        public List<FlashCardSetsFlashCard> FlashCards { get; set; }
    }

    public class FlashCardSetsGetDto
    {
        public int Id { get; set; }
       
        public int CreatedById { get; set; }
        public string SetName { get; set; }
        public List<FlashCardSetsFlashCardGetDto> FlashCards { get; set; }
    }
    public class FlashCardSetsCreateDto
    {

       
        public int CreatedById { get; set; }

        public string SetName { get; set; }
    }
    public class FlashCardSetsUpdateDto
    {


     
        public int CreatedById { get; set; }

        public string SetName { get; set; }
    }

    public class FlashCardSetsEntityTypeConfiguration : IEntityTypeConfiguration<FlashCardSets>
    {
        public void Configure(EntityTypeBuilder<FlashCardSets> builder)
        {
            builder.ToTable("FlashCardSets");
        }
    }

}



