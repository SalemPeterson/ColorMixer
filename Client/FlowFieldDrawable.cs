using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowFieldSimulator;

namespace Client
{
    
    class FlowFieldDrawable(FlowField<Color> flowField) : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor=Colors.White;
            canvas.FillRectangle(dirtyRect);
            flowField.velocityField.RandomizeField();
            flowField.Update();
            foreach (var particle in flowField.particles)
            {
                canvas.DrawCircle(particle.X, particle.Y, 1);
            }
        }
    }
}
