using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES

    [Space(5)] //indica el espacio en el Inspector publico, para que quede ordenado
    [Header("Movement")] //titulo de seccion para el Inspector
    public float speed = 2; //velocidad de movimiento 

    [Space(5)]
    [Header("Settings")]
    public GameObject shopController;
    public Interactable focus;
    public static int playerInventoryCapacity = 24;
    public static int playerMaximumInventoryCapacity = 48;
    public GameObject moneyObj;
    private TextMeshProUGUI textMoneyUI;
    public Animator anim; //componente animator  

    //PRIVATES  
    private AudioPlayerManagment audioPlayer;
    private ItemDetails itemActive;
    private PlayerAction actualAction;
    private RaycastHit hit;
    private GameObject GameController;
    private GameObject Player;
    private bool canClick = true;
    private BuyItem shopScript;
    GameObject buildSystem;
    private Vector3 interactionPoint;
    private double interactionDistance = 2.5;
    string[] layerBuild = { "BuildTerrain", "PlowTerrain" };
    private bool interactionEnabled = true;

    #endregion


    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        buildSystem = GameController.transform.Find("buildingSystem").gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");      
        anim = GetComponent<Animator>();
        textMoneyUI = moneyObj.GetComponent<TextMeshProUGUI>();
        audioPlayer = GetComponent<AudioPlayerManagment>();
        textMoneyUI.text = (InventoryManager.Instance.GetMoneyInInventory()).ToString();

        InventoryManager.Instance.AddItem(InventoryLocation.player, 501);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 502);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 503);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 504);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 102);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 406);
        InventoryManager.Instance.AddItem(InventoryLocation.player, 408);

        shopScript = shopController.GetComponent<BuyItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionEnabled)
        {
            PlayerWalk();
            PlayerRun();
            InteracctionStates();
            if (SceneManager.GetActiveScene().name == "ShopScene")
            {
                checkCanBuy();
            }
            else
            {
                checkCanBuy();
                itemActive = InventoryManager.Instance.GetItemActive();
                if (itemActive != null)
                {
                    switch (itemActive.itemType)
                    {
                        case ItemType.Collecting_tool: CollectPlant(); break;
                        case ItemType.Watering_tool: StartCoroutine(WaterPlants()); break;
                        case ItemType.Seed: StartCoroutine(PlantSeeds(itemActive.itemCode)); break;
                        case ItemType.CommoditySeed: StartCoroutine(PlantSeeds(itemActive.itemCode)); break;
                        case ItemType.Commodity:
                            buildSystem.GetComponent<PlaceObject>().SetObject(itemActive.prefab);
                            buildSystem.GetComponent<PlaceObject>().SetSnape(itemActive.stnapCell);
                            BuildObjects();
                            break;
                        case ItemType.Hoeing_tool:
                            HoeingDirt();
                            break;
                        case ItemType.Breaking_tool:
                            Demolition();
                            
                            break;
                    }
                }
                if (Input.GetMouseButtonDown(1) && canClick)
                {
                    // New ray
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // If the ray hits
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        // Check if hitted object is interactable
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            SetFocus(interactable);
                        }
                    }
                }
            }
        }
    }

    public void EnableInteraction()
    {
        interactionEnabled = true;
    }

    public void DisableInteraction()
    {
        interactionEnabled = false;
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused();

            }
            focus = newFocus;
        }

        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
            focus = null;
        }
    }

    public void setCanClick(bool b)
    {
        canClick = b;
    }


    /// <summary>
    /// Establece accion del jugador actual
    /// </summary>
    /// <param name="action">tipo de accion actual</param>
    public void ChangeAction(PlayerAction action)
    {        
        actualAction = action;
    }

    public void ActualStateIdle(string s)
    {
        actualAction = PlayerAction.idle;
        anim.SetBool(s, false);
    }
    public void ActionFinish()
    {
        canClick = true;
        ChangeAction(PlayerAction.idle);
        
    }
    //PRIVATE
    
    private void PlayerWalk()
    {
        if (actualAction == PlayerAction.idle || actualAction == PlayerAction.walk)
        {
            canClick = true; //Para evitar que la maquina se quede bloqueada.
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * speed;
            transform.position += movement;

            if (movement != Vector3.zero)
            {
                ChangeAction(PlayerAction.walk);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
                anim.SetBool("walk", true);              
                audioPlayer.playWalk();
                RemoveFocus();
            }
            else
            {
                audioPlayer.stopSteps();
                anim.SetBool("walk", false);
                ChangeAction(PlayerAction.idle);
            }
        }
        else
        {  
            if(actualAction != PlayerAction.run)
                audioPlayer.stopSteps();
        }
    }
    private void PlayerRun()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * speed;
        if (actualAction == PlayerAction.walk || actualAction == PlayerAction.run)
        {
            if (Input.GetKey(KeyCode.LeftShift) && movement != Vector3.zero)
            {
                ChangeAction(PlayerAction.run);
                audioPlayer.playRun();
                anim.SetBool("run", true);
                speed = 4;

                transform.position += movement;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            }
            else
            {
             
                ChangeAction(PlayerAction.walk);
                anim.SetBool("run", false);
                speed = 2;
            }
        }
    }
    /// <summary>
    /// Logica de regar plantas
    /// </summary>
    private IEnumerator WaterPlants()
    {
        Vector3 interactionPos = selectedPos();
        if (Input.GetMouseButtonDown(0) && canClick && DistanceFromPlayer(interactionPos) < interactionDistance)
        {
            hit = GameController.GetComponent<RayCastController>().GetHit();
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Plow")
                {
                    PlowController plow = hit.collider.gameObject.GetComponent<PlowController>();
                    interactionPoint = hit.point; //Cambiar de sitio a la funciona ChangeState
                    if (plow.wateredState == false)
                    {
                        canClick = false;
                        ChangeAction(PlayerAction.watering);
                        anim.SetTrigger("tr_watering");
                        audioPlayer.playWatering();
                        yield return new WaitForSeconds(1);
                        plow.waterPlow();
                    }
                }
            }
        }
    }

    private void Demolition()
    {             
        Vector3 interactionPos = selectedPos(); //Cambiar
        
        if (Input.GetMouseButtonDown(0) && DistanceFromPlayer(interactionPos) < interactionDistance + 2 && canClick)
        {
            hit = GameController.GetComponent<RayCastController>().GetHit();
           
            if (hit.collider != null)
            {
                
                interactionPoint = hit.point; //Cambiar de sitio a la funciona ChangeState
                canClick = false;
                ChangeAction(PlayerAction.demolishing);
                GameObject o = hit.collider.gameObject;
                Comodity comodity =  o.GetComponent<Comodity>();
                if (comodity != null)
                    comodity.Demolition();

                audioPlayer.playDemolition();
                anim.SetTrigger("tr_demolition");

            }

        }

    }
    /// <summary>
    /// Logica de plantar la semilla cuyo codigo es obtenido por parametro
    /// </summary>
    private IEnumerator PlantSeeds(int codeSeed)
    {
        Vector3 interactionPos = selectedPos();
        if (Input.GetMouseButtonDown(0) && DistanceFromPlayer(interactionPos) < interactionDistance && canClick)
        {
            hit = GameController.GetComponent<RayCastController>().GetHit();
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Plow")
                {
                    canClick = false;
                    PlowController plow = hit.collider.gameObject.GetComponent<PlowController>();
                    interactionPoint = hit.point; //Cambiar de sitio a la funciona ChangeState
                    if (plow.sownState == false)
                    {
                        ChangeAction(PlayerAction.placingSeeds);
                        anim.SetTrigger("tr_plant");
                        audioPlayer.playPlantSeed();
                        yield return new WaitForSeconds(0.2f);
                        plow.sowPlow(codeSeed);
                        InventoryManager.Instance.RemoveItem(InventoryLocation.player, codeSeed);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Logica de recolectar una planta o mueble
    /// </summary>
    private void CollectPlant()
    {
        Vector3 interactionPos = selectedPos();
        if (Input.GetMouseButtonDown(0) && DistanceFromPlayer(interactionPos) < interactionDistance)
        {
            hit = GameController.GetComponent<RayCastController>().GetHit();
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Plow")
                {
                    PlowController plow = hit.collider.gameObject.GetComponent<PlowController>();
                    interactionPoint = hit.point; //Cambiar de sitio a la funciona ChangeState
                    if (plow.grownState == true)
                    {
                        audioPlayer.playCollectPlant();
                        int codePlant = plow.collectPlow();
                        InventoryManager.Instance.AddItem(InventoryLocation.player, codePlant);
                    }
                }
            }
        }
    }


    private void BuildObjects()
    {
        buildSystem.GetComponent<PlaceObject>().setIsHogingTool(false);
        buildSystem.GetComponent<PlaceObject>().SetObject(itemActive.prefab);
        buildSystem.GetComponent<PlaceObject>().SetSnape(itemActive.stnapCell);
        buildSystem.GetComponent<PlaceObject>().Logica();

        if (Input.GetMouseButtonDown(0))
        {
            GameObject I = buildSystem.GetComponent<PlaceObject>().Build();
            if (I != null) //Objeto construido con exito
            {
                audioPlayer.playBuild();
                interactionPoint = buildSystem.GetComponent<PlaceObject>().GetPos(); //Cambiar de sitio a la funciona ChangeState
                ChangeAction(PlayerAction.building);
                InventoryManager.Instance.RemoveItem(InventoryLocation.player, itemActive.itemCode);
                anim.SetTrigger("tr_build");
            }
        }
    }

    /// <summary>
    /// Logica de arar el suelo.
    /// </summary>
    private void HoeingDirt()
    {
        buildSystem.GetComponent<PlaceObject>().setIsHogingTool(true);
        buildSystem.GetComponent<PlaceObject>().SetObject(itemActive.prefab);
        buildSystem.GetComponent<PlaceObject>().SetSnape(itemActive.stnapCell);
        buildSystem.GetComponent<PlaceObject>().Logica();

        if (Input.GetMouseButtonDown(0) && buildSystem.GetComponent<PlaceObject>().CheckBuild() && canClick)
        {
            canClick = false;
            ChangeAction(PlayerAction.hoeing);
            anim.SetTrigger("tr_plow");
            interactionPoint = buildSystem.GetComponent<PlaceObject>().GetPos();
            audioPlayer.playHoging();
            buildSystem.GetComponent<PlaceObject>().SetRotation(0f);
            StartCoroutine(buildSystem.GetComponent<PlaceObject>().BuildDelay(0.6f));
        }
    }

    private void checkCanBuy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit = GameController.GetComponent<RayCastController>().GetHit();
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Item")
                {

                    Item item = hit.collider.gameObject.GetComponent<Item>();
                    if (item != null)
                    {
                        audioPlayer.playBuy();
                        shopScript.BuyItemSelected(item);
                    }
                }
            }
        }
    }

    private void InteracctionStates()
    {
        switch (actualAction)
        {
            case PlayerAction.hoeing:
                RotationToActionPoint();
                break;
            case PlayerAction.placingSeeds:
                RotationToActionPoint();
                break;
            case PlayerAction.watering:
                RotationToActionPoint();
                break;
            case PlayerAction.building:
                RotationToActionPoint();
                break;
            case PlayerAction.demolishing:
                RotationToActionPoint();
                break;
        }
    }

    private void RotationToActionPoint()
    {
        var lookPos = interactionPoint - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }

    /// <summary>
    /// Devuelve la distancia entre la posicion de jugador y posCompare
    /// </summary>
    private float DistanceFromPlayer(Vector3 posCompare)
    {
        return Vector3.Distance(Player.transform.position, posCompare);
    }

    /// <summary>
    /// Devuelve la posicion clickada
    /// </summary>
    private Vector3 selectedPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(layerBuild));
        return new Vector3(hit.point.x, hit.point.y, hit.point.z);
    }
}






