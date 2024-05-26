using FlowFieldSimulator;
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
            flowField.particles.AsEnumerable();
            var oldParticles = flowField.particles.ToArray();
            for (int i = flowField.particles.Count - 1; i > 0 ; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    var particle = oldParticles[i];
                    var otherParticle = flowField.particles[j];
                    if (Vector2.Distance(particle.Position, otherParticle.Position) < particle.Radius + otherParticle.Radius)
                    {
                        flowField.particles[j] = new(
                            new Color(
                                (particle.Value.Red + otherParticle.Value.Red) / 2,
                                (particle.Value.Green + otherParticle.Value.Green) / 2,
                                (particle.Value.Blue + otherParticle.Value.Blue) / 2,
                                particle.Value.Alpha + otherParticle.Value.Alpha),
                            (particle.Position.X + otherParticle.Position.X) / 2,
                            (particle.Position.Y + otherParticle.Position.Y) / 2,
                            (float) Math.Sqrt(Math.Pow(particle.Radius, 2) + Math.Pow(otherParticle.Radius, 2))); // sqrt(2)*sqrt(r^2+R^2)
                        flowField.particles.RemoveAt(i);
                    }
                }
            }
        }

        private void OnScreenClicked(object sender, TappedEventArgs e)
        {
            if (sender is GraphicsView graphicsView)
            {
                if (SourceModeCheckBox.IsChecked)
                {
                    fluidSources.Add((new Vector2((float)(e.GetPosition(graphicsView)?.X ?? 0), (float)(e.GetPosition(graphicsView)?.Y ?? 0)), 
                                     new Color(random.Next(256), random.Next(256), random.Next(256),10)));
                }
                else
                flowField.Insert(new Color(random.Next(256), random.Next(256), random.Next(256), 10), (int)(e.GetPosition(graphicsView)?.X ?? 0), (int)(e.GetPosition(graphicsView)?.Y ?? 0));
            }
        }
    }

}
