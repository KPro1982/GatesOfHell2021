using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishOutput : Gate
{
    private Vector3 mOffset;
    private float mZCoord;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PullInput();
        Calculate();
    }

    public void OnMouseDown()  // only works on left button click
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

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
    }
    protected override void LightUpNodes()
    {
    }

    public override void Calculate()
    {
        base.Calculate();
        if (InputA != null && _outputValue == testB)
        {
            Debug.Log("Yea -- you did it.");
        }
    }
    
    
}