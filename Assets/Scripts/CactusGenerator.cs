using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusGenerator : MonoBehaviour {

    public Terrain terrain;
    public GameObject tree0;
    public GameObject tree1;

    public float threshold1 = 20;
    public GameObject noiseGenerator;

    private Vector3 treePos;
    private float[,] perlinNoise;
    private float terrainWidth;
    private float terrainLength;

    public GameObject pathfinder;
    public GameObject seeker;
    public GameObject target;

    void Start()
    {
        perlinNoise= noiseGenerator.GetComponent<NoiseGenerator>().perlinNoise;
        terrainWidth = terrain.terrainData.size.x;
        terrainLength = terrain.terrainData.size.z;
        for (int x=0;x<terrainWidth;x++)
            for(int y=0;y<terrainLength;y++)
            {
                if(terrain.terrainData.GetHeight(x,y)<threshold1)
                {
                    //print("height" + terrain.terrainData.GetHeight(x, y));
                    treePos.x = x;
                    treePos.z = y;
                    treePos.y = terrain.SampleHeight(new Vector3(x, 0, y))/*terrain.SampleHeight(new Vector3(x,0,y))*/;
                    if (Random.value > 0.97)
                    { Instantiate(tree0, treePos, Quaternion.identity); }
                }
                else
                {
                    treePos.x = x;
                    treePos.z = y;
                    treePos.y = terrain.SampleHeight(new Vector3(x, 0, y))/*terrain.SampleHeight(new Vector3(x,0,y))*/;
                    if (Random.value > 0.99)
                    Instantiate(tree1, treePos, Quaternion.identity);
                }
            }
        seeker.SetActive(true);
        target.SetActive(true);
        pathfinder.SetActive(true);
        //seeker.GetComponent<SeekerMovement>().enabled = true;
    }
}
