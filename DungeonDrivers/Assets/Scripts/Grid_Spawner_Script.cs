using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid_Spawner_Script : MonoBehaviour 
{

	public GameObject GridBlock, GridObject;
	public List<GameObject> gridBlocks, selectedGridBlocks = new List<GameObject>();
	public float xTiles, yTiles;
	public float tileSize;
	public int xLane, yLane;

	public GameObject CameraToUse;
	public LayerMask layerMask;


	public void SetupGrid()
	{
		float realXTiles = xTiles * tileSize;
		float realYTiles = yTiles * tileSize;
		
		//GridObject.transform.localPosition = new Vector3(-realXTiles / 2 + tileSize + 0.75f / 2, GridObject.transform.localPosition.y, realYTiles / 2);
		
		int gridID = 0;

		for(float i = (realYTiles-1)/2; i > -realYTiles/2; i -= tileSize)
		{
			yLane++;
			xLane = 0;
			for(float j = (-realXTiles+1)/2; j < realXTiles/2; j += tileSize)
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

	public void DestroyGrid()
	{
		foreach(GameObject gB in gridBlocks)
		{
			Destroy(gB);
		}
		gridBlocks.Clear();
	}

	public void SelectGridBlock(Vector3 gridBlockPos)
	{
		RaycastHit vHit = new RaycastHit();
		
		if(Physics.Linecast(CameraToUse.transform.position, gridBlockPos, out vHit, layerMask))
		{
			ActivateGridBlock(vHit.collider.gameObject);
		}
	}
	
	void ActivateGridBlock(GameObject GridBlockSelected)
	{
		GameObject HitGridBlock = GridBlockSelected;
		Grid_Block_Script Temp_Grid_Block_Script = HitGridBlock.GetComponent<Grid_Block_Script>();
		Temp_Grid_Block_Script.isActive = true;
	}
	
	public void SelectWithMouse()
	{
		RaycastHit vHit = new RaycastHit();
		
		Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(vRay, out vHit, 1000, layerMask)) 
		{
			SelectGridBlockWithMouse(vHit.collider.gameObject);
		}
	}
	
	void SelectGridBlockWithMouse(GameObject gridBlockSelected)
	{
		GameObject HitGridBlock = gridBlockSelected;
		Grid_Block_Script Temp_Grid_Block_Script = HitGridBlock.GetComponent<Grid_Block_Script>();
		if(Input.GetMouseButtonUp(0))
		{
			if(Temp_Grid_Block_Script.isSelected)
			{
				Temp_Grid_Block_Script.isSelected = false;
				for(int i = selectedGridBlocks.Count - 1; i > 0; i--)
				{
					if(selectedGridBlocks[i] == gridBlockSelected)
					{
						selectedGridBlocks.RemoveAt(i);
					}
				}
			}
			else
			{
				Temp_Grid_Block_Script.isSelected = true;
				selectedGridBlocks.Add(gridBlockSelected);
			}
		}
	}

	void DeactivateAllGridBlocks()
	{
		for(int i = 0; i < gridBlocks.Count; i++)
		{
			gridBlocks[i].GetComponent<Grid_Block_Script>().isActive = false;
		}
	}
	void DeselectAllGridBlocks()
	{
		for(int i = 0; i < gridBlocks.Count; i++)
		{
			gridBlocks[i].GetComponent<Grid_Block_Script>().isSelected = false;
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










