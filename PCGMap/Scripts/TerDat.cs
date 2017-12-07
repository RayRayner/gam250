using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerDat : MonoBehaviour {

	TerrainData TerData;

	void Start () {

	}

	// Use this for initialization
	public void Test (TerrainData data) {		
		TerData = gameObject.GetComponent<TerrainCollider> ().terrainData;
		float [,,] map = new float [data.alphamapWidth, data.alphamapHeight, 2];

		for (var y = 0; y < data.alphamapHeight; y++) {
			for (var x = 0; x < data.alphamapWidth; x++) {
				var normX = x * 1.0f / (data.alphamapWidth - 1);
				var normY = x * 1.0f / (data.alphamapHeight - 1);

				var angle = data.GetSteepness (normX, normY);
				var frac = angle / 90.0f;
				map[x, y, 0] = frac;
				map[x, y, 0] = 1 - frac;
			}
		}
		Debug.Log (data.alphamapWidth + "  " + data.alphamapHeight);
		data.SetAlphamaps (0, 0, map);

		
	}
}