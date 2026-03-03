using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    public class Player
    {
        public Guid id { get; set; }

        public string displayName { get; set; }

        public int number { get; set; }
        public Player(string name, int number) {
            this.displayName = name;
            this.number = number;
        }
    }
}
