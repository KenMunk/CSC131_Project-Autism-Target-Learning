using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEditorMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogFormat($"Set Editor will edit an existing set: {SetEditorState.editorModeEnabled}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
