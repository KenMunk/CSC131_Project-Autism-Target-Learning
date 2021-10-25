using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class Set
{
    // Set class contains a List of Targets, with each Target at an index, and a string that
    // describes something about the Set.

    // The List is set up here, along with a SetName that may or may not need to be used. 
    // SetName could maybe describe which child the set is being used for? Or possibly a 
    // description of which targets are in the set? 
    private List<Target> set { get; set; }
    private string SetName { get; set; }

    // SUMMARY
    // Creates a new List of Targets, of course without any Targets.
    // /SUMMARY
    public Set()
    {
        set = new List<Target>();
    }
    // SUMMARY
    // Creating a List from Json
    // Fixed error with implicitly converting
    // /SUMMARY
    public Set(string json)
    {
        Set NewSet = JsonUtility.FromJson<Set>(json);
	    this.set = NewSet.GetList();
	    this.SetName = NewSet.GetName();
    }

    // SUMMARY
    // Add a Target to the List. The index is for precise placement of the Target.
    // This method first detetcts if the Target is already added, then if not,
    // proceeds to add the Target to the List. This is to prevent confusion and having
    // exact duplicates of targets in a set. 
    // /SUMMARY
    public string AddToSet(Target target, int index)
    {
        if (set.Contains(target))
        {
            return "Target already in set";
        }
        else
        {
            set.Insert(index, target); 
            return "Target added to set at index " + index;
        }
    }

    // SUMMARY
    // Remove a Target from the Set. The method first checks if the Target even exists in the List,
    // and if it does, the Target is removed and a string is returned describing at which index the
    // Target was.
    // /SUMMARY
    public string RemoveFromSet(Target target)
    {
        if (set.Contains(target))
        {
            int index = set.IndexOf(target);
            set.Remove(target);
            return "Removed Target at index " + index;
        }
        else
        {
            return "Target not found in set.";
        }
    }

    // SUMMARY
    // Returns the List object. If for some reason there is a need for the List object, this
    // method would give it to you.
    // /SUMMARY
    public List<Target> GetList()
    {
        return set;
    }

    // SUMMARY
    // Returns the string that could describe the Set. 
    // /SUMMARY
    public string GetName()
    {
        return SetName;
    }

    // SUMMARY
    // I believe this will output the "Set" List in json format.
    // /SUMMARY
    public override string ToString()
    {
        string output = JsonUtility.ToJson(set);

        Debug.LogFormat($"Object converted to json with result = {output}");

        return (output);
    }
    // Takes the last item in the set and stores it, then it removes it and then inserts it in the front making everything shift over 1.
    public void rotateSet(List<Target> set)
    {
        int last;
        last = set[set.Count - 1];
        set.RemoveItem(set.Count - 1);
        set.InsertItem(0, last);
    }
}
