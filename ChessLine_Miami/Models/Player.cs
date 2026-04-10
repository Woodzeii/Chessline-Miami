namespace ChessLine_Miami.Models;
using Logic;

public class Player
{
     public Point FieldPos;
     public Point WorldPos;
     public bool IsAlive;
     public bool IsRushReady;

     public Player(Point fieldPos)
     {
          FieldPos = fieldPos;
          IsAlive = true;
          IsRushReady = true;
     }

     public void TryMove(int deltaX, int deltaY, Level level)
     {
          var newPos = new Point(FieldPos.X + deltaX, FieldPos.Y + deltaY);
          if (CollisionDetector.CanMoveTo(newPos, level))
          {
               FieldPos = newPos;
          }
     }
}