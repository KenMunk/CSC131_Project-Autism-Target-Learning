using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiClickManager : MonoBehaviour
{
    public List<MultiClickAction> actions = new List<MultiClickAction>();

    private int clickTimer = 0;
    public int clickCoolDown = 20;

    public int clicks = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.clickTimer > 0)
        {
            this.clickTimer--;
            if(this.clickTimer == 0)
            {
                for(int i = 0; i<this.actions.Count; i++)
                {
                    this.actions[i].attemptClick(this.clicks);
                }
            }
        }
    }

    public void sendClick()
    {
        this.clickTimer = this.clickCoolDown;
        this.clicks++;
    }

}
