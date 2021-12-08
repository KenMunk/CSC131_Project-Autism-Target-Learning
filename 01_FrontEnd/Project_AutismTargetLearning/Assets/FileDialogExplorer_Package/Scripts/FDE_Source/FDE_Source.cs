using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Video;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Additional FDE Extensions
/// </summary>
internal static class FDEExtensions
{
    /// <summary>
    /// Set text for multiple text-libraries of included object
    /// </summary>
    public static void SetText(this GameObject src, string txt)
    {
        if (src.GetComponent<Text>())
            src.GetComponent<Text>().text = txt;
        else if (src.GetComponent<TextMeshProUGUI>())
            src.GetComponent<TextMeshProUGUI>().text = txt;
        else if (src.GetComponent<TextMeshPro>())
            src.GetComponent<TextMeshPro>().text = txt;
    }

    /// <summary>
    /// Set text activation for multiple text-libraries of included object
    /// </summary>
    public static void SetTextActive(this GameObject src, bool active)
    {
        if (src.GetComponent<Text>())
            src.GetComponent<Text>().enabled = active;
        else if (src.GetComponent<TextMeshProUGUI>())
            src.GetComponent<TextMeshProUGUI>().enabled = active;
        else if (src.GetComponent<TextMeshPro>())
            src.GetComponent<TextMeshPro>().enabled = active;
    }

    /// <summary>
    /// Set input field text for multiple inputfield-libraries of included object
    /// </summary>
    public static void SetInputFieldText(this GameObject src, string txt)
    {
        if (src.GetComponent<InputField>())
            src.GetComponent<InputField>().text = txt;
        else if (src.GetComponent<TMP_InputField>())
            src.GetComponent<TMP_InputField>().text = txt;
    }

    /// <summary>
    /// Set input field event for multiple inputfield-libraries of included object
    /// </summary>
    public static void SetInputFieldEvent(this GameObject src, UnityAction<string> act)
    {
        if (src.GetComponent<InputField>())
            src.GetComponent<InputField>().onEndEdit.AddListener(act);
        else if (src.GetComponent<TMP_InputField>())
            src.GetComponent<TMP_InputField>().onEndEdit.AddListener(act);
    }

    /// <summary>
    /// Get input field text for multiple inputfield-libraries of included object
    /// </summary>
    public static string GetInputFieldText(this GameObject src)
    {
        if (src.GetComponent<InputField>())
            return src.GetComponent<InputField>().text;
        else if (src.GetComponent<TMP_InputField>())
            return src.GetComponent<TMP_InputField>().text;
        else
            return "";
    }
}

/// <summary>
/// File Dialog Explorer Written by Matej Vanco (originally 08/08/2017, Last Update 04/09/2021 [dd/mm/yyyy]).
/// Please do not change the UI hierarchy as the system fully depends on it. Thank you!
/// Any questions go to https://matejvanco.com/contact
/// </summary>
[AddComponentMenu("Matej Vanco/File Dialog Explorer")]
[System.Serializable]
public class FDE_Source : MonoBehaviour 
{
    //If enabled, the main path will be set to default App Startup
    public bool DefaultStartup_ApplicationStartUp = true;
    //If enabled, the system will be ready for mobile platforms
    public bool MobilePlatform = false;
    public string MainPath = "C:/";

    //If enabled, the dialog gameObject will show up
    public bool EnableDialogAfterStart = false;
    //If enabled, the dialog gameObject will remain activated after action process
    public bool KeepDialogAfterAction = false;
    //If enabled, user will be able to use Right Mouse Button to create, edit or copy files/ folders in drives
    public bool EnableDataCustomization = true;
    //[Recommended: enabled] If enabled, you won't be able to manipulate with exist files/folders in your computer. But you will be able to manipulate with files/folders created in Dialog Explorer by you
    public bool HighProtectionLevel = true;

    //If enabled, user will be able to use history dialog
    public bool EnableHistoryDialog = true;
    //If enabled, the History Dialog will be shown on application startup
    public bool ShowHistoryDialogOnStart = true;
    //If enabled, the generated history folders will contain just their names without full path
    public bool ShowHistoryFoldersNameOnly = true;
    //Maximum amount of the recently opened folders
    public int MaxStoredHistoryFolders = 25;

    //If enabled, the loading panel with Cancel button will be shown while loading large folders
    public bool ShowLoadingPanel = true;

    public enum FileAction : int 
    {
        Open = 0, 
        OpenInExplorer = 1, 
        Text_ReadToVariable = 2, 
        Text_ReadTo3DText = 3, 
        Text_ReadToUIText = 4, 
        Text_ReadToUGUITextMeshPro = 5,
        Image_ReadImageToSprite = 6, 
        Image_ReadImageToUIImage = 7, 
        Image_ReadImageToRenderer = 8,
        Video_ReadVideoToVideoPlayer = 9,
        CustomEvent = 10
    };

    public enum ReadType : int
    {
        ReadFileContent,
        ReadFileName,
        ReadFileNameWithoutExtension,
        ReadFileExtensionOnly,
        ReadFullFilePath,
        ReadFullFilePathWithoutFileName,
        ReadFileSizeInBytes,
        ReadFileSizeInKilobytes,
        ReadFileSizeInMegabytes
    };

    //Default icon for 'default' files
    public Sprite ICON_Files;
    //Default icon for 'default' directories/folders
    public Sprite ICON_Folders;
    //When images are too big, it may take a longer time to load... Set the size of maximum image file size into dialog[default 1024 kb = 1 mb]. Otherwise the image will be replaced by the image below...
    public int MaxImageDisplaySize = 1024;
    //Default icon for 'over-sized' image files
    public Sprite ICON_DefaultImageHolder;
    //Default extension while creating a new file WITHOUT dot!
    public string DefaultExtension = "txt";

    //Use custom font in the File Dialog
    public bool useCustomFont = false;
    public Font customFont;

    [System.Serializable]
    public class _RegisteredExtensions
    {
        [Header("Extension Name")]
        [Tooltip("Write extension WITHOUT dot (txt, exe, png, bmp etc)")]
        public string Extension = "txt";
        [Header("Extension Icon")]
        public Sprite Icon;
    }

    //If enabled, files that contains one of the registered extensions will show up, other files will be hidden
    public bool ShowFilesWithRegisteredExtensionsOnly = false;
    public _RegisteredExtensions[] RegisteredExtensions;

    //THIS script should be added to the actual FDE object
    private GameObject FDE_SourceObject;
    //Prefab of generated files in FDE
    public GameObject FDE_ItemPrefab;

