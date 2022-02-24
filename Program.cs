using System;
using System.Collections.Generic;
using System.Linq;

namespace ЕНГ_Task_05
{
    class Program
    {
        static void CreateMap(ref int[,] maze)
        {
            //Для матрицы любого размера
            Random rnd = new Random(DateTime.Now.Millisecond);
            int length = -1;
            var nextPreviousNull = new List<int>();
            var resultPreviousNull = new List<int>();
            int ChisloHodov = 75; //Число одновременных ходов
            int shansTupika = 25; //Шанс тупика
            int DimensionMatrix = maze.GetLength(0);
            for (int i = 0; i < DimensionMatrix; i++)
                for (int j = 0; j < DimensionMatrix; j++)
                    maze[i, j] = 1;
            for (int i = 0; i < DimensionMatrix; i++)
            {
                if (i == 0)
                {
                    maze[i, 1] = 0;
                    resultPreviousNull.Add(1);
                    continue;
                };
                if (i + 1 == DimensionMatrix)
                {
                    maze[i, resultPreviousNull[rnd.Next(0, resultPreviousNull.Count)]] = 0;
                    continue;
                };
                nextPreviousNull.Clear();
                for (int x = 0; x < resultPreviousNull.Count; x++)
                {
                    if (x + 1 != resultPreviousNull.Count && rnd.Next(0, shansTupika) == 0) continue;
                    maze[i, resultPreviousNull[x]] = 0;
                    nextPreviousNull.Add(resultPreviousNull[x]);
                    length = rnd.Next(0, resultPreviousNull[x]);
                    for (int j = resultPreviousNull[x] - 1; length != 0; j--)
                    {
                        if (maze[i - 1, j] != 0)
                        {
                            maze[i, j] = 0;
                            nextPreviousNull.Add(j);
                        }
                        else break;
                        length--;
                    };
                    length = rnd.Next(0, DimensionMatrix - 1 - resultPreviousNull[x]);
                    for (int j = resultPreviousNull[x] + 1; length != 0; j++)
                    {
                        if (maze[i - 1, j] != 0)
                        {
                            maze[i, j] = 0;
                            nextPreviousNull.Add(j);
                        }
                        else break;
                        length--;
                    };
                }

                resultPreviousNull.Clear();
                for (int k = 0; k < ChisloHodov; k++)
                    resultPreviousNull.Add(nextPreviousNull[rnd.Next(0, nextPreviousNull.Count)]);
                resultPreviousNull = resultPreviousNull.Distinct().ToList();
                for (int h = 0; h < resultPreviousNull.Count; h++)
                {
                    if (resultPreviousNull.Contains(resultPreviousNull[h] + 1))
                        resultPreviousNull.Remove(resultPreviousNull[h] + 1);
                    if (resultPreviousNull.Contains(resultPreviousNull[h] - 1))
                        resultPreviousNull.Remove(resultPreviousNull[h] - 1);
                };
            }
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            //ввод 
            int[,] maze = new int[30, 30]; //любой размер, но может не поместиться в консоль
            CreateMap(ref maze);  //создание карты

            //координаты игрока
            int x = 1, y = 1;
            while (true)
            {
                //рисуем лабиринт
                Console.Clear();
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    for (int j = 0; j < maze.GetLength(1); j++)
                    {
                        if (maze[i, j] == 0) Console.Write(".");
                        if (maze[i, j] == 1) Console.Write("|");

                    }
                    Console.WriteLine();
                }
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("+");
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;

                // обработка ввода
                ConsoleKeyInfo ki = Console.ReadKey(true);
                if (ki.Key == ConsoleKey.Escape) break;
                if (ki.Key == ConsoleKey.UpArrow && y == 0) continue;
                if (ki.Key == ConsoleKey.DownArrow && y == maze.GetLength(0) - 1)
                {
                    Console.WriteLine(string.Empty);
                    Console.WriteLine("Вы выйграли!!! Хотите снова играть, запустите игру заново.");
                    Console.ReadKey();
                    break;
                };
                if (ki.Key == ConsoleKey.LeftArrow && maze[y, x - 1] == 0) x--;
                if (ki.Key == ConsoleKey.RightArrow && maze[y, x + 1] == 0) x++;
                if (ki.Key == ConsoleKey.UpArrow && maze[y - 1, x] == 0) y--;
                if (ki.Key == ConsoleKey.DownArrow && maze[y + 1, x] == 0) y++;
            }
        }
    }
}
