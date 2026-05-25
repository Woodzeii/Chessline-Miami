using System.Drawing;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Models;

public class Player
{
     public const float FrameUpdateSpeed = 0.20f;
     public Point FieldPos;
     public PointF RenderFieldPos;
     public bool IsAlive;
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
          ResetRenderPosition();
     }

     public void ResetRenderPosition()
     {
          RenderFieldPos = new PointF(FieldPos.X, FieldPos.Y);
     }

     public void UpdateRenderPosition(float smoothing = FrameUpdateSpeed)
     {
          var target = new PointF(FieldPos.X, FieldPos.Y);
          RenderFieldPos = new PointF(
               RenderFieldPos.X + (target.X - RenderFieldPos.X) * smoothing,
               RenderFieldPos.Y + (target.Y - RenderFieldPos.Y) * smoothing
          );

          if (Math.Abs(target.X - RenderFieldPos.X) < 0.01f && Math.Abs(target.Y - RenderFieldPos.Y) < 0.01f)
          {
               RenderFieldPos = target;
          }
     }

     public void TryMove(int deltaX, int deltaY, Game game)
     {
          TryMove(deltaX, deltaY, game, 1);
     }

     public bool TryMoveOnPoint(Point newPos, Game game)
     {
          if (CollisionDetector.CanMoveTo(newPos, game))
          {
                FieldPos = newPos;
                return true;
          }
          return false;
     }

     public bool TryMove(int deltaX, int deltaY, Game game, int steps)
     {
          if (steps <= 1)
          {
               var newPos = new Point(FieldPos.X + deltaX, FieldPos.Y + deltaY);
               if (CollisionDetector.CanMoveTo(newPos, game))
               {
                    FieldPos = newPos;
                    return true;
               }
               return false;
          }

          var currentPos = FieldPos;
          for (int step = 1; step <= steps; step++)
          {
               var nextPos = new Point(currentPos.X + deltaX, currentPos.Y + deltaY);
               if (!CollisionDetector.CanMoveTo(nextPos, game))
               {
                    return false;
               }
               currentPos = nextPos;
          }

          FieldPos = currentPos;
          return false;
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
          return RushCooldown == 0;
     }
}