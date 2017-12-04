using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureGenerator : MonoBehaviour {

    public GameObject noiseGenerator;
    public Texture2D image0;
    public Texture2D image1;
    public int width = 256;
    public int height = 256;

    public float[,] noise;
    private Terrain terrain;
    private Texture2D blendedImage;

    void Start()
    {
        noise = noiseGenerator.GetComponent<NoiseGenerator>().perlinNoise;
        blendedImage = new Texture2D(width,height);
        terrain = GetComponent<Terrain>();
        SplatPrototype[] sp = new SplatPrototype[1];
        sp[0] = new SplatPrototype();
        sp[0].metallic = 0;

        BlendImages(image0, image1, noise);
        // save the image into the folder
        byte[] bytes=blendedImage.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/../Assets/Textures/SavedScreen.png",bytes); // save the blended image as png
        // reload it as the texture for splat prototype
        blendedImage.LoadImage(bytes);
        sp[0].texture = blendedImage;
        sp[0].tileSize = new Vector2(width,width); // apply only one texture onto the terrain.
        terrain.terrainData.splatPrototypes = sp;
        //Destroy(blendedImage);
    }

    void BlendImages (Texture2D image0, Texture2D image1, float[,] noise)
    {
        //pass each pixel's color into an 2d array
        Color[,] image0Col = new Color[image0.width, image0.height];
        for (int i = 0; i < image0.width; i++)
            for (int j = 0; j < image0.height; j++)
            {
                image0Col[i, j] = image0.GetPixel(i, j);
            }

        Color[,] image1Col = new Color[image1.width, image1.height];
        for (int i = 0; i < image0.width; i++)
            for (int j = 0; j < image0.height; j++)
            {
                image1Col[i, j] = image1.GetPixel(i, j);
            }

        //int width = image0.width;
        //int height = image0.height;
        Color[,] image = new Color[width, height];
        for(int i=0;i<width;i++)
            for(int j=0;j<height;j++)
            {
                image[i,j] = Interpolate(image0Col[i,j], image1Col[i,j], noise[i,j]);
                blendedImage.SetPixel(i, j, image[i, j]);
            }
    }

    Color Interpolate(Color x0, Color x1, float alpha)
    {
        Color col;
        // interpolate color and make them brighter
        col.a = 1;
        col.r = (x0.r * (1 - alpha) + x1.r * alpha)/**1.5f*/;
        col.g = (x0.g * (1 - alpha) + x1.g * alpha)/**1.5f*/;
        col.b = (x0.b * (1 - alpha) + x1.b * alpha)/**1.5f*/;
        return col;

    }
}
