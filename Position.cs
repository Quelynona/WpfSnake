using System;
using System.Collections.Generic;

namespace WpfSnake
{
  /// <summary>
  /// Представляет позицию на игровом поле с указанием строки и столбца.
  /// </summary>
  public class Position
  {
    #region Поля и свойства
    /// <summary>
    /// Получает номер строки.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Получает номер столбца.
    /// </summary>
    public int Column { get; }
    #endregion

    #region Конструктор
    /// <summary>
    /// Инициализирует новый экземпляр класса Position с указанными координатами.
    /// </summary>
    /// <param name="row">Номер строки.</param>
    /// <param name="column">Номер столбца.</param>
    public Position(int row, int column)
    {
      Row = row;
      Column = column;
    }
    #endregion

    #region Методы
    /// <summary>
    /// Смещает текущую позицию в соответствии с заданным направлением.
    /// </summary>
    /// <param name="dir">Направление для смещения.</param>
    /// <returns>Новая позиция после смещения.</returns>
    public Position Translate(Direction dir)
    {
      return new Position(Row + dir.RowOffset, Column + dir.ColumnOffset);
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      return obj is Position position &&
             Row == position.Row &&
             Column == position.Column;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      return HashCode.Combine(Row, Column);
    }

    /// <summary>
    /// Проверяет, равны ли две позиции.
    /// </summary>
    public static bool operator == (Position left, Position right)
    {
      return EqualityComparer<Position>.Default.Equals(left, right);
    }

    /// <summary>
    /// Проверяет, не равны ли две позиции.
    /// </summary>
    public static bool operator != (Position left, Position right)
    {
      return !(left == right);
    }
    #endregion
  }
}
