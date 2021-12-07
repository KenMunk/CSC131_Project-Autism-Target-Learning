using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetLibraryViewer : MonoBehaviour
{
    public GameObject targetGrid;
    private GameObject targetViewer;
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
        this.pageCount = Mathf.CeilToInt((float)TargetGenerator.targets.Length / 27.0f);
        this.updatePageDisplay();

        this.targetViewer = Resources.Load<GameObject>("Prefab_UI/TargetViewer--ImageDisplay");
        this.cellSize = this.targetGrid.GetComponent<GridLayoutGroup>().cellSize;

        //Wait, I'm doing something wrong here...
        this.previewTargets = new List<GameObject>();
        for (int i = 0; i < 27; i++)
        {
            GameObject tempTargetViewer = Instantiate(targetViewer, targetGrid.transform);
            tempTargetViewer.GetComponent<ImageDisplay>().originalDimensions = this.cellSize;
            tempTargetViewer.SetActive(false);
            this.previewTargets.Add(tempTargetViewer);

        }

        this.filteredTargetsInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.filteredTargetsInitialized)
        {
            if (this.filteredTargets.latestRawTargets(TargetGenerator.targets))
            {
                this.filteredTargets.setRawTargets(TargetGenerator.targets);
                this.pageCount = Mathf.CeilToInt((float)this.filteredTargets.getFilteredList().Count / 27.0f);
                this.pageNumber = 0;
                this.updatePageDisplay();

            }

            if (this.filteredTargets.getSearchString() != this.searchBox.text)
            {
                this.filteredTargets.setSearchString(this.searchBox.text);
            }
        }
    }

    public void gotoNextPage()
    {
        if (this.pageNumber < this.pageCount-1)
        {
            this.pageNumber++;
            this.updatePageDisplay();
        }
    }

    public void gotoPrevPage()
    {
        if (0 > this.pageNumber)
        {
            this.pageNumber--;
            this.updatePageDisplay();
        }
    }

    public void updatePageDisplay()
    {
        int power = 1;

        while((this.pageNumber/Mathf.Pow(10,(float)power) >= 10) || (this.pageCount / Mathf.Pow(10, (float)power) >= 10))
        {
            power++;
        }

        this.pageNumberText.text = string.Format("Page {0," + power + "} of {1," + power + "}",this.pageNumber+1, this.pageCount);
    }
}
