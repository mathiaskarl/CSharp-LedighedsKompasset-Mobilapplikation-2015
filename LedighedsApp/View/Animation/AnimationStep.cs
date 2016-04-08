namespace LedighedsApp.View.Animation
{
    public class AnimationStep
    {
        public int DurationInTicks { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsPause { get; set; }

        public AnimationStep(double x, double y, int duration)
        {
            X = x;
            Y = y;
            DurationInTicks = duration;
            IsPause = false;
        }

        public AnimationStep(int duration)
        {
            DurationInTicks = duration;
            IsPause = true;
        }
    }
}
