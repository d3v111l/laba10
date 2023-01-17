using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace laba10
{
    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string PlayerMarker;
        public string PlayerMarkerAfter;
        public ConsoleColor PlayerColor;
        public ConsoleColor PlayerColorAfter;

        public Player(int initialX, int initialY)
        {
            X = initialX;
            Y = initialY;
            PlayerMarker = "@";
            PlayerMarkerAfter = ".";
            PlayerColor = ConsoleColor.Blue;
            PlayerColorAfter = ConsoleColor.White;
        }

        public void Draw()
        {
            ForegroundColor = PlayerColor;
            SetCursorPosition(X, Y);
            Write(PlayerMarker);
        }

        //public void AntiFlicker()
        //{
        //    PlayerColorAfter = PlayerColor;
        //    SetCursorPosition(X, Y);
        //    PlayerMarkerAfter = PlayerMarker ;
        //    Write(PlayerMarkerAfter);
        //}

    }
}
