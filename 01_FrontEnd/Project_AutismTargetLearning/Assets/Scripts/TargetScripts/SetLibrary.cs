using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public static class SetLibrary
{

    public static List<Set> sets = new List<Set>();
    public static string fileName = "LocalSets";
    public static int selectedSet = -1;

    public static bool loadFailed = true;
    public static bool saveFailed = true;

    public static void loadSets()
    {
        Debug.LogFormat($"Searching for set file at {dataPath()}");
    }

    public static void saveSets()
    {
        SetFile tempSetFileData = new SetFile(sets);
        Debug.LogFormat($"Potential Set File for Path {dataPath()}:");
        Debug.Log(tempSetFileData.ToString());
    }

    public static string dataPath()
    {
        //return (fileName + ".setdata");
        return (Application.persistentDataPath + "/" + fileName + ".setdata");
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
        sets.RemoveAt(setID);
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
        return 0;
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
        return 0;
    }
}