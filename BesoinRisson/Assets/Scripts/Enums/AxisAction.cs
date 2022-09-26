namespace Enums
{
    public enum Axis
    {
        X,
        Y
    }

    public static class AxisExt
    {
        public static string ToUnityAxis(this Axis axis)
        {
            return axis == Axis.X ? "Horizontal" : "Vertical";
        }
        public static string ToUnityAxis1(this Axis axis)
        {
            return axis == Axis.X ? "Horizontal1" : "Vertical1";
        }
    }
}
