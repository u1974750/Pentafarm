using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWorld : MonoBehaviour
{
    public float cellSize = 1.45f;
    public float buildDistance;
    
    //Publicos
    public void ToCellCoord(ref float x, ref float z, SnapCell snap)
    {
        float resX, resZ;
        resX = resZ = 0;


        switch (snap)
        {
            case(SnapCell.center):
                SnapCenter(x, z, ref resX, ref resZ);
                break;
            case (SnapCell.vertex):
                SnapVertex(x, z, ref resX, ref resZ);
                break;
            case (SnapCell.edgH):
                SnapEdgeH(x,z, ref resX, ref resZ);
                break;
            case (SnapCell.edgV):
                SnapEdgeV(x,z,ref resX,ref resZ);                
                break;
        }


        //Return 
        x = resX;
        z = resZ;

      
    }
    public float GetCellSize()
    {
        return cellSize;
    }

    //Privados
    void SnapEdgeH(float x, float z, ref float resX, ref float resZ)
    {
        //Calcula en que numero de celda se encuentran las cordenadas xz.
        float numCellX = Mathf.Floor(x / cellSize);
        float numCellZ = Mathf.Floor(z / cellSize);

        //Calcula en que posicion empieza la celda en cordenadas del mundo.
        resX = numCellX * cellSize;
        resZ = numCellZ * cellSize;

        //Hace que la posicion sea el centro de la celda
        resX += cellSize / 2;
        resZ += cellSize / 2;

        if (z >= resZ) { resZ = resZ + cellSize / 2; }
        else { resZ = resZ - cellSize / 2; }

    }
    void SnapEdgeV(float x, float z, ref float resX, ref float resZ)
    {
        
        //Calcula en que numero de celda se encuentran las cordenadas xz.
        float numCellX = Mathf.Floor(x / cellSize);
        float numCellZ = Mathf.Floor(z / cellSize);

        //Calcula en que posicion empieza la celda en cordenadas del mundo.
        resX = numCellX * cellSize;
        resZ = numCellZ * cellSize;

        //Hace que la posicion sea el centro de la celda
        resX += cellSize / 2;
        resZ += cellSize / 2;

        if (x >= resX) { resX = resX + cellSize / 2; }
        else { resX = resX - cellSize / 2; }
    }

    void SnapVertex(float x, float z,ref float resX, ref float resZ)
    {
        //Calcula en que numero de celda se encuentran las cordenadas xz.
        float numCellX = Mathf.Floor(x / cellSize);
        float numCellZ = Mathf.Floor(z / cellSize);

        //Calcula en que posicion empieza la celda en cordenadas del mundo.
        resX = numCellX * cellSize;
        resZ = numCellZ * cellSize;

        //Hace que la posicion sea el centro de la celda
        resX += cellSize / 2;
        resZ += cellSize / 2;

        if (x > resX)
        {
           numCellX++;
        }
        if (z > resZ)
        {
            numCellZ++;
        }

        //Calcula en que posicion empieza la celda en cordenadas del mundo.
        resX = numCellX * cellSize;
        resZ = numCellZ * cellSize;

    }

    void SnapCenter(float x, float z, ref float resX, ref float resZ)
    {
        //Calcula en que numero de celda se encuentran las cordenadas xz.
        float numCellX = Mathf.Floor(x / cellSize);
        float numCellZ = Mathf.Floor(z / cellSize);

        //Calcula en que posicion empieza la celda en cordenadas del mundo.
        resX = numCellX * cellSize;
        resZ = numCellZ * cellSize;

        //Hace que la posicion sea el centro de la celda
        resX += cellSize / 2;
        resZ += cellSize / 2;
    }

   

}
