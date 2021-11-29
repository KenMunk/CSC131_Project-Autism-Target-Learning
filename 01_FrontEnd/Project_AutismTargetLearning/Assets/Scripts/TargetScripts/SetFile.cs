using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
//using Newtonsoft.Json;//If commented out, I haven't found a good workaround.

[System.Serializable]
public class SetFile
{
    
    public List<Set> sets { get; set; }

    public SetFile() { }

    public SetFile(List<Set> sets)
    {
        this.sets = sets;
    }

    public SetFile(string jsonText)
    {
        //SetFile temp = JsonUtility.FromJson<SetFile>(jsonText);
        Debug.Log(jsonText);
        this.sets = JsonConvert.DeserializeObject<SetFile>(jsonText).getSets();
    }

    public List<Set> getSets()
    {
        return (this.sets);
    }

    public override string ToString()
    {
        /*// Deprecating this junk from Unity [JsonUtility]
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
        //*/

        //Disable this code if the Newtonsoft json library can't get added in properly
        string outputJson = JsonConvert.SerializeObject(this);
        Debug.Log(outputJson);
        return (outputJson);
        //*///Keeping this as an available solution if I can get a good workaround.
        

        //Keep this here regardless of the implementation used because it will indicate that I forgot to undo something
       //return ("{\"ERROR\":\"JSON code disabled\"}");

    }
}
