using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitch : MonoBehaviour
{
    public string targetScene = "sampleScene";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchScene()
    {
        SceneManager.LoadScene(targetScene,LoadSceneMode.Single);
    }

    public void updateTargetScene(string newTarget)
    {
        this.targetScene = newTarget;
    }
}
