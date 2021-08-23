using System;
using System.Collections.Generic;

namespace OSKI_Solutions_Test.Models
{
    public partial class User
    {
        public User()
        {
            Test_User_Junction = new HashSet<Test_User_Junction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Test_User_Junction> Test_User_Junction { get; set; }
    }
}
