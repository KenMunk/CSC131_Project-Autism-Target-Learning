using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//[System.Serializable]//makes public variables visible in the editor
public class AutoDisplay : MonoBehaviour
{
    private float aspectRatio = 0;

    public Set targetSet;
    //private bool requestSent = false;

    public GameObject setError;

    public GameObject TargetViewerPrefab;

    public GameObject setTransporter;

    public List<GameObject> ViewerTiles = new List<GameObject>();
    public int cursor = 0;
    public int turns = 0;
    public bool tilesPrepared = false;

    public float desiredPadding = 50;
    public float tileSideLength = 500;
    public Rect targetViewingSpace;

    public int tableRows = 1;
    public int tableColumns = 1;
    public bool layoutDefined = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //Need to obtain the aspect ratio of the target display area
        this.targetViewingSpace = gameObject.GetComponent<RectTransform>().rect;
        aspectRatio = (Mathf.Ceil((this.targetViewingSpace.width / this.targetViewingSpace.height)*10))/10;



    }

    // Update is called once per frame
    void Update()
    {
        /* Disabling this section of code due to deprecation -- Kenneth Munk
        //check if a target set has already been selected
        if (this.setTransporter == null)
        {
            //check if the set transporter exists
            this.setTransporter = GameObject.Find("SetTransporter"); //current issue set transporter is not being found
            setError.SetActive(true);
        }
        else if (!requestSent)
        {

            //if the set transporter exists then get the set
            this.setTransporter.SendMessage("getSet", gameObject);
            setError.SetActive(true);
        }
        else if (!this.layoutDefined) { }//*/
        
        if(SetLibrary.selection() > -1)
        {
            this.targetSet = SetLibrary.getSet(SetLibrary.selection());
        }
        if (!this.layoutDefined)
        {
            this.determineSetLayout();
            setError.SetActive(false);
        }
        else if(this.ViewerTiles.Count < this.targetSet.GetList().Count)
        {
            this.deployTile();

        }
        else if (!this.tilesPrepared)
        {
            this.setImage(this.cursor);
            this.cursor++;
            if(this.cursor >= this.ViewerTiles.Count)
            {
                this.tilesPrepared = true;
            }
        }
    }

    /*disabling this code since it became deprecated --Kenneth Munk
    public void receiveSet(Set data)
    {
        Debug.LogFormat($"AutoDisplay receiving set: {data.GetName()}");
        this.targetSet = data;
        requestSent = this.targetSet != null;
    }//*/

    public void determineSetLayout()
    {
        if(this.targetSet != null)
        {
            //first we'll prime the layout with the first possible layout and then continue reducing the size
            //proportionate to the size used until the overall aspect ratio is less than the space aspect ratio
            //which would hopefully make it so that the images being presented are always large enough to be usable.
            int rows = 1;
            int columns = this.targetSet.GetList().Count;
            float rowHeight = this.targetViewingSpace.height / rows;
            this.tileSideLength = rowHeight - this.desiredPadding;

            //float tempAspectRatio = determineAspectRatio(rows, columns, rowHeight);
            float tempWidth = rowHeight * columns;
            float tempHeight = rowHeight * rows;
            //increase the rows until the columns are under the aspect ratio desireds
            while ((tempWidth > this.targetViewingSpace.width || tempHeight > this.targetViewingSpace.height))
            {
                //first try to shrink the size of the images until they can be folded over or until they meet the aspect ratio requirements
                while((tempWidth > this.targetViewingSpace.width || tempHeight > this.targetViewingSpace.height) && rowHeight > updateRowHeight(rows+1, this.targetViewingSpace.height)){

                    rowHeight -= 1;
                    this.tileSideLength = updateTileSquare(rowHeight, this.desiredPadding/2);
                    //tempAspectRatio = determineAspectRatio(rows, columns, rowHeight);
                    tempWidth = rowHeight * columns;
                    tempHeight = rowHeight * rows;

                }

                if(tempWidth > this.targetViewingSpace.width || tempHeight > this.targetViewingSpace.height)
                {
                    rows++;
                    columns = Mathf.CeilToInt(this.targetSet.GetList().Count / (float)rows);
                    rowHeight = updateRowHeight(rows, this.targetViewingSpace.height);
                    this.tileSideLength = updateTileSquare(rowHeight, this.desiredPadding / 2);
                    //tempAspectRatio = determineAspectRatio(rows, columns, rowHeight);
                }
            }

            this.tableRows = rows;
            this.tableColumns = columns;
            //this.targetViewingSpace.width = this.tableColumns * this.tileSideLength;

        }
        this.layoutDefined = true;
    }

    /*//Disabled due to lack of use
    public static float determineAspectRatio(int rows, int columns, float rowHeight)
    {
        float outputAspectRatio = 1;

        float rowLength = columns * rowHeight;

        outputAspectRatio = rowLength / (rows * rowHeight);

        return (outputAspectRatio);

    }//*/

    public static float updateRowHeight(int rows, float windowHeight)
    {
        float rowHeight = windowHeight / rows;
        return (rowHeight);

    }

    public static float updateTileSquare(float rowHeight, float paddingThickness)
    {
        float outputTileSide = rowHeight - (2 * paddingThickness);
        return (outputTileSide);
    }

    public void deployTile()
    {
        if(this.ViewerTiles.Count < this.targetSet.GetList().Count)
        {
            int cursor = this.ViewerTiles.Count;
            int currentColumn = (cursor % this.tableColumns);
            int currentRow = (cursor-currentColumn)/this.tableColumns;

            Vector2 startingOffsets = new Vector2(-((this.tileSideLength + this.desiredPadding)*this.tableColumns / 2) + ((this.tileSideLength+this.desiredPadding * 2) /2),
                                                  ((this.targetViewingSpace.height / 2) - ((this.tileSideLength + this.desiredPadding*2) / 2)));

            GameObject newTile = Instantiate(TargetViewerPrefab, gameObject.transform);
            newTile.transform.localPosition = new Vector3(startingOffsets.x + (this.tileSideLength * currentColumn), 
                                                          startingOffsets.y - (this.tileSideLength * currentRow),
                                                          0);
            this.ViewerTiles.Add(newTile);
        }
    }

    public void setImage(int cursor)
    {
        Sprite targetSprite = this.targetSet.GetList()[(this.cursor - this.turns)%this.targetSet.GetList().Count].getSprite();
        this.ViewerTiles[cursor].SendMessage("setOriginalDimensions", new Vector2(this.tileSideLength-this.desiredPadding, this.tileSideLength - this.desiredPadding));
        if (targetSprite != null)
        {
            this.ViewerTiles[cursor].SendMessage("setDisplayImage", targetSprite);
        }
        else
        {
            Debug.LogErrorFormat($"ERROR: Target {this.targetSet.GetList()[cursor].getName()} does not have a sprite assigned\n local files not supported, please use app resource files until further notice");
        }
    }

    public void rotateTargets()
    {
        this.turns++;
        this.cursor = 0;
        this.tilesPrepared = false;
    }
}
