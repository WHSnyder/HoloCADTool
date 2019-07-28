using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class Ball : MonoBehaviour
{

    ManipulationHandler ballhandle;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Awake()
    {
        ballhandle = GetComponent<ManipulationHandler>();
        rb = GetComponent<Rigidbody>();


        ballhandle.OnManipulationStarted.AddListener(onGrab);
        ballhandle.OnManipulationEnded.AddListener(onDrop);
        
    }

    public void onGrab(ManipulationEventData data)
    {
        rb.useGravity = false;
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.blue);
        
    }

    public void onDrop(ManipulationEventData data)
    {
        
        rb.useGravity = true;
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.red);
    }
}
    




