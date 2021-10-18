using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetImage : MonoBehaviour
{
    private Target targetData = new Target();
    private bool initializedImage = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.targetData.getType() != DesinationTypes.NULL && !initializedImage)
        {
            //load the image to the local image object when the image is detected
            Sprite targetImage = null;

            gameObject.GetComponent<Image>().sprite = targetImage;
        }
    }

    public void setTarget(Target newTarget)
    {
        this.targetData = newTarget;
    }
}
