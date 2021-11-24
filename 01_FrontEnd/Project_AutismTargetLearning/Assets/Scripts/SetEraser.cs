using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetEraser : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void confirmAction()
    {
        SetLibrary.removeSet(SetLibrary.selectedSet);
        SetLibrary.selectedSet = -1;
    }
}
