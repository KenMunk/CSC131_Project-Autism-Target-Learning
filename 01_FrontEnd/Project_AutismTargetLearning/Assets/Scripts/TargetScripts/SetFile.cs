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

    public List<Set> getSets()
    {
        return (this.sets);
    }
}
