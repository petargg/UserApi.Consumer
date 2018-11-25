using System;
using System.Collections.Generic;
using System.Text;

namespace UserApi.Consumer
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            return $"Person: {Name} Email: {Email}";
        }
    }
}
