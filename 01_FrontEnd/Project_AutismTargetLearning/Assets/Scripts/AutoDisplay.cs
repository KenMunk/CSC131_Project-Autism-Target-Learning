using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisplay : MonoBehaviour
{
    private float aspectRatio = 0;

    public Set targetSet = null;

    public GameObject setError;
    
    // Start is called before the first frame update
    void Start()
    {
        //Need to obtain the aspect ratio of the target display area
        Rect targetSpace = gameObject.GetComponent<RectTransform>().rect;
        aspectRatio = (Mathf.Ceil((targetSpace.width / targetSpace.height)*10))/10;



    }

    // Update is called once per frame
    void Update()
    {
        //check if a target set has already been selected
        if(targetSet == null)
        {
            //check if the set transporter exists
            GameObject setTransporter = GameObject.Find("SetTransporter");
            if (setTransporter != null)
            {
                //if the set transporter exists then get the set
                setTransporter.SendMessage("getSet", new Message(gameObject, null));
            }
            else
            {
                setError.SetActive(true);
            }
        }
    }

    public void receiveSet(Set data)
    {
        this.targetSet = data;
    }

    public void rotateTargets()
    {

    }
}
