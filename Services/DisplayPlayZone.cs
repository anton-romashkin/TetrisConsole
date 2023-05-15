//using System;
//using System.Text;
//using TetrisThird.Enums;

//namespace TetrisThird.Services;

//public class DisplayPlayZone
//{
//    public void DisplayInConsole(int[,] playZone)
//    {
//        string column = "";

//        var playZoneGridString = new StringBuilder();

//        for (int y = 0; y < playZone.GetLength(0); y++)
//        {
//            column += "|||";

//            for (int x = 0; x < playZone.GetLength(1); x++)
//            {
//                switch (playZone[y, x])
//                {
//                    case (int)GridStatus.Empty:
//                        column += "   ";
//                        break;
//                    case (int)GridStatus.Static:
//                        column += "[x]";
//                        break;
//                    case (int)GridStatus.Dynamic:
//                        column += "[ ]";
//                        break;
//                    case (int)GridStatus.Score:
//                        column += "***";
//                        break;
//                    default:
//                        column += "   ";
//                        break;
//                }
//            }

//            column += "|||";

//            playZoneGridString.AppendLine(column);

//            column = "";
//        }

//        playZoneGridString.AppendLine("||||||||||||||||||||||||||||||||||||");

//        Console.WriteLine(playZoneGridString.ToString());
//    }
//}

