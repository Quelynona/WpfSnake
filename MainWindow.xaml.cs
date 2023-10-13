using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfSnake
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Поля и свойства
    private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
    {
      { GridValue.Empty, Images.Empty },
      { GridValue.Snake, Images.Body },
      { GridValue.Food, Images.Food }
    };

    private readonly Dictionary<Direction, int> dirToRotation = new()
    {
      { Direction.Up, 0 },
      { Direction.Right, 90 },
      { Direction.Down, 180 },
      {Direction.Left, 270 }
    };

    private readonly int rows = 16, columns = 32;
    private readonly Image[,] gridImages;
    private GameState gameState;
    private bool gameRunning;
    #endregion

    #region Конструктор
    /// <summary>
    /// Инициализирует новый экземпляр класса MainWindow.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      gridImages = SetupGrid();
      gameState = new GameState(rows, columns);
    }
    #endregion

    #region Методы
    /// <summary>
    /// Запускает игру.
    /// </summary>
    private async Task RunGame()
    {
      Draw();
      await ShowCountDown();
      Overlay.Visibility = Visibility.Hidden;
      await GameLoop();
      await ShowGameOver();
      gameState = new GameState(rows, columns);
    }

    /// <summary>
    /// Обработчик события нажатия клавиш в окне (PreviewKeyDown).
    /// </summary>
    private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (Overlay.Visibility == Visibility.Visible)
      {
        e.Handled = true;
      }

      if (!gameRunning)
      {
        gameRunning= true;
        await RunGame();
        gameRunning= false;
      }
    }

    /// <summary>
    /// Обработчик события нажатия клавиш в окне (KeyDown).
    /// </summary>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (gameState.GameOver | gameState.GameWin)
      {
        return;
      }

      switch (e.Key)
      {
        case Key.Left:
          gameState.ChangeDirection(Direction.Left); 
          break;
        case Key.Right:
          gameState.ChangeDirection(Direction.Right);
          break;
        case Key.Up:
          gameState.ChangeDirection(Direction.Up);
          break;
        case Key.Down:
          gameState.ChangeDirection(Direction.Down);
          break;
      }
    }

    /// <summary>
    /// Основной игровой цикл.
    /// </summary>
    private async Task GameLoop()
    {
      while (!(gameState.GameOver | gameState.GameWin))
      {
        await Task.Delay(175);
        gameState.Move();
        Draw();
      }
    }

    /// <summary>
    /// Настроить сетку для отображения игрового поля.
    /// </summary>
    private Image[,] SetupGrid()
    {
      Image[,] images= new Image[rows, columns];
      GameGrid.Rows = rows;
      GameGrid.Columns = columns;
      GameGrid.Width = GameGrid.Height * (columns / (double)rows);

      for (int r = 0; r < rows; r++)
      {
        for (int c = 0; c < columns; c++)
        {
          Image image = new Image
          {
            Source = Images.Empty,
            RenderTransformOrigin = new Point(0.5, 0.5),
          };

          images[r, c] = image;
          GameGrid.Children.Add(image);
        }
      }

      return images;
    }

    /// <summary>
    /// Отрисовать игровое поле.
    /// </summary>
    private void Draw()
    {
      DrawGrid();
      DrawSnakeHead();
      ScoreText.Text = $"БАЛЛОВ: {gameState.Score}";
    }

    /// <summary>
    /// Отрисовать игровое поле.
    /// </summary>
    private void DrawGrid()
    {
      for (int r = 0; r < rows; r++)
      {
        for (int c = 0; c < columns; c++)
        {
          GridValue gridVal = gameState.Grid[r, c];
          gridImages[r, c].Source = gridValToImage[gridVal];
          gridImages[r, c].RenderTransform = Transform.Identity;
        }

      }
    }

    /// <summary>
    /// Отрисовать голову змеи.
    /// </summary>
    private void DrawSnakeHead()
    {
      Position headPos = gameState.HeadPosition();
      Image image = gridImages[headPos.Row, headPos.Column];
      image.Source = Images.Head;

      int rotation = dirToRotation[gameState.Dir];
      image.RenderTransform = new RotateTransform(rotation);
    }

    /// <summary>
    /// Отобразить анимацию мертвой змеи.
    /// </summary>
    private async Task DrawDeadSnake()
    {
      List<Position> positions = new List<Position>(gameState.SnakePositions());

      for (int i = 0; i < positions.Count; i++)
      {
        Position position= positions[i];
        ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
        gridImages[position.Row, position.Column].Source= source;
        await Task.Delay(50);
      }
    }

    /// <summary>
    /// Показать обратный отсчет перед началом игры.
    /// </summary>
    private async Task ShowCountDown() 
    {
      for (int i = 3; i >= 1 ; i--)
      {
        OverlayText.Text = i.ToString();
        await Task.Delay(500);
      }
    }

    /// <summary>
    /// Показать экран "Игра окончена".
    /// </summary>
    private async Task ShowGameOver()
    {
      await DrawDeadSnake();
      await Task.Delay(1000);
      Overlay.Visibility = Visibility.Visible;
      
      if (gameState.GameOver)
      {
        OverlayText.Text = "ПОПРОБУЙ СНОВА! НАЖМИ ЛЮБУЮ КНОПКУ";
      }
      else if (gameState.GameWin)
      {
        OverlayText.Text = "УРА! ПОБЕДА!";
      }
    }
    #endregion
  }
}
