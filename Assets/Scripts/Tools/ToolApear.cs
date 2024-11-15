using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolApear : MonoBehaviour
{ 
    public Vector3 startScale;
    public Vector3 scaleIncrement;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = startScale;
    }

    private void OnEnable()
    {
        transform.localScale = startScale;
    }

    // Update is called once per frame
    void Update()
    {
       if(transform.localScale.x < 1)
        {
            transform.localScale += scaleIncrement;
        }
    }
}
