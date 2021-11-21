using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetLibrary
{

    public static List<Set> sets = new List<Set>();
    public static string fileName = "UserSets";
    public static int selectedSet = -1;

    public static void loadSets()
    {

    }

    public static void saveSets()
    {
        
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
    
}
