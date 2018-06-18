using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingCell {

    private Transform vox;

    public Panel[] sides = new Panel[6];
    private Vector3 origin;
    private bool[] layout = { false, false, false, false, false, false }; // a array 
    private int size;
    // Use this for initialization

    public GameObject cell = new GameObject("Cell");
    
    
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public buildingCell(Transform vox, Vector3 origin, int size)
    {

        this.vox = vox;
        this.origin = origin;
        this.size = size;

    }
    public void updateCell(bool[] newLayout)
    {

        layout = newLayout;
        buildCell();

    }


    public void makeUninstantiatedCell() // creates the 6 empty panels  with their appropriate orientations
    {
        for ( int i =  0; i < 6; i++)
        {
            Vector3 offset;
          

            switch (i)
            {

                case 0: //bottom y-
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

        }

    }

    public void buildCell()
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
