using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetCarrier : MonoBehaviour
{
    public Set setData;

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
        /*
        if(this.setData != null)
        {
            data.sendReply("receiveSet", this.setData);
            Debug.LogWarningFormat($"sending {data.getSender().name} this set {this.setData}");
        }//*/

        data.sendReply("receiveSet", this.setData);
        Debug.LogWarningFormat($"sending {data.getSender().name} this set {this.setData}");
    }

    public void eraseCarrier()
    {
        Destroy(gameObject);
    }
}
