using FlowFieldSimulator;
using System.Drawing;
using System.Numerics;
static int HandleCollisions(int first, int second)
{
    return (first + second) % 10;
}
Random rand = new();
int width = 30, height = 30;
FlowField<int> flowField = new(width, height);

for (int k = 0; k < 1000; k++)
{
    flowField.velocityField.RandomizeField(); flowField.Insert(1, rand.Next(width), rand.Next(height));
    Console.WriteLine();
    for (int i = 0; i < width; i++)
    {
        for (int j = 0; j < height; j++)
        {
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
    Thread.Sleep(500);
}
Console.ReadLine();