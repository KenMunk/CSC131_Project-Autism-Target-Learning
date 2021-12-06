using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFilter : MonoBehaviour
{

    public GameObject filterInputField;
    public GameObject libraryView;
    private InputField filterText;
    // Start is called before the first frame update
    void Start()
    {
        filterText = filterInputField.GetComponentInChildren<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        filter();
    }

    void filter()
    {
        foreach (Transform target in libraryView.transform)
        {
            if (!target.GetComponent<Image>().sprite.name.ToLower().Contains(filterText.text.ToLower()))
            {
                target.gameObject.SetActive(false);
            }
            else if(filterText.text == "")
            {
                target.gameObject.SetActive(true);
            }
            else
            {
                target.gameObject.SetActive(true);
            }
        }
    }
}
