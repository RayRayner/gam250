using UnityEngine;

public class OwnPerlin : MonoBehaviour {

	[Range(0, 100000)]
	public float Seed;

	public int width = 256;
	public int height = 256;

	public float Scale = 1.0f;

	public float offsetX = 100f;
	public float offsetY = 100f;

	void Update () {
		Renderer Rend = GetComponent<Renderer> ();
		Rend.material.mainTexture = GenerateTexture ();
		Seed = Random.Range (0, 1000000);
	}

	Texture2D GenerateTexture() {
		Texture2D text23 = new Texture2D (width, height);

		//Generating perlin

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
					Color color = CalculateColor (x, y);
				text23.SetPixel(x, y, color);
			}
		}
		text23.Apply ();
		return text23;

	}

	Color CalculateColor (int x, int y) {

		float xCoord = (float)x / width * Scale + Seed;
		float yCoord = (float)y / height * Scale + Seed;
		float sample = Mathf.PerlinNoise (xCoord, yCoord);
//		print (sample);
		return new Color (sample, sample, sample);
	}

}
