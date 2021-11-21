using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetGenerator
{
    public static Target[] targets;
    public static Sprite[] images;

    public static void getTargets()
    {
        images = Resources.LoadAll<Sprite>("TargetImages");
        targets = new Target[images.Length];

        for(int i = 0; i<images.Length; i++)
        {
            targets[i] = new Target(images[i].name.Split('-')[0], images[i]);
        }
    }

    public static Target randomTarget()
    {
        int randomDraw = Random.Range((int)0, targets.Length * 10);
        return (targets[randomDraw % targets.Length]);
    }

    public static Target getTarget(int id)
    {
        return (targets[id % targets.Length]);
    }

    public static Set generateRandomSet(string setName)
    {
        int randomSetLength = Random.Range((int)3, (int)28);
        List<Target> setTargets = new List<Target>();
        for (int i = 0; i<randomSetLength; i++)
        {
            setTargets.Add(randomTarget());
        }

        return (new Set(setName, setTargets));
    }


}
