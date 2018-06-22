/*
 * Created by Brian Blalock
 * 
 * I wanted to sepereate the designing of the level from the rendering/creation of it
 * 
 * This class creates the path through the 2d layout of the level, then places the individual buildings, signified by increasing numbers
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesigner  {

    private int[,] layout;

    private Vector2[] exits;

    private Vector2[,] buildingsInfo;

    private int xDim;
    private int zDim;

    public LevelDesigner(int maxX, int maxZ)
    {

        xDim = maxX;
        zDim = maxZ;
        layout = new int[xDim, zDim];

    }

    private void setExists(Vector2[] exits)
    {

        this.exits = exits;

    }



    // plan the paths of the sections 
    private void planPath()
    {
        // loop through each exit, since we're creating a straight line from the exit to the center 
        for ( int exit = 0; exit < exits.Length; exit++)
        {

            Vector2 current = exits[exit]; 

            // if it is on the east or west side of section
            if( current[0] == 0 || current[0] == 1)
            {
                int z =(int)( zDim * current[1]); // get Z value of entrance / path
                int target =(int)( xDim / 2 + xDim % 2); // gets the int center of the layout axis 

                for (int x = (int)(current[0] * (xDim-1)); ( (current[0] == 0) ? x <= target : x >= target) ; x += (int)(1 + ( -2 * current[0])  ) )
                {   // the for loop switches direction depending if the entrance/exit is on the east or west, going up in from  to center if on west side, or from max to center if on west side
                    
                        layout[x, z] = 1;
                

                }


            }
            else   // else it is on the north or south
            {

                int x= (int)(xDim * current[0]); // get X value of entrance / path
                int target = (int)(zDim / 2 + zDim % 2); // gets the int value of center of the axis 

                for (int z = (int)( current[1] * (zDim - 1) ); (current[1] == 0) ? z <= target : z >= target; z +=  (int)( 1 * (-2 * current[1])  )  )
                {   // the for loop switches direction depending if the entrance/exit is on the north or south, going up in from 0 to center if on south side, or from max to center if on north side

                        layout[x, z] = 1;
                    

                }


            }   // end if
            

        }// end for


    }// end planPath


    private void planBuildings()
    {



    }

    private void addBuilding(int xLoc, int zLoc)
    {
        int xLen = 0; //xDim for the building 
        int zLen = 0; //zDim for the building 

        for ( int x = xLoc; x < xDim; x++)
        {

            for ( int z = zLoc; z< zDim; z++)
            {



            }


        }

    }

}
