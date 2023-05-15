//using System;
//using System.Drawing;
//using TetrisThird.Enums;
//using TetrisThird.Models;

//namespace TetrisThird.Services;

//public class FigureControlService
//{
//    List<Point> FigurePoints = new();

//    public FigureModel CurrentFigure { get; set; }
//    public FigureModel NextFigure { get; set; }

//    private int figureWidth => CurrentFigure.Form.GetLength(1);
//    private int figureHeight => CurrentFigure.Form.GetLength(0);

//    public Point Offset { get; set; }

//    public void CreateFigure()
//    {
//        FigureModel newFigure;

//        if (NextFigure == null)
//        {
//            newFigure = new();
//            newFigure.Initialize();

//            NextFigure = newFigure;
//        }

//        CurrentFigure = NextFigure;

//        newFigure = new();
//        newFigure.Initialize();

//        NextFigure = newFigure;
//    }

//    public int CountDynamicPoints(int[,] figure)
//    {
//        int matrixWidth = figure.GetLength(1);
//        int matrixHeight = figure.GetLength(0);

//        List<Point> dynamicPoints = new();

//        for (int y = 0; y < matrixHeight; y++)
//        {
//            for (int x = 0; x < matrixWidth; x++)
//            {
//                if (figure[y, x] == (int)GridStatus.Dynamic)
//                {
//                    Point newPoint = new() { X = x, Y = y };
//                    dynamicPoints.Add(newPoint);
//                }
//            }
//        }

//        int result = dynamicPoints.Count;

//        return result;
//    }

//    public List<Point> FindFigureCoordinates(int[,] figure, int[,] playZone, HashSet<int> searchValues)
//    {
//        int figureRows = figure.GetLength(0);
//        int figureCols = figure.GetLength(1);

//        int playZoneRows = playZone.GetLength(0);
//        int playZoneCols = playZone.GetLength(1);

//        List<Point> coordinates = new List<Point>();

//        for (int i = 0; i < playZoneRows - figureRows; i++)
//        {
//            for (int j = 0; j < playZoneCols - figureCols; j++)
//            {
//                bool found = true;

//                for (int k = 0; k < figureRows; k++)
//                {
//                    for (int l = 0; l < figureCols; l++)
//                    {
//                        int outerValue = playZone[i + k, j + l];
//                        int innerValue = figure[k, l];

//                        if (!searchValues.Contains(outerValue) || !searchValues.Contains(innerValue) || outerValue != innerValue)
//                        {
//                            found = false;
//                            break;
//                        }
//                    }

//                    if (!found)
//                    {
//                        break;
//                    }
//                }

//                if (found)
//                {
//                    coordinates.Add(new Point(i, j));
//                }
//            }
//        }


//        return coordinates;
//    }

//    public void RotateFigure()
//    {
//        int[,] rotatedFigure = new int[figureWidth, figureHeight];

//        // transpose

//        for (int y = 0; y < figureHeight; y++)
//        {
//            for (int x = 0; x < figureWidth; x++)
//            {
//                rotatedFigure[x, y] = CurrentFigure.Form[y, x];
//            }
//        }

//        // reverse each row

//        for (int y = 0; y < figureHeight; y++)
//        {
//            int startX = 0;
//            int endX = figureWidth - 1;

//            while (startX < endX)
//            {
//                int elementToSwap = rotatedFigure[y, startX];

//                rotatedFigure[y, startX] = rotatedFigure[y, endX];
//                rotatedFigure[y, endX] = rotatedFigure[y, startX];

//                startX++;
//                endX--;
//            }
//        }

//        CurrentFigure.Form = rotatedFigure;

//        // add offset

//        int playZoneWidth = playZone.GetLength(1);
//        int playZoneHeight = playZone.GetLength(0);

//        CurrentFigure = new int[playZoneHeight, playZoneWidth];

//        GetOffset(figure, playZone);
//        FigurePoints.Clear();

//        for (int y = 0; y < figureHeight; y++)
//        {
//            for (int x = 0; x < figureWidth; x++)
//            {
//                if (rotatedFigure[y, x] == (int)GridStatus.Dynamic)
//                {
//                    //ActiveFigure[y + Offset.Y, x + Offset.X] = (int)GridStatus.Dynamic;
//                    Point newPoint = new() { X = x + Offset.X, Y = y + Offset.Y};
//                    FigurePoints.Add(newPoint);
//                }
//            }
//        }

//        // check for static blocks

//        figureWidth = CurrentFigure.GetLength(1);
//        figureHeight = CurrentFigure.GetLength(0);

//        bool isRotatable = true;

//        //for (int y = 0; y < figureHeight; y++)
//        //{
//        //    for (int x = 0; x < figureWidth; x++)
//        //    {
//        //        if (ActiveFigure[y, x] == (int)GridStatus.Dynamic)
//        //        {
//        //            Point newPoint = new() { X = x, Y = y };
//        //            FigureAsPoints.Add(newPoint);
//        //        }
//        //    }
//        //}

//        foreach (var point in FigurePoints)
//        {
//            if (playZone[point.Y, point.X] == (int)GridStatus.Static)
//            {
//                isRotatable = false;
//            }
//        }

//        // clear original figure

//        if (isRotatable)
//        {
            

//            for (int x = 0; x < playZoneWidth; x++)
//            {
//                for (int y = 0; y < playZoneHeight; y++)
//                {
//                    if (playZone[y, x] == (int)GridStatus.Dynamic)
//                    {
//                        playZone[y, x] = (int)GridStatus.Empty;
//                    }
//                }
//            }

//            // add rotated figure

//            foreach (var point in FigurePoints)
//            {
//                playZone[point.Y, point.X] = (int)GridStatus.Dynamic;
//            }
//        }

//    }

//    public void GetOffset(FigureModel figure, int[,] playZone)
//    {
//        int playZoneWidth = playZone.GetLength(1);
//        int playZoneHeight = playZone.GetLength(0);

//        Point offset = new() { X = 0, Y = 0 };

//        bool firstPointHasBeenFound = false;

//        for (int y = 0; y < playZoneHeight; y++)
//        {
//            for (int x = 0; x < playZoneWidth; x++)
//            {
//                if (playZone[y, x] == (int)GridStatus.Dynamic
//                    && !firstPointHasBeenFound) 
//                {
//                    offset.X = x;
//                    offset.Y = y;

//                    firstPointHasBeenFound = true;
//                }
//            }
//        }

//        int figureWidth = figure.Form.GetLength(1);
//        int figureHeight = figure.Form.GetLength(0);

//        firstPointHasBeenFound = false;

//        for (int y = 0; y < figureHeight; y++)
//        {
//            for (int x = 0; x < figureWidth; x++)
//            {
//                if (figure.Form[y, x] == (int)GridStatus.Dynamic
//                    && !firstPointHasBeenFound)
//                {
//                    offset.X -= x;
//                    offset.Y -= y;

//                    firstPointHasBeenFound = true;
//                }
//            }
//        }

//        Offset = offset;
//    }

//    public void HandleKey(ConsoleKey key, FigureModel figure, int[,] playZone)
//    {
//        switch (key)
//        {
//            case ConsoleKey.Spacebar:
//                RotateFigure(figure, playZone);
//                break;

//            //case ConsoleKey.RightArrow:
//            //    Direction = (int)MoveDirection.Right;
//            //    MoveRight(playZone);
//            //    break;

//            //case ConsoleKey.DownArrow:
//            //    Direction = (int)MoveDirection.Down;
//            //    MoveDown(playZone);
//            //    break;

//            default:
//                break;
//        }
//    }
//}

