using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonCheck()
    {
        Debug.LogFormat($"{gameObject.name} press has been detected");
    }
}
