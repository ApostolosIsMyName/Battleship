using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Player
    {
        public string Name { get; set; }
        public int Win { get; set; }
        public bool IsPC { get; set; }
        public Board PlayerBoard;
        public Level GameLevel { get; set; }
    }
}
