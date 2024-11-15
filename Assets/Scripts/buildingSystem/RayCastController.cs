using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    //Private  
    int layer_mask = 0;
    RaycastHit hit;

    //variables publicas
    bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        layer_mask = LayerMask.GetMask("SelectableObject");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask); //Lanza un rayCast que colisiona con el suelo
       
    }


    void OnDrawGizmos()
    {    
        if(debug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

    /// <summary>
    ///Devuelve el Hit del rayo.
    /// </summary>
    public RaycastHit GetHit()
    {
        return hit;
    }

    /// <summary>
    ///Devuelve la posicion del objeto impactado.
    /// </summary>
    public Vector3 GetPos()
    {
        return hit.point;
    }

    /// <summary>
    ///Devuelve el gameObject impactado.
    /// </summary>
    public GameObject GetObject()
    {
        return hit.collider.gameObject;
    }


}
