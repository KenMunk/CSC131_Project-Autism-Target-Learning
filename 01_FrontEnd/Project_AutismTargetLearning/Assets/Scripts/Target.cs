using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains target name and target image path and destination type
/// </summary>
/// 
[System.Serializable]
public class Target
{
    /**
     * This class will not be used to load images, it is only to compartmentalize target data
     * and the image paths so that it is easier to transmit between objects within the application
     * and so that the targets can be stored in the database... once that's figured out
     * 
     */


    private string name { get; set; }
    private string path { get; set; }
    private DesinationTypes destinationType { get; set; }

    /// <summary>
    /// Creates a new instance of target data
    /// </summary>
    /// <param name="name">name of target</param>
    /// <param name="path">path to image for target</param>
    /// <param name="type">type of path destination for target</param>
    public Target(string name, string path, DesinationTypes type)
    {
        //Prevent character confusion in the name
        for(int i = 0; i< name.Length; i++)
        {
            switch (name.ToCharArray()[i])
            {
                case ',':
                case ';':
                case ':':
                case '[':
                case ']':
                    throw (new UnityException(string.Format($"Invalid character input detected in name: {name}")));
                default:
                    break;
            }
        }

        //Prevent character confusion in the path
        for (int i = 0; i < path.Length; i++)
        {
            switch (path.ToCharArray()[i])
            {
                case ',':
                case ';':
                case '{':
                case '}':
                    throw (new UnityException(string.Format($"Invalid character input detected in path: {path}")));
                default:
                    break;
            }
        }

        //If everything checks out, then assign values
        this.name = name;
        this.path = path;
        this.destinationType = type;
    }

    /// <summary>
    /// Initializes another instance of target data from json data
    /// 
    /// This is to enable for targets to be put into a shared repository
    /// </summary>
    /// <param name="json"></param>
    public Target(string json)
    {
        Target newTarget = JsonUtility.FromJson<Target>(json);
        this.name = newTarget.getName();
        this.path = newTarget.getPath();
        this.destinationType = newTarget.getType();
    }

    public Target()
    {
        this.name = null;
        this.path = null;
        this.destinationType = DesinationTypes.NULL;
    }

    /// <summary>
    /// Returns the name of the target
    /// </summary>
    /// <returns></returns>
    public string getName()
    {
        return (this.name);
    }

    /// <summary>
    /// Returns the path of the target
    /// </summary>
    /// <returns></returns>
    public string getPath()
    {
        return (this.path);
    }

    /// <summary>
    /// Returns the path type for the target
    /// </summary>
    /// <returns></returns>
    public DesinationTypes getType()
    {
        return (this.destinationType);
    }

    /// <summary>
    /// Export the data as a json format string
    /// </summary>
    /// <returns>json data for the class</returns>
    public override string ToString()
    {
        string output = JsonUtility.ToJson(this);

        Debug.LogFormat($"Object converted to json with result = {output}");

        return(output);
    }




}