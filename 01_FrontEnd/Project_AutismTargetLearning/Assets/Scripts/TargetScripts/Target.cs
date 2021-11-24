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

    
    public string name;
    public string path;
    public string sprite;
    private DesinationTypes destinationType = DesinationTypes.NULL;

    /// <summary>
    /// Creates a new instance of target data
    /// </summary>
    /// <param name="name">name of target</param>
    /// <param name="path">path to image for target</param>
    /// <param name="type">type of path destination for target</param>
    public Target(string name, string path, DesinationTypes type)
    {
        this.setDefault();
        //Prevent character confusion in the name
        for(int i = 0; i< name.ToCharArray().Length; i++)
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
        this.setDefault();
        Target newTarget = JsonUtility.FromJson<Target>(json);
        this.name = newTarget.getName();
        this.path = newTarget.getPath();
        this.destinationType = newTarget.getType();
    }

    public Target()
    {
        this.setDefault();
    }

    //Added 2021-11-16 -- Kenneth Munk
    /// <summary>
    /// Initializes a new target with only the name and sprite
    /// Path enumeration is null
    /// Path is null
    /// </summary>
    /// <param name="name"></param>
    /// <param name="image"></param>
    public Target(string name, Sprite image)
    {
        this.setDefault();
        this.name = name;
        this.destinationType = DesinationTypes.App;
        this.setSprite(image);
    }

    //Added 2021-11-16 -- Kenneth Munk
    /// <summary>
    /// Initializes default values for all entries
    /// </summary>
    public void setDefault()
    {
        this.name = "";
        this.path = "";
        this.sprite = "";
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

    //New addition 2021-11-16 -- Kenneth Munk
    /// <summary>
    /// Returns the sprite for the target if one is available, returns null if none is present
    /// </summary>
    /// <returns></returns>
    public Sprite getSprite()
    {
        if(this.sprite != "")
        {
            return Resources.Load<Sprite>("TargetImages/"+this.sprite);
        }
        else
        {
            throw (new UnityEngine.UnityException("Error no code in place to handle local resource loading for sprites at the moment"));
        }
    }

    public void setSprite(Sprite image)
    {
        this.sprite = image.name;
    }

    public bool hasSprite()
    {
        return (this.sprite != "");
    }




}