    //After-Click action
    public FileAction File_Action = FileAction.Open;
    //Further ReadType action after-click
    public ReadType Read_Type = ReadType.ReadFileContent;

    //Ignore specific directories to avoid further issues
    protected List<string> Disallowed_Folders = new List<string> { "$recycle.bin", "system volume information", "documents and settings", "recovery", "hiberfil", "pagefile" };

    //----------Script content - UI requirements (please do not change the hierarchy order)
    #region UI Content
    private GameObject UI_FullPath;
    private Button UI_BackButton;
    private Button UI_BackParentButton;
    private RectTransform UI_DialogContent;
    private GameObject UI_Info;
    private Slider UI_DialogSize;
    private GameObject UI_DialogSizeInfo;
    private Dropdown UI_Drivers;
    private TMP_Dropdown UI_DriversTMP;

    private GameObject UI_LoadingPanel;
    private Slider UI_LoadingPanel_Progress;
    private GameObject UI_LoadingPanel_ProgressText;
    private Button UI_LoadingPanel_Cancel;

    private GameObject UI_ScrollDialog;
    private GameObject UI_DialogInfo;
    private Button UI_ScrollDialog_Copy;
    private Button UI_ScrollDialog_Paste;
    private Button UI_ScrollDialog_Duplicate;
    private Button UI_ScrollDialog_Delete;
    private Button UI_ScrollDialog_CreateFile;
    private Button UI_ScrollDialog_CreateFolder;
    private Button UI_ScrollDialog_Delete2;
    private GameObject UI_ScrollDialog_CreatingSomethingInputField;
    private Button UI_ScrollDialog_AcceptInputField;
    private Button UI_ScrollDialog_Rename;
    private Button UI_ScrollDialog_Rename2;
    private GameObject UI_ScrollDialog_ProtectionQuestion;

    private Transform UI_HistoryDialog;
    private Transform UI_History_Content;
    private Transform UI_History_ItemPrefab;
    private Transform UI_History_OpenHistory;

    private bool UIPassed = false;
    #endregion

    //----------Internal Functions
    #region Internal Variables
    private readonly List<string> listOfPaths = new List<string>();
    private readonly List<string> listOfCreatedFileFolders = new List<string>();
    private string SelectedPath { get; set; }
    private string PathToPaste { get; set; }
    #endregion

    //----------Action Requirements
    #region Required Actions
    public TextMesh ActionOBJ_ReadTo3DText;
    public TextMeshProUGUI ActionOBJ_ReadToUGUITMP;
    public Text ActionOBJ_ReadToUIText;
    public SpriteRenderer ActionOBJ_ReadToSprite;
    public Image ActionOBJ_ReadToUIImage;
    public Renderer ActionOBJ_ReadToRenderer;
    public VideoPlayer ActionOBJ_VideoPlayer;

    public MonoBehaviour ActionOBJ_ReadToVariableMonoBeh;
    public string ActionOBJ_ReadToVariableVar;

    public UnityEvent Action_CustomEvent;
    #endregion

    //----------Start - Setting up dialog
    private void Awake () 
    {
        //Checking content 'n internal stuff
        FDE_SourceObject = this.gameObject;
        if (!FDE_ItemPrefab)
        {
            UnityEngine.Debug.LogError("File Dialog Explorer - Error - FDE_ItemPrefab is missing. FDE has been deleted.");
            DestroyImmediate(this);
        }

        UIPassed = A_Internal_GetUI();
        if (!UIPassed)
        {
            UnityEngine.Debug.LogError("File Dialog Explorer - Error - The UI didn't pass. FDE_SourceObject doesn't exist or the content is missing. FDE has been deleted.");
            DestroyImmediate(this);
        }

        //Initialize internal functions
        A_Internal_InitializeFunctions();

        //Setting up the maximum history folders
        MaxStoredHistoryFolders = MaxStoredHistoryFolders > 100 ? 100 : MaxStoredHistoryFolders; //---You can change the value, but this is recommended due to the performance drop

        //Setting up the main path depending on device target
        if (DefaultStartup_ApplicationStartUp) MainPath = MobilePlatform ? Application.persistentDataPath : Application.dataPath;

        listOfPaths.Add(MainPath);
        A_Internal_RefreshContent();

        if (!EnableDialogAfterStart)
            Action_CLOSE_DIALOG();
	}

    //----------Public functions
    #region Actions - Public
    /// <summary>
    /// Show dialog panel with starter path (leave it empty if the starter path is already defined)
    /// </summary>
    public void Action_SHOW_DIALOG(string StarterPath = "")
    {
        if (!string.IsNullOrEmpty(StarterPath))
            MainPath = StarterPath;

        FDE_SourceObject.SetActive(true);
        A_Internal_RefreshContent();
    }
    /// <summary>
    /// Close dialog panel
    /// </summary>
    public void Action_CLOSE_DIALOG()
    {
        FDE_SourceObject.SetActive(false);
    }

    /// <summary>
    /// Change click file action by enum (Read to text? Read to renderer?...)
    /// </summary>
    public void Action_ChangeClickFileAction(FileAction Action)
    {
        File_Action = Action;
    }
    /// <summary>
    /// Change click file action by index (Read to text? Read to renderer?...)
    /// </summary>
    public void Action_ChangeClickFileAction(int ActionIndex)
    {
        File_Action = (FileAction)ActionIndex;
    }

    /// <summary>
    /// Change read file type by enum (Read file content? Read just file name?...)
    /// </summary>
    public void Action_ChangeReadFileAction(ReadType readType)
    {
        Read_Type = readType;
    }
    /// <summary>
    /// Change read file type by index (Read file content? Read just file name?...)
    /// </summary>
    public void Action_ChangeReadFileAction(int readIndex)
    {
        Read_Type = (ReadType)readIndex;
    }
    #endregion


    //----------Movement of the dialog
    #region Dialog Drag Drop
    private float XOff = 0.0f;
    private float YOff = 0.0f;
    private void A_Internal_UI_BeginDrag()
    {
        XOff = FDE_SourceObject.transform.position.x - Input.mousePosition.x;
        YOff = FDE_SourceObject.transform.position.y - Input.mousePosition.y;
    }
    private void A_Internal_UI_Drag()
    {
        FDE_SourceObject.transform.position = new Vector3(XOff+Input.mousePosition.x,YOff+Input.mousePosition.y,0);
    }
    #endregion

