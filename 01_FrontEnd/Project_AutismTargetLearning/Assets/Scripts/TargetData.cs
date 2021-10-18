using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains target name and target image path and destination type
/// </summary>
public class TargetData
{
    private string name { get; set; }
    private string path { get; set; }
    private DesinationTypes destinationType { get; set; }

    public TargetData(string name, string path, DesinationTypes type)
    {
        
        for(int i = 0; i< name.Length; i++)
        {
            switch (name.ToCharArray()[i])
            {
                case ',':
                case ';':
                case '[':
                case ']':
                    throw (new UnityException(string.Format($"Invalid character input detected in name: {name}")));
                default:
                    break;
            }
        }

        for (int i = 0; i < path.Length; i++)
        {
            switch (path.ToCharArray()[i])
            {
                case ',':
                case ';':
                case '[':
                case ']':
                    throw (new UnityException(string.Format($"Invalid character input detected in path: {path}")));
                default:
                    break;
            }
        }

        this.name = name;
        this.path = path;
        this.destinationType = type;
    }

    public string getName()
    {
        return (this.name);
    }

    public string getPath()
    {
        return (this.path);
    }

    public DesinationTypes getType()
    {
        return (this.destinationType);
    }

    public override string ToString()
    {
        //Delimiter blocks are 
        string output = "[";
        output += name + ";";
        output += path + ";";
        output += string.Format($"{(int)(this.destinationType)}]");

        return base.ToString();
    }


}
