using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Kenneth Munk
/// 2021-11-13
/// 
/// Only handles the displaying of images and resizes the image UI to match the aspect ratio of the target image to be loaded
/// This does not handle the loading of target images
/// </summary>
public class ImageDisplay : MonoBehaviour
{
    public Sprite targetImage;

    private Sprite detectedImage;

    public Vector2 dimensionMultiplier;

    public Vector2 originalDimensions;

    //just in case the desired dimensions are going to be different
    public bool automatic = true;

    // Start is called before the first frame update
    void Start()
    {
        if (automatic == true)
        {
            RectTransform thisTransform = gameObject.GetComponent<RectTransform>();
            if (thisTransform != null)
            {
                this.originalDimensions = new Vector2(thisTransform.rect.width, thisTransform.rect.height);
            }
            else
            {
                Debug.LogError("ERROR: object missing recttransform", gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(originalDimensions != null)
        {
            if(this.detectedImage != targetImage)
            {
                updateGameobjectImage();
            }
        }
    }

    //Making it possible to pass an event that assigns the image for this object
    public void setDisplayImage(Sprite image)
    {
        this.targetImage = (Sprite)image;
        gameObject.name = image.name;

        
    }

    public void setOriginalDimensions(Vector2 dimensions)
    {
        this.originalDimensions = dimensions;
    }

    public void updateGameobjectImage()
    {
        Image thisImage = gameObject.GetComponent<Image>();

        if(thisImage != null)
        {
            thisImage.sprite = this.targetImage;
            this.detectedImage = this.targetImage;

            this.updateMultipliers();

            RectTransform thisRectTransform = gameObject.GetComponent<RectTransform>();

            Vector2 newDimensions = this.originalDimensions;
            newDimensions.x *= this.dimensionMultiplier.x;
            newDimensions.y *= this.dimensionMultiplier.y;

            thisRectTransform.sizeDelta = newDimensions;
        }
    }

    public void updateMultipliers()
    {
        if(this.targetImage != null)
        {
            this.dimensionMultiplier = new Vector2(1, 1);
            if(this.targetImage.textureRect.width >= this.targetImage.textureRect.height)
            {
                this.dimensionMultiplier.y = this.targetImage.textureRect.height / this.targetImage.textureRect.width;
            }
            else
            {
                this.dimensionMultiplier.x = this.targetImage.textureRect.width / this.targetImage.textureRect.height;
            }
        }
    }
}
