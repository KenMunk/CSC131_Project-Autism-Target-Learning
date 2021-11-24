using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFileManager : MonoBehaviour
{
    public GameObject fileFailIndicator;

    // Start is called before the first frame update
    void Start()
    {
        fileFailIndicator.SetActive(false);
        SetLibrary.loadSets();
        fileFailIndicator.SetActive(SetLibrary.loadFailed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
