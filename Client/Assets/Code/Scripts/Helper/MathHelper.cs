namespace ScotlandYard.Scripts.Helper
{
    using UnityEngine;

    public class MathHelper
    {
        public const float TAU = 6.2831853071795862f;
        public const float EPSILON = 1.4F;

        public static Vector2 GetUnitVectorByAngle(float angleRad)
        {
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float ModFloat(float x, float m)
        {
            float r = x % m;
            return r < 0 ? r + m : r;
        }

        public static int ModInt(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
