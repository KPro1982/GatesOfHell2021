using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IOTypes
{
    StartOn,
    StartOff,
    FinishOn,
    FinishOff
}
public class LevelControl : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public GameObject[] Starts;
    public GameObject[] Finish;
    

    /*public void CreateLevel(int level)
       {
                 GameObject gObj = null;
                 CalcOffset();
                 var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
  
                 mouseWorldPos.z = 0;
                 mouseWorldPos.x += 100;
  
                 foreach (IOTypes io in Levels[level])
                 {
                     if (io == IOTypes.StartOn)
                     {
                         gObj = Instantiate(AndGatePrefab, mouseWorldPos, Quaternion.identity);
                     }
                 }
        
       }*/
    public void CalcOffset()
    {
        mZCoord = Camera.main.WorldToScreenPoint(GetComponent<Transform>().position).z;
        mOffset = GetComponent<Transform>().position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


     
}