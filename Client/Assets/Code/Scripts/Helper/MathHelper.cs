namespace ScotlandYard.Scripts.Helper
{
    using UnityEngine;

    public class MathHelper
    {
        public const float TAU = 6.2831853071795862f;

        public static Vector2 GetUnitVectorByAngle(float angleRad)
        {
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
    }
}
