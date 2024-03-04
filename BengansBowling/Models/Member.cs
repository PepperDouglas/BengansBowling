using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }

        public Member(string name, string address, string telephone) {
            Name = name;
            Address = address;
            Telephone = telephone;
        }
    }
}
