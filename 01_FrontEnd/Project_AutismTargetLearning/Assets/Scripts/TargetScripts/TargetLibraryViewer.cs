using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetLibraryViewer : MonoBehaviour
{
    public GameObject targetGrid;
    private GameObject targetViewer;
    public Vector2 cellSize;

    public Target[] lastList;
    public List<GameObject> previewTargets = new List<GameObject>();
    public int cursor = 0;

    // Start is called before the first frame update
    void Start()
    {
        TargetGenerator.loadTargets();
        this.targetViewer = Resources.Load<GameObject>("Prefab_UI/TargetViewer--ImageDisplay");
    }

    // Update is called once per frame
    void Update()
    {
        if(this.lastList != TargetGenerator.targets)
        {
            this.cellSize = targetGrid.GetComponent<GridLayoutGroup>().cellSize;
            this.lastList = TargetGenerator.targets;
            this.previewTargets = new List<GameObject>();

            //Wait, I'm doing something wrong here...
            for(int i = 0; i<this.lastList.Length; i++)
            {
                GameObject tempTargetViewer = Instantiate(targetViewer,targetGrid.transform);
                tempTargetViewer.GetComponent<ImageDisplay>().originalDimensions = this.cellSize;
                tempTargetViewer.GetComponent<ImageDisplay>().targetImage = this.lastList[i].getSprite();

            }
        }
    }
}
