using System;

namespace OSKI_Solutions_Test.Models
{
    public partial class Test_User_Junction
    {
        public Guid TestId { get; set; }
        public Guid UserId { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsPassed { get; set; }


        public virtual Test Test { get; set; }
        public virtual User User { get; set; }
    }
}
