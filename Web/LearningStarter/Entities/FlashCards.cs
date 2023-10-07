using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Entities
{
    public class FlashCards
    {
        public int Id { get; set; }
        public int FlashCardSetId { get; set; }
        public string Question { get; set; }

        public string Answer { get; set; }
    }

    public class FlashCardsGetDto
    {
        public int Id { get; set; }
      public int FlashCardSetId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    public class FlashCardsCreateDto
    {

      
        public string Question { get; set; }

        public string Answer { get; set; }
    }
    public class FlashCardsUpdateDto
    {


      
        public string Question { get; set; }

        public string Answer { get; set; }
    }
    
    public class FlashCardsEntityTypeConfiguration : IEntityTypeConfiguration<FlashCards>
    {
        public void Configure(EntityTypeBuilder<FlashCards> builder)
        {
            builder.ToTable("FlashCards");
        }
    }

}
