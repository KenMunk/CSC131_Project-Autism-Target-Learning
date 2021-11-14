using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imageBroadcaster : MonoBehaviour
{
    public List<Sprite> images = new List<Sprite>(4);
    public int currentImageID = 0;
    public GameObject targetImage;

    public int ticks = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(targetImage != null)
        {
            this.tickTimerEvent(100);
        }
    }
    
    public void sendImage()
    {
        targetImage.SendMessage("setDisplayImage", this.images[this.currentImageID]);

        //prime for the next image
        this.currentImageID = (this.currentImageID + 1) % this.images.Count;
    }

    public void tickTimerEvent(int atTicks)
    {
        if(ticks == 0)
        {
            this.sendImage();
        }
        ticks++;
        ticks %= atTicks;
    }
}
