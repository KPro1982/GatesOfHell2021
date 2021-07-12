using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject InputA;
    public GameObject InputB;
    public GameObject Output;

    public GameObject InternalA;
    public GameObject InternalB;
    public GameObject InternalOutput;

    public bool _inputValueA;
    public bool _inputValueB;
    public bool _outputValue;
    public GateType gateType;
    public bool testA;
    public bool testB;
    public bool testMode = true;
    private Vector3 mOffset;
    private float mZCoord;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

        PullInput();
        Calculate();
        LightUpNodes();
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

    protected virtual void LightUpNodes()
    {
        if (InternalA != null)
        {
            if (_inputValueA)
            {
                InternalA.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                InternalA.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (InternalB != null)
        {
            if (_inputValueB)
            {
                InternalB.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                InternalB.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (InternalOutput != null)
        {
            if (_outputValue)
            {
                InternalOutput.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                InternalOutput.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    protected void PullInput()
    {
     
            if (InputA != null)
            {
                var incomingGateA = InputA.GetComponent<Gate>();
                _inputValueA = incomingGateA._outputValue;
                
            }
            else if (testMode)
            {
                _inputValueA = testA;
            }

            if (InputB != null)
            {
                var incomingGateB = InputB.GetComponent<Gate>();
                _inputValueB = incomingGateB._outputValue;
            } else if (testMode)
            {
                _inputValueB = testB;
            }
      
    }


    public virtual void Calculate()
    {
        var calcResult = false;
  
            if (gateType == GateType.And)
            {
                calcResult = _inputValueA && _inputValueB;
            }

            if (gateType == GateType.Or)
            {
                calcResult = _inputValueA || _inputValueB;
            }

            if (gateType == GateType.Nand)
            {
                calcResult = !(_inputValueA && _inputValueB);
            }

            if (gateType == GateType.Nor)
            {
                calcResult = !(_inputValueA || _inputValueB);
            }

            if (gateType == GateType.Xor)
            {
                calcResult = _inputValueA ^ _inputValueB;
            }

            if (gateType == GateType.XNor)
            {
                calcResult = _inputValueA == _inputValueB;
            }

            if (gateType == GateType.Not)
            {
                calcResult = !_inputValueA;
            }

   

        _outputValue = calcResult;
    }
}

