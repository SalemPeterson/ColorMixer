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
            Particle<T> particle = new(obj, x, y, 2);
            particle.Velocity = velocityField.GetVelocity(x, y);
            particles.Add(particle);
        }
        public void Update()
        {
            foreach (var particle in particles)
            {
                particle.Velocity += velocityField.GetVelocity(particle.Position.X, particle.Position.Y);
                particle.Velocity = Vector2.Clamp(particle.Velocity, new(-maxSpeed,-maxSpeed), new(maxSpeed, maxSpeed));
                particle.Position.X += particle.Velocity.X;
                particle.Position.X = Mod(particle.Position.X, Width);
                particle.Position.Y += particle.Velocity.Y;
                particle.Position.Y = Mod(particle.Position.Y, Height);
            }
        }

        private static float Mod(float a, float b)
        {
            return ((a % b) + b) % b;
        }

        public class Particle<Type>(Type value, float x, float y, float radius)
        {
            public Type Value = value;
            public Vector2 Position = new Vector2(x, y);
            public float Radius = radius;
            public Vector2 Velocity = new();
        }

    }
}
