using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initTerrain : MonoBehaviour {
	void Start() {
		/* After the diamondSquareAlgorithm.cs script is launched, 
		 * call the methods that generate the terrain. */
		diamondSquareAlgorithm algorithm = gameObject.GetComponent(typeof(diamondSquareAlgorithm))
			as diamondSquareAlgorithm;

		Terrain terrain = gameObject.GetComponent<Terrain>();

		Vector3 TS = terrain.terrainData.size;
		terrain.transform.position = new Vector3(-TS.x / 2, 0, -TS.z / 2);

		algorithm.Reset();
		algorithm.ExecuteDiamondSquare();
	}
}
