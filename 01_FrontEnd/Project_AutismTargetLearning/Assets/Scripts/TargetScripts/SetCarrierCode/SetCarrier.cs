using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetCarrier : MonoBehaviour
{
    public Set setData { get; set; }

    public GameObject lastReceiver;

    void Awake() 
    {
        
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

    public void getSet(GameObject toRequestor)
    {
        /*
        if(this.setData != null)
        {
            data.sendReply("receiveSet", this.setData);
            Debug.LogWarningFormat($"sending {data.getSender().name} this set {this.setData}");
        }//*/
        if(this.setData != null)
        {
            Set tempSet = this.setData;
            this.lastReceiver = toRequestor;
            toRequestor.SendMessage("receiveSet", tempSet);
            Debug.LogFormat($"Sending set: {this.setData.GetName()}");
        }
        //Debug.LogWarningFormat($"sending {toRequestor.name} this set {this.setData}");
    }

    public void eraseCarrier()
    {
        Destroy(gameObject);
    }
}
