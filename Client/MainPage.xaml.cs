using FlowFieldSimulator;
namespace Client
{
    public partial class MainPage : ContentPage
    {
        private FlowField<Color> flowField;
        private IDispatcherTimer timer;
        public MainPage()
        {
            InitializeComponent();
            flowField = new((int)PlaySurface.WidthRequest, (int)PlaySurface.HeightRequest);
            PlaySurface.Drawable = new FlowFieldDrawable(flowField);
            timer = Dispatcher.CreateTimer();
            int FPS = 30;
            timer.Interval = TimeSpan.FromSeconds(1 / FPS);
            timer.Tick += (s,e) => PlaySurface.Invalidate();
            timer.Start();
        }

        private void OnScreenClicked(object sender, TappedEventArgs e)
        {
            if (sender is GraphicsView graphicsView)
            {
                flowField.Insert(new Color(), (int)(e.GetPosition(graphicsView)?.X ?? 0), (int)(e.GetPosition(graphicsView)?.Y ?? 0));
            }
        }
    }

}
