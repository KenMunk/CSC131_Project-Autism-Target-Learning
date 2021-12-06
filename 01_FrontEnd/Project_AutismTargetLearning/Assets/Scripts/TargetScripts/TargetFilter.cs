using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TargetFilter
{
    public Target[] rawArray;
    public List<Target> filteredList;
    public string searchString = "";

    public void setSearchString(string searchString)
    {
        this.searchString = searchString;
        this.updateFilteredList();
    }

    public void setRawTargets(Target[] rawArray)
    {
        this.rawArray = rawArray;
        this.updateFilteredList();
    }

    public void updateFilteredList()
    {
        this.filteredList = new List<Target>();
        if(this.searchString == "")
        {
            for(int i = 0; i<this.rawArray.Length; i++)
            {
                this.filteredList.Add(rawArray[i]);
            }
        }
        else
        {
            //search works by taking the search string wrapping it in
            //wildcards, then checking if the name of each target has a match

            Regex searchFilter = new Regex(@$"*{this.searchString}*");

            for (int i = 0; i < rawArray.Length; i++)
            {
                if (searchFilter.IsMatch(this.rawArray[i].name))
                {
                    this.filteredList.Add(this.rawArray[i]);
                }
            }
        }
    }

    public TargetFilter(Target[] rawTargets)
    {
        this.setRawTargets(rawTargets);
        this.setSearchString("");
    }

    public TargetFilter(Target[] rawTargets, string searchString)
    {
        this.setRawTargets(rawTargets);
        this.setSearchString(searchString);
    }

    public List<Target> getFilteredList()
    {
        return (this.filteredList);
    }

    public bool latestRawTargets(Target[] anotherArrayOfTargets)
    {
        return (anotherArrayOfTargets == this.rawArray);
    }
}
