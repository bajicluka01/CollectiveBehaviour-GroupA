
using UnityEngine;

public class Numbers
{
    // this gives gaussian distributed random numbers using the Box–Muller transform method 
    public static float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }

    static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);
        // s is now: s < 1.0 && s > 0   
        s = Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
        return v1 * s;
    }

    // given a 2D vector rotate it for the given angle given in degrees
    // returns a rotated vector
    public static Vector2 Rotate2D(Vector2 v, float angle)
    {
        Vector3 v3D = new(v.x,v.y,0);
        Vector3 rotated3DVector = Quaternion.Euler(0,0,angle) * v3D;
        Vector2 result = new(rotated3DVector.x,rotated3DVector.y);
        return result;
    }
}
