namespace ChessLine_Miami.Models;
using Logic;

public class Player
{
     public Point FieldPos;
     public Point WorldPos;
     public bool IsAlive;
     //public bool IsRushReady;
     public int RushCooldown;
     public const int StepsToRush = 5;
     public bool IsAttacking;
     public Point AttackTarget;
     public bool HavePistol;
     public bool HaveShotgun;
     public bool Ammo;

     public Player(Point fieldPos)
     {
          FieldPos = fieldPos;
          IsAlive = true;
          
          RushCooldown = StepsToRush;
          IsAttacking = false;
          AttackTarget = new Point(-1, -1);
     }

     public void TryMove(int deltaX, int deltaY, Game game)
     {
          TryMove(deltaX, deltaY, game, 1);
     }

     public void TryMove(int deltaX, int deltaY, Game game, int steps)
     {
          if (steps <= 1)
          {
               var newPos = new Point(FieldPos.X + deltaX, FieldPos.Y + deltaY);
               if (CollisionDetector.CanMoveTo(newPos, game))
               {
                    FieldPos = newPos;
               }
               return;
          }

          var currentPos = FieldPos;
          for (int step = 1; step <= steps; step++)
          {
               var nextPos = new Point(currentPos.X + deltaX, currentPos.Y + deltaY);
               if (!CollisionDetector.CanMoveTo(nextPos, game))
               {
                    return;
               }
               currentPos = nextPos;
          }

          FieldPos = currentPos;
     }

     public void SetAttackTarget(Point target)
     {
          AttackTarget = target;
          IsAttacking = true;
     }

     public void ClearAttack()
     {
          IsAttacking = false;
          AttackTarget = new Point(-1, -1);
     }
     public bool IsRushReady()
     {
          if (RushCooldown ==0)
               return true;
          return false;
     }
}