using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LearningStarter.Entities
{
    public class TestQuestions
    {
        public int Id { get; set; }
        public string Question {  get; set; }

    }

    public class TestQuestionsGetDto
    {
        public int Id { get; set; }
        public string Question { get; set; }

    }
    public class TestQuestionsCreateDto
    {
        public string Question { get; set; }
    }

    public class TestQuestionsUpdateDto
    {
        public string Question { get; set; }
    }

    public class TestQuestionsEntityTypeConfiguration : IEntityTypeConfiguration<TestQuestions>
    {
        public void Configure(EntityTypeBuilder<TestQuestions> builder)
        {
            builder.ToTable("TestQuestions");
        }
    }
}
