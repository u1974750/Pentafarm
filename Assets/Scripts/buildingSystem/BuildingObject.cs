using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{

    //Variables privadas
    int obstaculos = 0;
    public Material matPrewiewOk;
    public Material matPrewiewErr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {  
      
        if(!other.CompareTag("Floor"))
        {            
            obstaculos++;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Floor"))
        {                 
            obstaculos--;       
        }
        
    }

    /// <summary>
    /// Devuelve true si el objeto no esta obstruido.
    /// </summary>
    public bool IsBuildeable()
    {
       
        return obstaculos == 0;
    }

   

}


