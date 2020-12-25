using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitXAxis : MonoBehaviour {

	Vector3 startPos;

	// Use this for initialization
	void Start() {
		startPos = transform.position;
	}

	public int orbitSpeed;
	public float displacement;

	// Update is called once per frame
	void Update() {
		Vector3 orbit_centre = new Vector3(0.0f, 0.0f, displacement);
		transform.RotateAround(startPos + orbit_centre, Vector3.right, orbitSpeed * Time.deltaTime);
	}
}
