using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

//manage the concept panel gui
public class Concept_View : Task_View, IConcept_View
{

    //events
    public event EventHandler<AddConceptEvent> addConceptEvent;
    public event EventHandler<MainButtonEvent> selectConceptEvent;
    public event EventHandler<MainButtonEvent> removeConceptEvent;
   // public event EventHandler<UpdateStateEvent> validTaskEvent;

    //parameter
    private Button m_newConcept; //buffer used to save the id of the last button created => to replace by int id
    private int m_idNewConcept; //id of the last concept created. Will replace m_newConcept
    //gui
   //no need anymore private Button m_quitPanel; //Button to quit the concept mode

    PanelBoard_Controller m_panelBoard; //this module is a gui module so its linked directly to the view part. It's an independant module that can be used in other application
    // Start is called before the first frame update
    
    void Awake()
    {
        InitGUI();
    }
    void Start()
    {
        IPanelBoardView panelboard = GameObject.Find("ConceptPanelBoard").AddComponent<PanelBoard_View>();
        panelboard.Init("GUI/ButtonPanelTemplate");
        m_panelBoard = new PanelBoard_Controller(panelboard);
        m_panelBoard.addPanelEvent += HandleNewConcept;
        m_panelBoard.onMainButtonEvent += HandleSelectConcept;
        m_panelBoard.removePanelEvent += HandleRemoveConcept;
        //DisplayPanel(false); test to move it in initGui part
    }
/* move it to Task_View abstract class
    void InitGUI()
    {
        DisplayPanel(false);
        m_quitPanel = GameObject.Find("QuitButton").GetComponent<Button>();
        m_quitPanel.onClick.AddListener(QuitPanel);
        Button validButton = GameObject.Find("ValidButton").GetComponent<Button>();
        validButton.onClick.AddListener(ValidateStep);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
    /* moved to Task_view abstract class
    public void DisplayPanel(bool _val) //show or hide the concept panel
    {
        CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = _val?1:0;
        cg.interactable = _val;
        cg.blocksRaycasts = _val;
    }*/

    private void HandleNewConcept(object _sender, AddPanelEvent _args)
    {
        Debug.Log("inside HandleNewConcept");
        FileReader.Instance.OpenFile(FilePathHandler);
        m_idNewConcept = _args.ButtonId;
        m_newConcept = m_panelBoard.GetPanel(m_idNewConcept).transform.Find("MainButton").GetComponent<Button>();// _args.ButtonConcept.GetComponent<Button>();
    }

    private void HandleRemoveConcept(object _sender, MainButtonEvent _args)
    {
        removeConceptEvent(this, _args);
    }

    //used as delegate to get the path from the OpenAsset Button Function
    private void FilePathHandler(string _path)
    {
        Debug.Log("test delegate : path = " + _path);
        if (_path.ToLower() != null && (_path.Contains(".png")|| _path.Contains(".jpg")|| _path.Contains(".jpeg")))
        {
            StartCoroutine(GetTextureFromPath(_path));
        }
        else
        {   //if an error occur, remove the last panel created
            m_panelBoard.RemovePanel(m_newConcept.transform.parent.gameObject);
        }
    }

    private IEnumerator GetTextureFromPath(string _path)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_path);
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            m_panelBoard.RemovePanel(m_panelBoard.GetPanel(m_idNewConcept));
        }
        else
        {
            Debug.Log("Texture = " + ((DownloadHandlerTexture)www.downloadHandler).texture);
            CreateNewButtonConcept(((DownloadHandlerTexture)www.downloadHandler).texture, _path);
        }
    }

    private void CreateNewButtonConcept(Texture2D _imgConcept,string _path)
    {
        Debug.Log("new concept = " + m_newConcept);
        m_newConcept.gameObject.GetComponent<RawImage>().texture = _imgConcept as Texture;
        addConceptEvent(this, new AddConceptEvent(m_idNewConcept, _path)); //send the signal to the controller to save the new concept
    }

    private void HandleSelectConcept(object _sender, MainButtonEvent _eventArgs)
    {
        selectConceptEvent(this, _eventArgs); //get signal from the panelBoard module and give it to the controller to handle actions
    }
    /* try move it to mother class
    private void QuitPanel()
    {
        DisplayPanel(false);
    }*/

    public void LoadConcepts(string[] _imgs)
    {
        StartCoroutine(LoadMultipleTexture(_imgs));
        
    }

    public IEnumerator LoadMultipleTexture(string[] _imgs)
    {
        foreach (string img in _imgs)
        {
            m_idNewConcept = m_panelBoard.CreateNewPanel();
            Debug.Log("Load concept : id = "+ m_idNewConcept + " path = " + img);
            m_newConcept = m_panelBoard.GetPanel(m_idNewConcept).transform.Find("MainButton").GetComponent<Button>(); //new concept check if possibility to factorise this line
            FilePathHandler(img);
            float delay = 0.5f; //change it with some user prefs
            yield return new WaitForSeconds(delay);
        }
    }

    /*try move it in mother class
    public void ValidateStep()
    {
        Debug.Log("Validate the step");
        validConceptEvent(this, new UpdateStateEvent(TaskState.Done));
        QuitPanel();
    }*/

}
