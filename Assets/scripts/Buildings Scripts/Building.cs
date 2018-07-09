
/*
 * Created by Brian Blalock
 * 
 * This class is used to proceduarlly generate buildings of a certain size for use in  
 * a game currently in development 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

    Transform Voxel; 

    // colors for the building's color pallette *** not yet implemented ***
    public Color color1; 
    public Color color2;

    //size of building's dimensions
    int maxX;
    int maxZ;
    int maxY;

    //cell size in quantity of voxels
    int size;

    //gameobject of building
    public GameObject building = new GameObject("Building");

    private int[,,] structure;
    // used for initial design of the buildings Layout
    // [X,Z,Y] XZY -> Cell location 

    private Cell[,,] cells; // xzy

    private List<Vector2> emptyCellLocations = new List<Vector2>();  // used to specify x,z coordinates that should not be built on, used to make more specific building shapes 

    public Building(int x,  int z, int y, int size, Transform vox)
    {

        maxX = x;
        maxY = y;
        maxZ = z;
        Voxel = vox;
        this.size = size;

    }
    public Building(int x, int z, int y, int size, Transform vox, List<Vector2> empty)
    {

        maxX = x;
        maxY = y;
        maxZ = z;
        Voxel = vox;
        this.size = size;
        emptyCellLocations = empty;

    }

    public void constructBuilding()
    {

        planBuilding();     //plan it
        fillCells();        //build the cells
        constructPanels();  //render the panels

    }

    

    //Iterates through thebuilding's cells, determines which panels are external/should be created, then calls updateCell() to render each cell 
    private void constructPanels()      
    {

        for (int x = 0; x < maxX; x++)
        {

            for (int z = 0; z < maxZ; z++)
            {

                for (int y = 0; y < maxY; y++)
                {
                    bool[] sides = { false, false, false, false, false, false }; // cell default is to have to sides created
                    //top
                    string print = "";
                   if (structure[x, z, y] == 1) {                           // check that the cell needs to be filled 
                        if (z == 0 || structure[x, z - 1, y] == 0)          //checks if panel faceing -Z needs to be rendered
                            sides[2] = true;
                        if (x == 0 || structure[x - 1, z, y] == 0)          // -X
                            sides[1] = true;
                        if (z == maxZ - 1 || structure[x, z + 1, y] == 0)   //+Z
                            sides[4] = true;
                        if (x == maxX - 1 || structure[x + 1, z, y] == 0)   //+X
                            sides[3] = true;
                        if (y == maxY - 1 || structure[x, z, y + 1] == 0)   // Top of cell
                            sides[5] = true;
                   }
                   else if (y == 0 )    // if not part of the main structure but on ground level, create a floor
                        sides[0] = true;
                    
                  /*  for( int i = 0; i < 6; i++)                   // debug code to check cell values
                    {

                       print += (sides[i] ? "True ": "False " );

                    }
                    Debug.Log(print);*/

                    cells[x, z,y ].updateCell(sides);               //have the cell construct the needed panels
                }
            } 
        }
    }


                
    // Fills the cell structure with empty cells in their needed location.
    private void fillCells()    
    {
        cells = new Cell[maxX, maxZ, maxY];         // instantiate array


        for ( int x = 0; x < maxX; x++)
        {
            
            for ( int z = 0; z < maxZ; z++)
            {

                for ( int y = 0; y< maxY; y++)
                {

                    Vector3 cellPosition =  new Vector3(x * (size/4f) , y * (size / 4f), z * (size / 4f));      //cell position is based off of where in the structure it is being placed * the amount of voxels in one dimension of a panel
                                                                                                                // size is divided by 4 because of the .25 scale on the Voxels, This may be changed later to have it be modifiable
                    cells[x, z, y] = new Cell( Voxel, size );                                           // create cell 
                    cells[x, z, y].color1 = color1;
                    cells[x, z, y].color2 = color2;
                    cells[x, z, y].makeUninstantiatedCell();                                                    // builds empty panels
                    cells[x, z, y].cell.transform.SetParent(building.transform);                                // sets parent to keep heirarchy clean and maintain local position
                    cells[x, z, y].cell.transform.localPosition = cellPosition;                                 // sets postion of the cell relative to the origin of the building.
                    cells[x, z, y].cell.transform.localRotation = Quaternion.identity;
                  
                }
         
            }
        }

    }

    //generates structure of the building 
    private void planBuilding() 
    {
        structure = new int[maxX, maxZ, maxY]; 

        int maxLayerSize = maxX * maxZ;                                             // max amount of cells that can be in a single Y layer



        int layerSize = maxLayerSize - Random.Range(1, ((maxLayerSize)/2) );        // chooses the size of the first layer of the building , minimum is 1/2 of max size, max is max-1
        int lastLayerSize = layerSize;                                              // keeps track of previous layer so upper layer is always smaller

        // Debug.Log(layerSize.ToString());
        // only for the first floor
        for (int l = 0 ; l < layerSize; l++)                //loop through the number of cells to be placed
        {
           // Debug.Log(l.ToString());
            bool placed = false;                            // while the current cell hasn't been placed 

            while (!placed)
            {
               // Debug.Log("in while");
                int pickZ = Random.Range(0, maxZ);          // pick a Z value
                for ( int x = 0; x < maxX; x++)             // loop through X values at that Z
                {

                    if (structure[x, pickZ, 0 ] == 0 && !emptyCellLocations.Contains(new Vector2(x,pickZ)) )       //If there is an empty cell spot , and it isn't one of the spots that shouldnt be filled
                    {

                        structure[x, pickZ, 0] = 1;         //place the cell there
                        
                        placed = true;                      // end the while loop
                       
                        break;                              // break out of the for loop
                    }
                }       

                // if the for loop concludes without placing the cell, then the loop will restart and another Z will be picked
            }
            // end of the while
        }
        //end of the creation of the first floor. This is spereate from the other layer becuase this layer doesn't have to check to see if the slot below it is full or empty

        // this loop is nearly identical to the last with a few exceptions
        for ( int y = 1 ; y < maxY; y++)
        {
            layerSize = maxLayerSize - Random.Range(0, (maxLayerSize / 2)) - (maxLayerSize - lastLayerSize) ;   // similar to last loop, but takes into account the size of the previous layer
            lastLayerSize = layerSize;

            for (int l = 0; l < layerSize; l++)
            {

                bool placed = false;

                while (!placed)
                {

                    int pickZ = Random.Range(0, maxZ);
                    for (int x = 0; x < maxX; x++)
                    {

                        if (structure[x, pickZ, y] == 0 && structure[x,pickZ, y - 1] == 1 ) // checks to make sure that the layer below the cell is a filled cell
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


        /* loop that prints the layout of the building 
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
        */
    }

}
