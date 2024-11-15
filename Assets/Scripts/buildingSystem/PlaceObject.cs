using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    //Variables privadas
    private RaycastHit hit;
    Vector3 buildPos;
    Vector3 posCheckObject;
    GameObject prewiew;
    GameObject GameController;
    CellWorld CellWorld;
    GameObject Player;
    Vector3 rotation;
    SnapCell snapBuild = SnapCell.center;
    ItemDetails itemActive;
    bool hogingTool = false;
    string[] layerBuild = { "BuildTerrain", "PlowTerrain" };
    int layer_Plow = 0;
    //Variables Publicas  
    public float buildDistance;
    public bool debugMode = false;
    public GameObject buildObject;
    public GameObject BuildParent;
    public Material matPrewiewOk;
    public Material matPrewiewErr;

    //ExtracosasPara comprovar funcionamiento
    private int itemAnt;
    public GameObject Inventory;
    public InventoryManager IM;
    public List<GameObject> construibles;

    //TO DO: Que el objeto Checkbuild se redimensione segun el objeto el qual se vaya a construir.

    // Start is called before the first frame update
    void Start()
    {
        SetSnape(SnapCell.center);
       
        GameController = GameObject.FindGameObjectWithTag("GameController");
        Player = GameObject.FindGameObjectWithTag("Player");
        CellWorld = GameController.GetComponent<CellWorld>();        
        layer_Plow = LayerMask.GetMask("PlowTerrain");
    }

    // Update is called once per frame
    void Update()
    {
        itemActive = InventoryManager.Instance.GetItemActive();
        DestructionPrewiew();
    }

    //Privado   

    void DestructionPrewiew()
    {
        if (prewiew != null)
        {
            if (itemActive == null || itemActive.itemType != ItemType.Commodity && itemActive.itemType != ItemType.Hoeing_tool)
            { 
                Destroy(prewiew);
            }           
            if(hit.collider == null)
            {
                Destroy(prewiew);
            }
        }

       
    }

 

    /// <summary>
    /// Comprueva si necesita canviar el tipo de snipe y lo cambia en el caso de que sea necesario.
    /// </summary>
    void checkSnipeType()
    {
        if (snapBuild == SnapCell.edgH || snapBuild == SnapCell.edgV)
        {
           snapBuild = ((rotation.y / 90) % 2 == 0) ? SnapCell.edgH : SnapCell.edgV;
            
        }
        
    }

    void OnDrawGizmos()
    {
        if(debugMode)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(buildPos, 0.1f);                
        }
       
    }

    /// <summary>
    ///  Transforma El objeto a un objeto de Prewiew.
    /// </summary>
    GameObject ToPreviewObject(GameObject obj)
    {
        // GameObject objParent = Instantiate(BuildParent, buildPos, Quaternion.identity);
        obj.layer = 0;
        obj.GetComponent<BoxCollider>().isTrigger = true;
        obj.AddComponent<BuildingObject>();
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().isKinematic = true;
        Bed bed = obj.GetComponent<Bed>();
        if (bed != null)
        {
            Destroy(bed);
        }
        // obj.transform.parent = objParent.transform;

        return obj;
    }
    
    float Distance()
    {             
        return Vector3.Distance(Player.transform.position, buildPos); ;
    }
    
    /// <summary>
    /// estableze el snap pasado por parametros.
    /// </summary>
    public void SetSnape(SnapCell snap)
    {
        snapBuild = snap;
        checkSnipeType();
    }

    //Publico
    public void Logica()
    {


        //Calcula la posicion donde vamos a construir.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
       if(hogingTool)
        {       
            Physics.Raycast(ray, out hit, Mathf.Infinity, layer_Plow);
            //Debug.Log("hol.a");
        }
        else
        {          
           
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(layerBuild));
        }

      
        float x = hit.point.x;
        float z = hit.point.z;
        CellWorld.ToCellCoord(ref x, ref z, snapBuild);
        buildPos = new Vector3(x, hit.point.y, z);
        
        if(hit.collider != null)
        {
            if (prewiew == null)//Crea una instancia de prewiew.
            {
                if (buildObject != null)
                {
                    prewiew = Instantiate(buildObject, buildPos, Quaternion.identity);
                    prewiew = ToPreviewObject(prewiew);
                }
            }
            else //Logica de la instancia prewiew.
            {

                prewiew.transform.position = buildPos;

                if (Input.GetMouseButtonDown(1))
                {
                    Rotate();
                    checkSnipeType();
                };

                prewiew.transform.rotation = Quaternion.Euler(rotation);

                if (CheckBuild())
                {
                    setMaterialOk(prewiew);
                }
                else
                {
                    setMaterialErr(prewiew);
                }

            }
        }

      
    }

    /// <summary>
    ///  Devuelve la posicion donde se va a construir.
    /// </summary>
    public Vector3 GetPos()
    {
        return buildPos;
    }

    /// <summary>
    ///  Devuelve True si el objeto es construible.
    /// </summary>
    public bool CheckBuild()
    {       
        return prewiew == null ? false : (prewiew.GetComponent<BuildingObject>().IsBuildeable() && Distance()<= buildDistance);
    }

    /// <summary>
    ///  El objeto prefab es el objeto que queremos construir.
    /// </summary>
    public void SetObject(GameObject prefab)
    {
        if(prefab != buildObject)
        {          
            Destroy(prewiew);
        }
       
        buildObject = prefab;
    }

    /// <summary>
    ///  Construye el objeto, si falla develve null, sino la instancia.
    /// </summary>
    public GameObject Build()
    {
        if (CheckBuild())
        {            
            return Instantiate(buildObject, buildPos, Quaternion.Euler(rotation) ) ; 
        }
        return null;
    }
    /// <summary>
    ///  Construye el objeto con un delay en segundos.
    /// </summary>
    public IEnumerator BuildDelay(float delay)
    {
        
        if (CheckBuild())
        {
            Vector3 auxBuildPos = buildPos;
            yield return new WaitForSeconds(delay);
            SetRotation(0f); //Cutrada puesta para la demo
            Instantiate(buildObject, auxBuildPos, Quaternion.Euler(rotation));
        }
      
    }

    /// <summary>
    /// Rota el obteto 90 grados en el eje y.
    /// </summary>
    public void Rotate()
    {
        rotation.y += 90;  
    }

    public void SetRotation(float degrees )
    {
        rotation.y = degrees;
    }

    /// <summary>
    /// Coloca El material de posible construir en el objeto obj.
    /// </summary>    
    public void setMaterialOk(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material = matPrewiewOk;
    }

    /// <summary>
    /// Coloca El material de imposible construir en el objeto obj.
    /// </summary>   
    public void setMaterialErr(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material = matPrewiewErr;
    }

    public void setIsHogingTool(bool b)
    {
        hogingTool = b;
    }

      

   

}
