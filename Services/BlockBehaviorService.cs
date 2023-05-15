//using System;
//using System.Drawing;
//using TetrisThird.Enums;
//using TetrisThird.Models;

//namespace TetrisThird.Services;

//public class BlockBehaviorService
//{
//    public List<Point> DynamicBlocks { get; set; } = new();

//    public List<int> LinesToClear { get; set; } = new();

//    //public int Direction { get; set; }

//    //public FigureModel CurrentFigure { get; set; }

//    //public FigureModel NextFigure { get; set; }


//    public void BlockLogicFlow(int[,] playZone)
//    {
//        ClearScoredLines(playZone);

//        FindDynamicBlocks(playZone);

//        MoveDynamicBlocksDown(playZone);

//        ScoreLine(playZone);
//    }

//    public void FindDynamicBlocks(int[,] playZone, bool reverseSorting = false)
//    {
//        DynamicBlocks.Clear();

//        if (!reverseSorting)
//        {
//            for (int x = 0; x < playZone.GetLength(1); x++)
//            {
//                for (int y = playZone.GetLength(0) - 1; y >= 0; y--)
//                {
//                    if (playZone[y, x] == (int)GridStatus.Dynamic)
//                    {
//                        Point newPoint = new() { X = x, Y = y };
//                        DynamicBlocks.Add(newPoint);
//                    }
//                }
//            }
//        }
//        else
//        {
//            for (int x = playZone.GetLength(1) - 1; x >= 0; x--)
//            {
//                for (int y = playZone.GetLength(0) - 1; y >= 0; y--)
//                {
//                    if (playZone[y, x] == (int)GridStatus.Dynamic)
//                    {
//                        Point newPoint = new() { X = x, Y = y };
//                        DynamicBlocks.Add(newPoint);
//                    }
//                }
//            }
//        }
//    }

//    public void MoveDynamicBlocksDown(int[,] playZone)
//    {
//        bool isDownMovable = true;

//        FindDynamicBlocks(playZone);

//        foreach (var point in DynamicBlocks)
//        {
//            if (point.Y == playZone.GetLength(0) - 1)
//            {
//                isDownMovable = false;
//            }
//            else if (playZone[point.Y + 1, point.X] == (int)GridStatus.Static)
//            {
//                isDownMovable = false;
//            }
//        }

//        if (isDownMovable)
//        {
//            foreach (var point in DynamicBlocks)
//            {
//                playZone[point.Y, point.X] = (int)GridStatus.Empty;
//                playZone[point.Y + 1, point.X] = (int)GridStatus.Dynamic;
//            }
//        }
//        else
//        {
//            SetDynamicBlocksAsStatic(playZone);
//        }
//    }

//    public void SetDynamicBlocksAsStatic(int[,] playZone)
//    {
//        for (int x = 0; x < playZone.GetLength(1); x++)
//        {
//            for (int y = 0; y < playZone.GetLength(0); y++)
//            {
//                if (playZone[y, x] == (int)GridStatus.Dynamic)
//                {
//                    playZone[y, x] = (int)GridStatus.Static;
//                }
//            }
//        }
//    }

//    public void ScoreLine(int[,] playZone)
//    {
//        FindLinesToClear(playZone);

//        foreach (var y in LinesToClear)
//        {
//            for (int x = 0; x < playZone.GetLength(1); x++)
//            {
//                playZone[y, x] = (int)GridStatus.Score;
//            }
//        }
//    }

//    public void FindLinesToClear(int[,] playZone)
//    {
//        LinesToClear.Clear();

//        for (int y = 0; y < playZone.GetLength(0); y++)
//        {
//            int nStaticPointsInLine = 0;

//            for (int x = 0; x < playZone.GetLength(1); x++)
//            {
//                if (playZone[y, x] == (int)GridStatus.Static || playZone[y, x] == (int)GridStatus.Score)
//                    nStaticPointsInLine++;
//            }

//            if (nStaticPointsInLine == 10)
//            {
//                LinesToClear.Add(y);
//            }
//        }
//    }

//    public void ClearScoredLines(int[,] playZone)
//    {
//        FindLinesToClear(playZone);

//        if (LinesToClear.Count > 0)
//        {
//            int numberOfLinesToClear = LinesToClear.Count;
//            int firstLineToClear = LinesToClear.Min();

//            for (int x = 0; x < playZone.GetLength(1); x++)
//            {
//                for (int y = firstLineToClear - 1; y >= 0; y--)
//                {
//                    playZone[y + numberOfLinesToClear, x] = playZone[y, x];

//                    playZone[y, x] = (int)GridStatus.Empty;
//                }
//            }
//        }
//    }

    

//    //public void HandleKey(ConsoleKey key, int[,] playZone)
//    //{
//    //    switch (key)
//    //    {
//    //        case ConsoleKey.LeftArrow:
//    //            Direction = (int)MoveDirection.Left;
//    //            MoveLeft(playZone);
//    //            break;

//    //        case ConsoleKey.RightArrow:
//    //            Direction = (int)MoveDirection.Right;
//    //            MoveRight(playZone);
//    //            break;

//    //        case ConsoleKey.DownArrow:
//    //            Direction = (int)MoveDirection.Down;
//    //            MoveDown(playZone);
//    //            break;

//    //        default:
//    //            Direction = (int)MoveDirection.Undefined;
//    //            break;
//    //    }
//    //}

//    //public void MoveLeft(int[,] playZone)
//    //{
//    //    FindDynamicBlocks(playZone);

//    //    bool isLeftMovable = true;

//    //    foreach (var point in DynamicBlocks)
//    //    {
//    //        if (point.X == 0)
//    //        {
//    //            isLeftMovable = false;
//    //        }
//    //        else if (playZone[point.Y, point.X - 1] == (int)GridStatus.Static)
//    //        {
//    //            isLeftMovable = false;
//    //        }
//    //    }

//    //    if (isLeftMovable)
//    //    {
//    //        foreach (var point in DynamicBlocks)
//    //        {
//    //            playZone[point.Y, point.X] = (int)GridStatus.Empty;
//    //            playZone[point.Y, point.X - 1] = (int)GridStatus.Dynamic;
//    //        }
//    //    }
//    //}

//    //public void MoveRight(int[,] playZone)
//    //{
//    //    bool reversedXSort = true;

//    //    FindDynamicBlocks(playZone, reversedXSort);

//    //    bool isRightMovable = true;

//    //    foreach (var point in DynamicBlocks)
//    //    {
//    //        if (point.X == playZone.GetLength(1) - 1)
//    //        {
//    //            isRightMovable = false;
//    //        }
//    //        else if (playZone[point.Y, point.X + 1] == (int)GridStatus.Static)
//    //        {
//    //            isRightMovable = false;
//    //        }
//    //    }

//    //    if (isRightMovable)
//    //    {
//    //        foreach (var point in DynamicBlocks)
//    //        {
//    //            playZone[point.Y, point.X] = (int)GridStatus.Empty;
//    //            playZone[point.Y, point.X + 1] = (int)GridStatus.Dynamic;
//    //        }
//    //    }
//    //}
//}

