using System.Numerics;

namespace FlowFieldSimulator
{
    public class VelocityField
    {
        private readonly Vector2[,] field;
        FastNoiseLite noise;
        public float zOffset = 0;
        public float zIncrement = 1;

        public VelocityField(float width, float height)
        {
            field = new Vector2[(int)width, (int)height];
            Random rng = new();
            noise = new(rng.Next());
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            RandomizeField();
        }

        public Vector2 GetVelocity(float x, float y)
        {
            return field[(int)x, (int)y];
        }

        public void RandomizeField()
        {
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    double angle = noise.GetNoise(x, y, zOffset) * Math.PI;
                    field[x, y] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                }
            }
            zOffset += zIncrement;
        }
    }
}
