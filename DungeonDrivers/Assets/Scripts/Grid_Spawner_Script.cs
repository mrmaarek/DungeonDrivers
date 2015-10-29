using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid_Spawner_Script : MonoBehaviour 
{

	public GameObject GridBlock, GridObject;
	public List<GameObject> gridBlocks = new List<GameObject>();
	public float xTiles, yTiles;
	public float tileSize;
	public int xLane, yLane;
	
	public void SetupGrid()
	{
		float realXTiles = xTiles * tileSize;
		float realYTiles = yTiles * tileSize;
		
		GridObject.transform.localPosition = new Vector3(-realXTiles / 2 + tileSize + 0.75f / 2, GridObject.transform.localPosition.y, realYTiles / 2);
		
		int gridID = 0;

		for(float i = 0; i > -realYTiles; i -= tileSize)
		{
			yLane++;
			xLane = 0;
			for(float j = 0; j < realXTiles; j += tileSize)
			{
				xLane++;
				gridID++;
				SpawnGridBlock(j,i, gridID, xLane, yLane);
			}
		}
	}

	void SpawnGridBlock(float xPos, float zPos, int gridId, int xL, int yL)
	{
		GameObject newBlock = Instantiate(GridBlock);
		newBlock.transform.parent = GridObject.transform;
		newBlock.name = newBlock.name + gridId;
		newBlock.transform.localPosition = new Vector3(xPos, zPos, 0);
		newBlock.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
		newBlock.GetComponent<Grid_Block_Script>().xLane = xL;
		newBlock.GetComponent<Grid_Block_Script>().yLane = yL;
		gridBlocks.Add(newBlock);
	}

	public void LaneEvent1(int inNTurns, int laneXN)
	{
		foreach(GameObject laneBlock in gridBlocks)
		{
			if(laneBlock.GetComponent<Grid_Block_Script>().xLane == laneXN)
			{

			}
		}
	}
}

[System.Serializable]
public class LaneEvent
{
	public int laneEventId;
	public int turnsLeft;
	public int damage;
	public int xLane;

	public List<GameObject> laneWarnings = new List<GameObject>();

	public LaneEvent(int lEventId, int tLeft, int tDamage, int txLane)
	{
		laneEventId = lEventId;
		turnsLeft = tLeft;
		damage = tDamage;
		xLane = txLane;
	}
}










