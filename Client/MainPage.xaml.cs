using FlowFieldSimulator;
using Microsoft.Maui.Controls;
using System.Numerics;
using System.Xml.Schema;
namespace Client
{
    public partial class MainPage : ContentPage
    {
        private FlowField<Color> flowField;
        private IDispatcherTimer timer;
        private List<(Vector2, Color)> fluidSources = [];
        private Random random = new();
        public MainPage()
        {
            InitializeComponent();
            flowField = new((float)PlaySurface.WidthRequest, (float)PlaySurface.HeightRequest);
            PlaySurface.Drawable = new FlowFieldDrawable(flowField);
            timer = Dispatcher.CreateTimer();
            int FPS = 30;
            timer.Interval = TimeSpan.FromSeconds(1 / FPS);
            timer.Tick += OnTick;
            timer.Start();
        }

        private void OnTick(object? sender, EventArgs e)
        {
            foreach (var source in fluidSources)
            {
                flowField.Insert(source.Item2, source.Item1.X, source.Item1.Y);
            }
            UpdateParticles();
            PlaySurface.Invalidate();
        }

        private void UpdateParticles()
        {
            flowField.velocityField.RandomizeField();
            flowField.Update();
            for (int i = flowField.particles.Count - 1; i > 0 ; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    var particle = flowField.particles[i];
                    var otherParticle = flowField.particles[j];
                    var distance = Vector2.Distance(particle.Position, otherParticle.Position);
                    var minimumDistance = particle.Radius + otherParticle.Radius;
                    if (distance < minimumDistance)
                    {
                        var newPosition = particle.Position - otherParticle.Position;
                        newPosition = Vector2.Normalize(newPosition) * (minimumDistance - distance) / 2 ;
                        flowField.particles[i].Position += newPosition;
                        flowField.particles[i].Position.X = Mod(flowField.particles[i].Position.X, (float)PlaySurface.WidthRequest);
                        flowField.particles[i].Position.Y = Mod(flowField.particles[i].Position.Y, (float)PlaySurface.HeightRequest);
                        flowField.particles[j].Position -= newPosition;
                        flowField.particles[j].Position.X = Mod(flowField.particles[j].Position.X, (float)PlaySurface.WidthRequest);
                        flowField.particles[j].Position.Y = Mod(flowField.particles[j].Position.Y, (float)PlaySurface.HeightRequest);
                    }
                }
            }
        }
        private static float Mod(float a, float b)
        {
            return ((a % b) + b) % b;
        }

        private void OnScreenClicked(object sender, TappedEventArgs e)
        {
            if (sender is GraphicsView graphicsView)
            {
                if (SourceModeCheckBox.IsChecked)
                {
                    fluidSources.Add((new Vector2((float)(e.GetPosition(graphicsView)?.X ?? 0), (float)(e.GetPosition(graphicsView)?.Y ?? 0)), 
                                     new Color(random.Next(256), random.Next(256), random.Next(256), 75)));
                }
                else
                flowField.Insert(new Color(random.Next(256), random.Next(256), random.Next(256), 75), (int)(e.GetPosition(graphicsView)?.X ?? 0), (int)(e.GetPosition(graphicsView)?.Y ?? 0));
            }
        }
        private void OnDeleteSourcesButtonPressed(object sender, EventArgs e)
        {
            if (sender == DeleteSourcesButton)
            {
                fluidSources.Clear();
            }
        }
        private void OnFillMapButtonPressed(object sender, EventArgs e)
        {
            if (sender == FillMapButton)
            {
                for (int x = 15; x < (int)PlaySurface.WidthRequest; x += 30)
                {
                    for (int y = 15; y < (int)PlaySurface.HeightRequest; y += 30)
                    {
                        flowField.Insert(new Color(random.Next(256), random.Next(256), random.Next(256), 75), x, y);
                    }
                }
            }
        }
        private void OnDeleteParticlesButtonPressed(object sender, EventArgs e)
        {
            if (sender == DeleteParticlesButton)
            {
                flowField.particles.Clear();
            }
        }
    }

}
