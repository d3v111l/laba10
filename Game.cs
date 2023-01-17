using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Pastel;
using Figgle;
using System.Threading.Channels;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Collections.Immutable;

namespace laba10
{
    class Game
    {
        private World MyWorld;
        private Player CurrentPLayer;

        public void Start()
        {
            
            //SetCursorPosition(4, 2);
            //Write("x");
            Clear();
            Title = "Amazing Maze";
            CursorVisible = false;
            SetWindowSize(140, 40);
            
            string path = System.IO.Directory.GetCurrentDirectory() + @"\\maps\\";
            string[] arrOfFiles = Directory.GetFiles(path);
            for (int i = 0; i < arrOfFiles.Length; i++)
            {
                arrOfFiles[i] = arrOfFiles[i].Split('\\').Last();
            }
            
            Random rnd = new Random();
            int mapVariant = rnd.Next(0,7);

            string[,] grid = LevelParser.ParseFileToArray(@"maps\\" + arrOfFiles[mapVariant]);
            
            CurrentPLayer = new Player(1, 1);
            MyWorld = new World(grid);

            RunGameLoop();
        }

        private void Highscores()
        {
            Clear();

            ForegroundColor = ConsoleColor.Green;
            WriteLine(FiggleFonts.Rectangles.Render("Top 5 highscores"));
            ResetColor();

            string[] HighestScores = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + @"\\highscores.txt");

            int[] Descend = new int[HighestScores.Length];
            for (int i = 0; i < HighestScores.Length; i++)
            {
                Descend[i] = Convert.ToInt32(HighestScores[i]);
            }

            Array.Sort(Descend, (x, y) => x.CompareTo(y));

            for (int i = 0; i < 5; i++)
            {
                WriteLine(i+1 + ". " + Descend[i] + " seconds");
            }
            Thread.Sleep(3000);
            PlayAgain();
        }
        public void PlayAgain()
        {
            try
            {
                
                WriteLine("\n\nDo you want to play again?");
                WriteLine("1 - yes\t2 - no\t3 - highscores");
                int variants = Convert.ToInt32(ReadLine());
                if (variants == 1)
                {
                    Start();
                }
                else if (variants == 2)
                {
                    Process.GetCurrentProcess().Kill();
                }
                else if (variants == 3)
                {
                    Clear();
                    Highscores();
                }
                else if (variants > 3 || variants < 1)
                {
                    Console.WriteLine("You did something wrong");
                    Thread.Sleep(500);
                    PlayAgain();
                }
                Clear();
            }
            catch 
            {
                //Console.WriteLine("Something went wrong :(");
                Thread.Sleep(300);
                PlayAgain();
            }
        }

        private void DisplayIntro()
        {
            ForegroundColor = ConsoleColor.Green;
            WriteLine(FiggleFonts.Larry3d.Render("Amazing Maze !"));
            ResetColor();
            WriteLine("\nUse arrow keys to move");
            Write("\nThe point is to get to an:");
            WriteLine(" X".Pastel("#6bff93"));
            WriteLine("Be careful!");
            WriteLine("Press any key to start...");
            ReadKey(true);
        }


        private void DrawLevel()
        {
            Clear();
            MyWorld.Draw();
            CurrentPLayer.Draw();
            
        }

        private void HandlePlayerInput()
        {
            ConsoleKey key;
            do
            {
                ConsoleKeyInfo keyInfo = ReadKey(true);
                key = keyInfo.Key;
            } while (KeyAvailable);

            
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPLayer.X, CurrentPLayer.Y - 1))
                    {
                        CurrentPLayer.Y -= 1;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPLayer.X, CurrentPLayer.Y + 1))
                    {
                        CurrentPLayer.Y += 1;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPLayer.X - 1, CurrentPLayer.Y))
                    {
                        CurrentPLayer.X -= 1;
                    }
                   
                    break;

                case ConsoleKey.RightArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPLayer.X + 1, CurrentPLayer.Y))
                    {
                        CurrentPLayer.X += 1;
                    }
                    break;

                default:
                    break;
            }
        }

        public void RunGameLoop()
        {
            
            DisplayIntro();
            //Clear();
            //MyWorld.Draw();

            var time1 = DateTime.Now;
            while (true)
            {
                //CurrentPLayer.Draw();

                DrawLevel();

                // Check for player input from the keyboard and move the player.
                HandlePlayerInput();

                
                // Check if the player has reached the exit or enemy and end the game if so.
                string elementAtPlayerPos = MyWorld.GetElementAt(CurrentPLayer.X, CurrentPLayer.Y);
                if (elementAtPlayerPos == "X")
                {
                    Clear();
                    ResetColor();

                    var time2 = DateTime.Now;
                    var time = time2.Subtract(time1);
                    string score = time.ToString().Remove(0,3);
                    score = score.Replace(".", ":");
                    string[] highscores = score.Split(new char[] { ':' });
                    int highscoreCount = (Convert.ToInt32(highscores[0]) * 60) + Convert.ToInt32(highscores[1]);
                    WriteLine("Your score is:");
                    WriteLine(highscoreCount + " seconds");

                    string ScorePath = System.IO.Directory.GetCurrentDirectory() + @"\\highscores.txt";
                    File.AppendAllText(ScorePath, highscoreCount.ToString() + Environment.NewLine);

                    PlayAgain();
                }

                else if (elementAtPlayerPos == "╬")
                {
                    Clear();
                    ForegroundColor = ConsoleColor.Red;
                    SetCursorPosition(0, 10);
                    WriteLine(FiggleFonts.ThreePoint.Render("YOU ARE DEAD!"));
                    SetCursorPosition(0,0);
                    System.Threading.Thread.Sleep(2000);
                    ResetColor();
                    Clear();

                    PlayAgain();

                    //CurrentPLayer.AntiFlicker();
                };
                // Give the console a chance to render.
                System.Threading.Thread.Sleep(1);  
            }
        }
    }
}
