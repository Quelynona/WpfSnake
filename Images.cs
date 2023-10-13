using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfSnake
{
  /// <summary>
  /// Класс, предоставляющий доступ к изображениям для игры Snake.
  /// </summary>
  public static class Images
  {
    #region Константы
    /// <summary>
    /// Пустая клетка.
    /// </summary>
    public readonly static ImageSource Empty = LoadImage("Empty.png");

    /// <summary>
    /// Тело змеи.
    /// </summary>
    public readonly static ImageSource Body = LoadImage("Body.png");

    /// <summary>
    /// Голова змеи.
    /// </summary>
    public readonly static ImageSource Head = LoadImage("Head.png");

    /// <summary>
    /// Еда для змеи.
    /// </summary>
    public readonly static ImageSource Food = LoadImage("Food.png");

    /// <summary>
    /// Мёртвое тело змеи.
    /// </summary>
    public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");

    /// <summary>
    /// Мёртвая голова змеи.
    /// </summary>
    public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");
    #endregion

    #region Методы
    /// <summary>
    /// Загрузка изображения из файла.
    /// </summary>
    /// <param name="fileName">Имя файла из папки Assets.</param>
    /// <returns>Объект ImageSource.</returns>
    private static ImageSource LoadImage(string fileName)
    {
      return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
    }
    #endregion
  }
}
