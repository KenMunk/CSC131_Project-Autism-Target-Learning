using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEditorModeController : MonoBehaviour
{
    public void editorMode()
    {
        SetEditorState.setEditorModeEnabled(true);
        Debug.LogFormat($"Set Editor will edit an existing set: {SetEditorState.editorModeEnabled}");
    }

    public void creatorMode()
    {
        SetEditorState.setEditorModeEnabled(false);
        Debug.LogFormat($"Set Editor will edit an existing set: {SetEditorState.editorModeEnabled}");
    }
}
