using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static bool gameRunning = true;

    public static float scrollRate = -2.0f;
    public static float startScrollRate = -2.0f;

    public static float maxSurfaceHeight = 7.0f;
    public static float minSurfaceHeight = 2.0f;
    public static float minHeightDelta = 0.5f;

    public static float maxSurfaceWidth = 4.0f;
    public static float minSurfaceWidth = 2.0f;

    public static float maxRotationDeg = 30.0f;
    public static float minRotationDeg = 10.0f;

    public static float destructionLimit = -11.0f;
    public static float creationLimit = 11.0f;

    public static float screenBottom = -5.0f;

    public static float minGap = 1.0f;
    //public static float maxGap = 2.0f;

    public static float initialX = -7.0f;
    public static float initialY = -1.0f;
}
