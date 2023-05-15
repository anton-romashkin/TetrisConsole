using System;
using System.Timers;
using System.Drawing;
using TetrisConsole.Constants;
using TetrisConsole.Extensions;
using TetrisConsole.Models;
using TetrisConsole.Services;
using TetrisConsole.Enums;

namespace TetrisConsole.Workers;

public class TetrisWorker
{
    private readonly int[,] _board = new int[GameConstants.PlayZoneWidth, GameConstants.PlayZoneHeight];
    private readonly Random _random = new Random();

    private readonly ScoringService _scoringService = new ScoringService();
    private readonly MovingService _movingService = new MovingService();
    private readonly DisplayService _displayService = new DisplayService();

    private FigureModel _currentFigure = null!;
    private FigureModel _nextFigure = null!;
    private FigureModel? _holdFigure;

    private readonly int[,] _nextFigureSection = new int[4, 4];
    private readonly int[,] _holdFigureSection = new int[4, 4];

    private readonly System.Timers.Timer _gameFlowTimer = new();
    private int _gameSpeed;
    private int _level;
    private bool _isGameOVer;
    private bool _isValidToHold;

    /// <summary>
    /// Initializes the game components, sets up the game loop with a timer, and subscribes to event
    /// handlers for board changes and figure section changes. The method then handles the input with
    /// the HandleKey() method, and repeats until the game is over. When the game ends, after that
    /// the method displays the end-game screen.
    /// </summary>
    public void Start()
    {
        _displayService.DrawBorder();
        _gameSpeed = GameConstants.InitialGameSpeed;
        _gameFlowTimer.Interval = _gameSpeed;
        _gameFlowTimer.Elapsed += OnGameFlowTimerElapsed;
        _gameFlowTimer.Start();

        ConsoleKeyInfo keyInfo = default;

        LoadFigures();
        _displayService.DrawFigure(_currentFigure);

        do
        {
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);

                Task.Run(() => HandleKey(keyInfo.Key));
            }

        } while (!_isGameOVer);

        _displayService.DisplayEndGameScreen(_scoringService.Score);

        _gameFlowTimer.Elapsed -= OnGameFlowTimerElapsed;
        _gameFlowTimer.Stop();
    }

    /// <summary>
    /// Event handler that is triggered when a timer elapses,
    /// calls Update() medthod using the await keyword.
    /// </summary>
    private async void OnGameFlowTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        await Update();
    }

    /// <summary>
    /// Updates the game by erasing the current figure, moving it down, adding it to the board,
    /// checking for completed lines, creating a new figure, and drawing the new figure.
    /// Finally, the game level and speed are updated.
    /// </summary>
    /// <returns>Completed task.</returns>
    private Task Update()
    {
        _displayService.EraseFigure(_currentFigure);

        if (_movingService.MoveDown(_currentFigure, _board))
        {
            _displayService.DrawFigure(_currentFigure);
            return Task.CompletedTask;
        }

        AddFigureToBoard(_currentFigure);
        CheckLines();

        _currentFigure = _nextFigure;
        _nextFigure = CreateNewFigure();

        _displayService.DrawFigure(_currentFigure);

        UpdateLevelAndSpeed();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Updates the game difficulty based on the number of lines cleared. if the level is less than 11, the game speed
    /// is adjusted and the figures fall 15% faster. The game flow timer is then stopped,
    /// the interval is updated with the new game speed, and the timer is restarted.
    /// </summary>
    private void UpdateLevelAndSpeed()
    {
        int currentLevel = _scoringService.LinesCleared / 10;

        if (currentLevel <= _level)
            return;

        _level++;

        if (_level < 11)
        {
            _gameSpeed = (int)(_gameSpeed - _gameSpeed * 0.15);
        }

        _gameFlowTimer.Stop();
        _gameFlowTimer.Interval = _gameSpeed;
        _gameFlowTimer.Start();
    }

    /// <summary>
    /// Creates a new FigureModel instance by generating a random figure type,
    /// initializing it, and checking if it is valid to move to its initial position.
    /// If the figure cannot be moved, null is returned and the game is over.
    /// </summary>
    /// <returns>Created FigureModel.</returns>
    private FigureModel CreateNewFigure()
    {
        int randomIndexFigure = _random.Next(GameConstants.FigureTypesCount) + 1;
        var type = (FigureTypeEnum)Enum.Parse(typeof(FigureTypeEnum), randomIndexFigure.ToString());

        var createdFigure = new FigureModel();
        createdFigure.Initialize(type);

        var newPosition = createdFigure.Position;

        bool isMoveValid = _movingService.IsMoveValid(createdFigure, newPosition, _board);

        if (!isMoveValid)
        {
            _isGameOVer = true;
        }

        UpdateFigureSection(createdFigure, _nextFigureSection);
        _isValidToHold = true;

        return createdFigure;
    }

    /// <summary>
    ///  Initializes the _currentFigure and _nextFigure fields if they are null.
    ///  It uses the CreateNewFigure method to create the _nextFigure.
    /// </summary>
    private void LoadFigures()
    {
        _nextFigure ??= CreateNewFigure();

        _currentFigure ??= _nextFigure;

        _nextFigure = CreateNewFigure();
    }

    /// <summary>
    /// Sets the _isGameOver flag to true.
    /// </summary>
    private void StopGame() => _isGameOVer = true;

    /// <summary>
    /// Erases the current figure from the display, performs the action based on the key that was pressed,
    /// and then redraws the current figure on the display.
    /// </summary>
    /// <param name="key">Pressed keyboard key.</param>
    public void HandleKey(ConsoleKey key)
    {
        _displayService.EraseFigure(_currentFigure);

        switch (key)
        {
            case ConsoleKey.LeftArrow:
                _movingService.MoveLeft(_currentFigure, _board);
                break;
            case ConsoleKey.RightArrow:
                _movingService.MoveRight(_currentFigure, _board);
                break;
            case ConsoleKey.Spacebar:
                _movingService.DropFigure(_currentFigure, _board);
                break;
            case ConsoleKey.DownArrow:
                _movingService.MoveDown(_currentFigure, _board);
                break;
            case ConsoleKey.UpArrow:
                _currentFigure = _movingService.Rotate(_currentFigure, _board);
                break;
            case ConsoleKey.C:
                HoldFigure(_currentFigure);
                break;
            case ConsoleKey.Escape:
                StopGame();
                break;
        }

        _displayService.DrawFigure(_currentFigure);
    }

    /// <summary>
    /// Takes a FigureModel object, adds its blocks to the game board by updating
    /// the values in the _board array based on the figure's position and color,
    /// and then invokes the BoardChanged event.
    /// </summary>
    /// <param name="figure">Figure to land on the board.</param>
    private void AddFigureToBoard(FigureModel? figure)
    {
        if (figure == null)
        {
            return;
        }

        foreach (Point block in figure.Blocks)
        {
            int x = figure.Position.X + block.X;
            int y = figure.Position.Y + block.Y;
            _board[x, y] = (int)figure.Color;
        }

        _displayService.DrawGameBoard(_board);
    }

    /// <summary>
    /// Checks if the current figure is valid to hold. If no figures were held before then puts
    /// the current figure in the pocket and creates new figure. 
    /// Otherwise swaps the current figure with previously held figure. After that, the figure section in
    /// the game interface is updated. Finally, the isValidToHold flag is set to false to prevent
    /// holding the figure multiple times in one move.
    /// </summary>
    /// <param name="figure">Figure to hold.</param>
    private void HoldFigure(FigureModel? figure)
    {
        if (figure == null)
        {
            return;
        }

        if (!_isValidToHold)
            return;

        var newFigure = new FigureModel();
        newFigure.Initialize(figure.Type);

        if (_holdFigure == null)
            _currentFigure = CreateNewFigure();
        else
            _currentFigure = _holdFigure;

        _holdFigure = newFigure;
        UpdateFigureSection(newFigure, _holdFigureSection);

        _isValidToHold = false;
    }

    /// <summary>
    /// Clears the figure section of the game interface then redraws
    /// it with fhe FigureModel object provided.
    /// </summary>
    /// <param name="figure">Figure to draw.</param>
    /// <param name="section">Section to clear and update.</param>
    private void UpdateFigureSection(FigureModel figure, int[,] section)
    {
        for (int x = 0; x < GameConstants.FigureSectionWidth; x++)
        {
            for (int y = 0; y < GameConstants.FigureSectionHeight; y++)
            {
                section[x, y] = 0;
            }
        }

        foreach (Point block in figure.Blocks)
        {
            int x = block.X;
            int y = block.Y;
            section[x, y] = (int)figure.Color;
        }

        _displayService.DisplayInterface(_nextFigureSection, _holdFigureSection, _scoringService.Score, _level);
    }

    /// <summary>
    /// Checks every row in the game board to see if it is completely filled with blocks.
    /// If a row is full, the RemoveLine() method is called to remove that line and shift
    /// all the blocks above it down one row. The number of lines cleared is counted and passed
    /// to the _scoringService to update the game score. Finally, the _displayService is used
    /// to update the game board visuals.
    /// </summary>
    private void CheckLines()
    {
        int linesCleared = 0;

        for (int y = GameConstants.PlayZoneHeight - 1; y >= 0; y--)
        {
            if (IsLineFull(y))
            {
                RemoveLine(y);
                linesCleared++;
                y++;
            }
        }

        _scoringService.AddScoreForClearedLines(linesCleared);

        _displayService.DrawGameBoard(_board);
    }

    /// <summary>
    /// Checks if a given row in the game board is completely filled with blocks.
    /// </summary>
    /// <param name="row">Row to check.</param>
    /// <returns>True if the row is full and false otherwise.</returns>
    private bool IsLineFull(int row)
    {
        for (int x = 0; x < GameConstants.PlayZoneWidth; x++)
        {
            if (_board[x, row] == 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///  Shifts all the blocks above a given row down by one row and clears the top row.
    ///  This is done by copying the values of each block in the rows above the given
    ///  row to the row below it and clearing the top row.
    /// </summary>
    /// <param name="row">Row to remove.</param>
    private void RemoveLine(int row)
    {
        for (int y = row; y > 0; y--)
        {
            for (int x = 0; x < GameConstants.PlayZoneWidth; x++)
            {
                _board[x, y] = _board[x, y - 1];
            }
        }

        for (int x = 0; x < GameConstants.PlayZoneWidth; x++)
        {
            _board[x, 0] = 0;
        }
    }
}

