using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCarrier : MonoBehaviour
{
    private Set setData = null;

    void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //nothing to do here   
    }

    // Update is called once per frame
    void Update()
    {
        //nothing to do here
    }

    public void setSet(Set data)
    {
        this.setData = data;
    }

    public void getSet(Message data)
    {
        data.sendReply("receiveSet", this.setData);
    }

    public void eraseCarrier()
    {
        Destroy(gameObject);
    }
}
