using UnityEngine;

// Adapted from Robert Stivanson's work. Please check the README.md file for attribution.

[RequireComponent(typeof(TerrainCollider))]
public class diamondSquareAlgorithm : MonoBehaviour {

	// The terrain object used to create the landscape
	private TerrainData data;

	// The size of the sides (length and width) of the terrain object 
	private int size;

	// Randomises the corner values of the terrain object
	private bool randomizeCornerValues = true;

	// The heightmap used
	private float[,] heights;

	// Determines the peakedness of the landscape
	private float roughness = 0.96f;

	public float Roughness {
		get { return roughness; }
		set { roughness = Mathf.Clamp(value, 0.001f, 1.999f); }
	}


	// Initialises the terrain object
	private void Awake() {
		data = transform.GetComponent<TerrainCollider>().terrainData;
		size = data.heightmapResolution;

		SetSeed((int)Random.Range(-1000.0f, 1000.0f));
		Reset();

		return;
	}


	// Initiates a random seed number
	public void SetSeed(int seed) {
		Random.InitState(seed);

		return;
	}

	// Flips the value of the randomizeCornerValues flag
	public void ToggleRandomizeCornerValues() {
		randomizeCornerValues = !randomizeCornerValues;

		return;
	}
		
	// Resets the height values for the terrain
	public void Reset() {
		heights = new float[size, size];

		// If the corners of the terrain need to randomised
		if (randomizeCornerValues) {
			heights[0, 0] = Random.value;
			heights[size - 1, 0] = Random.value;
			heights[0, size - 1] = Random.value;
			heights[size - 1, size - 1] = Random.value;
		}

		// Updates the terrain data
		data.SetHeights(0, 0, heights);

		return;
	}


	// Uses Diamond Square algorithm to determine height for each vertex
	public void ExecuteDiamondSquare() {
		
		heights = new float[size, size];
		float average, range = 0.5f;
		int sideLength, halfSide, x, y;

		// While the side length is greater than 1
		for (sideLength = size - 1; sideLength > 1; sideLength /= 2) {
			halfSide = sideLength / 2;

			// Performs the Diamond Step part of the algorithm
			for (x = 0; x < size - 1; x += sideLength) {
				for (y = 0; y < size - 1; y += sideLength) {


					// Finds the average of the corners of the selected vertices
					average = heights[x, y];
					average += heights[x + sideLength, y];
					average += heights[x, y + sideLength];
					average += heights[x + sideLength, y + sideLength];
					average /= 4.0f;

					// Adds an adittional random value to the middle of the vertices 
					average += (Random.value * (range * 2.0f)) - range;
					heights[x + halfSide, y + halfSide] = average;
				}
			}

			// Performs the Square Step part of the algorithm
			for (x = 0; x < size - 1; x += halfSide) {
				for (y = (x + halfSide) % sideLength; y < size - 1; y += sideLength) {


					// Gets the average height value of the corners
					average = heights[(x - halfSide + size - 1) % (size - 1), y];
					average += heights[(x + halfSide) % (size - 1), y];
					average += heights[x, (y + halfSide) % (size - 1)];
					average += heights[x, (y - halfSide + size - 1) % (size - 1)];
					average /= 4.0f;

					// Adds an adittional random value
					average += (Random.value * (range * 2.0f)) - range;

					// Makes the height value the calculate average of the vertices around it
					heights[x, y] = average;

					// If it is an edge piece, set the height of the opposite edge to be the same
					if (x == 0) {
						heights[size - 1, y] = average;
					}

					if (y == 0) {
						heights[x, size - 1] = average;
					}
				}
			}

			// Lowers the range of the random value
			range -= range * 0.5f * roughness;
		}

		// Updates the heights of the terrain
		data.SetHeights(0, 0, heights);

		return;
	}
		
	// Returns the number of vertices that need to be skipped given a certain depth
	public int GetStepSize(int depth) {
		
		// Returns an invalid step size when the depth is not valid
		if (!ValidateDepth(depth)) {
			return -1;
		}

		// Returns the number of vertices to skip 
		return (int)((size - 1) / Mathf.Pow(2, (depth - 1)));
	}
		
	// Returns the maximum depth given the terrain's size
	public int MaxDepth() {
		
		// 0.69314718056f = Natural Log of 2
		return (int)((Mathf.Log(size - 1) / 0.69314718056f) + 1);
	}

	// Returns true is the depth is above zero and below the maximum depth
	private bool ValidateDepth(int depth) {
		if (depth > 0 && depth <= MaxDepth()) {
			return true;
		}

		return false;
	}
}