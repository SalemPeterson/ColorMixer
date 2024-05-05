using System.Numerics;

namespace FlowFieldSimulator
{
    public class FlowField<T>
    {
        public T?[,] positions;
        public VelocityField velocityField;
        public delegate T? HandleCollision(T? first, T? second);
        private readonly HandleCollision handleCollision;

        public FlowField(int width, int height, HandleCollision handleCollision) 
        {
            velocityField = new(width, height);
            positions = new T[width, height];
            this.handleCollision = handleCollision;
        }
        public void Insert(T obj, int x, int y)
        {
            positions[x, y] = handleCollision(obj, positions[x, y]);
        }
        public void Update()
        {
            int width = positions.GetLength(0);
            int height = positions.GetLength(1);
            T?[,] newPositions = new T?[width, height];
            for (int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Vector2 velocity = velocityField.GetVelocity(i, j);
                    Vector2 newPosition = velocity + new Vector2(i, j);
                    newPosition = new Vector2(Mod((int)newPosition.X, width), Mod((int)newPosition.Y, height));
                    newPositions[(int)newPosition.X, (int)newPosition.Y] = handleCollision(newPositions[(int)newPosition.X, (int)newPosition.Y], positions[i, j]);
                }
            }
            positions = newPositions;
        }

        private static int Mod(int a, int b)
        {
            return ((a % b) + b) % b;
        }
    }
}
