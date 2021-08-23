using System;

namespace OSKI_Solutions_Test.Models
{
    public partial class Answers
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid TestId { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Test Test { get; set; }
    }
}
