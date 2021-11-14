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

    public Vector2 dimensionMultiplier = Vector2.one;

    public Vector2 originalDimensions;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform thisTransform = gameObject.GetComponent<RectTransform>();
        if(thisTransform != null)
        {
            this.originalDimensions = new Vector2(thisTransform.rect.width, thisTransform.rect.height);
        }
        else
        {
            Debug.LogError("ERROR: object missing recttransform", gameObject);
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
    public void setImage(object image)
    {
        if (image.GetType() == targetImage.GetType())
        {
            this.targetImage = (Sprite)image;
        }
        else
        {
            Debug.LogError("Invalid object passed", this);
        }
    }

    public void updateGameobjectImage()
    {
        Image thisImage = gameObject.GetComponent<Image>();

        if(thisImage == null)
        {
            thisImage.sprite = this.targetImage;
            this.detectedImage = this.targetImage;

            this.updateDimensions();

            RectTransform thisRectTransform = gameObject.GetComponent<RectTransform>();
            thisRectTransform.sizeDelta = this.originalDimensions * this.dimensionMultiplier;
        }
    }

    public void updateDimensions()
    {
        if(this.targetImage != null)
        {
            this.dimensionMultiplier = new Vector2(1, 1);
            if(this.targetImage.rect.width >= this.targetImage.rect.height)
            {
                this.dimensionMultiplier.x = this.targetImage.rect.height / this.targetImage.rect.width;
            }
            else
            {
                this.dimensionMultiplier.x = this.targetImage.rect.width / this.targetImage.rect.height;
            }
        }
    }
}
