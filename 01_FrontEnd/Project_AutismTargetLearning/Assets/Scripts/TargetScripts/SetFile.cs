using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetFile
{

    public List<Set> sets;

    public SetFile(List<Set> sets)
    {
        this.sets = sets;
    }

    public SetFile(string jsonText)
    {
        SetFile temp = JsonUtility.FromJson<SetFile>(jsonText);
        this.sets = temp.getSets();
    }

    public List<Set> getSets()
    {
        return (this.sets);
    }

    public override string ToString()
    {
        return (JsonUtility.ToJson(this));
    }
}
