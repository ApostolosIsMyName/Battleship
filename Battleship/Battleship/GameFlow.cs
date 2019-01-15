using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class GameFlow
    {
        Game gm;

        public GameFlow()
        {
            gm = new Game() { IsPlayer1 = false, Player1 = new Player(), Player2 = new Player() };
        }

        public void Start()
        {
            GameSetup GameSetup = new GameSetup(gm);
            GameSetup.Setup();

            do
            {
                GameSetup.SetBoard();
                ShotResponse shotresponse;
                do
                {
                    OutputHandler.ResetScreen(new Player[] { gm.Player1, gm.Player2 });
                    OutputHandler.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player1 : gm.Player2);
                    OutputHandler.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1);
                    Coordinate ShotPoint = new Coordinate(1, 1);
                    shotresponse = Shot(gm.IsPlayer1 ? gm.Player2 : gm.Player1, gm.IsPlayer1 ? gm.Player1 : gm.Player2, out ShotPoint);

                    OutputHandler.ResetScreen(new Player[] { gm.Player1, gm.Player2 });
                    OutputHandler.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player1 : gm.Player2);
                    OutputHandler.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1);
                    OutputHandler.ShowShotResult(shotresponse, ShotPoint, gm.IsPlayer1 ? gm.Player1.Name : gm.Player2.Name);
                    if (shotresponse.ShotStatus != Shots.Victory)
                    {
                        Console.WriteLine("Press any key to continue to switch to " + (gm.IsPlayer1 ? gm.Player2.Name : gm.Player1.Name));
                        gm.IsPlayer1 = !gm.IsPlayer1;
                        Console.ReadKey();
                    }
                } while (shotresponse.ShotStatus != Shots.Victory);

            } while (InputHandler.CheckQuit());
        }


        private ShotResponse Shot(Player victim, Player Shoter, out Coordinate ShotPoint)
        {
            ShotResponse fire; Coordinate WhereToShot;
            do
            {
                if (!Shoter.IsPC)
                {
                    WhereToShot = InputHandler.GetShotLocationFromUser();
                    fire = victim.PlayerBoard.FireShot(WhereToShot);
                    if (fire.ShotStatus == Shots.Invalid || fire.ShotStatus == Shots.Duplicate)
                        OutputHandler.ShowShotResult(fire, WhereToShot, "");
                }
                else
                {
                    WhereToShot = InputHandler.GetShotLocationFromComputer(victim.PlayerBoard, Shoter.GameLevel);
                    fire = victim.PlayerBoard.FireShot(WhereToShot);
                }
                if (fire.ShotStatus == Shots.Victory)
                {
                    if (gm.IsPlayer1) gm.Player1.Win += 1;
                    else gm.Player2.Win += 1;
                }
            } while (fire.ShotStatus == Shots.Duplicate || fire.ShotStatus == Shots.Invalid);
            ShotPoint = WhereToShot;
            return fire;
        }
    }
}
