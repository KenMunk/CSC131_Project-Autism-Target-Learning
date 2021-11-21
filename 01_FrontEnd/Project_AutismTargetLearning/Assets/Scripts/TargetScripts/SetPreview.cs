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
    private float height = 400;

    private int deploymentCursor = 0;

    private bool allowSetup = false;
    // Start is called before the first frame update
    void Start()
    {
        this.TargetPreviewPrefab = Resources.Load<GameObject>("Prefab_UI/TargetViewer");
        this.height = gameObject.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.allowSetup)
        {
            this.deployImage();
            this.sendSprites();
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
    }

    public bool readyToPreview()
    {
        return (this.TargetPreviewPrefab != null && this.previewSet != null);
        
    }

    public void deployImage()
    {
        if (this.deploymentCursor < this.previewSet.GetList().Count && this.stage == 0)
        {
            GameObject previewImage = Instantiate(this.TargetPreviewPrefab);
            previewImage.name = string.Format($"{this.previewSet.GetName()}_Image_{this.deploymentCursor}");
            previewImage.transform.localPosition = new Vector3(150 + (325 * deploymentCursor), (-2)*((this.height - 150)/3), 0);
            this.deploymentCursor++;

            if(this.deploymentCursor < this.previewSet.GetList().Count)
            {
                this.stage = 1;
            }

            this.targetsPreviewed.Add(previewImage);
        }
        
    }

    public void sendSprites()
    {
        if(this.deploymentCursor < this.previewSet.GetList().Count && this.stage == 1)
        {
            this.targetsPreviewed[this.deploymentCursor].SendMessage("setDisplayImage", this.previewSet.GetList()[this.deploymentCursor]);
            this.targetsPreviewed[this.deploymentCursor].SendMessage("setOriginalDimensions", new Vector2(this.height - 150, this.height - 150));
            this.deploymentCursor++;

            if(this.deploymentCursor < this.previewSet.GetList().Count)
            {
                this.stage = 2;
            }
        }
    }

    public void startRotating()
    {
        setPreviewSetPrefabToRotate(gameObject.name);
    }

    public static string previewSetPrefabToRotate = "";

    public static void setPreviewSetPrefabToRotate(string previewSetCodeName)
    {
        previewSetPrefabToRotate = previewSetCodeName;
    }

    public void moveTargets()
    {
        if(gameObject.name == previewSetPrefabToRotate)
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
                    this.targetsPreviewed[i].transform.localPosition = new Vector3(-150, currentPosition.y, 0);
                }
            }
        }
    }
}
