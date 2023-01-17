using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace laba10
{
    internal class World
    {
        private string[,] Grid;
        private int Rows;
        private int Cols;

        public World(string[,] grid)
        {
            Grid = grid;
            Rows = Grid.GetLength(0);
            Cols = Grid.GetLength(1);
        }


        public void Draw()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    string element = Grid[y, x];
                    SetCursorPosition(x, y);

                    //string bgColor;
                    //if (y % 2 == 0)
                    //{
                    //    //bgColor = "#b855d3";
                    //    bgColor = "#12070f";
                    //}
                    //else
                    //{
                    //    //bgColor = "#5f51d6";
                    //    bgColor = "#07036";
                    //}
                    //Write(element.PastelBg(bgColor));

                    if (element == "X")
                    {
                        ForegroundColor = ConsoleColor.Green;
                    }
                    else if (element == "╬")
                    {
                        ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.White;
                    }

                    Write(element);
                }
            }
        }

        public string GetElementAt(int x, int y)
        {
            return Grid[y, x];
        }

        public bool IsPositionWalkable(int x, int y)
        {
            // Check bounds first.
            if (x < 0 || y < 0 || x >= Cols || y >= Rows)
            {
                return false;
            }

            // Check if the grid is walkable tile.


            return Grid[y, x] == "." || Grid[y, x] == "X" || Grid[y,x] == "╬";

        }

    }
}
