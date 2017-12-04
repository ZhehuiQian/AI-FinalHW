using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour {

    public int width = 257;
    public int height = 257;

    public float[,] perlinNoise;
    public int octave = 5;              // how many layers you are putting together
    public float amplitude = 1.0f;      // how tall the feature should be 
    public float lacunarity = 1.0f;
    public float persistence = 0.5f;    // what makes the amplitude shrink

    //diamondsquare stuff
    public float[,] heightvalue;    // the height of each point of the terrain
    //public int width = 257;         // width=height=2^n+1
    public int depth = 100;
    public int division = 256;      // how many squares eventually will be made on the terrain per(col/row)
    public float MaxHeight = 1;
    public float MinHeight = 0;




    void Awake()
    {
        //perlinNoise = new float[width,height];
        perlinNoise = GenerateNoise(GeneratePerlinNoise(GenerateWhiteNoise(width, height), octave), GenerateDiamondNoise());

        //perlinNoise = GeneratePerlinNoise(GenerateWhiteNoise(width, height), octave);
    }

    float[,] GenerateWhiteNoise(int width, int height)
    {
        // generate white noise: create an array with random values between 0 and 1
        float[,] noise = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //noise[i, j] = Mathf.Sin(Random.Range(0.0f, 6.2831852f)); // this function can make the mountain 
                noise[i, j] = Random.value;
                //print("noise" + noise[i, j]);
            }
        }
        return noise;
    }

    float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);
        float[,] smoothNoise = new float[width, height];

        int samplePeriod = 1 << octave; // calculates 2^k
        float sampleFrequency = (1.0f / samplePeriod) * lacunarity;

        for (int i = 0; i < width; i++)
        {
            // calculate the horizontal sampling indices
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width; // wrap around
            float horizontal_blend = (i - sample_i0) * sampleFrequency;

            for (int j = 0; j < height; j++)
            {
                // calculate the vertical sampling indices
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % height; // wrap around
                float vertical_blend = (j - sample_j0) * sampleFrequency;

                // blend the top twp corners
                float top = Interpolate(baseNoise[sample_i0, sample_j0],
                    baseNoise[sample_i1, sample_j0], horizontal_blend);

                // blend the bottom two corners
                float bottom = Interpolate(baseNoise[sample_i0, sample_j1],
                    baseNoise[sample_i1, sample_j1], horizontal_blend);

                // final blend
                smoothNoise[i, j] = Interpolate(top, bottom, vertical_blend);
            }
        }

        return smoothNoise;
    }

    float Interpolate(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + alpha * x1; // linear interpolationW
    }

    float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);

        float[][,] smoothNoise = new float[octaveCount][,]; // an array of 2D array containing

        // generate smooth noise
        for (int i = 0; i < octaveCount; i++)
        {
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
        }
        float[,] perlinNoise = new float[width, height];
        float totalAmplitude = 0.0f;

        // blend noise together
        for (int octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistence;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
                }
            }
        }

        //normalization
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                perlinNoise[i, j] /= totalAmplitude;
            }
        }

        return perlinNoise;
    }

    float[,] GenerateDiamondNoise()
    {
        //
        int iterations = (int)Mathf.Log(division, 2);   // how many diamondsquare should we perform
        int numSquares = 1;
        int squareSize = division;


        // initialize the four corners
        heightvalue = new float[width, width];
        heightvalue[0, 0] = Random.Range(MinHeight, MaxHeight);
        heightvalue[width - 1, 0] = Random.Range(MinHeight, MaxHeight);
        heightvalue[0, width - 1] = Random.Range(MinHeight, MaxHeight);
        heightvalue[width - 1, width - 1] = Random.Range(MinHeight, MaxHeight);
        // for test
        //print(heightvalue[0, 0]);
        //print(heightvalue[width - 1, 0]);
        //print(heightvalue[0, width - 1]);
        //print(heightvalue[width - 1, width - 1]);

        //use loops for doing diamond squares
        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    // do diamond square steps
                    DiamondSquareStep(row, col, squareSize, MaxHeight);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            MaxHeight *= 0.5f;
        }

        return heightvalue;
    }
    void DiamondSquareStep(int row, int col, int size, float offset)
    {
        // square step
        int halfSize = (int)(size * 0.5);
        int topLeftX = col;
        int topLeftY = row;
        int botLeftX = col;
        int botLeftY = row + size;
        int midX = col + halfSize;
        int midY = row + halfSize;

        heightvalue[midX, midY] = (heightvalue[topLeftX, topLeftY] + heightvalue[topLeftX + size, topLeftY]
            + heightvalue[botLeftX, botLeftY] + heightvalue[botLeftX + size, botLeftY]) * 0.25f + Random.Range(-offset, offset);
        //print(heightvalue[midX, midY]);

        // diamond step
        heightvalue[topLeftX + halfSize, topLeftY] =
            (heightvalue[topLeftX, topLeftY] + heightvalue[topLeftX + size, topLeftY] + heightvalue[midX, midY]) / 3 + Random.Range(-offset, offset);
        heightvalue[midX - halfSize, midY] =
            (heightvalue[topLeftX, topLeftY] + heightvalue[midX, midY] + heightvalue[botLeftX, botLeftY]) / 3 + Random.Range(-offset, offset);
        heightvalue[botLeftX + halfSize, botLeftY] =
            (heightvalue[botLeftX, botLeftY] + heightvalue[midX, midY] + heightvalue[botLeftX + size, botLeftY]) / 3 + Random.Range(-offset, offset);
        heightvalue[midX + halfSize, midY] =
            (heightvalue[topLeftX + size, topLeftY] + heightvalue[midX, midY] + heightvalue[botLeftX + size, botLeftY]) / 3 + Random.Range(-offset, offset);
    }

    float[,] GenerateNoise(float[,] noise1, float[,] noise2)
    {
        float[,] noise = new float[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                noise[i, j] = noise1[i, j] * 0.5f + noise2[i, j] * 0.5f;
            }
        return noise;
    }

}
