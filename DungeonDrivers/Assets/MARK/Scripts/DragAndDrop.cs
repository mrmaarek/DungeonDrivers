using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	//Creating a 
	public Transform returnToParent = null;

	// Making a placeholder so that we can sort our cards directly where we want to have them.
	GameObject placeHolder = null;
	public Transform placeHolderParent = null;

	//Debug.Log("Start of the Drag");
	public void OnBeginDrag(PointerEventData dragData)
	{
		// Make a new gameobject for the placeholder
		placeHolder = new GameObject ();
		// Hold the transform of his parent.
		placeHolder.transform.SetParent (this.transform.parent);
		// Add a layour element to our placeholder.
		LayoutElement le = placeHolder.AddComponent<LayoutElement> ();
		// Set the width  and height to be the same as the other cards.
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeHolder.transform.SetSiblingIndex (this.transform.GetSiblingIndex());

		//Hold the position of the first parent (Hand)
		returnToParent = this.transform.parent;

		placeHolderParent = returnToParent;

		this.transform.SetParent (this.transform.parent.parent);

		// Ignore the raycasts so that we can drop it 
		// in any location we want.
		this.GetComponent<CanvasGroup> ().blocksRaycasts = false;

	}

	public void OnDrag(PointerEventData dragData)
	{
		//Debug.Log ("While dragging");
		this.transform.position = dragData.position;
		if (placeHolder.transform.parent != placeHolderParent) 
		{
			placeHolder.transform.SetParent(placeHolderParent);
		}

		Debug.Log (this.transform.gameObject); //LayOut

		//While we are dragging the object, give the card some alpha values.
		Image cardImage = this.GetComponent<Image>();
		cardImage.color = new Color (1f, 1f, 1f, 0.5f);

		int newSiblingIndex = placeHolderParent.childCount;
		//If the card.x that we are dragging is < the card next to it.


		for (int i = 0; i < placeHolderParent.childCount; i++) 
		{
			if (this.transform.position.x < placeHolderParent.GetChild(i).position.x)
			{
				newSiblingIndex = i;

				if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
				{
					newSiblingIndex --;
				}
				break;
			}
		}

		placeHolder.transform.SetSiblingIndex (newSiblingIndex);

	}

	public void OnEndDrag(PointerEventData dragData)
	{
		//Debug.Log ("End of the Drag");
		//Set the card back to where it came from.
		this.transform.SetParent (returnToParent);
		this.transform.SetSiblingIndex (placeHolder.transform.GetSiblingIndex());

		//Reset to the normal color.
		Image cardImage = this.GetComponent<Image>();
		cardImage.color = new Color (1f, 1f, 1f, 1f);

		//When we stop dragging turn the raycasts back on!
		this.GetComponent<CanvasGroup> ().blocksRaycasts = true;

		//Destroy the placeholder to avoid whitespacing.
		Destroy (placeHolder);
	}

}