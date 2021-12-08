using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClickMessager : MonoBehaviour
{
    public GameObject targetGameobject;
    public string methodName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickAction()
    {
        this.targetGameobject.SendMessage(methodName);
    }
}
