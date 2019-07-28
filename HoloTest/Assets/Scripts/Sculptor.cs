using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class Sculptor : MonoBehaviour
{

    ManipulationHandler ballhandle;
    Rigidbody rb;

    Sculptable rock;


    // Start is called before the first frame update
    void Awake()
    {
        ballhandle = GetComponent<ManipulationHandler>();
        rb = GetComponent<Rigidbody>();


        ballhandle.OnManipulationStarted.AddListener(onGrab);
        ballhandle.OnManipulationEnded.AddListener(onDrop);

    }

    private void Start()
    {
        rock = GameObject.Find("Rock").GetComponent<Sculptable>();
    }

    public void onGrab(ManipulationEventData data)
    {
        rock.track = true;
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.blue);

    }

    public void onDrop(ManipulationEventData data)
    {
        rock.track = false;
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.red);
    }
}





