/*
 * Created by Brian Blalock
 * 
 * 
 * Mainly just a way to group panels together at one location and update them at once.
 * 
 * 
 * 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

    private Transform vox;

    public Panel[] sides = new Panel[6];

    // colors for the building's color pallette *** not yet implemented ***
    public Color color1;
    public Color color2;

    private bool[] layout = { false, false, false, false, false, false }; // a array 
    private int size;
    // Use this for initialization

    public GameObject cell = new GameObject("Cell");
    

    public Cell(Transform vox,  int size) // asses in the voxel prefab and size of the panels
    {

        this.vox = vox;
        this.size = size;

    }
    public void updateCell(bool[] newLayout)    //creates panels based on new layout
    {

        layout = newLayout;
        buildCell();

    }


    public void makeUninstantiatedCell() // creates the 6 empty panels  with their appropriate orientations
    {
        for ( int i =  0; i < 6; i++)    // creates empty panels in their appropriate places and orientations
        {
            Vector3 offset;
          

            switch (i)
            {

                case 0: //bottom ( floor panel )
                    sides[i] = new Panel(Quaternion.identity, vox, size);   

                    break;

                case 1:// x-
                    sides[i] = new Panel( Quaternion.Euler(0, 0, 90), vox, size);
                    break;

                case 2://z-
                    sides[i] = new Panel( Quaternion.Euler(-90, 0, 0), vox, size);
                    break;

                case 3://x+
                    offset = new Vector3( (size -1) / 4f, (size - 1) / 4f, 0f);
                    sides[i] = new Panel( Quaternion.Euler(0, 0, -90), vox, size);
                    sides[i].panel.transform.localPosition = offset;
                    break;

                case 4://z+
                    offset = new Vector3(0f, (size - 1) / 4f, (size - 1) / 4f);
                    sides[i] = new Panel( Quaternion.Euler(90, 0, 0), vox, size);
                    sides[i].panel.transform.localPosition = offset;
                    break;

                case 5://y+

                    offset = new Vector3(0f, (size - 1) / 4f, 0f);
                    sides[i] = new Panel(Quaternion.identity, vox, size);
                    sides[i].panel.transform.localPosition = offset;
                    break;

            }

            sides[i].panel.transform.SetParent(cell.transform);
            sides[i].color1 = color1;
            sides[i].color2 = color1;

        }

    }

    public void buildCell()             // Just loops through the panels and has them be built if needed
    {

       
        for(int i  = 0; i < 6; i++)
        {

            if(layout[i])
            {

                sides[i].buildPanel();

            }

        }


    }

}
