using System.Numerics;

namespace FlowFieldSimulator
{
    public class VelocityField
    {
        private readonly Vector2[,] field;

        public VelocityField(float width, float height)
        {
            field = new Vector2[(int)width, (int)height];
            RandomizeField();
        }

        public Vector2 GetVelocity(float x, float y)
        {
            return field[(int)x, (int)y];
        }

        public void RandomizeField()
        {
            FastNoiseLite noise = new(0);
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    double angle = noise.GetNoise(x, y) * Math.PI;
                    field[x, y] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                }
            }
        }
    }
}
