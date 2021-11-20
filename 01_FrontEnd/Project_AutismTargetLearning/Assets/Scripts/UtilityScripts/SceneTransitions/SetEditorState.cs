using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetEditorState
{
    public static bool editorModeEnabled = true;

    public static void setEditorModeEnabled(bool state)
    {
        editorModeEnabled = state;
    }

    public static bool isEditorModeEnabled()
    {
        return (editorModeEnabled);
    }
}
