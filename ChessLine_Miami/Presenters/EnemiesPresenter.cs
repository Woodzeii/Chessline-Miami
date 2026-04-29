using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Presenters;

public class EnemiesPresenter
{
    private readonly Game _game;
    private readonly Random _random = new Random();

    public EnemiesPresenter(Game game)
    {
        _game = game;
    }

    public List<Enemy> Enemies => _game.Enemies;

    public void UpdateEnemies()
    {
        foreach (var enemy in _game.Enemies.Where(e => e.IsAlive))
        {
            MoveEnemyByType(enemy);
        }
        
    }
    

    public void MoveEnemyByType(Enemy enemy)
    {
        // Проверяем видимость игрока
        if (!enemy.IsPlayerSeen)
        {
            enemy.IsPlayerSeen = PathFinder.IsPlayerSeen(_game.Player, enemy, _game.Level);
        }
        //enemy.IsPlayerSeen = PathFinder.IsPlayerSeen(_game.Player, enemy, _game.Level);
        
        
        var moves = GetValidMoves(enemy);
        
        // Отладочная информация
        System.Diagnostics.Debug.WriteLine($"Enemy at ({enemy.Pos.X},{enemy.Pos.Y}): {moves.Count} moves, sees player: {enemy.IsPlayerSeen}");
        
        if (moves.Count > 0)
        {
            Point move;
            
            if (enemy.IsPlayerSeen)
            {
                // Если видит игрока - выбираем ближайший ход
                move = GetNearestMove(enemy, moves);
            }
            //else
            //{
                //// Случайное движение (блуждание)
                //move = moves[_random.Next(moves.Count)];
            //}
            else move = Point.Empty;
            
            enemy.TryMove(move.X, move.Y, _game);
            System.Diagnostics.Debug.WriteLine($"Enemy moved to ({enemy.Pos.X},{enemy.Pos.Y})");
        }
    }

    private Point GetNearestMove(Enemy enemy, List<Point> moves)
    {
        var playerPos = _game.Player.FieldPos;
        var bestMove = moves[0];
        var bestDistance = double.MaxValue;
        
        foreach (var move in moves)
        {
            var newPos = new Point(enemy.Pos.X + move.X, enemy.Pos.Y + move.Y);
            var distance = Math.Sqrt(
                Math.Pow(newPos.X - playerPos.X, 2) + 
                Math.Pow(newPos.Y - playerPos.Y, 2)
            );
            
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestMove = move;
            }
        }
        
        return bestMove;
    }

    public List<Point> GetValidMoves(Enemy enemy)
    {
        return enemy.Type switch
        {
            EnemyType.Pawn => GetPawnMoves(enemy),
            EnemyType.Knight => GetKnightMoves(enemy),
            EnemyType.Bishop => GetBishopMoves(enemy),
            EnemyType.Rook => GetRookMoves(enemy),
            EnemyType.Queen => GetQueenMoves(enemy),
            _ => new List<Point>()
        };
    }

    private List<Point> GetPawnMoves(Enemy enemy)
    {
        var dirs = new List<Point>();
        dirs.Add(new Point(0, 1));
        dirs.Add(new Point(1, 0));
        dirs.Add(new Point(-1, 0));
        dirs.Add(new Point(0, -1));

        // Пешка двигается только вниз (по полю)
        var moves = new List<Point>();
        
        foreach (var dir in dirs)
        {
            var newPos = new Point(enemy.Pos.X + dir.X, enemy.Pos.Y + dir.Y);
            if (IsValidMove(newPos))
                moves.Add(dir);
        }
        return moves;
    }

    private List<Point> GetKnightMoves(Enemy enemy)
    {
        // Конь - буква Г
        var offsets = new[]
        {
            new Point(-2, -1), new Point(-2, 1),
            new Point(-1, -2), new Point(-1, 2),
            new Point(1, -2), new Point(1, 2),
            new Point(2, -1), new Point(2, 1)
        };
        
        return offsets
            .Select(o => new Point(enemy.Pos.X + o.X, enemy.Pos.Y + o.Y))
            .Where(IsValidMove)
            .Select(o => new Point(o.X - enemy.Pos.X, o.Y - enemy.Pos.Y))
            .ToList();
    }

    private List<Point> GetBishopMoves(Enemy enemy)
    {
        // Слон - по диагонали
        return GetSlidingMoves(enemy, new[] { new Point(1, 1), new Point(1, -1), new Point(-1, 1), new Point(-1, -1) });
    }

    private List<Point> GetRookMoves(Enemy enemy)
    {
        // Ладья - по прямым
        return GetSlidingMoves(enemy, new[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) });
    }

    private List<Point> GetQueenMoves(Enemy enemy)
    {
        // Ферзь - и по диагонали, и по прямым
        return GetSlidingMoves(enemy, new[]
        {
            new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0),
            new Point(1, 1), new Point(1, -1), new Point(-1, 1), new Point(-1, -1)
        });
    }

    private List<Point> GetSlidingMoves(Enemy enemy, Point[] directions)
    {
        var moves = new List<Point>();
        
        foreach (var dir in directions)
        {
            var current = enemy.Pos;
           var step = 1;
            while (true)
            {
                var next = new Point(enemy.Pos.X + dir.X * step, enemy.Pos.Y + dir.Y * step);
                if (!IsValidMove(next)) break;
                moves.Add(new Point(dir.X * step, dir.Y * step));
                step++;
            }
        }
        
        return moves;
    }

    private bool IsValidMove(Point pos)
    {
        if (pos.X < 0 || pos.X >= _game.Level.Size.Width ||
            pos.Y < 0 || pos.Y >= _game.Level.Size.Height)
            return false;
        
        var sector = _game.Level.Field[pos.X, pos.Y];
        return sector != SectorType.Wall && sector != SectorType.Lava;
    }

    public void MoveEnemy(Enemy enemy, int deltaX, int deltaY)
    {
        if (enemy.IsAlive)
        {
            enemy.TryMove(deltaX, deltaY, _game);
        }
    }

    public List<Enemy> GetActiveEnemies()
    {
        return _game.Enemies.Where(e => e.IsAlive).ToList();
    }
}