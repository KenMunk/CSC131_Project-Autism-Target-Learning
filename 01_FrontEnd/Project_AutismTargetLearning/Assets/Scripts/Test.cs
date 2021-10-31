using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject imageBox;
    public void SetText()
    {
        //int arraySize = 5;
        //for (int i = 0; i < arraySize; i++)
        //    Instantiate(imageBox, new Vector3(1, 1, 0), Quaternion.identity, GameObject.FindWithTag("GridImage").transform);
        GameObject img1 = new GameObject(); //Create the GameObject
        img1.transform.SetParent(GameObject.FindWithTag("GridImage").transform);
        img1.transform.localScale = new Vector3(1, 1, 0);
        Image imgSprite = img1.AddComponent<Image>(); //Add the Image Component script
        imgSprite.sprite = Resources.Load<Sprite>("testimage"); 
        img1.SetActive(true); //Activate the GameObject

    }
  
}
