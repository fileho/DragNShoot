using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tweens
{
    public static float Smoothstep(float t)
    {
        float start = t * t;
        float stop = 1 - (1 - t) * (1 - t);

        return (1 - t) * start + t * stop;
    }
}
