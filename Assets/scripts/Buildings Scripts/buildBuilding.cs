using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildBuilding : MonoBehaviour {

    public Transform Voxel;
    public Color32 color1;
    public Color32 color2;

    public int maxX;
    public int maxZ;
    public int maxY;

    public int size;

    

    private int[,,] structure;
    // used for initial creation of the LAyout
    // [X,Z,Y] XZY -> Cell location 

    private buildingCell[,,] cells; // xzy

    void Start () {
       
        constructBuilding();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public buildBuilding(int x, int y, int z)
    {

        maxX = x;
        maxY = y;
        maxZ = z;

    }

    public void constructBuilding()
    {

        planBuilding();
        fillCells();
        constructPanels();

    }

    private void constructPanels()
    {

        for (int x = 0; x < maxX; x++)
        {

            for (int z = 0; z < maxZ; z++)
            {

                for (int y = 0; y < maxY; y++)
                {
                    bool[] sides = { false, false, false, false, false, false };
                    //top
                    string print = "";
                   if (structure[x, z, y] == 1) { 
                        if (z == 0 || structure[x, z - 1, y] == 0)
                            sides[2] = true;
                        if (x == 0 || structure[x - 1, z, y] == 0)
                            sides[1] = true;
                        if (z == maxZ - 1 || structure[x, z + 1, y] == 0)
                            sides[4] = true;
                        if (x == maxX - 1 || structure[x + 1, z, y] == 0)
                            sides[3] = true;
                        if (y == maxY - 1 || structure[x, z, y + 1] == 0)
                            sides[5] = true;
                  }
                    if (y == 0 && !sides[1] && !sides[2] && !sides[3] && !sides[4] && !sides[5])
                        sides[0] = true;
                    
                    for( int i = 0; i < 6; i++)
                    {

                       print += (sides[i] ? "True ": "False " );

                    }
                    Debug.Log(print);

                    cells[x, z,y ].updateCell(sides);
                }
            }
        }
    }
                

    private void fillCells()
    {
        cells = new buildingCell[maxX, maxZ, maxY];


        for ( int x = 0; x < maxX; x++)
        {
            
            for ( int z = 0; z < maxZ; z++)
            {

                for ( int y = 0; y< maxY; y++)
                {

                    Vector3 cellPosition =  new Vector3(x * (size/4f) , y * (size / 4f), z * (size / 4f));
                    cells[x, z, y] = new buildingCell( Voxel, cellPosition, size );
                    cells[x, z, y].makeUninstantiatedCell();
                    cells[x, z, y].cell.transform.SetParent(gameObject.transform);
                    cells[x, z, y].cell.transform.localPosition = cellPosition;

                }
         
            }
        }

    }

    private void planBuilding()
    {
        structure = new int[maxX, maxZ, maxY];

        int maxLayerSize = maxX * maxZ;



        int layerSize = maxLayerSize - Random.Range(0, (maxLayerSize/2) );
        int lastLayerSize = layerSize;

        Debug.Log(layerSize.ToString());
        // only for the first floor
        for (int l = 0 ; l < layerSize; l++)
        {
           // Debug.Log(l.ToString());
            bool placed = false;

            while (!placed)
            {
               // Debug.Log("in while");
                int pickZ = Random.Range(0, maxZ);
                for ( int x = 0; x < maxX; x++)
                {

                    if (structure[x, pickZ, 0 ] == 0)
                    {

                        structure[x, pickZ, 0] = 1;
                        
                        placed = true;
                       
                        break;
                    }
                }       
            }
        }


        for ( int y = 1 ; y < maxY; y++)
        {
            layerSize = maxLayerSize - Random.Range(0, (maxLayerSize / 2)) - (maxLayerSize - lastLayerSize) ;
            lastLayerSize = layerSize;

            for (int l = 0; l < layerSize; l++)
            {

                bool placed = false;

                while (!placed)
                {

                    int pickZ = Random.Range(0, maxZ);
                    for (int x = 0; x < maxX; x++)
                    {

                        if (structure[x, pickZ, y] == 0 && structure[x,pickZ, y - 1] == 1 )
                        {

                            structure[x,  pickZ, y] = 1;
                            placed = true;
                           // Debug.Log("placed");
                            break;
                        }
                    }
                }
            }
        }

        string str = "[";
        for( int y= 0; y < maxY; y++)
        {
            str += "y[";
            for(int x = 0; x < maxX; x++)
            {
                str += "x[z";
                for(int z = 0; z < maxZ; z++)
                {
                    str += structure[x, z, y].ToString() + ", ";
                }
                str += "]";
            }
            str += "]";
        }
        str += "]";
        Debug.Log(str);

    }

}
