using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetSaver : MonoBehaviour
{

    public GameObject currentSetObject;
    public GameObject nameInputField;
    public GameObject SetSelector;
    public GameObject SetEditorScript;
    public EventSystem eventSystem;
    private Dropdown Set_Dropdown;
    private InputField Name_Field;
    private SetEditor EditorScript;
    private Set SavedSet;

    public void Start()
    {
        Set_Dropdown = SetSelector.GetComponent<Dropdown>();
        Name_Field = nameInputField.GetComponent<InputField>();
        EditorScript = SetEditorScript.GetComponent<SetEditor>();
        updateSetSelector();
    }

    public void saveSet()
    {
        SavedSet = new Set();

        if (currentSetObject.transform.childCount > 0 && Name_Field.text != "")
        {
            Debug.LogFormat("start of Save loop", currentSetObject);
            SavedSet.setName(Name_Field.text);
            for (int i = 0; i < currentSetObject.transform.childCount; i++)
            {
                //Debug.LogFormat(currentSetObject.transform.GetChild(i).GetComponent<Image>().sprite.name);
                Target tempTarget = new Target(currentSetObject.transform.GetChild(i).GetComponent<Image>().sprite.name
                        , currentSetObject.transform.GetChild(i).GetComponent<Image>().sprite);
                SavedSet.AddToSet(tempTarget, i);
            }
            if (isUnique(Name_Field.text))
            {
                SetLibrary.addSet(SavedSet);
            }
            else
            {
                SetLibrary.updateSet(SetLibrary.findSetIndex(Name_Field.text),SavedSet);
            }
            updateSetSelector();
            //clearSet();
        }
        else
        {
            Debug.LogFormat("Content does not contain any Targets", currentSetObject);
        }
    }

    public bool isUnique(string setName)
    {
        if (Name_Field.text != SetLibrary.findSet(setName).name)
        {
            return true;
        }
        return false;
    }

    public void updateSetSelector()
    {
        Set_Dropdown.ClearOptions();
        List<string> list = new List<string>();
        foreach (Set newSet in SetLibrary.sets)
        {
            list.Add(newSet.name);
        }
        Set_Dropdown.AddOptions(list);
    }

    public void clearSet()
    {
        EditorScript.clearCurrentSet();
    }

    public void loadSetintoCurrentSet()
    {
        clearSet();
        string tempOptionString = Set_Dropdown.options[Set_Dropdown.value].text;
        if (SetLibrary.sets.Count > 0)
        {
            Set tempSet = SetLibrary.findSet(tempOptionString);
            //Debug.LogFormat("Set Library Index: " + i);
            if (tempOptionString == tempSet.name)
            {
                foreach(Target target in tempSet.set)
                {
                    foreach(Transform transform in EditorScript.library.transform)
                    {
                        if (target.sprite.name == transform.GetComponent<Image>().sprite.name)
                        {
                            EditorScript.addPicturetoSet(transform);
                        }
                    }
                }
            }
            Name_Field.text = tempSet.name;
        }
    }
}
