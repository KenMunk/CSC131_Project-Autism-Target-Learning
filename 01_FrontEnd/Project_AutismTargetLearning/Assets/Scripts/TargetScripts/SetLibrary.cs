using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SetLibrary
{

    public static List<Set> sets = new List<Set>();
    public static string fileName = "UserSets";
    public static int selectedSet = -1;

    public static void loadSets()
    {
        string UserSetData = "";

        BinaryFormatter binaryFormat = new BinaryFormatter();
        string loadPath = Application.persistentDataPath + "/" + fileName + ".userData";
        FileStream loadStream = new FileStream(loadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        UserSetData = (string)binaryFormat.Deserialize(loadStream);
        Debug.Log(UserSetData);
        SetFile tempData = new SetFile(UserSetData);
        sets = tempData.getSets();
    }

    public static void saveSets()
    {
        string UserSetData = "";
        SetFile tempFileData = new SetFile(sets);
        UserSetData = tempFileData.ToString();

        BinaryFormatter binaryFormat = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "/" + fileName + ".userData";
        FileStream saveStream = new FileStream(savePath, FileMode.OpenOrCreate,FileAccess.Write);

        binaryFormat.Serialize(saveStream, UserSetData);
        saveStream.Close();

    }

    public static void updateSet(int setID, Set updatedSet)
    {
        sets[setID] = updatedSet;
    }

    public static void addSet(Set newSet)
    {
        sets.Add(newSet);
    }

    public static void removeSet(int setID)
    {
        Set setToDelete = sets[selectedSet];
        if (setID > -1)
        {
            setToDelete = sets[setID];
        }
        Debug.LogFormat($"Attempting to delete set {setToDelete.GetName()}");
        while (findSetIndex(setToDelete) >= 0)
        {
            sets.Remove(setToDelete);
        }
        selectedSet = -1;
    }

    public static void cloneSet(int setID)
    {
        Set cloneSet = sets[setID];
        cloneSet.setName(cloneSet.GetName() + "_Clone");
        sets.Add(cloneSet);
    }

    public static Set getSet(int setID)
    {
        return (sets[setID]);
    }

    public static void resetSelection()
    {
        selectedSet = -1;
    }

    public static void selectSet(int setID)
    {
        if(sets.Count > 0)
        {
            selectedSet = setID % sets.Count;
        }
        else
        {
            selectedSet = -1;
        }
    }

    public static int selection()
    {
        return selectedSet;
    }
    
    public static Set findSet(string setName)
    {
        Set tempSet = new Set();
        if (sets.Count > 0)
        {
            foreach (Set newSet in sets)
            {
                if (newSet.name == setName)
                {
                    return newSet;
                }
            }
        }
        return tempSet;
    }

    public static int findSetIndex(Set set)
    {
        for(int i = 0; i < sets.Count; i++)
        {
            if (sets[i] == set)
            {
                return i;
            }
        }
        return -1;
    }

    public static int findSetIndex(string setName)
    {
        for (int i = 0; i < sets.Count; i++)
        {
            if (sets[i].name == setName)
            {
                return i;
            }
        }
        return -1;
    }
}
