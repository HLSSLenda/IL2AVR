using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
namespace System;

[Pure]
public static class Math
{
    [Pure]
    internal static class IL2AVR
    {
        private static float Add(float x, float y)
        {
            short rx = Unsafe.As<float, short>(ref x);
            short ry = Unsafe.As<float, short>(ref y);
            
        }
        private static double Add(double x, double y)
        {
            int rx = Unsafe.As<double, int>(ref x);
            int ry = Unsafe.As<double, int>(ref y);
        }
        private static float Sub(float x, float y)
        {
            short rx = Unsafe.As<float, short>(ref x);
            short ry = Unsafe.As<float, short>(ref y);
            if (rx == ry)
                return 0;
        }
        private static double Sub(double x, double y)
        {
            int rx = Unsafe.As<double, int>(ref x);
            int ry = Unsafe.As<double, int>(ref y);
            if (rx == ry)
                return 0;
        }
        private static float Mul(float x, float y)
        {
        }
        private static double Mul(double x, double y)
        {
        }
        private static float Div(float x, float y)
        {
        }
        private static double Div(double x, double y)
        {
        }
    }
    public static int Abs(int x) => x | (1 << sizeof(int) * 2);
    public static int Max(int x, int y) => x > y ? x : y;
    public static int Min(int x, int y) => x < y ? x : y;
}
