
using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance; //puede ser referenciado directamente usando el nombre de clase
    public static T Instance 
    {
        get { return instance;}
    }

    protected virtual void Awake() //runea cuando se inicializa un GameObject
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
