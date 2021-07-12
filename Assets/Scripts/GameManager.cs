using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject WireStartGate
    {
        get => _wireStartGate;
        set => _wireStartGate = value;
    }

    public GameObject WireEndGate
    {
        get => _wireEndGate;
        set => _wireEndGate = value;
    }

    private GameObject _wireStartGate;
    private GameObject _wireEndGate;
    private string _wireEndInputTerminal;

    public string WireEndInputTerminal
    {
        get => _wireEndInputTerminal;
        set => _wireEndInputTerminal = value;
    }

    public GameObject levelController;
    public GameObject AndGatePrefab;
    public GameObject OrGatePrefab;
    public GameObject NotGatePrefab;
    public GameObject XorGatePrefab;
    public GameObject XNorGatePrefab;
    public GameObject NAndGatePrefab;
    public GameObject NorGatePrefab;

    public GameObject[] LevelList;

    private List<GameObject> PlayerGates = new List<GameObject>();
    private Vector3 mOffset;
    private float mZCoord;



    public bool DrawingWire
    {
        get => _drawingWire;
        set => _drawingWire = value;
    }

    private bool _drawingWire;
    private LineRenderer line;
    private Vector3 mousePos;
    public Material material;
    private int currLines = 0;
    
    int gameLevel = 0;


    // Start is called before the first frame update
    private void Start()
    {
        LevelList[gameLevel].GetComponent<LevelControl>().CreateLevel();
    }

    // Update is called once per frame
    private void Update()
    {
        DrawWire();
        
    }
    public void OnContinueButton()
    {
        gameLevel++;
        LevelList[gameLevel].GetComponent<LevelControl>().CreateLevel();
    }

    public void SpawnGate(GateType type)
    {
        GameObject gObj = null;
        CalcOffset();
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPos.z = 0;
        mouseWorldPos.x += 100;

        if (type == GateType.And)
        {
            gObj = Instantiate(AndGatePrefab, mouseWorldPos, Quaternion.identity);
        }

        if (type == GateType.Or)
        {
            gObj = Instantiate(OrGatePrefab, mouseWorldPos, Quaternion.identity);
        }

        if (type == GateType.Not)
        {
            gObj = Instantiate(NotGatePrefab, mouseWorldPos, Quaternion.identity);
        }

        if (type == GateType.Xor)
        {
            gObj = Instantiate(XorGatePrefab, mouseWorldPos, Quaternion.identity);
        }

        if (type == GateType.XNor)
        {
            gObj = Instantiate(XNorGatePrefab, mouseWorldPos, Quaternion.identity);
        }

        if (type == GateType.Nand)
        {
            gObj = Instantiate(NAndGatePrefab, mouseWorldPos, Quaternion.identity);
        }

        if (type == GateType.Nor)
        {
            gObj = Instantiate(NorGatePrefab, mouseWorldPos, Quaternion.identity);
        }


        PlayerGates.Add(gObj);
        Debug.Log("Spawned");
    }

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

    void createLine()
    {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 2f;
        line.endWidth = 2f;
        line.useWorldSpace = false;
        line.numCapVertices = 50;
    }


    private void DrawWire()
    {
        if (Input.GetMouseButton(1) && DrawingWire && line == null) // after right click on target
        {
            createLine();

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);
        }

        if (Input.GetMouseButton(1) && line != null )  // dragging a line
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            line.SetPosition(1, mousePos);
        }

        if (Input.GetMouseButtonUp(1) && !DrawingWire)  // dropped wire on connection
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (line != null)
            {
                line.SetPosition(1, mousePos);
            }

            line = null;
            currLines++;
            var startGateScript = (Gate)WireStartGate.GetComponent<Gate>();
            var endGateScript = (Gate)WireEndGate.GetComponent<Gate>();
           
            if (WireEndInputTerminal == "InputA")
            {
                startGateScript.Output = WireEndGate;
                endGateScript.InputA = WireStartGate;
            }
            else
            {
                startGateScript.Output = WireEndGate;
                endGateScript.InputB = WireStartGate;
            }
            

        }

        if (Input.GetMouseButtonUp(1) && DrawingWire)
        {
            if (line != null)
            {
                line.positionCount = 0;
                line = null;
                DrawingWire = false;
            }
        }
    }
}


public enum GateType
{
    And,
    Or,
    Not,
    Xor,
    XNor,
    Nand,
    Nor
}