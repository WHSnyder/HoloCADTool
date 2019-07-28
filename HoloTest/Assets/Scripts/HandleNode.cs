using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class HandleNode : MonoBehaviour
{

    ManipulationHandler handle = null;

    public static Vector3 source;

    Sculptable sculpty;
    Transform track;

    private Vector3 lastpos;

    private void Awake()
    {
        handle = GetComponent<ManipulationHandler>();

        handle.OnManipulationStarted.AddListener( onDrag );
        handle.OnManipulationEnded.AddListener( onDrop );

        sculpty = GameObject.Find("Rock").GetComponent<Sculptable>();
        
    }

    public void onDrag(ManipulationEventData data)
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        sculpty.prevtracked = lastpos;
        sculpty.tracked = this.transform;
        sculpty.track = true;
    }

    public void onDrop(ManipulationEventData data)
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        sculpty.track = false;
        sculpty.tracked = null;
    }

    public void Update()
    {
        lastpos = this.transform.position;
    }
}
