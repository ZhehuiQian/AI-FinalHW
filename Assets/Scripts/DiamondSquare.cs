using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare : MonoBehaviour {


    public float[,] heightvalue;    // the height of each point of the terrain
    public int width = 257;         // width=height=2^n+1
    public int depth = 10;
    public int division = 256;      // how many squares eventually will be made on the terrain per(col/row)
    public float MaxHeight=1;
    public float MinHeight = 0;
    //public float offset = 5;        // displacement
    Terrain terrain;

    private void Awake()
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

        //terrain generation
        terrain = GetComponent<Terrain>();
        terrain.terrainData.heightmapResolution = width + 1;
        terrain.terrainData.size = new Vector3(width, depth, width);
        terrain.terrainData.SetHeights(0, 0, heightvalue);


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
            + heightvalue[botLeftX, botLeftY] + heightvalue[botLeftX+size, botLeftY]) * 0.25f + Random.Range(-offset, offset);
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
}
