using UnityEngine;
using Random = UnityEngine.Random;

public class PlowController : MonoBehaviour
{
    [Header("OBJECTS")]
    public GameObject seeds;
    public GameObject plantState1;
    public GameObject plantState2;
    public GameObject plantState3;
    public Material watered;
    public Material notWatered;

    private const int MAX_GROWN = 3; //maximo de crecimiento de la planta
    private const int MAX_VEGETABLE_SEED = 199; //maximo codigo de semilla de una verdura

    private bool isSown = false; //sembrado
    private bool isWatered = false; //regado
    private bool isGrown = false; //listo para recoger
    private int levelOfGrown = 0; //describe cuanto ha crecido la planta
    private int seedCode = 0; //codigo de lo plantado
    private int resultCode = 0; //codigo del objeto a recoger

    //raycast
    private Vector3 plowCoords; 
    

    public bool grownState //crecida para recoger
    {
        get { return isGrown; }
        set { isGrown = value; }
    }
    public bool wateredState //regar
    {
        get { return isWatered; }
        set { isWatered = value; }
    }
    public bool sownState //semillitas
    {
        get { return isSown; }
        set { isSown = value; }
    }

    private void Start()
    {
        TimeManager.OnDayChanged += DayCheck;
    }

    /// <summary>
    /// Función para colocar semillas en la casilla si es posible
    /// </summary>
    public void sowPlow(int code)
    {
        seedCode = code;
        isSown = true;
        Instantiate(seeds,gameObject.transform);
    }

    /// <summary>
    /// Función para regar la casilla si es posible 
    /// </summary>
    public void waterPlow()
    {
        isWatered = true;
        gameObject.GetComponentInChildren<MeshRenderer>().material = watered;            
    }

    /// <summary>
    /// Función para recolectar el fruto de la casilla si es posible
    /// </summary>
    public int collectPlow()
    {
        isSown = false;
        isGrown = false;
        levelOfGrown = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        return resultCode;
    }

    /// <summary>
    /// Aumenta el nivel de crecido de la planta
    /// </summary>
    private void growPlant()
    {
        if (!isGrown && isSown)
        {
            if (isWatered)
            {
                isWatered = false;
                gameObject.GetComponentInChildren<MeshRenderer>().material = notWatered;
                levelOfGrown++;
                if (levelOfGrown == MAX_GROWN) //ha crecido del todo
                {
                    isGrown = true;
                    resultCode = seedCode + 100;
                    if (seedCode > MAX_VEGETABLE_SEED) //no es verdura, hay que escoger variante aleatoria del mueble
                    {
                        int variant = Random.Range(0, 4);
                        resultCode += variant; 
                    }
                }
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                switch (levelOfGrown)
                {
                    case 1: Instantiate(plantState1, gameObject.transform); break;
                    case 2: Instantiate(plantState2, gameObject.transform); break;
                    case 3: Instantiate(plantState3, gameObject.transform); break;
                }
            }
        }
    }

    /// <summary>
    /// Al pasar las doce de la noche, la planta crece
    /// </summary>
    private void DayCheck()
    {
        growPlant();
        if (isWatered && isSown)
        {
            isWatered = false;
            gameObject.GetComponentInChildren<MeshRenderer>().material = notWatered;
        }
    }
}
