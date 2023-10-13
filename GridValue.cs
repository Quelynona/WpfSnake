namespace WpfSnake
{
  /// <summary>
  /// Перечисление, представляющее значения ячеек игрового поля.
  /// </summary>
  public enum GridValue
  {
    /// <summary>
    /// Пустая ячейка.
    /// </summary>
    Empty,
    /// <summary>
    /// Ячейка, занятая змеей.
    /// </summary>
    Snake,
    /// <summary>
    /// Ячейка с едой.
    /// </summary>
    Food,
    /// <summary>
    /// Ячейка за пределами игрового поля.
    /// </summary>
    Outside
  }
}
