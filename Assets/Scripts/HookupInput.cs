using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookupInput : MonoBehaviour
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    private void Awake()
    {
        // initialize variables
      
        _gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        if (Input.GetMouseButton(1) && _gameManager.DrawingWire)  // capture right mouse button release
        {
            Debug.Log("finished drawing wire...");
            _gameManager.DrawingWire = false;
            _gameManager.WireEndGate = gameObject.transform.parent.gameObject;
            _gameManager.WireEndInputTerminal = this.name;
        }
        
    }
}
