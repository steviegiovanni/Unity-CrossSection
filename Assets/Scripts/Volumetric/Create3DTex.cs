using System.Collections.Generic;
using UnityEngine;

public class Create3DTex : MonoBehaviour
{
    public List<Texture2D> scans;
    public Texture3D tex;
    public int size = 16;

    void Start()
    {
        if (scans.Count == 0) return;
        int width = scans[0].width;
        int height = scans[0].height;
        int depth = scans.Count;

        //tex = new Texture3D(size, size, size, TextureFormat.ARGB32, true);
        tex = new Texture3D(width, height, depth, TextureFormat.ARGB32, true);

        //var cols = new Color[size * size * size];
        var cols = new Color[width * depth * height];

        float mul = 1.0f / (size - 1);
        int idx = 0;
        Color c = Color.white;
        /*for (int z = 0; z < size; ++z)
        {
            for (int y = 0; y < size; ++y)
            {
                for (int x = 0; x < size; ++x, ++idx)
                {
                    c.r = ((x) != 0) ? x * mul : 1 - x * mul;
                    c.g = ((x) != 0) ? x * mul : 1 - x * mul;
                    c.b = ((x) != 0) ? x * mul : 1 - x * mul;
                    
                    if((x - size/2.0f)* (x - size / 2.0f) + (y - size / 2.0f) * (y - size / 2.0f) + (z - size / 2.0f) * (z - size / 2.0f) <= size * size / 9.0f)
                    {
                        c.r = 1;
                        c.g = 1;
                        c.b = 1;
                    }
                    else
                    {
                        c.r = 0.2f;
                        c.g = 0.2f;
                        c.b = 0.2f;
                    }

                    cols[idx] = c;
                }
            }
        }*/

        for (int z = 0; z < depth; ++z)
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x, ++idx)
                {
                    cols[idx] = scans[z].GetPixel(x, y);
                    if (cols[idx].r <= 0.5f)
                        cols[idx].a = 0.0f;
                    else if (cols[idx].r >= 0.85f)
                        cols[idx].a = 1.0f;
                    else
                        cols[idx].a = cols[idx].r;
                    //if(cols[idx].r <= 0.5f)
                    //    cols[idx].a = 0.0f;
                }
            }
        }

        tex.SetPixels(cols);
        tex.Apply();
        GetComponent<Renderer>().material.SetTexture("_Volume", tex);
        GetComponent<Renderer>().material.SetFloat("_Density", 2.0f);
        GetComponent<Renderer>().material.SetInt("_SamplingQuality", scans.Count);
        GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
    }
}