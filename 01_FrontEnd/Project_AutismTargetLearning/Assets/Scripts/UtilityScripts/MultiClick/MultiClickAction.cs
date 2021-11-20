using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiClickAction
{
    public int clickRequirement = 1;
    public TargetDelagate targetAction;

    public void attemptClick(int clicksCounted)
    {
        if(clicksCounted == this.clickRequirement)
        {
            targetAction.runAction();
        }
    }
}
