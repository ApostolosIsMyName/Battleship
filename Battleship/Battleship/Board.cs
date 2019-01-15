using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
   
        public class Board
        {
            public const int xCoordinator = 10;
            public const int yCoordinator = 10;
            private Dictionary<Coordinate, ShotRecord> ShotHistory;
            private int _currentShipIndex;

            public Ship[] Ships { get; private set; }

            public Board()
            {
                ShotHistory = new Dictionary<Coordinate, ShotRecord>();
                Ships = new Ship[5];
                _currentShipIndex = 0;
            }

            public ShotResponse FireShot(Coordinate coordinate)
            {
                var response = new ShotResponse();

                // is this coordinate on the board?
                if (!IsValidCoordinate(coordinate))
                {
                    response.ShotStatus = Shots.Invalid;
                    return response;
                }

                // did they already try this position?
                if (ShotHistory.ContainsKey(coordinate))
                {
                    response.ShotStatus = Shots.Duplicate;
                    return response;
                }

                CheckShipsForHit(coordinate, response);
                CheckForVictory(response);

                return response;
            }

            public ShotRecord CheckCoordinate(Coordinate coordinate)
            {
                if (ShotHistory.ContainsKey(coordinate))
                {
                    return ShotHistory[coordinate];
                }
                else
                {
                    return ShotRecord.Unknown;
                }
            }

            public ShipPlacing PlaceShip(ShipPlacement request)
            {
                if (_currentShipIndex > 4)
                    throw new Exception("You can not add another ship, 5 is the limit!");

                if (!IsValidCoordinate(request.Coordinate))
                    return ShipPlacing.NotEnoughSpace;

                Ship newShip = ShipAllocator.CreateShip(request.ShipType);
                switch (request.Direction)
                {
                    case ShipDirection.Down:
                        return PlaceShipDown(request.Coordinate, newShip);
                    case ShipDirection.Up:
                        return PlaceShipUp(request.Coordinate, newShip);
                    case ShipDirection.Left:
                        return PlaceShipLeft(request.Coordinate, newShip);
                    default:
                        return PlaceShipRight(request.Coordinate, newShip);
                }
            }

            private void CheckForVictory(ShotResponse response)
            {
                if (response.ShotStatus == Shots.HitAndSunk)
                {
                    // did they win?
                    if (Ships.All(s => s.IsSunk))
                        response.ShotStatus = Shots.Victory;
                }
            }

            private void CheckShipsForHit(Coordinate coordinate, ShotResponse response)
            {
                response.ShotStatus = Shots.Miss;

                foreach (var ship in Ships)
                {
                    // no need to check sunk ships
                    if (ship.IsSunk)
                        continue;

                    Shots status = ship.FireAtShip(coordinate);

                    switch (status)
                    {
                        case Shots.HitAndSunk:
                            response.ShotStatus = Shots.HitAndSunk;
                            response.ShipImpacted = ship.ShipName;
                            ShotHistory.Add(coordinate, ShotRecord.Hit);
                            break;
                        case Shots.Hit:
                            response.ShotStatus = Shots.Hit;
                            response.ShipImpacted = ship.ShipName;
                            ShotHistory.Add(coordinate, ShotRecord.Hit);
                            break;
                    }

                    // if they hit something, no need to continue looping
                    if (status != Shots.Miss)
                        break;
                }

                if (response.ShotStatus == Shots.Miss)
                {
                    ShotHistory.Add(coordinate, ShotRecord.Miss);
                }
            }

            private bool IsValidCoordinate(Coordinate coordinate)
            {
                return coordinate.XCoordinate >= 1 && coordinate.XCoordinate <= xCoordinator &&
                coordinate.YCoordinate >= 1 && coordinate.YCoordinate <= yCoordinator;
            }

            private ShipPlacing PlaceShipRight(Coordinate coordinate, Ship newShip)
            {
                // y coordinate gets bigger
                int positionIndex = 0;
                int maxY = coordinate.YCoordinate + newShip.BoardPositions.Length;

                for (int i = coordinate.YCoordinate; i < maxY; i++)
                {
                    var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);
                    if (!IsValidCoordinate(currentCoordinate))
                        return ShipPlacing.NotEnoughSpace;

                    if (OverlapsAnotherShip(currentCoordinate))
                        return ShipPlacing.Overlap;

                    newShip.BoardPositions[positionIndex] = currentCoordinate;
                    positionIndex++;
                }

                AddShipToBoard(newShip);
                return ShipPlacing.Ok;
            }

            private ShipPlacing PlaceShipLeft(Coordinate coordinate, Ship newShip)
            {
                // y coordinate gets smaller
                int positionIndex = 0;
                int minY = coordinate.YCoordinate - newShip.BoardPositions.Length;

                for (int i = coordinate.YCoordinate; i > minY; i--)
                {
                    var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);

                    if (!IsValidCoordinate(currentCoordinate))
                        return ShipPlacing.NotEnoughSpace;

                    if (OverlapsAnotherShip(currentCoordinate))
                        return ShipPlacing.Overlap;

                    newShip.BoardPositions[positionIndex] = currentCoordinate;
                    positionIndex++;
                }

                AddShipToBoard(newShip);
                return ShipPlacing.Ok;
            }

            private ShipPlacing PlaceShipUp(Coordinate coordinate, Ship newShip)
            {
                // x coordinate gets smaller
                int positionIndex = 0;
                int minX = coordinate.XCoordinate - newShip.BoardPositions.Length;

                for (int i = coordinate.XCoordinate; i > minX; i--)
                {
                    var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                    if (!IsValidCoordinate(currentCoordinate))
                        return ShipPlacing.NotEnoughSpace;

                    if (OverlapsAnotherShip(currentCoordinate))
                        return ShipPlacing.Overlap;

                    newShip.BoardPositions[positionIndex] = currentCoordinate;
                    positionIndex++;
                }

                AddShipToBoard(newShip);
                return ShipPlacing.Ok;
            }

            private ShipPlacing PlaceShipDown(Coordinate coordinate, Ship newShip)
            {
                // y coordinate gets bigger
                int positionIndex = 0;
                int maxX = coordinate.XCoordinate + newShip.BoardPositions.Length;

                for (int i = coordinate.XCoordinate; i < maxX; i++)
                {
                    var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                    if (!IsValidCoordinate(currentCoordinate))
                        return ShipPlacing.NotEnoughSpace;

                    if (OverlapsAnotherShip(currentCoordinate))
                        return ShipPlacing.Overlap;

                    newShip.BoardPositions[positionIndex] = currentCoordinate;
                    positionIndex++;
                }

                AddShipToBoard(newShip);
                return ShipPlacing.Ok;
            }

            private void AddShipToBoard(Ship newShip)
            {
                Ships[_currentShipIndex] = newShip;
                _currentShipIndex++;
            }

            private bool OverlapsAnotherShip(Coordinate coordinate)
            {
                foreach (var ship in Ships)
                {
                    if (ship != null)
                    {
                        if (ship.BoardPositions.Contains(coordinate))
                            return true;
                    }
                }

                return false;
            }
        }
    }

