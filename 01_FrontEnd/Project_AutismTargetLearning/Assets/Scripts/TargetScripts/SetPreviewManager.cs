using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPreviewManager : MonoBehaviour
{
    public static bool debugMode = true;
    public int setCursor = 0;

    public GameObject previewSetPrefab;

    public List<GameObject> previewSets = new List<GameObject>();

    public RectTransform windowDimensions;
    private float existingWidth = 400;
    //private float existingHeight = 400;
    // Start is called before the first frame update
    void Start()
    {
        if(this.previewSetPrefab == null)
        {
            this.previewSetPrefab = Resources.Load<GameObject>("Prefab_UI/SetPreview");
        }
        this.windowDimensions = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogFormat($"Sets counted {SetLibrary.sets.Count}");
        if(!debugMode && this.previewSets.Count < SetLibrary.sets.Count)
        {
            this.deployPreviewSet();
        }
        else if(this.previewSets.Count == SetLibrary.sets.Count && this.setCursor != SetLibrary.sets.Count)
        {
            this.prepareSet();
        }
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.existingWidth, 425 * SetLibrary.sets.Count);
        gameObject.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.existingWidth, 425 * SetLibrary.sets.Count);
    }

    public void deployPreviewSet()
    {
        GameObject newPreviewSet = Instantiate(this.previewSetPrefab,gameObject.transform);
        newPreviewSet.transform.localPosition = new Vector3(newPreviewSet.transform.localPosition.x, -200 - (410 * this.previewSets.Count));
        this.previewSets.Add(newPreviewSet);
    }

    public void prepareSet()
    {
        this.previewSets[this.setCursor].SendMessage("setPreviewSet", SetLibrary.sets[this.setCursor]);
        this.previewSets[this.setCursor].SendMessage("setSetID", this.setCursor);
        this.previewSets[this.setCursor].SendMessage("setSetupStatus", true);

        this.setCursor++;
    }

    public void disableDebugMode()
    {
        debugMode = false;
    }
}
