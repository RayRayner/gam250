using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenOLD: MonoBehaviour {
	[Range(0, 20000)]
	public int Seed;

	public Vector3 TerrainSize;

	public int TerrainX, TerrainZ;

	public int m_heightMapSize = 513;
	public int m_terrainSize = 2048;
	public int m_alphaMapSize = 1024;

	// Prototype stuff

	SplatPrototype[] Splats;
//	TreePrototype[] Trees;
	DetailPrototype[] Details;

	Texture2D splat0, splat1;
	float SplatSize0 = 10f;
	float SplatSize1 = 2f;
	Texture2D detail0, detail1, detail2;

	[Range (0,32)]
	public float MaxHeight;

	[Range (0, 32)]
	public int Octaves;
	[Range (0, 32)]
	public int Dimensions;

	int tileX;
	int tileY;

	Terrain[,] Ter;

	void Prototypes () {

		Splats = new SplatPrototype[2];

		Splats [0] = new SplatPrototype ();
		Splats [0].texture = splat0;
		Splats [0].tileSize = new Vector2 (SplatSize0, SplatSize0);

		Splats [1] = new SplatPrototype ();
		Splats [1].texture = splat1;
		Splats [1].tileSize = new Vector2 (SplatSize1, SplatSize1);

		Details = new DetailPrototype[3];

		Details [0] = new DetailPrototype ();
		Details [0].prototypeTexture = detail0;
		Details [0].renderMode = DetailRenderMode.GrassBillboard;
		Details [0].healthyColor = Color.green;
		Details [0].dryColor = Color.grey;

		Details [1] = new DetailPrototype ();
		Details [1].prototypeTexture = detail1;
		Details [1].renderMode = DetailRenderMode.GrassBillboard;
		Details [1].healthyColor = Color.green;
		Details [1].dryColor = Color.grey;

		Details [2] = new DetailPrototype ();
		Details [2].prototypeTexture = detail2;
		Details [2].renderMode = DetailRenderMode.GrassBillboard;
		Details [2].healthyColor = Color.green;
		Details [2].dryColor = Color.grey;
	}

//	/*
	void FillAlphaMap(TerrainData terrainData)
	{
		float[,,] map = new float[m_alphaMapSize, m_alphaMapSize, 2];

		for (int x = 0; x < m_alphaMapSize; x++)
		{
			for (int z = 0; z < m_alphaMapSize; z++)
			{
				// Get the normalized terrain coordinate that
				// corresponds to the the point.
				float normX = x * 1.0f / (m_alphaMapSize - 1);
				float normZ = z * 1.0f / (m_alphaMapSize - 1);

				// Get the steepness value at the normalized coordinate.
				float angle = terrainData.GetSteepness(normX, normZ);

				// Steepness is given as an angle, 0..90 degrees. Divide
				// by 90 to get an alpha blending value in the range 0..1.
				float frac = angle / 90.0f;
				map[z, x, 0] = frac;
				map[z, x, 1] = 1.0f - frac;

			}
		}

		terrainData.alphamapResolution = m_alphaMapSize;
		terrainData.SetAlphamaps (0, 0, map);
		//								new float[m_alphaMapSize,m_alphaMapSize,0]
	}
//	*/

	// int tileX, int tileZ
	void FillHeights(float[,] htmap, int tileX, int tileZ)
	{
		float ratio = (float)m_terrainSize / (float)m_heightMapSize;

		for (int x = 0; x < m_heightMapSize; x++)
			for (int z = 0; z < m_heightMapSize; z++)
			{
				float worldPosX = (x + tileX * (m_heightMapSize - 1)) * ratio;
				float worldPosZ = (z + tileZ * (m_heightMapSize - 1)) * ratio;

				htmap [z, x] = Mathf.PerlinNoise (worldPosZ * 2, worldPosX * 2);

			}
		}

	// Use this for initialization
	void Start () {

		m_heightMapSize = Mathf.ClosestPowerOfTwo(m_heightMapSize) + 1;

		float[,] htmap = new float[m_heightMapSize, m_heightMapSize];
		FillHeights (htmap, m_heightMapSize,m_heightMapSize);

		Prototypes ();

		TerrainData tData = new TerrainData ();
		tData.heightmapResolution = m_heightMapSize;
		tData.size = new Vector3(m_terrainSize, MaxHeight, m_terrainSize);
		tData.name = "TerrainDataPCG";
		tData.SetHeights (0, 0, htmap);

		Ter = new Terrain[TerrainX,TerrainZ];

		FillAlphaMap (tData);

//		Terrain.CreateTerrainGameObject (tData);
		//		GameObject.FindObjectOfType<Terrain> ().gameObject.AddComponent<TerDat> ();

		for (int x = 0; x < tileX; x++)
		{
			for (int z = 0; z < tileY; z++)
			{
				FillHeights(htmap, x, z);

//				TerrainData terrainData = new TerrainData();

				tData.heightmapResolution = m_heightMapSize;
				tData.SetHeights(0, 0, htmap);
				tData.size = new Vector3(m_terrainSize, MaxHeight, m_terrainSize);
				tData.splatPrototypes = Splats;
//				tData.treePrototypes = m_treeProtoTypes;
				tData.detailPrototypes = Details;

				FillAlphaMap(tData);

					Ter[x, z] = Terrain.CreateTerrainGameObject(tData).GetComponent<Terrain>();
					Ter[x, z].transform.position = Vector3.zero;
//					Ter[x, z].heightmapPixelError = m_pixelMapError;
//					Ter[x, z].basemapDistance = m_baseMapDist;
					Ter[x, z].castShadows = false;

//				FillTreeInstances(m_terrain[x, z], x, z);
//				FillDetailMap(m_terrain[x, z], x, z);

			}
		}
	}

	// Update is called once per frame
	void Update () {

	}	
}