﻿using UnityEngine;

public class TextureScaler {
    /// <summary>
    ///     Returns a scaled copy of given texture.
    /// </summary>
    /// <param name="tex">Source texure to scale</param>
    /// <param name="width">Destination texture width</param>
    /// <param name="height">Destination texture height</param>
    /// <param name="mode">Filtering mode</param>
    public static Texture2D ScaleAndReturnNewTextureObject(Texture2D src, int width, int height, FilterMode mode = FilterMode.Point) {
        Rect texR = new Rect(0, 0, width, height);
        ScaleGPU(src, width, height, mode);

        //Get rendered data back to a new texture
        Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false,false);
        result.Reinitialize(width, height, TextureFormat.RGBA32,false);
        result.ReadPixels(texR, 0, 0, false);
        return result;
    }

    /// <summary>
    /// Scales the texture data of the given texture.
    /// </summary>
    /// <param name="tex">Texure to scale</param>
    /// <param name="width">New width</param>
    /// <param name="height">New height</param>
    /// <param name="mode">Filtering mode</param>
    public static void Scale(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Point) {
        Rect texR = new Rect(0, 0, width, height);
        ScaleGPU(tex, width, height, mode);

        // Update new texture
        tex.Reinitialize(width, height);
        tex.ReadPixels(texR, 0, 0, false);
        tex.Apply(false);        //Remove this if you hate us applying textures for you :)
    }

    // Internal unility that renders the source texture into the RTT - the scaling method itself.
    static void ScaleGPU(Texture2D src, int width, int height, FilterMode fmode)
    {
        //We need the source texture in VRAM because we render with it
        src.filterMode = fmode;
        src.Apply(false);

        //Using RTT for best quality and performance. Thanks, Unity 5
        //RenderTexture rtt = new RenderTexture(width, height, 32);
        RenderTexture rtt = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);

        //Set the RTT in order to render to it
        Graphics.SetRenderTarget(rtt);

        //Setup 2D matrix in range 0..1, so nobody needs to care about sized
        GL.LoadPixelMatrix(0, 1, 1, 0);

        //Then clear & draw the texture to fill the entire RTT.
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }
}
