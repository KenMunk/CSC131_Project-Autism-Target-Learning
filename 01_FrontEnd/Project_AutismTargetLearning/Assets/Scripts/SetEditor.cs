using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetEditor : MonoBehaviour
{
    public GameObject library;
    public GameObject currentSet;
    public GameObject prefabTarget;
    private GameObject Highlighted;
    public EventSystem eventSystem;
    public GameObject numberofTargets;


    // Start is called before the first frame update
    void Start()
    {
        
        //for (int i = 0; i < 2; i++)
        foreach (Object Target in Resources.LoadAll("TargetImages", typeof(Sprite)))
        {
            addTargettoLibrary((Sprite) Target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        getSelectedButton();
        getTargets();

    }

    public void addPicturetoSet()
    {
        try
        {
            Highlighted.transform.SetParent(currentSet.transform);
            //library.GetComponentsInChildren<Image>()[0].transform.SetParent(currentSet.transform);
        }
        catch 
        {

        }
    }

    public void removePictureFromSet()
    {
        try
        {
            Highlighted.transform.SetParent(library.transform);
            //currentSet.GetComponentsInChildren<Image>()[0].transform.SetParent(library.transform);
        }
        catch
        {

        }
    }

    private void HighlightObject(GameObject Target)
    {
        Highlighted = Target;
    }

    private void addTargettoLibrary(Sprite Target)
    {
        var item_go = Instantiate(prefabTarget);
        //item_go.GetComponent<Image>().sprite = Resources.Load<Sprite>(Target);
        item_go.GetComponent<Image>().sprite = (Sprite)Target;
        item_go.transform.SetParent(library.transform);
        item_go.transform.localScale = Vector2.one;
        //item_go.GetComponent<Button>().onClick.AddListener(highlightObject);
    }

    private void getSelectedButton()
    {
        try
        {
            if (eventSystem.currentSelectedGameObject.name.Equals("TargetImage(Clone)"))
            {
                HighlightObject(eventSystem.currentSelectedGameObject.gameObject);
            }
        }
        catch
        {

        }
    }

    public void getTargets()
    {
        numberofTargets.GetComponent<Text>().text = "Targets: " + currentSet.transform.childCount.ToString();
    }
}
