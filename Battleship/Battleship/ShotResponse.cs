using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class ShotResponse
    {
        public Shots ShotStatus { get; set; }
        public string ShipImpacted { get; set; }
    }
}

