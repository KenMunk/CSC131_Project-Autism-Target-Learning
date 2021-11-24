using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPreview : MonoBehaviour
{
    public Set previewSet;

    public GameObject TargetPreviewPrefab;

    public List<GameObject> targetsPreviewed = new List<GameObject>();
    public float rotationLength = 600;
    private int stage = 0;
    public float height = 400;
    public float width;

    public int deploymentCursor = 0;
    public int spriteCursor = 0;

    public int setID = -1;

    private bool allowSetup = false;
    // Start is called before the first frame update
    void Start()
    {

        this.width = gameObject.GetComponent<RectTransform>().rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.TargetPreviewPrefab == null)
        {
            this.TargetPreviewPrefab = Resources.Load<GameObject>("Prefab_UI/TargetViewer--ImageDisplay");
        }
        else if (this.previewSet != null && this.TargetPreviewPrefab != null && this.stage < 2)
        {
            //Debug.LogFormat($"Loading data for target {this.deploymentCursor}");
            if(this.stage == 0)
            {
                this.deployImage();
            }
            else if(this.stage == 1 && this.deploymentCursor < this.previewSet.GetList().Count)
            {
                Debug.LogFormat($"Attempting to update the sprite for {this.targetsPreviewed[this.deploymentCursor].name}");
                this.sendSprites();
            }

            this.deploymentCursor++;
        }
        else if(this.targetsPreviewed.Count == this.previewSet.GetList().Count && this.stage == 2)
        {
            Debug.LogFormat($"Attempting to rotate preview for set {gameObject.name}");
            this.moveTargets();
        }
        
    }

    public void setSetupStatus(bool newStatus)
    {
        this.allowSetup = newStatus;
    }

    public void setPreviewSet(Set setToPreview)
    {
        this.previewSet = setToPreview;
        this.rotationLength = 325 * this.previewSet.GetList().Count;
        gameObject.name = this.previewSet.GetName();
    }

    public void setSetID(int id)
    {
        this.setID = id;
    }

    public bool readyToPreview()
    {
        return (this.TargetPreviewPrefab != null && this.previewSet != null);
        
    }

    public void deployImage()
    {
        if (this.targetsPreviewed.Count < this.previewSet.GetList().Count && this.stage == 0)
        {
            GameObject previewImage = Instantiate(this.TargetPreviewPrefab,gameObject.transform);
            previewImage.name = string.Format($"{this.previewSet.GetName()}_Image_{this.targetsPreviewed.Count}");
            //previewImage.transform.localPosition = new Vector3(-(this.width/2), 0, 0);
            previewImage.transform.localPosition = new Vector3((float)325*this.targetsPreviewed.Count, 0, 0);

            this.targetsPreviewed.Add(previewImage);

            if (this.targetsPreviewed.Count == this.previewSet.GetList().Count)
            {
                Debug.LogFormat($"Moving to sprite populating of preview for set {this.previewSet.GetName()}");
                this.stage = 1;
                this.deploymentCursor = -1;
            }

        }
        
    }

    public void sendSprites()
    {
        if(this.targetsPreviewed.Count == this.previewSet.GetList().Count && this.stage == 1)
        {
            Debug.LogFormat($"Sending information for target {this.targetsPreviewed[this.deploymentCursor].name}");
            this.targetsPreviewed[this.deploymentCursor].SendMessage("setDisplayImage", this.previewSet.GetList()[this.deploymentCursor].getSprite());
            this.targetsPreviewed[this.deploymentCursor].SendMessage("setOriginalDimensions", new Vector2(325, 325));

            if(this.deploymentCursor == this.previewSet.GetList().Count -1)
            {
                this.stage = 2;
                this.deploymentCursor = -1;
            }
        }
    }

    public void startRotating()
    {
        setPreviewSetPrefabToRotate(this.setID);
    }

    public static int previewSetPrefabToRotate = -1;

    public static void setPreviewSetPrefabToRotate(int previewSetCodeName)
    {
        previewSetPrefabToRotate = previewSetCodeName;
    }

    public void moveTargets()
    {
        if(this.setID == previewSetPrefabToRotate)
        {
            for (int i = 0; i < this.targetsPreviewed.Count; i++)
            {
                Vector3 currentPosition = this.targetsPreviewed[i].transform.localPosition;
                if (currentPosition.x + 10 < this.rotationLength)
                {
                    this.targetsPreviewed[i].transform.localPosition = new Vector3((currentPosition.x + 10), currentPosition.y, 0);
                }
                else if (currentPosition.x + 10 >= this.rotationLength)
                {
                    this.targetsPreviewed[i].transform.localPosition = new Vector3(-325, currentPosition.y, 0);
                }
            }
        }
    }

    public void selectSet()
    {
        if(this.setID == previewSetPrefabToRotate)
        {
            previewSetPrefabToRotate = -1;
            gameObject.transform.parent.gameObject.SendMessage("setSelected");
            //gameObject.SendMessage("switchScene");
        }
        else
        {
            this.startRotating();
            SetLibrary.selectSet(this.setID);
        }
    }
}
