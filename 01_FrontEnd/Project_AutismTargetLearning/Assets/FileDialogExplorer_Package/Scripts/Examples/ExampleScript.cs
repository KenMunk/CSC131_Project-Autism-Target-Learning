using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExampleScript : MonoBehaviour {

	
    public string AnyText;
    public TextMesh AnyTextResult;
	
	void Update () 
    {
        AnyTextResult.text = AnyText;
	}
}
