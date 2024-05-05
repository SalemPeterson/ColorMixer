using System.Numerics;

namespace FlowFieldSimulator
{
    public class VelocityField
    {
        private readonly Vector2[,] field;

        public VelocityField(int width, int height)
        {
            field = new Vector2[width, height];
            RandomizeField(-1, 1);
        }

        public Vector2 GetVelocity(int x, int y)
        {
            return field[x, y];
        }

        public void RandomizeField(int min, int max)
        {
            Random rand = new();
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = new Vector2(rand.Next(min, max + 1), rand.Next(min, max + 1));
                }
            }
        }
    }
}
