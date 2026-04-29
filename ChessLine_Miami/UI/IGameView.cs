using ChessLine_Miami.Models;
namespace ChessLine_Miami.UI;

public interface IGameView
{
    EnemiesViewer EnemiesViewer { get; }
    LevelViewer LevelViewer { get; }
    PlayerViewer PlayerViewer { get; }

    // Событие: Форма просто говорит, что нажата клавиша
    event Action<Keys> KeyPressed;

    // Свойства: Презентер будет "запихивать" сюда данные для отрисовки
    Size ClientSize { get; }
    
    
    void SetGame(Game game);

    void Redraw(); // Просто вызов Invalidate()
    
}