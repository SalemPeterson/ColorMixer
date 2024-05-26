using FlowFieldSimulator;

namespace Client
{
    
    class FlowFieldDrawable(FlowField<Color> flowField) : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.DrawRectangle(dirtyRect);
            canvas.FillColor = Colors.WhiteSmoke;
            canvas.FillRectangle(dirtyRect);
            
            DrawParticles(flowField, canvas);
        }

        private static void DrawParticles(FlowField<Color> flowField, ICanvas canvas)
        {
            foreach (var particle in flowField.particles)
            {
                canvas.FillColor = particle.Value;
                canvas.FillCircle(particle.Position.X, particle.Position.Y, particle.Radius);
            }
        }
    }
}
