using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSetDeployment : MonoBehaviour
{
    public string testSetName = "TestSet";
    public List<Target> content = new List<Target>();

    public GameObject SetTransporter;
    public GameObject testCarrier;

    // Start is called before the first frame update
    void Start()
    {
        //DO NOT SET A PARENT HERE IT WILL CAUSE SO MANY PROBLEMS
        this.testCarrier = Instantiate(SetTransporter);
        this.testCarrier.name = "SetTransporter";
        Invoke("loadSet", 0.1f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadSet()
    {

        testCarrier.SendMessage("setSet", new Set(this.testSetName, this.content));
        //self destruct because we should be done with the test
        Destroy(gameObject);
    }
}
