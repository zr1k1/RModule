using UnityEngine;

public static class DpToPixConverter {
    public static float ConvertDpToPixels(float canvasScalerHeight, float dpValue) {
        var pixels = dpValue;
        pixels = pixels * Screen.dpi / 160f;
        pixels *= canvasScalerHeight / (float)Screen.height;

        return pixels;
    }
}
