using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSnake
{
  /// <summary>
  /// Представляет состояние игры "Змейка".
  /// </summary>
  public class GameState
  {
    #region Поля и свойства
    /// <summary>
    /// Количество строк в игровом поле.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Количество столбцов в игровом поле.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Двумерный массив, представляющий игровое поле с ячейками.
    /// </summary>
    public GridValue[,] Grid { get; }

    /// <summary>
    /// Направление движения змеи.
    /// </summary>
    public Direction Dir { get; private set; }

    /// <summary>
    /// Текущий счет в игре.
    /// </summary>
    public int Score { get; private set; }

    /// <summary>
    /// Значение, указывающее, завершена ли игра.
    /// </summary>
    public bool GameOver { get; private set; }
    
    /// <summary>
    /// Значение, указывающее, завершена ли игра победой.
    /// </summary>
    public bool GameWin { get; private set; }

    /// <summary>
    /// Список изменений направления движения змеи.
    /// </summary>
    private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();

    /// <summary>
    /// Список позиций змеи на игровом поле.
    /// </summary>
    private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();

    /// <summary>
    /// Генератор случайных чисел для размещения еды на игровом поле.
    /// </summary>
    private readonly Random random = new Random();
    #endregion

    #region Конструктор
    /// <summary>
    /// Инициализирует новый экземпляр класса GameState с указанным количеством строк и столбцов в игровом поле.
    /// </summary>
    /// <param name="rows">Количество строк в игровом поле.</param>
    /// <param name="colums">Количество столбцов в игровом поле.</param>
    public GameState(int rows, int colums)
    {
      Rows = rows;
      Columns = colums;
      Grid = new GridValue[rows, colums];
      Dir = Direction.Right;
      GameWin = false;
      GameOver = false;

      AddSnake();
      AddFood();
    }
    #endregion

    #region Методы
    /// <summary>
    /// Добавляет начальное положение змеи на игровое поле.
    /// </summary>
    private void AddSnake()
    {
      int r = Rows / 2;

      for (int c = 1; c <= 3; c++)
      {
        Grid[r, c] = GridValue.Snake;
        snakePositions.AddFirst(new Position(r, c));
      }
    }

    /// <summary>
    /// Возвращает пустые позиции на игровом поле.
    /// </summary>
    private IEnumerable<Position> EmptyPosition()
    {
      for (int r = 0; r < Rows; r++)
      {
        for (int c = 0; c < Columns; c++)
        {
          if (Grid[r, c] == GridValue.Empty)
          {
            yield return new Position(r, c);
          }
        }
      }
    }

    /// <summary>
    /// Добавляет еду на игровое поле.
    /// </summary>
    private void AddFood()
    {
      List<Position> empty = new List<Position>(EmptyPosition());

      if (empty.Count == 0)
      {
        GameWin = true;
        return;
      }

      Position pos = empty[random.Next(0, empty.Count)];
      Grid[pos.Row, pos.Column] = GridValue.Food;
    }

    /// <summary>
    /// Получает позицию головы змеи.
    /// </summary>
    public Position HeadPosition()
    {
      return snakePositions.First.Value;
    }

    /// <summary>
    /// Получает позицию хвоста змеи.
    /// </summary>
    public Position TailPosition()
    {
      return snakePositions.Last.Value;
    }

    /// <summary>
    /// Получает позиции змеи на игровом поле.
    /// </summary>
    public IEnumerable<Position> SnakePositions()
    {
      return snakePositions;
    }

    /// <summary>
    /// Добавляет голову змеи на указанную позицию.
    /// </summary>
    private void AddHead(Position pos)
    {
      snakePositions.AddFirst(pos);
      Grid[pos.Row, pos.Column] = GridValue.Snake;
    }

    /// <summary>
    /// Удаляет хвост змеи.
    /// </summary>
    private void RemoveTail()
    {
      Position tail =snakePositions.Last.Value;
      Grid[tail.Row, tail.Column] = GridValue.Empty;
      snakePositions.RemoveLast();
    }

    /// <summary>
    /// Получает последнее направление из списка изменений направления.
    /// Если список пуст, возвращает текущее направление змеи.
    /// </summary>
    private Direction GetLstDirection()
    {
      if (dirChanges.Count == 0)
      {
        return Dir;
      }

      return dirChanges.Last.Value;
    }

    /// <summary>
    /// Проверяет, можно ли изменить направление движения на указанное.
    /// </summary>
    /// <param name="newDir">Новое направление.</param>
    /// <returns>True, если можно изменить направление, иначе False.</returns>
    private bool CanChangeDirectiron(Direction newDir)
    {
      if (dirChanges.Count == 2)
      {
        return false;
      }

      Direction lastDir = GetLstDirection();
      return newDir != lastDir && newDir != lastDir.Opposite(); 
    }

    /// <summary>
    /// Изменяет направление движения змеи.
    /// </summary>
    /// <param name="dir">Новое направление.</param>
    public void ChangeDirection(Direction dir)
    {
      if (CanChangeDirectiron(dir))
      {
        dirChanges.AddLast(dir);
      }
    }

    /// <summary>
    /// Проверяет, находится ли позиция вне игрового поля.
    /// </summary>
    private bool OutsideGrid(Position pos)
    {
      return pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Columns;
    }

    /// <summary>
    /// Проверяет, на что попадет змея при движении в новую позицию головы.
    /// </summary>
    /// <param name="newHeadPos">Новая позиция головы змеи.</param>
    /// <returns>Значение GridValue, представляющее результат столкновения.</returns>
    private GridValue WillHit(Position newHeadPos)
    {
      if (OutsideGrid(newHeadPos))
      {
        return GridValue.Outside;
      }

      if (newHeadPos == TailPosition())
      {
        return GridValue.Empty;
      }

      return Grid[newHeadPos.Row, newHeadPos.Column];
    }

    /// <summary>
    /// Выполняет шаг движения змеи.
    /// </summary>
    public void Move()
    {
      if (dirChanges.Count > 0)
      {
        Dir = dirChanges.First.Value;
        dirChanges.RemoveFirst();
      }

      Position newHeadPos = HeadPosition().Translate(Dir);
      GridValue hit = WillHit(newHeadPos);

      if (hit == GridValue.Outside || hit == GridValue.Snake)
      {
        GameOver = true;
      }
      else if (hit == GridValue.Empty)
      {
        RemoveTail();
        AddHead(newHeadPos);
      }
      else if (hit == GridValue.Food)
      {
        AddHead(newHeadPos);
        Score++;
        AddFood();
      }

    }
    #endregion
  }
}
