using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetLibraryViewer : MonoBehaviour
{
    public GameObject targetGrid;
    private GameObject targetViewer;
    private GameObject targetButton;
    public GameObject previewWindow;
    public Text pageNumberText;
    public InputField searchBox;
    public Vector2 cellSize;

    public TargetFilter filteredTargets;
    bool filteredTargetsInitialized = false;

    //The targets will be divided into pages 
    public int pageCount = 0;
    public int pageNumber = 0;
    public int lastPageNumber = 0;
    public List<GameObject> previewTargets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        TargetGenerator.loadTargets();
        this.filteredTargets = new TargetFilter(TargetGenerator.targets);
        this.pageCount = Mathf.CeilToInt((float)this.filteredTargets.getFilteredList().Count / 27.0f);
        this.updatePageDisplay();

        this.targetButton = Resources.Load<GameObject>("Prefab_UI/TargetButton");
        this.targetViewer = Resources.Load<GameObject>("Prefab_UI/TargetViewer--ImageDisplay");
        this.cellSize = this.targetGrid.GetComponent<GridLayoutGroup>().cellSize;

        //Wait, I'm doing something wrong here...
        this.previewTargets = new List<GameObject>();
        for (int i = 0; i < 27; i++)
        {
            GameObject tempTargetButton = Instantiate(this.targetButton, this.targetGrid.transform);

            GameObject tempTargetViewer = Instantiate(this.targetViewer, tempTargetButton.transform);

            tempTargetButton.GetComponent<ClickMessager>().targetGameobject = tempTargetViewer;
            tempTargetButton.GetComponent<ClickMessager>().methodName = "sendPreview";

            tempTargetViewer.GetComponent<ImageDisplay>().previewWindow = this.previewWindow;
            tempTargetViewer.GetComponent<ImageDisplay>().originalDimensions = this.cellSize;
            tempTargetViewer.SetActive(false);
            this.previewTargets.Add(tempTargetViewer);

        }
        this.updateTargetsPage();

        this.filteredTargetsInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.filteredTargetsInitialized)
        {
            if (!this.filteredTargets.latestRawTargets(TargetGenerator.targets))
            {
                this.filteredTargets.setRawTargets(TargetGenerator.targets);
                this.pageCount = Mathf.CeilToInt((float)this.filteredTargets.getFilteredList().Count / 27.0f);
                this.pageNumber = 0;
                this.updatePageDisplay();
                this.updateTargetsPage();

            }

            if (this.filteredTargets.getSearchString() != this.searchBox.text)
            {
                this.filteredTargets.setSearchString(this.searchBox.text);
                this.updateTargetsPage();
                this.updatePageDisplay();
                this.pageNumber = 0;
            }
        }
    }

    public void gotoNextPage()
    {
        Debug.Log("Page change button detected");
        if (this.pageNumber < this.pageCount-1)
        {
            Debug.LogFormat($"Opening page {this.pageNumber}");
            this.pageNumber++;
            this.updatePageDisplay();
            this.updateTargetsPage();
        }
        //this.pageNumber++;
    }

    public void gotoLastPage()
    {
        if (0 < this.pageNumber)
        {
            Debug.LogFormat($"Opening page {this.pageNumber}");
            this.pageNumber--;
            this.updatePageDisplay();
            this.updateTargetsPage();
        }
    }

    public void updatePageDisplay()
    {
        this.pageCount = Mathf.CeilToInt((float)this.filteredTargets.getFilteredList().Count / 27.0f);

        int power = 1;

        while((this.pageNumber/Mathf.Pow(10,(float)power) >= 10) || (this.pageCount / Mathf.Pow(10, (float)power) >= 10))
        {
            power++;
        }

        this.pageNumberText.text = string.Format("Page {0," + power + "} of {1," + power + "}",this.pageNumber+1, this.pageCount);
    }

    public void updateTargetsPage()
    {
        for(int i = 0; i<27; i++)
        {
            int targetID = this.pageNumber * 27 + i;
            bool inList = (targetID) < this.filteredTargets.getFilteredList().Count;
            this.previewTargets[i].SetActive(inList);
            if (inList)
            {
                this.previewTargets[i].GetComponent<ImageDisplay>().targetImage = this.filteredTargets.getFilteredList()[targetID].getSprite();
                this.previewTargets[i].SendMessage("updateMultipliers");
            }
        }
    }
}
