using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour {

    public Transform Voxel; 

    public int xDim;
    public int zDim;

    // colors for the building's color pallette *** not yet implemented ***
    public Color color1;
    public Color color2;
    public Color pathColor;
    public int cellSize;

    private int[,] levelLayout =
    {   // 1 -> path ,2 and above-> individual buildings
        { 2, 2 ,2, 1, 3, 3, 3}, // right is x+, down is z+
        { 2, 2, 2, 1, 3, 3, 3},
        { 2, 2, 2, 1, 3, 3, 3},
        { 1 ,1, 1, 1, 1, 1, 1},
        { 4, 4, 5 ,5 ,6 ,6 ,6},
        { 4, 4 ,5 ,5, 6 ,6 ,6},
        { 4, 4, 5 ,5 ,6 ,6 ,6 }
    };
    
    private Dictionary<int, Building> buildings = new Dictionary<int, Building>();

	// Use this for initialization
	void Start () {
        buildLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

    // loop through levelLayout
    public void buildLevel()
    {
        Vector2 center = getCenterOfPath();

        for (int x = 0; x < xDim; x++)
        {

            for (int z = 0; z < zDim; z++)
            {

                if( levelLayout[x,z] > 1 && !buildings.ContainsKey( levelLayout[x,z] ) )
                {
                    Debug.Log("building" + levelLayout[x,z]);
                    createBuilding(x, z, center);
                }

            }
        }

        constructPaths();

    }

    /*
     * 
     * this should create buildings in the right orientation based on the intersection layout I am using to demo this. I'm almost certainly going to have to do 
     * some heavy rewriting of this to allow for more layouts. 
     * 
     * */

    public void createBuilding(int xOffset, int zOffset, Vector2 cent)
    {

        int xLen = 0;                           // dimensions of the buildings
        int zLen = 0;                           // start with the max size that can bui built in space without going out of bounds 

        int buildingNum = levelLayout[xOffset, zOffset];    // gets the specific number of the building, since each building is specified by a unique number in the layout

        for (int x = xOffset; x < xDim && levelLayout[x, zOffset] == buildingNum; x++)      //loop through the layout of the level, from starting corner of building till you find the edge of section or you hit a different number
        {

            xLen++;
           
        }


        for (int z = zOffset; z < zDim && levelLayout[xOffset, z] == buildingNum; z++)      //loop through the layout of the level, from starting corner of building till you find the edge of section or you hit a different number
        {

            zLen++;

        }



        int bHeight = Random.Range(1, 5);                // pick a height for the building

        Vector3 offset = new Vector3();                 // place holder for the local posiion of the building 
        Quaternion rotation = Quaternion.identity;      // place holder for the rotation
        Building temp;                                  // same for the neew building
        Vector2 center = cent;            // gets center of the path through the section

        float angleFromBuildingToCenter = Vector2.SignedAngle(center - new Vector2((int)(xLen / 2 + xOffset), (int)(zLen / 2 + zOffset)) , new Vector2(xDim / 2, zDim ) );
        // gets the angle fron the center of the building to the center of the path. 

        // 
        if (angleFromBuildingToCenter >= 135 || angleFromBuildingToCenter <= -135)
        {// face left, -X, move origin along x and z to far right corner, rotate -180 

            offset = new Vector3(offsetDistance(xOffset + xLen)-.25f, 0, offsetDistance(zOffset + zLen) -.25f);
            rotation = Quaternion.Euler(0, -180, 0);
            temp = new Building(xLen, zLen, bHeight, cellSize, Voxel);


        }
        else if(angleFromBuildingToCenter <=135 && angleFromBuildingToCenter > 45)// face up , +Z , move origin along x, rotate 90 around Y, flip xlen and zlen
        {

         

            offset = new Vector3(offsetDistance(xOffset), 0, offsetDistance(zOffset + zLen) -.25f);
            rotation = Quaternion.Euler(0, 90, 0);
            temp = new Building(zLen, xLen, bHeight, cellSize, Voxel);

        }
        else if( angleFromBuildingToCenter <= 45 && angleFromBuildingToCenter >= -45)// face right +X , origin is default, orientation is default
        {

            offset = new Vector3(offsetDistance(xOffset), 0, offsetDistance(zOffset));
            temp = new Building(xLen, zLen, bHeight, cellSize, Voxel);

        }
        else // face down, -Z,  move along Z, rotate -90, flip zlen and xlen
        {

            offset = new Vector3(offsetDistance(xOffset + xLen) -.25f, 0, offsetDistance(zOffset));
            rotation = Quaternion.Euler(0, -90, 0);
            temp = new Building(zLen, xLen, bHeight, cellSize, Voxel);

        }

      
        temp.building.transform.SetParent(gameObject.transform); 

        temp.building.transform.localPosition = offset;
        temp.building.transform.localRotation = rotation;
        temp.color1 = color1;
        temp.color2 = color2;
        temp.constructBuilding();
        buildings.Add(buildingNum, temp); 
        


    }
    private float offsetDistance(int cellNum)
    {

        return cellNum * (cellSize / 4);

    }

    private void constructPaths()
    {

        for ( int x = 0; x < xDim; x++)
        {

            for (int z = 0; z < zDim; z++)
            {

                if (levelLayout[x, z] == 1) { 
                    Cell path = new Cell(Voxel, cellSize);                                           // create cell 
                    path.color1 = pathColor;
                    path.color2 = pathColor;
                    path.makeUninstantiatedCell();                                                    // builds empty panels
                    path.cell.transform.SetParent(gameObject.transform);                                // sets parent to keep heirarchy clean and maintain local position
                    path.cell.transform.localPosition = new Vector3(offsetDistance(x), 0, offsetDistance(z));// sets postion of the cell relative to the origin of the building.
                    path.cell.transform.localRotation = Quaternion.identity;
                    
                    path.updateCell(  new bool[] {true, false,false,false,false, false} )  ;
                  }

            }
        }


    }

    private Vector2 getCenterOfPath()
    {

        Vector2 vectorSum = new Vector2();
        int numberOfPathVectors = 0;
        for (int x = 0; x < xDim; x++)
        {

            for(int z = 0; z< zDim; z++)
            {

                if(levelLayout[x,z] == 1)
                {

                    vectorSum += new Vector2(x, z);
                    ++numberOfPathVectors;

                }

            }

        }

        Debug.Log(vectorSum / numberOfPathVectors);

        return vectorSum / numberOfPathVectors;

    }

}
