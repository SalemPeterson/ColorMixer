using FlowFieldSimulator;
using System.Drawing;
using System.Numerics;
static int HandleCollisions(int first, int second)
{
    return (first + second) % 10;
}
Random rand = new();
int width = 10, height = 10;
FlowField<int> flowField = new(width, height, HandleCollisions);
int[,] pastPositions = new int[width, height];
for (int i = 0; i < width; i++)
{
    for (int j = 0; j < height; j++)
    {
        Vector2 vector = flowField.velocityField.GetVelocity(i, j);
        Console.Write($"[{vector.X},{vector.Y}]");
    }
    Console.Write('\n');
}
flowField.Insert(1, rand.Next(width), rand.Next(height));
for (int k = 0; k < 1000; k++)
{
    if (k % 10 == 9) { flowField.velocityField.RandomizeField(-1, 1); flowField.Insert(1, rand.Next(width), rand.Next(height)); }
    Console.WriteLine();
    for (int i = 0; i < width; i++)
    {
        for (int j = 0; j < height; j++)
        {
            int num = flowField.positions[i, j];
            if (num == 0) num = pastPositions[i, j];
            else { pastPositions[i, j] = flowField.positions[i, j]; num = 1; }
            if (pastPositions[i,j] > 0) pastPositions[i, j] = pastPositions[i, j] + 1 % 10;
            switch (num)
            {
                case 1: Console.ForegroundColor = ConsoleColor.Red; break;
                case 2: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case 3: Console.ForegroundColor = ConsoleColor.Green; break;
                case 4: Console.ForegroundColor = ConsoleColor.Cyan; break;
                case 5: Console.ForegroundColor = ConsoleColor.Blue; break;
                case 6: Console.ForegroundColor = ConsoleColor.Magenta; break;
                case 7: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
                case 8: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case 9: Console.ForegroundColor = ConsoleColor.Gray; break;
            }
            string directionStr = "0";
            Vector2 direction = flowField.velocityField.GetVelocity(i,j);
            if (direction.X > 0 && direction.Y == 0) directionStr = ".";
            else if (direction.X < 0 && direction.Y == 0) directionStr = "^";
            else if (direction.X == 0 && direction.Y > 0) directionStr = ">";
            else if (direction.X == 0 && direction.Y < 0) directionStr = "<";
            else if (direction.X < 0 && direction.Y < 0) directionStr = "`";
            else if (direction.X > 0 && direction.Y > 0) directionStr = @"\";
            else if (direction.X > 0 && direction.Y < 0) directionStr = ",";
            else if (direction.X < 0 && direction.Y > 0) directionStr = "'";
            Console.Write($"[{directionStr}]");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.Write('\n');
    }
    flowField.Update();
    Thread.Sleep(500);
}
Console.ReadLine();