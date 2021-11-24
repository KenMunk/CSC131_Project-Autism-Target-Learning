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
        string testOutput = "";
        for(int i = 0; i<this.sets.Count; i++)
        {
            testOutput += JsonUtility.ToJson(this.sets[i]);
            if (i > 0 && i < this.sets.Count - 1)
            {
                testOutput += "\n";
            }
        }
        Debug.Log(testOutput);

        return (JsonUtility.ToJson(this));
    }
}
