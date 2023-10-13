using System;
using System.Collections.Generic;

namespace WpfSnake
{
  /// <summary>
  /// Представляет направление движения.
  /// </summary>
  public class Direction
  {
    #region Константы
    /// <summary>
    /// Направление влево.
    /// </summary>
    public readonly static Direction Left = new Direction(0, -1);

    /// <summary>
    /// Направление вправо.
    /// </summary>
    public readonly static Direction Right = new Direction(0, 1);

    /// <summary>
    /// Направление вправо.
    /// </summary>
    public readonly static Direction Up = new Direction(-1, 0);

    /// <summary>
    /// Направление вниз.
    /// </summary>
    public readonly static Direction Down = new Direction(1, 0);

    #endregion

    #region Поля и свойства
    /// <summary>
    /// Получает смещение по строкам.
    /// </summary>
    public int RowOffset { get;}

    /// <summary>
    /// Получает смещение по столбцам.
    /// </summary>
    public int ColumnOffset { get;}
    #endregion

    #region Конструктор
    /// <summary>
    /// Инициализирует новый экземпляр класса Direction с указанными смещениями по строкам и столбцам.
    /// </summary>
    /// <param name="rowOffset">Смещение по строкам.</param>
    /// <param name="columnOffset">Смещение по столбцам.</param>
    private Direction(int rowOffset, int columnOffset)
    {
      RowOffset = rowOffset;
      ColumnOffset = columnOffset;
    }
    #endregion

    #region Методы
    /// <summary>
    /// Возвращает противоположное направление.
    /// </summary>
    public Direction Opposite()
    {
      return new Direction(-RowOffset, -ColumnOffset);
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      return obj is Direction direction &&
             RowOffset == direction.RowOffset &&
             ColumnOffset == direction.ColumnOffset;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      return HashCode.Combine(RowOffset, ColumnOffset);
    }

    /// <summary>
    /// Проверяет, равны ли два направления.
    /// </summary>
    public static bool operator ==(Direction left, Direction right)
    {
      return EqualityComparer<Direction>.Default.Equals(left, right);
    }

    /// <summary>
    /// Проверяет, не равны ли два направления.
    /// </summary>
    public static bool operator !=(Direction left, Direction right)
    {
      return !(left == right);
    }
    #endregion
  }
}
