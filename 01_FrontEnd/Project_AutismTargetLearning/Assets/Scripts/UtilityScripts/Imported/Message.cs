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
        if(_data != null)
        {
            this.data = Temp.Obj(_data);
            this.dataType = _data.GetType();
        }
        else
        {
            this.data = null;
            this.dataType = null;
        }
    }

    public GameObject getSender()
    {
        return ((GameObject)Temp.Obj(this.sender));
    }

    public object getData()
    {
        if(this.data != null)
        {
            return (Temp.Obj(this.data));
        }
        else
        {
            return null;
        }

    }

    public Type getType()
    {
        if(this.dataType != null)
        {
            return ((Type)Temp.Obj(this.dataType));
        }
        else
        {
            return (null);
        }
    }

    public void sendReply(string command, object _data)
    {
        this.getSender().SendMessage(command, _data);
    }

    
}
