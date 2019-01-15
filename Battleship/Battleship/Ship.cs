using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Ship
    {
        public ShipType ShipType { get; private set; }
        public string ShipName { get { return ShipType.ToString(); } }
        public Coordinate[] BoardPositions { get; set; }
        private int _lifeRemaining;
        public bool IsSunk { get { return _lifeRemaining == 0; } }

        public Ship(ShipType shipType, int numberOfSlots)
        {
            ShipType = shipType;
            _lifeRemaining = numberOfSlots;
            BoardPositions = new Coordinate[numberOfSlots];
        }

        public Shots FireAtShip(Coordinate position)
        {
            if (BoardPositions.Contains(position))
            {
                _lifeRemaining--;

                if (_lifeRemaining == 0)
                    return Shots.HitAndSunk;

                return Shots.Hit;
            }

            return Shots.Miss;
        }
    }
}
