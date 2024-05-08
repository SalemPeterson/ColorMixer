using System.Numerics;

namespace FlowFieldSimulator
{
    public class FlowField<T>
    {
        public List<Particle<T>> particles;
        public VelocityField velocityField;
        public delegate T? HandleCollision(T? first, T? second);
        public float Width, Height;
        private float maxSpeed = 4f;
        public FlowField(float width, float height) 
        {
            Width = width;
            Height = height;
            velocityField = new(width, height);
            particles = [];
        }
        public void Insert(T obj, float x, float y)
        {
            Particle<T> particle = new(obj, x, y);
            particle.Velocity = velocityField.GetVelocity(x, y);
            particles.Add(particle);
        }
        public void Update()
        {
            foreach (var particle in particles)
            {
                particle.Velocity += velocityField.GetVelocity(particle.X, particle.Y);
                particle.Velocity = Vector2.Clamp(particle.Velocity, new(-maxSpeed,-maxSpeed), new(maxSpeed, maxSpeed));
                particle.X += particle.Velocity.X;
                particle.X = Mod(particle.X, Width);
                particle.Y += particle.Velocity.Y;
                particle.Y = Mod(particle.Y, Height);
            }
        }

        private static float Mod(float a, float b)
        {
            return ((a % b) + b) % b;
        }

        public class Particle<Type>(Type obj, float x, float y)
        {
            public Type Value = obj;
            public float X = x;
            public float Y = y;
            public Vector2 Velocity = new();
        }
    }
}
