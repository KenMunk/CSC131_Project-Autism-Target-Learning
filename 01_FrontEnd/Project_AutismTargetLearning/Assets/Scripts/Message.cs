using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataUtil;
using System;

/*

Code Authored by Kenneth Munk
2021-01-13

Purpose:
Transmitting data between gameobjects easier


*/

public class Message
{
    public GameObject sender;
    public object data;
    public Type dataType;

    public Message(GameObject _sender, object _data)
    {
        this.sender = (GameObject)Temp.Obj(_sender);
        this.data = Temp.Obj(_data);
        this.dataType = _data.GetType();
    }

    public GameObject getSender()
    {
        return ((GameObject)Temp.Obj(this.sender));
    }

    public object getData()
    {
        return (Temp.Obj(this.data));
    }

    public Type getType()
    {
        return ((Type)Temp.Obj(this.dataType));
    }

    public void sendReply(string command, object _data)
    {
        this.getSender().SendMessage(command, _data);
    }

    
}
