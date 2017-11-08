using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGen : MonoBehaviour {

	public Vector3 TerrainSize;

	// Use this for initialization
	void Start () {
		TerrainData tData = new TerrainData ();
		tData.size = TerrainSize;
		Terrain.CreateTerrainGameObject (tData);
		GameObject.FindObjectOfType<Terrain> ().gameObject.AddComponent<TerDat> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}		
}
