using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPreviewTester : MonoBehaviour
{
    public bool testReady = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.testReady)
        {
            int setsNeeded = Random.Range((int)5, (int)25);

            TargetGenerator.loadTargets();

            for(int i = 0; i<setsNeeded; i++)
            {
                SetLibrary.addSet(TargetGenerator.generateRandomSet(string.Format($"Random_Set_{i}")));
            }

            this.testReady = true;
        }
    }
}
