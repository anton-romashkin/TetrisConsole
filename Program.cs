using System;
using TetrisConsole.Workers;

class Program
{
    static void Main(string[] args)
    {
        var tetris = new TetrisWorker();
        tetris.Start();
    }
}

