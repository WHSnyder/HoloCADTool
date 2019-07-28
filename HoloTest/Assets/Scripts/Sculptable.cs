using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

using System.Linq;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;


public class Sculptable : MonoBehaviour
{

    ManipulationHandler handler;


    GameObject brush = null;
    MeshFilter filter;
    Mesh mesh;

    Vector3[] verts;
    int[] tris;
    Vector2[] uvs;

    public float range = .005f;


    public bool track = false;

    Vector3 handpos;

    GameObject[] trackers;

    GameObject tracker;

    public Transform tracked;
    public Vector3 curtracked, prevtracked;





    // Start is called before the first frame update
    void Start()
    {
       // brush = GameObject.Find("Sculptor");
        filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;

        verts = mesh.vertices;
        tris = mesh.triangles;
        uvs = mesh.uv;


        tracker = GameObject.Find("v");

        trackers = new GameObject[mesh.vertices.Length];

        for (int i = 0; i < trackers.Length; i++)
        {
            GameObject newt = Instantiate(tracker, transform.TransformPoint(verts[i]), Quaternion.identity);
            newt.SetActive(true);

            newt.transform.SetParent(this.transform);

            trackers[i] = newt;
        }

        tracker.SetActive(false);
    }







    // Update is called once per frame
    void Update()
    {

        if (!track || tracked == null)
        {
            return;
        }

        curtracked = tracked.position;

        verts = new Vector3[mesh.vertices.Length];
        tris = mesh.triangles;
        uvs = mesh.uv;

        Vector3 cur, cur_wrld ,diff;
        float coef;

        diff = curtracked - prevtracked;

        for (int i = 0; i < verts.Length; i++)
        {
            cur = mesh.vertices[i];
            cur_wrld = transform.TransformPoint(cur);

            coef = 1 - Mathf.Clamp(Vector3.Magnitude(cur_wrld - curtracked)/2,0,range);


            if (coef < 1 && coef > 0)
            {
                verts[i] = transform.InverseTransformPoint(cur_wrld + coef * diff);
                trackers[i].transform.position = transform.TransformPoint(verts[i]);
            }
            else
            {
                verts[i] = transform.InverseTransformPoint(  transform.TransformPoint(mesh.vertices[i]) );
            }
        }

        mesh.vertices = verts;
        prevtracked = curtracked;
    }




}
