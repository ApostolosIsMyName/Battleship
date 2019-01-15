using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship
{
    public class OutputHandler
    {
        static int counttime = 0;

        public static void ShowFlashScreen()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("        ***********************************");
            Console.Write("        *"); Console.ForegroundColor = ConsoleColor.White; Console.Write("    Welcome to Battleship!!!     "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("*");
            Console.WriteLine("        ***********************************");
            Console.ForegroundColor = ConsoleColor.White;

            Timer t = new Timer(ClearFlashScreen, null, 0, 1000);
            Thread.Sleep(2100); // Simulating other work (10 seconds)
            t.Dispose(); // Cancel the timer now
        }

        private static void ClearFlashScreen(Object state)
        {
            if (counttime < 2)
                counttime += 1;
            else
            {
                Console.Clear();
                counttime = 0;
            }
        }

        public static void ShowHeader()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow ;
            Console.WriteLine("        ***************************");
            Console.Write("        *"); Console.ForegroundColor = ConsoleColor.White;
            Console.Write("        BATTLESHIP       "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("*");
            Console.WriteLine("        ***************************");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ShowAllPlayer(Player[] player)
        {
            string str = "Player 1: " + player[0].Name + "(Win: " + player[0].Win + ")\t Player 2: " + player[1].Name + "(win: " + player[1].Win + ")";
            if (player[1].IsPC)
                str += "\tLevel: " + player[1].GameLevel.ToString();
            Console.WriteLine(str);
            Console.WriteLine("");
        }




        public static void ShowWhoseTurn(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(player.Name + " turn... ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;

        }

        static string GetLetterFromNumber(int number)
        {
            string result = "";
            switch (number)
            {
                case 1:
                    result = "A";
                    break;
                case 2:
                    result = "B";
                    break;
                case 3:
                    result = "C";
                    break;
                case 4:
                    result = "D";
                    break;
                case 5:
                    result = "E";
                    break;
                case 6:
                    result = "F";
                    break;
                case 7:
                    result = "G";
                    break;
                case 8:
                    result = "H";
                    break;
                case 9:
                    result = "I";
                    break;
                case 10:
                    result = "J";
                    break;
                default:
                    break;
            }
            return result;
        }

        public static void DrawHistory(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("  ");
            for (int y = 1; y <= 10; y++)
            {
                Console.Write(y);
                Console.Write(" ");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 1; x <= 10; x++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(GetLetterFromNumber(x) + " ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int y = 1; y <= 10; y++)
                {
                    //Console.Write(y);
                    ShotRecord history = player.PlayerBoard.CheckCoordinate(new Coordinate(x, y));
                    switch (history)
                    {
                        case ShotRecord.Hit:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("H");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case ShotRecord.Miss:
                            Console.Write("M");
                            break;
                        case ShotRecord.Unknown:
                            Console.Write(" ");
                            break;
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("");
        }

        public static void ShowShotResult(ShotResponse shotresponse, Coordinate c, string playername)
        {
            String str = "";
            switch (shotresponse.ShotStatus)
            {
                case Shots.Duplicate:
                    Console.ForegroundColor = ConsoleColor.Red;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Duplicate shot location!";
                    break;
                case Shots.Hit:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Hit!";
                    break;
                case Shots.HitAndSunk:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Hit and Sunk, " + shotresponse.ShipImpacted + "!";
                    break;
                case Shots.Invalid:
                    Console.ForegroundColor = ConsoleColor.Red;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Invalid hit location!";
                    break;
                case Shots.Miss:
                    Console.ForegroundColor = ConsoleColor.White;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Miss!";
                    break;
                case Shots.Victory:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Hit and Sunk, " + shotresponse.ShipImpacted + "! \n\n";
                    str += "       ******\n";
                    str += "       ******\n";
                    str += "        **** \n";
                    str += "         **  \n";
                    str += "         **  \n";
                    str += "       ******\n";
                    str += "Game Over, " + playername + " wins!";
                    break;
            }
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }

        static void test()
        {
            List<object> obj = new List<object>();
            Player player1 = new Player();
            Board board1 = new Board();
            obj.Add(board1);
            obj.Add(player1);
        }

        public static void ResetScreen(Player[] player)
        {
            Console.Clear();
            OutputHandler.ShowHeader();
            OutputHandler.ShowAllPlayer(player);
        }
    }
}
