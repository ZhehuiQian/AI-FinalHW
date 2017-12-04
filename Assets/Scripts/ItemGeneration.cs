using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneration : MonoBehaviour {

    public Terrain terrain;
    public GameObject item_A;
    public GameObject item_B;
    public GameObject item_C;

    private Vector3 APos;
    private Vector3 BPos;
    private Vector3 CPos;
    private float terrainWidth;
    private float terrainLength;

    // Use this for initialization
    void Start()
    {
        terrainWidth = terrain.terrainData.size.x;
        terrainLength = terrain.terrainData.size.z;

        APos.x = Random.Range(0, terrainWidth/2);
        APos.z = Random.Range(0, terrainLength/2);
        APos.y = terrain.SampleHeight(new Vector3(APos.x, 0, APos.z));
        Instantiate(item_A, APos, Quaternion.identity);

        BPos.x = Random.Range(terrainWidth/2, terrainWidth);
        BPos.z = Random.Range(0, terrainLength/2);
        BPos.y = terrain.SampleHeight(new Vector3(BPos.x, 0, BPos.z));
        Instantiate(item_B, BPos, Quaternion.identity);

        CPos.x = Random.Range(0, terrainWidth/2);
        CPos.z = Random.Range(terrainLength/2, terrainLength);
        CPos.y = terrain.SampleHeight(new Vector3(CPos.x, 0, CPos.z));
        Instantiate(item_C, CPos, Quaternion.identity);

    }
}