    //----------Correction of Dialog UI & Functionality
    #region Internal - Set Up Dialog Content
    private bool A_Internal_GetUI()
    {
        try
        {
            UI_DialogInfo = FDE_SourceObject.transform.Find("DialogInfo").gameObject;

            UI_FullPath = FDE_SourceObject.transform.Find("FullPath").gameObject;
            UI_BackButton = FDE_SourceObject.transform.Find("BackButton").GetComponent<Button>();
            UI_BackParentButton = FDE_SourceObject.transform.Find("BackParentButton").GetComponent<Button>();
            UI_DialogContent = FDE_SourceObject.transform.Find("PCContent").transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            UI_Info = FDE_SourceObject.transform.Find("FileInfo").gameObject;
            UI_DialogSize = FDE_SourceObject.transform.Find("FilesSize").GetComponent<Slider>();
            UI_DialogSizeInfo = FDE_SourceObject.transform.Find("FilesSize").transform.Find("Text").gameObject;
            UI_Drivers = FDE_SourceObject.transform.Find("Drivers").GetComponent<Dropdown>();
            UI_DriversTMP = FDE_SourceObject.transform.Find("Drivers").GetComponent<TMP_Dropdown>();

            UI_ScrollDialog = FDE_SourceObject.transform.Find("ScrollDialog").gameObject;
            UI_ScrollDialog_Copy = UI_ScrollDialog.transform.Find("File").transform.Find("Copy").GetComponent<Button>();
            UI_ScrollDialog_Paste = UI_ScrollDialog.transform.Find("Folder").transform.Find("Paste").GetComponent<Button>();
            UI_ScrollDialog_Duplicate = UI_ScrollDialog.transform.Find("File").transform.Find("Duplicate").GetComponent<Button>();
            UI_ScrollDialog_Delete = UI_ScrollDialog.transform.Find("File").transform.Find("Delete").GetComponent<Button>();

            UI_ScrollDialog_CreateFile = UI_ScrollDialog.transform.Find("Folder").transform.Find("Create File").GetComponent<Button>();
            UI_ScrollDialog_CreateFolder = UI_ScrollDialog.transform.Find("Folder").transform.Find("Create Folder").GetComponent<Button>();
            UI_ScrollDialog_Delete2 = UI_ScrollDialog.transform.Find("Folder").transform.Find("Delete").GetComponent<Button>();

            UI_ScrollDialog_CreatingSomethingInputField = UI_ScrollDialog.transform.Find("Input").gameObject;
            UI_ScrollDialog_AcceptInputField = UI_ScrollDialog_CreatingSomethingInputField.transform.Find("Accept").GetComponent<Button>();

            UI_ScrollDialog_Rename = UI_ScrollDialog.transform.Find("File").transform.Find("Rename").GetComponent<Button>();
            UI_ScrollDialog_Rename2 = UI_ScrollDialog.transform.Find("Folder").transform.Find("Rename").GetComponent<Button>();

            UI_HistoryDialog = FDE_SourceObject.transform.Find("HistoryPanel");
            UI_History_Content = FDE_SourceObject.transform.Find("HistoryPanel").Find("Scroll View").transform.GetChild(0).GetChild(0);
            UI_History_ItemPrefab = FDE_SourceObject.transform.Find("HistoryPanel").Find("PrefabButton");
            UI_History_ItemPrefab.gameObject.SetActive(false);
            UI_History_OpenHistory = FDE_SourceObject.transform.Find("OpenHistory");
            UI_HistoryDialog.gameObject.SetActive(false);
            if (!EnableHistoryDialog)
                UI_History_OpenHistory.gameObject.SetActive(false);
            else if (ShowHistoryDialogOnStart)
                UI_HistoryDialog.gameObject.SetActive(true);

            UI_LoadingPanel = FDE_SourceObject.transform.Find("LoadingPanel").gameObject;
            UI_LoadingPanel_Progress = UI_LoadingPanel.transform.Find("LoadingValue").GetComponent<Slider>();
            UI_LoadingPanel_ProgressText = UI_LoadingPanel.transform.Find("Text").gameObject;
            UI_LoadingPanel_Cancel = UI_LoadingPanel.transform.Find("Cancel").GetComponent<Button>();
            UI_LoadingPanel.SetActive(false);

            UI_ScrollDialog_ProtectionQuestion = UI_ScrollDialog.transform.Find("ProtectionQuestion").gameObject;

            UI_ScrollDialog_ProtectionQuestion.SetActive(false);
            UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(false);

            UI_ScrollDialog.SetActive(false);

            if(useCustomFont && customFont)
            {
                foreach (Text t in GetComponentsInChildren<Text>(true))  t.font = customFont;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private void A_Internal_InitializeFunctions()
    {
        UI_FullPath.SetInputFieldEvent(delegate
        {
            A_Internal_SearchByAddress();
        });

        UI_BackButton.onClick.AddListener(delegate
        {
            A_Internal_Back();
        });

        UI_BackParentButton.onClick.AddListener(delegate
        {
            A_Internal_BackToParent();
        });

        UI_DialogSize.onValueChanged.AddListener(delegate
        {
            A_Internal_ChangeSize();
        });

        if (UI_Drivers)
        {
            UI_Drivers.ClearOptions();

            foreach (string d in Directory.GetLogicalDrives())
            {
                if (Directory.Exists(d))
                    UI_Drivers.options.Add(new Dropdown.OptionData() { text = d });
            }

            UI_Drivers.onValueChanged.AddListener(delegate
            {
                A_Internal_ChangeDriver();
            });
            UI_Drivers.captionText.text = UI_Drivers.options[0].text;
            UI_Drivers.value = 0;

            if (string.IsNullOrEmpty(MainPath) || !Directory.Exists(MainPath))
                MainPath = UI_Drivers.captionText.text;
        }
        if (UI_DriversTMP)
        {
            UI_DriversTMP.ClearOptions();

            foreach (string d in Directory.GetLogicalDrives())
            {
                if (Directory.Exists(d))
                    UI_DriversTMP.options.Add(new TMP_Dropdown.OptionData() { text = d });
            }

            UI_DriversTMP.onValueChanged.AddListener(delegate
            {
                A_Internal_ChangeDriver();
            });
            UI_DriversTMP.captionText.text = UI_DriversTMP.options[0].text;
            UI_DriversTMP.value = 0;

            if (string.IsNullOrEmpty(MainPath) || !Directory.Exists(MainPath))
                MainPath = UI_DriversTMP.captionText.text;
        }

        EventTrigger.Entry e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.PointerClick;
        e.callback.AddListener(delegate
        {
            if (EnableDataCustomization)
                A_Internal_RefreshProtectionList();
            A_Internal_InitializeScrollDia("");
        });
        EventTrigger.Entry e2 = new EventTrigger.Entry();
        e2.eventID = EventTriggerType.Drag;
        e2.callback.AddListener(delegate
        {
            A_Internal_UI_Drag();
        });
        EventTrigger.Entry e3 = new EventTrigger.Entry();
        e3.eventID = EventTriggerType.BeginDrag;
        e3.callback.AddListener(delegate
        {
            A_Internal_UI_BeginDrag();
        });
        FDE_SourceObject.transform.Find("PCContent").GetComponent<EventTrigger>().triggers.Add(e);
        FDE_SourceObject.transform.Find("PCContent").GetComponent<EventTrigger>().triggers.Add(e2);
        FDE_SourceObject.transform.Find("PCContent").GetComponent<EventTrigger>().triggers.Add(e3);

        UI_ScrollDialog_Copy.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(0);
        });
        UI_ScrollDialog_Paste.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(1);
        });
        UI_ScrollDialog_Duplicate.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(2);
        });
        UI_ScrollDialog_Delete.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(3);
        });



        UI_ScrollDialog_CreateFile.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(4);
        });
        UI_ScrollDialog_CreateFolder.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(5);
        });
        UI_ScrollDialog_Delete2.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(3);
        });
        UI_ScrollDialog_Rename.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(6);
        });
        UI_ScrollDialog_Rename2.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_ScrollButton(7);
        });


        UI_ScrollDialog_AcceptInputField.GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_EnterInputField();
        });


        UI_ScrollDialog_ProtectionQuestion.transform.Find("Yes").GetComponent<Button>().onClick.AddListener(delegate
        {
            A_Internal_EnterInputField();
        });
        UI_ScrollDialog_ProtectionQuestion.transform.Find("No").GetComponent<Button>().onClick.AddListener(delegate
        {
            UI_ScrollDialog_ProtectionQuestion.SetActive(false);
        });


        UI_LoadingPanel_Cancel.onClick.AddListener(delegate
        {
            StopAllCoroutines();
            UI_LoadingPanel.SetActive(false);
        });

    }
    #endregion

    //----------DIALOG Content Refresher [Back, Add file etc]
    #region Dialog Content Refresher
    private void A_Internal_RefreshContent()
    {
        if (File_Action == FileAction.Open || File_Action == FileAction.OpenInExplorer)
            UI_DialogInfo.SetText("Browse Files & Folders");
        else if (File_Action == FileAction.Image_ReadImageToRenderer || File_Action == FileAction.Image_ReadImageToSprite || File_Action == FileAction.Image_ReadImageToUIImage)
            UI_DialogInfo.SetText("Select Image File");
        else if (File_Action == FileAction.Text_ReadTo3DText || File_Action == FileAction.Text_ReadToUIText || File_Action == FileAction.Text_ReadToVariable)
            UI_DialogInfo.SetText("Select Text File");

        bool ImFile = A_Internal_GetFileType_ImFile(MainPath);

        UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(false);

        if (ImFile)
        {
            if (!File.Exists(MainPath))
                FDE_Error("File doesn't exist");
        }
        else
        {
            if (!Directory.Exists(MainPath))
                FDE_Error("Directory doesn't exist");
        }

        UI_FullPath.SetInputFieldText(MainPath);

        //---------------------------Generating files & folders
        StopAllCoroutines();
        StartCoroutine(A_Internal_RefreshAsyncContent());
       
        if(EnableHistoryDialog)
        {
            if (UI_History_Content.transform.childCount > 0)
                for (int i = UI_History_Content.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(UI_History_Content.transform.GetChild(i).gameObject);
                }

            int c = 0;
            List<string> copy = new List<string>(listOfPaths);
            foreach(string s in copy)
            {
                if(Directory.Exists(s) == false)
                {
                    listOfPaths.RemoveAt(c);
                    continue;
                }
                GameObject newButton = Instantiate(UI_History_ItemPrefab.gameObject, UI_History_Content);
                newButton.gameObject.SetActive(true);
                newButton.name = c.ToString();

                Text t = newButton.GetComponentInChildren<Text>();
                TextMeshProUGUI tmp = newButton.GetComponentInChildren<TextMeshProUGUI>();

                if (!ShowHistoryFoldersNameOnly)
                {
                    if (t) t.text = s;
                    else if (tmp) tmp.text = s;
                }
                else
                {
                    if (t) t.text = string.IsNullOrEmpty(Path.GetFileName(s)) ? s : Path.GetFileName(s);
                    else if (tmp) tmp.text = string.IsNullOrEmpty(Path.GetFileName(s)) ? s : Path.GetFileName(s);
                }
                newButton.GetComponent<Button>().onClick.AddListener(delegate { A_Internal_Back(int.Parse(newButton.name, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture)); });
                c++;
            }
        }

        UI_DialogContent.transform.localScale = Vector3.one;
        UI_DialogContent.transform.localRotation = Quaternion.identity;
        UI_DialogContent.transform.localPosition = Vector3.zero;

        return;
    }

    private IEnumerator A_Internal_RefreshAsyncContent()
    {
        if (ShowLoadingPanel)
            UI_LoadingPanel.SetActive(true);

        if (UI_DialogContent.transform.childCount > 0)
        {
            UI_LoadingPanel_Progress.minValue = 0;
            UI_LoadingPanel_Progress.maxValue = UI_DialogContent.transform.childCount;
            for (int i = UI_DialogContent.transform.childCount - 1; i >= 0; i--)
            {
                UI_LoadingPanel_Progress.value = i;
                UI_LoadingPanel_ProgressText.SetText("Refreshing " + i.ToString() + "/" + UI_LoadingPanel_Progress.maxValue.ToString());
                Destroy(UI_DialogContent.transform.GetChild(i).gameObject);
                yield return null;
            }
        }

        UI_LoadingPanel_Progress.minValue = 0;
        UI_LoadingPanel_Progress.maxValue = Directory.GetDirectories(MainPath).Length;

        //-----------------------------------------------------------------Generating Folders
        for (int i = 0; i < Directory.GetDirectories(MainPath).Length; i++)
        {
            UI_LoadingPanel_Progress.value = i;
            UI_LoadingPanel_ProgressText.SetText("Loading Folders " + i.ToString() + "/" + Directory.GetDirectories(MainPath).Length.ToString());
            string currentPath = Directory.GetDirectories(MainPath)[i];
            try
            {
                if (Disallowed_Folders.Contains(Path.GetFileName(currentPath).ToLower()))
                    continue;
                try
                {
                    string testAccess = (Directory.GetDirectories(currentPath).Length).ToString();
                }
                catch { continue; }

                GameObject newDirectory = Instantiate(FDE_ItemPrefab, UI_DialogContent.transform);
                newDirectory.transform.Find("Icon").GetComponent<RawImage>().texture = ICON_Folders.texture;
                newDirectory.transform.Find("Name").gameObject.SetText(Path.GetFileName(currentPath));
                if (useCustomFont && customFont && newDirectory.transform.Find("Name").GetComponent<Text>())
                    newDirectory.transform.Find("Name").GetComponent<Text>().font = customFont;
                newDirectory.transform.Find("Path").gameObject.SetText(currentPath);
                newDirectory.GetComponentInChildren<Button>().onClick.AddListener(delegate
                {
                    A_ProcessFile(newDirectory.transform.Find("Path").GetComponent<Text>() ? newDirectory.transform.Find("Path").GetComponent<Text>().text :
                        newDirectory.transform.Find("Path").GetComponent<TextMeshProUGUI>().text);
                });

                FDE_CustomEventTrigger cE = newDirectory.GetComponentInChildren<Button>().gameObject.AddComponent<FDE_CustomEventTrigger>();
                cE.OnEnter = delegate
                {
                    A_GetInfo(newDirectory.transform.Find("Path").GetComponent<Text>() ? newDirectory.transform.Find("Path").GetComponent<Text>().text :
                        newDirectory.transform.Find("Path").GetComponent<TextMeshProUGUI>().text);
                };
                cE.OnExit = delegate
                {
                    UI_Info.SetTextActive(false);
                };
                cE.OnClick = delegate
                {
                    A_Internal_InitializeScrollDia(newDirectory.transform.Find("Path").GetComponent<Text>() ? newDirectory.transform.Find("Path").GetComponent<Text>().text :
                        newDirectory.transform.Find("Path").GetComponent<TextMeshProUGUI>().text);
                };
            }
            catch { continue;  }
            yield return null;
        }

        UI_LoadingPanel_Progress.minValue = 0;
        UI_LoadingPanel_Progress.maxValue = Directory.GetFiles(MainPath).Length;

        //-----------------------------------------------------------------Generating Files
        for (int i = 0; i < Directory.GetFiles(MainPath).Length; i++)
        {
            UI_LoadingPanel_Progress.value = i;
            UI_LoadingPanel_ProgressText.SetText("Loading Files " + i.ToString() + "/" + Directory.GetFiles(MainPath).Length.ToString());

            string currentPath = Directory.GetFiles(MainPath)[i];
            Sprite icon = null;
            if (ShowFilesWithRegisteredExtensionsOnly)
            {
                bool found = false;
                foreach (_RegisteredExtensions regExts in RegisteredExtensions)
                {
                    if ("." + regExts.Extension == Path.GetExtension(currentPath))
                    {
                        found = true;
                        icon = regExts.Icon;
                        break;
                    }
                }
                if (!found) continue;
            }

            GameObject newFile = Instantiate(FDE_ItemPrefab, UI_DialogContent.transform);

            if (icon == null)
            {
                foreach (_RegisteredExtensions regExts in RegisteredExtensions)
                {
                    if ("." + regExts.Extension == Path.GetExtension(currentPath))
                    {
                        icon = regExts.Icon;
                        break;
                    }
                }
            }

            if(icon != null)
                newFile.transform.Find("Icon").GetComponent<RawImage>().texture = icon.texture;

            if (!icon)
            {
                string ext = Path.GetExtension(currentPath);
                if (ext == ".png" || ext == ".jpg")
                {
                    RawImage img = newFile.transform.Find("Icon").GetComponent<RawImage>();
                    Texture2D txt = new Texture2D(100, 100);
                    FileInfo f = new FileInfo(currentPath);
                    long size_ = f.Length;
                    float calculatedSize = MaxImageDisplaySize * 1024;
                    if (size_ <= calculatedSize)
                    {
                        byte[] b = File.ReadAllBytes(currentPath);
                        txt.LoadImage(b);
                        txt.Apply();
                      //  Sprite sp = Sprite.Create(txt, new Rect(Vector2.zero, new Vector2(txt.width, txt.height)), Vector2.zero);
                        newFile.transform.Find("Icon").GetComponent<RawImage>().texture = txt;
                    }
                    else if (ICON_DefaultImageHolder != null)
                        newFile.transform.Find("Icon").GetComponent<RawImage>().texture = ICON_DefaultImageHolder.texture;
                }
                else
                    newFile.transform.Find("Icon").GetComponent<RawImage>().texture = ICON_Files.texture;
            }
            newFile.transform.Find("Name").gameObject.SetText(Path.GetFileName(currentPath));
            if (useCustomFont && customFont && newFile.transform.Find("Name").GetComponent<Text>())
                newFile.transform.Find("Name").GetComponent<Text>().font = customFont;
            newFile.transform.Find("Path").gameObject.SetText(currentPath);
            newFile.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                A_ProcessFile(newFile.transform.Find("Path").GetComponent<Text>() ? newFile.transform.Find("Path").GetComponent<Text>().text
                    : newFile.transform.Find("Path").GetComponent<TextMeshProUGUI>().text);
            });

            FDE_CustomEventTrigger cE = newFile.GetComponentInChildren<Button>().gameObject.AddComponent<FDE_CustomEventTrigger>();
            cE.OnEnter = delegate
            {
                A_GetInfo(newFile.transform.Find("Path").GetComponent<Text>() ? newFile.transform.Find("Path").GetComponent<Text>().text
                    : newFile.transform.Find("Path").GetComponent<TextMeshProUGUI>().text);
            };
            cE.OnExit = delegate
            {
                UI_Info.SetTextActive(false);
            };
            cE.OnClick = delegate
            {
                A_Internal_InitializeScrollDia(newFile.transform.Find("Path").GetComponent<Text>() ? newFile.transform.Find("Path").GetComponent<Text>().text
                    : newFile.transform.Find("Path").GetComponent<TextMeshProUGUI>().text);
            };

            yield return null;
        }

        UI_LoadingPanel.SetActive(false);
    }

    private void A_Internal_Back()
    {
        if (listOfPaths.Count - 1 > 0)
            listOfPaths.RemoveAt(listOfPaths.Count - 1);

        if (Directory.Exists(listOfPaths[listOfPaths.Count - 1]))
            MainPath = listOfPaths[listOfPaths.Count - 1];
        else
            MainPath = UI_DriversTMP == null ? UI_Drivers.options[0].text : UI_DriversTMP.options[0].text;
        
        A_Internal_RefreshContent();
    }
    private void A_Internal_Back(int toIndex)
    {
        if (listOfPaths.Count <= 1)
            return;
        if(toIndex < 0 || toIndex >= listOfPaths.Count)
        {
            FDE_Error("Back index is too high (higher than directory history array) or lower than 0");
            return;
        }
        if (Directory.Exists(listOfPaths[toIndex]))
            MainPath = listOfPaths[toIndex];
        else
            MainPath = UI_DriversTMP == null ? UI_Drivers.options[0].text : UI_DriversTMP.options[0].text;

        A_Internal_RefreshContent();
    }
    private void A_Internal_BackToParent()
    {
        if (!Directory.Exists(MainPath))
            return;
        DirectoryInfo di = new DirectoryInfo(MainPath);
        if (di.Parent == null)
            return;
        MainPath = di.Parent.FullName;

        A_Internal_RefreshContent();
    }
    private void A_Internal_SearchByAddress()
    {
        if (!string.IsNullOrEmpty(UI_FullPath.GetInputFieldText()) && UI_FullPath.GetInputFieldText() != MainPath)
            A_ProcessFile(UI_FullPath.GetInputFieldText());
        else
            FDE_Error("Address is empty or already opened");
    }

    private void A_Internal_ChangeDriver()
    {
        if(UI_DriversTMP || UI_Drivers)
        {
            if (UI_Drivers && !string.IsNullOrEmpty(UI_Drivers.captionText.text))
                A_ProcessFile(UI_Drivers.captionText.text);
            else if (UI_DriversTMP && !string.IsNullOrEmpty(UI_DriversTMP.captionText.text))
                A_ProcessFile(UI_DriversTMP.captionText.text);
            else FDE_Error("Address is empty or already opened");
        }
        else
            FDE_Error("Address is empty or already opened");
    }
    private void A_Internal_ChangeSize()
    {
        UI_DialogContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(UI_DialogSize.value, UI_DialogSize.value);
        UI_DialogSizeInfo.SetText("Size: " + UI_DialogSize.value.ToString("0"));
    }

    private bool A_Internal_GetFileType_ImFile(string Path_)
    {
        try
        {
            FileAttributes at = File.GetAttributes(Path_);
            if ((at & FileAttributes.Directory) == FileAttributes.Directory)
                return false;
            else
                return true;
        }
        catch { return false; }
    }
    private void A_Internal_InitializeScrollDia(string Path_)
    {
        if (!EnableDataCustomization)
            return;
        if (!string.IsNullOrEmpty(Path_) && Input.GetMouseButtonUp(1))
        {
            UI_ScrollDialog.SetActive(true);

            SelectedPath = Path_;

            if (A_Internal_GetFileType_ImFile(SelectedPath))
            {
                UI_ScrollDialog.transform.Find("File").gameObject.SetActive(true);
                UI_ScrollDialog.transform.Find("Folder").gameObject.SetActive(false);
            }
            else
            {
                UI_ScrollDialog.transform.Find("File").gameObject.SetActive(false);
                UI_ScrollDialog.transform.Find("Folder").gameObject.SetActive(true);
            }

            UI_ScrollDialog_Delete.interactable = true;
            UI_ScrollDialog_Delete2.interactable = true;
            UI_ScrollDialog_Rename.interactable = true;
            UI_ScrollDialog_Rename2.interactable = true;
            UI_ScrollDialog_Duplicate.interactable = true;
            UI_ScrollDialog_Copy.interactable = true;
            UI_ScrollDialog_Paste.interactable = false;
            UI_ScrollDialog.transform.position = Input.mousePosition;
        }
        else
        {
            if (string.IsNullOrEmpty(Path_))
            {
                UI_ScrollDialog_Delete2.interactable = false;
                UI_ScrollDialog_Rename2.interactable = false;
                UI_ScrollDialog.transform.Find("File").gameObject.SetActive(false);
                UI_ScrollDialog.transform.Find("Folder").gameObject.SetActive(true);
            }
                
            if (Input.GetMouseButtonUp(1))
                UI_ScrollDialog.SetActive(true);
            else
            {
                UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(false);
                UI_ScrollDialog.SetActive(false);
            }

            if (!string.IsNullOrEmpty(PathToPaste))
                UI_ScrollDialog_Paste.interactable = true;
            else
                UI_ScrollDialog_Paste.interactable = false;

            UI_ScrollDialog_Delete.interactable = false;
            UI_ScrollDialog_Duplicate.interactable = false;
            UI_ScrollDialog_Copy.interactable = false;
            UI_ScrollDialog.transform.position = Input.mousePosition;

            SelectedPath = MainPath;
        }

        if (!A_Internal_CheckFileForProtection(Path_))
        {
            UI_ScrollDialog_Delete.interactable = false;
            UI_ScrollDialog_Delete2.interactable = false;
            UI_ScrollDialog_Rename.interactable = false;
            UI_ScrollDialog_Rename2.interactable = false;
            UI_ScrollDialog_Duplicate.interactable = false;
            UI_ScrollDialog_Copy.interactable = false;
        }
    }

    private bool A_Internal_CheckFileForProtection(string currentPath)
    {
        if (!HighProtectionLevel)
            return true;
        else
        {
            foreach(string s in listOfCreatedFileFolders)
            {
                if (s.Replace("/", "").Replace(@"\", "") == currentPath.Replace("/", "").Replace(@"\", ""))
                    return true;
            }
            return false;
        }
    }
    private void A_Internal_RefreshFileForProtection(string filepath, int addAction = 0, string SecondPath = "")
    {
        if (filepath.Length > 4 && (filepath.Substring(3,1) == "/" || filepath.Substring(3, 1) == @"\"))
            filepath = filepath.Remove(3, 1);

        filepath = filepath.Replace("/", @"\");
        SecondPath = SecondPath.Replace("/", @"\");
        if (addAction == 1)
        {
            int index = 0;
            foreach(string s in listOfCreatedFileFolders)
            {
                if (s.Replace("/", "").Replace(@"\", "") == filepath.Replace("/", "").Replace(@"\", ""))
                {
                    listOfCreatedFileFolders[index] = SecondPath;
                    break;
                }
                index++;
            }
        }
        else if (addAction == 2)
            listOfCreatedFileFolders.Add(filepath);
        else if (addAction == 3)
            listOfCreatedFileFolders.Remove(filepath);
    }
    private void A_Internal_RefreshProtectionList()
    {
        for(int i = 0; i< listOfCreatedFileFolders.Count;i++)
        {
            string s = listOfCreatedFileFolders[i];
            if (A_Internal_GetFileType_ImFile(s))
            {
                if (!File.Exists(s))
                    listOfCreatedFileFolders.RemoveAt(i);
            }
            else
            {
                if (!Directory.Exists(s))
                    listOfCreatedFileFolders.RemoveAt(i);
            }
        }
    }
        
    private void A_Internal_ScrollButton(int index)
    {
        switch(index)
        {
            case 0:
                PathToPaste = SelectedPath;
                break;

            case 1:
                try
                {
                    string Name = Path.GetFileName(PathToPaste);

                    if (File.Exists(SelectedPath + "/" + Path.GetFileName(PathToPaste)))
                        Name = Path.GetFileNameWithoutExtension(Name) + Directory.GetFiles(SelectedPath).Length.ToString() + Path.GetExtension(Name);
                    File.Copy(PathToPaste, SelectedPath + "/" + Name);
                    A_Internal_RefreshFileForProtection(SelectedPath + "/" + Name, 2);
                    PathToPaste = "";
                }
                catch
                { }

                A_Internal_RefreshContent();
                break;

            case 2:
                try
                {
                    string path = Path.GetDirectoryName(SelectedPath) + "/" + Path.GetFileNameWithoutExtension(SelectedPath) + Directory.GetFiles(Path.GetDirectoryName(SelectedPath)).Length.ToString() + Path.GetExtension(SelectedPath);
                    File.Copy(SelectedPath,path);
                    A_Internal_RefreshFileForProtection(path, 2);
                }
                catch
                { }

                A_Internal_RefreshContent();
                break;

            case 3:
                actionIndex = 4;
                UI_ScrollDialog_ProtectionQuestion.SetActive(true);
                break;

            case 4:
                actionIndex = 0;
                UI_ScrollDialog_CreatingSomethingInputField.SetInputFieldText("");
                UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(true);
                break;

            case 5:
                actionIndex = 1;
                UI_ScrollDialog_CreatingSomethingInputField.SetInputFieldText("");
                UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(true);
                break;

            case 6:
                actionIndex = 2;
                UI_ScrollDialog_CreatingSomethingInputField.SetInputFieldText(Path.GetFileName(SelectedPath));
                UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(true);
                break;

            case 7:
                actionIndex = 3;
                UI_ScrollDialog_CreatingSomethingInputField.SetInputFieldText(Path.GetFileName(SelectedPath));
                UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(true);
                break;
        }
       
        if(index<3) UI_ScrollDialog.SetActive(false);
    }
    private int actionIndex;
    private void A_Internal_EnterInputField()
    {
        UI_ScrollDialog_CreatingSomethingInputField.gameObject.SetActive(false);
        UI_ScrollDialog_ProtectionQuestion.SetActive(false);
        UI_ScrollDialog.SetActive(false);

        switch (actionIndex)
        {
            //----------Create New File & Folder
            case 0:
                if (string.IsNullOrEmpty(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()) || File.Exists(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()))
                    return;

                string fileName = UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText();
                if (string.IsNullOrEmpty(DefaultExtension))
                    DefaultExtension = "txt";
                if (DefaultExtension.Contains("."))
                    DefaultExtension = DefaultExtension.Replace(".", "");
                if (!fileName.Contains("."))
                    fileName += "."+ DefaultExtension;
                File.Create(MainPath + "/" + fileName).Dispose();
                A_Internal_RefreshFileForProtection(MainPath + "/" + fileName, 2);

                A_Internal_RefreshContent();
                break;

            case 1:
                if (string.IsNullOrEmpty(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()) || Directory.Exists(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()))
                    return;

                Directory.CreateDirectory(MainPath + "/" + UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText());
                A_Internal_RefreshFileForProtection(MainPath + "/" + UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText(), 2);

                A_Internal_RefreshContent();
                break;


            //---------Renaming files & folders
            case 2:
                if (string.IsNullOrEmpty(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()) || File.Exists(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()))
                    UI_ScrollDialog_CreatingSomethingInputField.SetInputFieldText(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText() + Random.Range(1, 999));

                File.Move(SelectedPath, MainPath + "/" + UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText());
                A_Internal_RefreshFileForProtection(SelectedPath, 1, MainPath + "/" + UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText());

                A_Internal_RefreshContent();
                break;

            case 3:
                if (string.IsNullOrEmpty(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()) || Directory.Exists(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText()))
                    UI_ScrollDialog_CreatingSomethingInputField.SetInputFieldText(UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText() + Random.Range(1, 999));

                Directory.Move(SelectedPath, MainPath + "/" + UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText());
                A_Internal_RefreshFileForProtection(SelectedPath, 1, MainPath + "/" + UI_ScrollDialog_CreatingSomethingInputField.GetInputFieldText());

                A_Internal_RefreshContent();
                break;


            //---------Deleting files & folders
            case 4:
                try
                {
                    if (A_Internal_GetFileType_ImFile(SelectedPath))
                        File.Delete(SelectedPath);
                    else
                        Directory.Delete(SelectedPath, true);

                    A_Internal_RefreshFileForProtection(SelectedPath, 3);
                }
                catch(IOException e) { FDE_Error("Could not delete the file/ directory ["+ e.Message +"] - " + SelectedPath); }
                A_Internal_RefreshContent();
                break;
        }
    }
    #endregion

    //----------After File click functions
    #region AfterFile Click processes
    private void A_ProcessFile(string Path_)
    {
        try
        {
            bool ImFile = A_Internal_GetFileType_ImFile(Path_);

            UI_ScrollDialog.SetActive(false);

            if (ImFile)
            {
                switch(File_Action)
                {

                    //----------------------------Basic File Actions
                    case FileAction.Open:
                        Process.Start(Path_);
                        break;
                    case FileAction.OpenInExplorer:
                        Process.Start(Path.GetDirectoryName(Path_));
                        break;

                    //----------------------------Logical-Variable Related File Actions
                    case FileAction.Text_ReadToVariable:
                        if (!ActionOBJ_ReadToVariableMonoBeh)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }

                        try
                        {
                            ActionOBJ_ReadToVariableMonoBeh.GetType().GetField(ActionOBJ_ReadToVariableVar).SetValue(ActionOBJ_ReadToVariableMonoBeh, A_ReturnReadType(Path_));
                        }
                        catch
                        {
                            UnityEngine.Debug.LogError("FDE error - variable could not be found.");
                            return;
                        }
                        break;

                    //----------------------------Visual File Actions
                    case FileAction.Image_ReadImageToRenderer:
                        if (!ActionOBJ_ReadToRenderer)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }
                        if (Path.GetExtension(Path_) != ".png" && Path.GetExtension(Path_) != ".jpg" && Path.GetExtension(Path_) != ".bmp" && Path.GetExtension(Path_) != ".tga" && Path.GetExtension(Path_) != ".gif")
                            return;

                        Texture2D t = new Texture2D(1, 1);
                        t.LoadImage(File.ReadAllBytes(Path_));
                        t.name = Path.GetFileNameWithoutExtension(Path_);
                        t.Apply();
                        ActionOBJ_ReadToRenderer.material.mainTexture = (Texture)t;
                        break;

                    case FileAction.Image_ReadImageToSprite:
                        if (!ActionOBJ_ReadToSprite)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }
                        if (Path.GetExtension(Path_) != ".png" && Path.GetExtension(Path_) != ".jpg" && Path.GetExtension(Path_) != ".bmp" && Path.GetExtension(Path_) != ".tga" && Path.GetExtension(Path_) != ".gif")
                            return;

                        t = new Texture2D(1, 1);
                        t.LoadImage(File.ReadAllBytes(Path_));
                        t.name = Path.GetFileNameWithoutExtension(Path_);
                        t.Apply();
                        ActionOBJ_ReadToSprite.sprite = Sprite.Create(t, new Rect(Vector2.zero, new Vector2(t.width, t.height)), Vector2.zero);
                        break;

                    case FileAction.Image_ReadImageToUIImage:
                        if (!ActionOBJ_ReadToUIImage)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }
                        if (Path.GetExtension(Path_) != ".png" && Path.GetExtension(Path_) != ".jpg" && Path.GetExtension(Path_) != ".bmp" && Path.GetExtension(Path_) != ".tga" && Path.GetExtension(Path_) != ".gif")
                            return;

                        t = new Texture2D(1, 1);
                        t.LoadImage(File.ReadAllBytes(Path_));
                        t.name = Path.GetFileNameWithoutExtension(Path_);
                        t.Apply();
                        ActionOBJ_ReadToUIImage.sprite = Sprite.Create(t, new Rect(Vector2.zero, new Vector2(t.width, t.height)), Vector2.zero);
                        break;
                    case FileAction.Video_ReadVideoToVideoPlayer:
                        if (!ActionOBJ_VideoPlayer)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }

                        ActionOBJ_VideoPlayer.url = Path_;
                        ActionOBJ_VideoPlayer.Play();
                        break;

                    //----------------------------ASCII/ Text File Actions
                    case FileAction.Text_ReadTo3DText:
                        if (!ActionOBJ_ReadTo3DText)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }
                        ActionOBJ_ReadTo3DText.text = A_ReturnReadType(Path_);
                        break;

                    case FileAction.Text_ReadToUIText:
                        if (!ActionOBJ_ReadToUIText)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }
                        ActionOBJ_ReadToUIText.text = A_ReturnReadType(Path_);
                        break;

                    case FileAction.Text_ReadToUGUITextMeshPro:
                        if (!ActionOBJ_ReadToUGUITMP)
                        {
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                            return;
                        }
                        ActionOBJ_ReadToUGUITMP.text = A_ReturnReadType(Path_);
                        break;

                    //----------------------------Custom File Actions
                    case FileAction.CustomEvent:
                        if (Action_CustomEvent != null)
                            Action_CustomEvent.Invoke();
                        else
                            UnityEngine.Debug.LogError("FDE error - missing required object.");
                        break;
                }

                if (!KeepDialogAfterAction)
                    Action_CLOSE_DIALOG();
            }
            else
            {
                if (MaxStoredHistoryFolders > listOfPaths.Count)
                    listOfPaths.Add(Path_);
                else if (listOfPaths.Count > 0)
                {
                    listOfPaths.RemoveAt(0);
                    listOfPaths.Add(Path_);
                }
                MainPath = Path_;
                A_Internal_RefreshContent();
            }
        }
        catch { UnityEngine.Debug.LogError("FDE_Source - Could not process the clicked file..."); }
    }
    private void A_GetInfo(string Path_)
    {
        try
        {
            bool ImFile = A_Internal_GetFileType_ImFile(Path_);

            UI_Info.SetTextActive(true);

            if (ImFile)
            {
                long fileSize = new FileInfo(Path_).Length;
                UI_Info.SetText("Type: File    Size: " + (fileSize/1024).ToString() + "kb    Created: " + File.GetCreationTime(Path_).ToShortDateString());
            }
            else
                UI_Info.SetText("Type: Folder    Size: " + (Directory.GetDirectories(Path_).Length).ToString() + " Folders    Created: " + Directory.GetCreationTime(Path_).ToShortDateString());
        }
        catch { UI_Info.SetText(""); }
    }
    private string A_ReturnReadType(string Path_)
    {
        switch(Read_Type)
        {
            case ReadType.ReadFileContent:
                return File.ReadAllText(Path_);
            case ReadType.ReadFileName:
                return Path.GetFileName(Path_);
            case ReadType.ReadFileNameWithoutExtension:
                return Path.GetFileNameWithoutExtension(Path_);
            case ReadType.ReadFileExtensionOnly:
                return Path.GetExtension(Path_);
            case ReadType.ReadFullFilePathWithoutFileName:
                return Path.GetDirectoryName(Path_);
            case ReadType.ReadFullFilePath:
                return Path_;
            case ReadType.ReadFileSizeInBytes:
                return File.ReadAllBytes(Path_).Length.ToString();
            case ReadType.ReadFileSizeInKilobytes:
                return (File.ReadAllBytes(Path_).Length / 1024).ToString();
            case ReadType.ReadFileSizeInMegabytes:
                return (File.ReadAllBytes(Path_).Length / (1024*2)).ToString();
        }

        return "ERROR";
    }
    #endregion

    //----------Error
    private void FDE_Error(string Exception)
    {
        UnityEngine.Debug.LogError("FDE Error-Warning: " + Exception);
        return;
    }
}