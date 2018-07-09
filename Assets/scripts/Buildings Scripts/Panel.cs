/*
written by Brian Blalock

This class is a handler for creating holding a flat panel of voxels 



*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel  {

    private Transform vox;

    private Quaternion orientation;
    private Vector3 origin;
    
    public Color color1;
    public Color color2;
    private int size; // panel size

    public GameObject panel = new GameObject("Panel");

    public Transform[,] panelVoxs; 



    public Panel(Quaternion orientation, Transform vox, int size)
    {

        this.orientation = orientation;
        panel.transform.localRotation = orientation;
 
        this.vox = vox;
        this.size = size;

    }

    public void buildPanel()                                // constructs the flat plane of voxels
    {
        

        panelVoxs = new Transform[size, size];
        for (int x = 0; x < size; x++)
        {

           for ( int z = 0; z < size; z++)
            {

                Transform tempVoxel = vox;
                
                panelVoxs[x,z] = Object.Instantiate(tempVoxel , panel.transform);
                panelVoxs[x, z].transform.localPosition =  new Vector3(x / 4f, 0f, z / 4f);
                panelVoxs[x,z].GetComponent<Renderer>().material.color = Color32.Lerp(color1, color2, Random.Range(0.0f, 1.0f));  // get a color in the range

            }

        }

     //   Object.Instantiate(panel, origin, orientation);

    }
}
