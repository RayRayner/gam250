using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System;
using UnityEngine;
using UnityEngine.Windows;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapGen : MonoBehaviour
{

    [ExecuteInEditMode()]

    public int toolModeInt = 0;
    public int optionsInt = 0;
    public int pictureInt = 0;


    private float[,] FinalMap;


    public int seed;
    public int heightMapSize, terrainSize, alphaMapSize, tileX, tileZ;

    [Range(0,100)]
    public float MaxHeight;
    public float scale;

    [CustomEditorHeaders("Variables for Perlin Noise", 20, 99, 11, 158, 1)]
    [Range(1,100)]
    public float ScaleMin;
    [Range(1, 100)]
    public float ScaleMax;

    public int octaves = 10;

    //Prototypes
    SplatPrototype[] splats;
    TreePrototype[] trees;
    DetailPrototype[] details;

    [CustomEditorHeaders("Textures", 20, 99,11,158,1)]
    public Texture2D splat0;
    public Texture2D splat1;
    float splat0Size = 10f;
    float splat1Size = 2f;
    public Texture2D detail0, detail1, detail2;

    Terrain[,] Ter;
    public Vector2 offset;

    public float snack;
    

    [CustomEditorHeaders("Pretty Pictures", 20, 140, 140, 140, 1)]
    
    public Texture2D HeightMapTexture;
    public Texture2D BiomeMapTexture;

    [CustomEditorHeaders("Use This for a predifined map", 20, 130, 103, 123, 1)]
    public bool UsePredefinedMap;
    public Texture2D PredefinedMap;

    //Saving the perlin noise map
    public void SaveTexture()
    {
        AssetDatabase.CreateAsset(HeightMapTexture, "Assets/Temp/" + System.DateTime.Now.ToFileTime() + ".asset");
        AssetDatabase.SaveAssets();        
    }
    //Creating various prototypes (needed)
    void Prototypes ()
    {
        splats = new SplatPrototype[2];

        splats[0] = new SplatPrototype();
        splats[0].texture = splat0;
        splats[0].tileSize = new Vector2(splat0Size, splat0Size);

        splats[1] = new SplatPrototype();
        splats[1].texture = splat1;
        splats[1].tileSize = new Vector2(splat1Size, splat1Size);

        Debug.Log("Splats Made");

        details = new DetailPrototype[3];

        details[0] = new DetailPrototype();
        details[0].prototypeTexture = detail0;
        details[0].renderMode = DetailRenderMode.GrassBillboard;
        details[0].healthyColor = Color.green;
        details[0].dryColor = Color.grey;

        details[1] = new DetailPrototype();
        details[1].prototypeTexture = detail1;
        details[1].renderMode = DetailRenderMode.GrassBillboard;
        details[1].healthyColor = Color.green;
        details[1].dryColor = Color.grey;

        details[2] = new DetailPrototype();
        details[2].prototypeTexture = detail2;
        details[2].renderMode = DetailRenderMode.GrassBillboard;
        details[2].healthyColor = Color.green;
        details[2].dryColor = Color.grey;

        Debug.Log("Details Made");
    }

    void ImageToFloat (float[,] imgMap, int tileX, int tileZ)
    {

        //  PredefinedMap.filterMode = FilterMode.Point;
   //     PredefinedMap.width = heightMapSize;
     //   PredefinedMap.height = heightMapSize;
        for (int x = 0; x < PredefinedMap.width; x++)
        {
            for (int z = 0; z < PredefinedMap.height; z++)
            {
                PredefinedMap.filterMode = FilterMode.Point;
                var Colour = PredefinedMap.GetPixels( z, x, PredefinedMap.width, PredefinedMap.height);
                imgMap[z, x] = Colour[x + z].grayscale;
            }
        }
        print("Finished Image Map");
    }

    void BiomeMap (float[,] htMap2, int tileX, int tileZ)
    {
        BiomeMapTexture = new Texture2D(heightMapSize, heightMapSize);
        float ratio = (float)terrainSize / (float)heightMapSize;

        for (int x = 0; x < heightMapSize; x++)
        {
            for (int z = 0; z < heightMapSize; z++)
            {
                float worldPosX2 = (x + tileX * (heightMapSize - 1) * ratio);
                float worldPosZ2 = (z + tileZ * (heightMapSize - 1) * ratio);

                htMap2[z, x] = Mathf.PerlinNoise(worldPosX2 / scale + seed, worldPosZ2 / scale + seed);
                Color color = CalculateColor2((int)worldPosX2, (int)worldPosZ2);
                BiomeMapTexture.SetPixel((int)worldPosX2, (int)worldPosZ2, color);
            }
        }
        BiomeMapTexture.Apply();
        Debug.Log("Heights 2 Done");
    }

    void FillHeights(float[,] htMap, int tileX, int tileZ, int octaves)
    {
        HeightMapTexture = new Texture2D(heightMapSize, heightMapSize);
        float ratio = (float)terrainSize / (float)heightMapSize;

        for (int x = 0; x < heightMapSize; x++)
        {
            for (int z = 0; z < heightMapSize; z++)
            {
                for (int i = 0; i < octaves; i++)
                {
                    float worldPosX = (x + tileX * (heightMapSize - 1) * ratio);
                    float worldPosZ = (z + tileZ * (heightMapSize - 1) * ratio);

                    htMap[z, x] = Mathf.PerlinNoise(worldPosX / scale + seed, worldPosZ / scale + seed);
                    Color color = CalculateColor(x, z);
                    HeightMapTexture.SetPixel((int)worldPosX, (int)worldPosZ, color);
                }
            }
        }
        HeightMapTexture.Apply();
        Debug.Log("Heights Done");
    }

    void FillAlphaMap(TerrainData terrainData)
    {
        float[,,] map = new float[alphaMapSize, alphaMapSize, 2];

        for (int x = 0; x < alphaMapSize; x++)
        {
            for (int z = 0; z < alphaMapSize; z++)
            {
                float normX = x * 1.0f / (alphaMapSize - 1);
                float normZ = z * 1.0f / (alphaMapSize - 1);

                float angle = terrainData.GetSteepness(normX, normZ);

                float frac = angle / 90.0f;

                map[z, x, 0] = frac;
                map[z, x, 1] = 1.0f - frac;
            }
        }

        terrainData.alphamapResolution = alphaMapSize;
        terrainData.SetAlphamaps(0, 0, map);

        Debug.Log("Alpha Map Done");
    }

    Color CalculateColor(int x, int y)
    {

        float xCoord = (float)x / heightMapSize * scale + seed;
        float yCoord = (float)y / heightMapSize * scale + seed;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        //		print (sample);
        return new Color(sample, sample, sample);
    }

    Color CalculateColor2(int x, int y)
    {

        float xCoord = (float)x / heightMapSize * scale + seed;
        float yCoord = (float)y / heightMapSize * scale + seed;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        //		print (sample);
        return new Color(sample, sample, sample);
    }

    public void FindDifference(float[,] height, float[,] other)
    {
        for (int x = 0; x < height.Length; x++)
        {
            for (int z = 0; z < height.Length; z++)
            {
                //FinalMap[x, z] = height[x, z] - other[x, z];
            }
        }
        //Debug.Log(height[2,2] - other[2,2]);
    }

    //Runs at the start
    private void Start()
    {
        if (seed == 0)
        {
            seed = UnityEngine.Random.Range(1, 100000);
        }
        scale = UnityEngine.Random.Range((int)ScaleMin, (int)ScaleMax);

        heightMapSize = Mathf.ClosestPowerOfTwo(heightMapSize) + 1;
        float[,] htMap = new float[heightMapSize, heightMapSize];
        float[,] bmMap = new float[heightMapSize, heightMapSize];
//        float[,] fnlMap = new float[heightMapSize, heightMapSize];
         float[,] imageMap = new float[PredefinedMap.height, PredefinedMap.width];
        //float[,] imageMap = new float[heightMapSize, heightMapSize];

        Ter = new Terrain[tileX, tileZ];

        Prototypes();


        Debug.Log("Beginning Start loop");

        for (int x = 0; x < tileX; x++)
        {
            for (int z = 0; z < tileZ; z++)
            {

                FillHeights(htMap, x, z, octaves);
                BiomeMap(bmMap, x, z);
               // ImageToFloat(imageMap, x, z);
  //              FindDifference(htMap, bmMap);

                TerrainData tData = new TerrainData();
                tData.heightmapResolution = heightMapSize;
                tData.size = new Vector3(terrainSize, MaxHeight, terrainSize);
                tData.name = "PCGTerrainData";
                if (toolModeInt == 1)
                {
                  //  tData.SetHeights(0, 0,imageMap);
                  //  Debug.Log("Tool Mode Int = " + toolModeInt);
                }
                else
                {
                //    Debug.Log("Tool Mode Int = " + toolModeInt);
                    tData.SetHeights(0, 0, htMap);
                }

                tData.splatPrototypes = splats;
                tData.treePrototypes = trees;
                tData.detailPrototypes = details;

                FillAlphaMap(tData);
                //Creating the terrain
                Ter[x, z] = Terrain.CreateTerrainGameObject(tData).GetComponent<Terrain>();
                Ter[x, z].castShadows = false;
                Ter[x, z].transform.position = new Vector3(terrainSize * x + offset.x, 0, terrainSize * z + offset.y);
            }
        }    
    }

}
