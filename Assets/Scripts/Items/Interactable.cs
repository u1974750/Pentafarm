using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    protected bool isFocus = false;
    protected Transform playerTransform;
    protected bool hasInteracted = false;
    protected bool canInteract = true;

    public virtual void Interact ()
    {
        // This method is meant to be overwitten
        Debug.Log("Interacting with " + transform.name);
    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            if (distance <= radius && canInteract)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused (Transform playerTransform)
    {
        isFocus = true;
        this.playerTransform = playerTransform;
        hasInteracted = false;
    }
    
    public void OnDefocused ()
    {
        isFocus = false;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void EnableInteraction()
    {
        canInteract = true;
    }

    public void DisableInteraction()
    {
        canInteract = false;
    }
}
