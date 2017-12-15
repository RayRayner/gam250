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
    public float persistance = 0.5f;
    public float lacunarity = 2f;

    float maxNoiseHeight = float.MinValue;
    float minNoiseHeight = float.MaxValue;

    //Prototypes
    SplatPrototype[] splats;
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
        //Works once per run time
        AssetDatabase.CreateAsset(HeightMapTexture, "Assets/Temp/Terriain" + System.DateTime.Now.Day.ToString()+System.DateTime.Now.Month.ToString()+System.DateTime.Now.Year.ToString()+System.DateTime.Now.Millisecond.ToString()+ ".asset");
        AssetDatabase.SaveAssets();        
    }
    //Creating various prototypes (needed)
    void Prototypes ()
    {
        //Ground textures
        splats = new SplatPrototype[2];

        splats[0] = new SplatPrototype();
        splats[0].texture = splat0;
        splats[0].tileSize = new Vector2(splat0Size, splat0Size);

        splats[1] = new SplatPrototype();
        splats[1].texture = splat1;
        splats[1].tileSize = new Vector2(splat1Size, splat1Size);

        //Grass textures
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

    }

    void ImageToFloat (float[,] imgMap, int tileX, int tileZ)
    {
        //Make sure its filtermode correlates to whats needed
        PredefinedMap.filterMode = FilterMode.Point;
      //For each coordinate on the map
        for (int x = 0; x < PredefinedMap.width; x++)
        {
            for (int z = 0; z < PredefinedMap.height; z++)
            {
                //Gets the point as a variant of grey
                imgMap[x, z] = PredefinedMap.GetPixel(x, z).grayscale;
            }
        }
    }

    void FillHeights(float[,] htMap, int tileX, int tileZ, int octaves, float persistance, float lacunarity)
    {
        //Makes a new texture to fill
        HeightMapTexture = new Texture2D(heightMapSize, heightMapSize);
        float ratio = (float)terrainSize / (float)heightMapSize;

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;
        //For each coordinate
        for (int x = 0; x < heightMapSize; x++)
        {
            for (int z = 0; z < heightMapSize; z++)
            {
                //For each octave
                for (int i = 0; i < octaves; i++)
                {
                    //Gets its world pos
                    float worldPosX = (x + tileX * (heightMapSize - 1) * ratio * frequency);
                    float worldPosZ = (z + tileZ * (heightMapSize - 1) * ratio * frequency);
                    //Making the perlin map
                    float perlinValue = Mathf.PerlinNoise(worldPosX / scale + seed, worldPosZ / scale + seed);
                    //Setting the height
                    noiseHeight += perlinValue * amplitude;
                    //Increasing values
                    amplitude *= persistance;
                    frequency *= lacunarity;
                    //Getting the colour
                    Color color = CalculateColor(x, z);
                    //Setting the pixel
                    HeightMapTexture.SetPixel((int)worldPosX, (int)worldPosZ, color);
                }
                //Making some limits for the heights
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                //Actually setting the height to the height map
                htMap[x, z] = HeightMapTexture.GetPixel(x, z).grayscale;

            }
        }

       //Apply the texture
        HeightMapTexture.Apply();
    }
    //Making an alpha map
    void FillAlphaMap(TerrainData terrainData)
    {
        //New triple float
        float[,,] map = new float[alphaMapSize, alphaMapSize, 2];
        //Going through each coord
        for (int x = 0; x < alphaMapSize; x++)
        {
            for (int z = 0; z < alphaMapSize; z++)
            {
                //Getting a normal of the coord
                float normX = x * 1.0f / (alphaMapSize - 1);
                float normZ = z * 1.0f / (alphaMapSize - 1);
                //Setting the angle
                float angle = terrainData.GetSteepness(normX, normZ);
                //Setting the final angle
                float frac = angle / 90.0f;
                //Making the angle known on the map
                map[z, x, 0] = frac;
                map[z, x, 1] = 1.0f - frac;
            }
        }
        //Setting its resolution
        terrainData.alphamapResolution = alphaMapSize;
        //Applying it to the terrain
        terrainData.SetAlphamaps(0, 0, map);
    }

    Color CalculateColor(int x, int y)
    {
        //Getting the coordinates on the map
        float xCoord = (float)x / heightMapSize * scale + seed;
        float yCoord = (float)y / heightMapSize * scale + seed;
        //Getting the perlin noise at the coord
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        //Returning the colour
        return new Color(sample, sample, sample);
    }

    //Runs at the start
    private void Start()
    {
        //If the seed is 0, makes a random seed
        if (seed == 0)
        {
            seed = UnityEngine.Random.Range(1, 100000);
        }
        //Setting how zoomed in the map is compared to the perlin map
        scale = UnityEngine.Random.Range((int)ScaleMin, (int)ScaleMax);
        //Setting the size of the terrain mesh
        heightMapSize = Mathf.ClosestPowerOfTwo(heightMapSize) + 1;
        float[,] htMap = new float[heightMapSize, heightMapSize];
        float[,] imageMap = new float[PredefinedMap.height, PredefinedMap.width];

        Ter = new Terrain[tileX, tileZ];

        Prototypes();

        for (int x = 0; x < tileX; x++)
        {
            for (int z = 0; z < tileZ; z++)
            {
                //Making the terrain data
                TerrainData tData = new TerrainData();
                //Setting its resolution
                tData.heightmapResolution = heightMapSize;
                //Setting its size as a vector 3
                tData.size = new Vector3(terrainSize, MaxHeight, terrainSize);
                //Setting its name
                tData.name = "PCGTerrainData";
                //Deciding if its making the terrain off a picture or randomly
                //If picture is selected..
                if (toolModeInt == 1)
                {
                    //Makes the image heightmap
                    ImageToFloat(imageMap, x, z);
                    //Applies it in the terrain data
                    tData.SetHeights(0, 0,imageMap);
                }
                else
                {
                    //Makes a random heightmap
                    FillHeights(htMap, x, z, octaves, persistance, lacunarity);
                    //Setting the heights in the terrain data
                    tData.SetHeights(0, 0, htMap);
                }

                //Setting all the prototypes
                tData.splatPrototypes = splats;
                tData.detailPrototypes = details;
                //Filling the alpha map
                FillAlphaMap(tData);
                //Creating the terrain
                Ter[x, z] = Terrain.CreateTerrainGameObject(tData).GetComponent<Terrain>();
                //Making it so the terrain doesnt cast shadows
                Ter[x, z].castShadows = false;
                //Setting the terrain position (needed for tiling)
                Ter[x, z].transform.position = new Vector3(terrainSize * x + offset.x, 0, terrainSize * z + offset.y);
            }
        }
    }
}
