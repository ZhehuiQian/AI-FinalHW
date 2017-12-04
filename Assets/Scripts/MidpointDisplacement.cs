using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointDisplacement : MonoBehaviour {

    public int width = 257;
    //public int height = 256;
    private float[,] heightvalue;
    public float MinHeight=0;
    public float MaxHeight=15;
    public float roughness; // controls the "roughness" of the fractal
    public float displace=5;  // the maximum deviation value

    void Awake()
    {
        heightvalue = new float[width, width];
        print(heightvalue.Length);
    }
    void Start()
    {
        DiamondSqure();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();

    }

    void Update()
    {
    }

    void DiamondSqure(int seed = 0)
    {
        int size = width-1;
        // assign a height value to each corner
        heightvalue[0, 0] = Random.Range(MinHeight, MaxHeight);
        heightvalue[0, size] = Random.Range(MinHeight, MaxHeight);
        heightvalue[size, 0] = Random.Range(MinHeight, MaxHeight);
        heightvalue[size, size] = Random.Range(MinHeight, MaxHeight);

        // temporary variables
        float s0, s1, s2, s3, d0, d1, d2, d3, cn;

        for (int i = size; i > 1; i /= 2)
        {
            //diamond step
            for (int y = 0; y < (width+i/2); y += i)
                for (int x = 0; x < (width+i/2); x += i)
                {

                    s0 = heightvalue[x, y];
                    s1 = heightvalue[(x + i), y];
                    s2 = heightvalue[x, (y + i)];
                    s3 = heightvalue[(x + i), (y + i)];
                    //get the center value
                    heightvalue[(x + i / 2), (y + i / 2)] = (s0 + s1 + s2 + s3) / 4 + displace * Random.Range(-1, 1);
                    print("s0" + s0);
                    print("s1" + s1);
                    print("s2" + s2);
                    print("s3" + s3);

                }


            //square step
            for (int y = 0; y  < width; y += i)
            {
                for (int x = 0; x < width; x += i)
                {
                    s0 = heightvalue[x, y];
                    s1 = heightvalue[x + i, y];
                    s2 = heightvalue[x, y + i];
                    s3 = heightvalue[x + i, y + i];
                    cn = heightvalue[(x + i / 2), (y + i / 2)];

                    d0 = y <= 0 ? (s0 + s1 + cn) / 3.0f : (s0 + s1 + cn + heightvalue[(x + i / 2), (y - i / 2)]) / 4.0f;
                    d1 = x <= 0 ? (s0 + cn + s2) / 3.0f : (s0 + cn + s2 + heightvalue[(x - i / 2), (y + i / 2)]) / 4.0f;
                    d2 = x >= size - i ? (s1 + cn + s3) / 3.0f : (s1 + cn + s3 + heightvalue[(x + i + (i / 2)), (y + (i / 2))]) / 4.0f;
                    d3 = y >= size - i ? (cn + s2 + s3) / 3.0f : (cn + s2 + s3 + heightvalue[(x + (i / 2)), (y + i + (i / 2))]) / 4.0f;

                    heightvalue[(x + (i / 2)), y] = d0 + displace * Random.Range(0, 1);
                    heightvalue[x, (y + (i / 2))] = d1 + displace * Random.Range(0, 1);
                    heightvalue[(x + i), (y + (i / 2))] = d2 + displace * Random.Range(0, 1);
                    heightvalue[(x + (i / 2)), (y + i)] = d3 + displace * Random.Range(0, 1);

                }
            }
        }
        //return heightvalue;

    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, width);

        // genreate perlin noise map fot the texture

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        // from tutorial
        //float xCoord = (float)x / width * scale + offsetX;
        //float yCoord = (float)y / height * scale + offsetY;
        //float sample = Mathf.PerlinNoise(xCoord, yCoord); // need write my own perlinnoise
        //return new Color(sample, sample, sample);
        float sample = heightvalue[x, y];
        return new Color(sample, sample, sample);

    }

}
