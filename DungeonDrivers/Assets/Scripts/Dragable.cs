using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // At the start of the drag
    public void OnBeginDrag(PointerEventData mousePointerData)
    {

    }

    // While dragging
    public void OnDrag(PointerEventData mousePointerData)
    {
        this.transform.position = mousePointerData.position;
    }

    // When you release the drag
    public void OnEndDrag(PointerEventData mousePointerData)
    {

    }
}
