using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour {

    public Transform Voxel; 

    public int xDim;
    public int zDim;

    public int cellSize;

    private int[,] levelLayout =
    {   // 1 -> path ,2 and above-> individual buildings
        { 2, 2 ,2, 1, 3, 3, 3}, // right is x+, down is z+
        { 2, 2, 2, 1, 3, 3, 3},
        { 2, 2, 2, 1, 3, 3, 3},
        { 1 ,1, 1, 1, 1, 1, 1},
        { 4, 4, 4 ,1 ,6 ,6 ,6},
        { 5, 5 ,5 ,1 ,6 ,6 ,6},
        { 5, 5, 5 ,1 ,6 ,6 ,6 }
    };

    private Dictionary<int, Building> buildings = new Dictionary<int, Building>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

    // loop through levelLayout
    public void buildLevel()
    {

        for (int x = 0; x < xDim; x++)
        {

            for (int z = 0; z < zDim; z++)
            {



            }
        }

    }

    /*
     * 
     * this should create buildings in the right orientation based on the intersection layout I am using to demo this. I'm almost certainly going to have to do 
     * some heavy rewriting of this to allow for more layouts. 
     * 
     * */

    public void createBuilding(int xOffset, int zOffset)
    {

        int xLen = xDim- xOffset;
        int zLen = zDim- zOffset;
        int buildingNum = levelLayout[xOffset, zOffset];
        for (int x = xOffset; x < xLen + xOffset; x++)
        {

            for( int z = zOffset; z < zLen + zOffset; z++)
            {

                if (z != zDim - 1 && levelLayout[x, z + 1] != buildingNum)
                    zLen = z;
                if (x != xDim - 1 && levelLayout[x + 1, z] != buildingNum)
                    xLen = x;

            }

        }

        int bHeight = Random.Range(1, 4);

        Vector3 offset = new Vector3();
        Quaternion rotation = Quaternion.identity;
        Building temp;

        if (zOffset > zDim)
        {
            offset = new Vector3(xOffset * (cellSize / 4), 0, (zOffset + xLen) * (cellSize / 4));
            rotation = new Quaternion.euler(0, 90, 0);
            temp = new Building(zLen, xLen, bHeight, cellSize, Voxel);

        }


        Building temp = new Building(xLen, zLen,bHeight, cellSize, Voxel );
        temp.building.transform.SetParent(gameObject.transform);

        temp.building.transform.localPositon = ;


        temp.constructBuilding();
    }

}
