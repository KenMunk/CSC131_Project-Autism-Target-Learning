using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHider : MonoBehaviour
{

    public GameObject hideButton;
    private Image topbarObject;
    private Color colorChange;
    // Start is called before the first frame update
    void Start()
    {
        topbarObject = hideButton.transform.parent.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        float alpha = topbarObject.color.a;
        switch (alpha)
        {
            case 0:
                colorChange = new Color(topbarObject.color.r, topbarObject.color.g, topbarObject.color.b, 1);
                topbarObject.color = colorChange;
                break;
            case 1:
                colorChange = new Color(topbarObject.color.r, topbarObject.color.g, topbarObject.color.b, 0);
                topbarObject.color = colorChange;
                break;
        }
    }
}
