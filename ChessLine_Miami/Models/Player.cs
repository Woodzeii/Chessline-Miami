namespace ChessLine_Miami.Models;

public class Player
{
     public Point Pos;
     public bool IsAlive;
     public bool IsRushReady;

     public Player(Point pos)
     {
          Pos = pos;
          IsAlive = true;
          IsRushReady = true;
     }
}