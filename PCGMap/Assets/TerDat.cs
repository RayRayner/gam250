using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerDat : MonoBehaviour {

	TerrainData TerData;

	// Use this for initialization
	void Awake () {
		TerData = gameObject.GetComponent<TerrainData> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
