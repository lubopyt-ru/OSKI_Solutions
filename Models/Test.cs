using System;
using System.Collections.Generic;

namespace OSKI_Solutions_Test.Models
{
    public partial class Test
    {
        public Test()
        {
            Answers = new HashSet<Answers>();
            Test_User_Junction = new HashSet<Test_User_Junction>();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Answers> Answers { get; set; }
        public virtual ICollection<Test_User_Junction> Test_User_Junction { get; set; }
    }
}
