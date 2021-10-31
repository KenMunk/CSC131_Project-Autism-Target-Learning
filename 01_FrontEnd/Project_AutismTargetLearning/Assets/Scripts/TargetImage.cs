using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO; // namespace to use File.ReadAllBytes

public class TargetImage : MonoBehaviour
{
    private Target targetData = new Target();
    private bool initializedImage = false;

    public byte[] fileData; // load data inside a byte array 0x89,0x50,0x4E,0x47,0x0D...
    public Image imageToDisplay; // Assign in Inspector the UI Image


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //---------------------------------------
    // 10/30/21 - Vadym 
    // Resource: http://www.lucedigitale.com/blog/unity-load-jpg-local-resource-or-www-and-apply-to-texture-or-to-ui-image/
    // Techically if you feed it the right info it should work
    //---------------------------------------

    IEnumerator isDownloading(string url) //For imgs from the web
    {
        yield return new WaitForSeconds(1); // wait for one sec, without it you will have a compiiler error


        var www = new WWW(url); // 1.start a download of the given URL           
        yield return www;       // 2.wait until the download is done
                                // 3.create a texture in DXT1 format
        Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);

        www.LoadImageIntoTexture(texture); //4.load data into a texture
        Rect rec = new Rect(0, 0, texture.width, texture.height); //5.create a rect using texture dimensions
        Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100); // 6.convert the texture to sprite
        imageToDisplay.sprite = spriteToUse; //7.change the sprite of UI Image

        www.Dispose(); //8.drop the web connection NOTE: Unity drop automatically the connection at the end of the download, but we put it as a precaution

    }// END IEnumerator
//---------------------------------------

    // Update is called once per frame
    void Update()
    {
        
        if(this.targetData.getType() != DesinationTypes.NULL && !initializedImage)
        {
            //load the image to the local image object when the image is detected
            Sprite targetImage = null;

//---------------------------------------
// 10/30/21 - Vadym - wasn't able to test
//---------------------------------------
                if (this.targetData.getType() == DesinationTypes.LocalFile) //From Local File
                {
                    fileData = File.ReadAllBytes(this.targetData.getPath()); // 1.read the bytes array
                    Texture2D tex = new Texture2D(2, 2);                 // 2.create a texture named tex
                    tex.LoadImage(fileData);                             // 3.load inside tx the bytes and use the correct image size 
                    Rect rec = new Rect(0, 0, tex.width, tex.height);    // 4.create a rect using the textute dimensions
                    Sprite spriteToUse = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100); //5. convert the texture in sprite
                    imageToDisplay.sprite = spriteToUse;                 //6.load the sprite used by UI Image
                }
                else if (this.targetData.getType() == DesinationTypes.Web) //From Remote File
                {
                    isDownloading(this.targetData.getPath());
                }
            //else //From Remote File -- Not sure how that pathing would work in this case

            //TODO: Call a new target when this texture is loaded

 //-------------------------------------


            gameObject.GetComponent<Image>().sprite = targetImage;
        }
    }

 
    public void setTarget(Target newTarget)
    {
        this.targetData = newTarget;
    }
}
