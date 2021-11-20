using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPreview : MonoBehaviour
{
    public Set previewSet;

    public GameObject TargetPreviewPrefab;

    public List<GameObject> targetsPreviewed = new List<GameObject>();

    private int deploymentCursor = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.TargetPreviewPrefab = Resources.Load<GameObject>("Prefab_UI/TargetViewer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPreviewSet(Set setToPreview)
    {
        this.previewSet = setToPreview;
    }

    public bool readyToPreview()
    {
        return (this.TargetPreviewPrefab != null && this.previewSet != null);
        
    }

    public void deployImage()
    {
        if (this.deploymentCursor < this.previewSet.GetList().Count)
        {
            GameObject previewImage = Instantiate(this.TargetPreviewPrefab);
            previewImage.name = string.Format($"{this.previewSet.GetName()}_Image_{this.deploymentCursor}");
            previewImage.transform.localPosition = new Vector3(150 + (325 * deploymentCursor), -200, 0);
            this.deploymentCursor++;
        }
    }

    public void sendSprites()
    {

    }
}
