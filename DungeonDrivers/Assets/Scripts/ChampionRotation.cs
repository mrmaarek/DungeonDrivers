using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ChampionRotation : MonoBehaviour
{
    [SerializeField]
    //private GameObject Kaylessa, Grimmet;

	// Update is called once per frame
	void Update ()
    {

        Debug.DrawLine(Input.mousePosition, Vector3.forward, Color.red, 1000);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
        }


        //Mouse pressed
        if (Input.GetMouseButton(0))
        {

            print(gameObject.name);
            if (gameObject.tag == "ChampKaylessa")
            {
                //Rotate(Kaylessa);
            }
            if (gameObject.tag == "ChampGrimmet")
            {
                //Rotate(Grimmet);
            }
            
        }
    }

    void Rotate(GameObject name)
    {
        name.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2);
    }
}
