using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetDelagate
{
    public GameObject target;

    public string action;

    public void runAction()
    {
        target.SendMessage(action);
    }
}
