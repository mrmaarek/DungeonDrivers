using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MapSpawnerScript : MonoBehaviour 
{


	public bool startSpawning;

	public List<GameObject> ObjectsMovingAlong = new List<GameObject>();
	private List<GameObject> spawnedLevelTiles = new List<GameObject>();

	public float totalLength;

	public CurrentLevelType currentLevelType;
	public enum CurrentLevelType
	{
		Cave,
		Town,
		Forest
	}
	 
	public float cameraSpeed;
	private float tileTimer;
	private int currentTile;

	public float tileLength;

	public float lastPlacementPosition;

	public List<TileForList> tiles = new List<TileForList>();

	public float totalPercentage;

	/*
	void Start()
	{
		ObjectsMovingAlong.Add(GameObject.Find("Main Camera"));
	}
*/
	void Update () 
	{
		if(startSpawning)
		{
			SpawnCicle();
			foreach(GameObject MovingObject in ObjectsMovingAlong)
			{
				MoveAlong(MovingObject);
			}
		}

		CheckSpawnPercentage();
	}

	void CheckSpawnPercentage()
	{
		totalPercentage = TotalPercentage();

		if(totalPercentage < 100)
		{
			Debug.LogError("Spawnpercentage too low, increase the total to 100%");
		}
		else if(totalPercentage > 100)
		{
			Debug.LogError("Spawnpercentage too high, lower the total to 100%");
		}
	}


	int TotalPercentage()
	{
		int percentage = 0;
		foreach(TileForList tile in tiles)
		{
			percentage += tile.spawnPercentage;
		}

		return percentage;
	}


	void SpawnCicle()
	{
		/*
		tileTimer += Time.deltaTime;
		
		float spawnSpeed = tileLength / cameraSpeed;
		*/
		if(spawnedLevelTiles.Count <= 2)
		{
			SpawnTile();
		}
		//else if(ObjectsMovingAlong[0].transform.position.z >= spawnedLevelTiles[spawnedLevelTiles.Count - 2].transform.position.z - 20)
		else if(totalLength < ObjectsMovingAlong[0].transform.position.z + 30)
		{
			SpawnTile();
		}
	}
	/*
	void SpawnCicle()
	{
		tileTimer += Time.deltaTime;

		float spawnSpeed = tileLength / cameraSpeed;

		if(tileTimer >= spawnSpeed)
		{
			SpawnTile();
			DestroyTile();
			tileTimer = 0;
		}
	}
*/
	void MoveAlong(GameObject MovingObject)
	{
		Vector3 pos = MovingObject.transform.position;
		pos.z += cameraSpeed * Time.deltaTime;
		MovingObject.transform.position = pos;

	}

	void SpawnTile()
	{

		int randomPercentage = Random.Range(1,101);

		int currentPercentage = 0;

		GameObject chosenTile = null;

		foreach(TileForList tile in tiles)
		{
			currentPercentage += tile.spawnPercentage;

			if(currentPercentage >= randomPercentage)
			{
				chosenTile = tile.Tile;
				break;
			}
		}

		//GameObject chosenTile = levelTiles[Random.Range(0, levelTiles.Count)];

		tileLength = chosenTile.GetComponent<Level_Tile_Script>().lenght;


		Vector3 tilePos = new Vector3(0, 0, totalLength);

		GameObject newTile = Instantiate(chosenTile, tilePos, transform.rotation) as GameObject;


		totalLength += tileLength;

		//tileLength = newTile.GetComponent<Level_Tile_Script>().lenght;
		currentTile += 1;
		spawnedLevelTiles.Add(newTile);
		DestroyTile();
	}

	void DestroyTile()
	{
		if(currentTile > 5)
		{
			Destroy(spawnedLevelTiles[currentTile - 6]);
			//spawnedLevelTiles.RemoveAt(currentTile - 8);
		
		}
	}
}

[System.Serializable]
public class TileForList
{
	public string name;
	public GameObject Tile;
	public int spawnPercentage;


}
