using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    void Update()
    {
       
    }

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

    public void OnPointerEnter(PointerEventData mousePointerdata)
    {
        //mousePointerdata.
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        this.transform.localScale = new Vector3(2, 2, 2);
        Debug.Log("Entered ..." + this.gameObject.name );

        Canvas myCanvas = this.GetComponentInChildren<Canvas>();
        myCanvas.transform.localPosition = new Vector3(-100, 100, 0);

    }

    public void OnPointerExit(PointerEventData mousePointerdata)
    {
        //Reset

        this.transform.localScale = new Vector3(1,1,1);
        Canvas myCanvas = this.GetComponentInChildren<Canvas>();
        myCanvas.transform.localPosition = new Vector3(0, 0, 0);

        Debug.Log(this.gameObject.name + "Exit...");
    }
}
