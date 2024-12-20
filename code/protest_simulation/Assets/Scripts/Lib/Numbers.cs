
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
}
