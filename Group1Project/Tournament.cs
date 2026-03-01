using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Tournament
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        
        public void AddDivision(Division division)
        {
            // Code to add a division to the tournament
        }
    }
}
