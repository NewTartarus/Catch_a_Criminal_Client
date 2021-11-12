namespace ScotlandYard.Scripts.Helper
{
    using UnityEngine;

    public class GizmosHelper
    {
        public static void DrawWiredCircle(Vector3 position, Quaternion rot, float radius, int detail = 3)
        {
            Vector3[] points3d = new Vector3[detail];
            for (int i = 0; i < detail; i++)
            {
                float t = i / (float)detail;
                float angleRad = t * MathHelper.TAU;
                Vector2 point2d = MathHelper.GetUnitVectorByAngle(angleRad) * radius;

                points3d[i] = position + rot * point2d;
            }

            for (int i = 0; i < detail - 1; i++)
            {
                Gizmos.DrawLine(points3d[i], points3d[i + 1]);
            }
            Gizmos.DrawLine(points3d[detail - 1], points3d[0]);
        }
    }
}
