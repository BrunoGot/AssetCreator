using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

//manage the concept panel gui
public class Concept_View : MonoBehaviour, IConcept_View
{

    //events
    public event EventHandler<AddConceptEvent> addConceptEvent;
    public event EventHandler<MainButtonEvent> selectConceptEvent;

    //parameter
    private Button m_newConcept; //buffer used to save the id of the last button created
    //gui
    private Button m_quitPanel; //Button to quit the concept mode

    PanelBoard_Controller m_panelBoard; //this module is a gui module so its linked directly to the view part. It's an independant module that can be used in other application
    // Start is called before the first frame update
    
    void Awake()
    {
        InitGUI();
    }
    void Start()
    {
        m_panelBoard = new PanelBoard_Controller();
        m_panelBoard.addPanelEvent += HandleNewConcept;
        m_panelBoard.onMainButtonEvent += HandleSelectConcept;
        DisplayPanel(false);
    }

    void InitGUI()
    {
        m_quitPanel = GameObject.Find("QuitButton").GetComponent<Button>();
        m_quitPanel.onClick.AddListener(QuitPanelMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayPanel(bool _val) //show or hide the concept panel
    {
        CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = _val?1:0;
        cg.interactable = _val;
        cg.blocksRaycasts = _val;
    }

    private void HandleNewConcept(object _sender, AddPanelEvent _args)
    {
        Debug.Log("inside HandleNewConcept");
        FileReader.Instance.OpenFile(FilePathHandler);
        m_newConcept = _args.ButtonConcept.GetComponent<Button>();
    }

    //used as delegate to get the path from the OpenAsset Button Function
    private void FilePathHandler(string _path)
    {
        Debug.Log("test delegate : path = " + _path);
        if (_path != null && (_path.Contains(".png")|| _path.Contains(".jpg")|| _path.Contains(".jpeg")))
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
            m_panelBoard.RemovePanel(m_newConcept.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("Texture = " + ((DownloadHandlerTexture)www.downloadHandler).texture);
            CreateNewButtonConcept(((DownloadHandlerTexture)www.downloadHandler).texture, _path);
        }
    }

    private void CreateNewButtonConcept(Texture2D _imgConcept,string _path)
    {
        m_newConcept.gameObject.GetComponent<RawImage>().texture = _imgConcept as Texture;
        addConceptEvent(this, new AddConceptEvent(m_newConcept.gameObject.GetInstanceID(), _path)); //send the signal to the controller to save the new concept
    }

    private void HandleSelectConcept(object _sender, MainButtonEvent _eventArgs)
    {
        selectConceptEvent(this, _eventArgs); //get signal from the panelBoard module and give it to the controller to handle actions
    }

    private void QuitPanelMode()
    {
        gameObject.SetActive(false);
    }

    public void LoadConcepts(string[] _imgs)
    {
        StartCoroutine(LoadMultipleTexture(_imgs));
        
    }

    public IEnumerator LoadMultipleTexture(string[] _imgs)
    {
        foreach (string img in _imgs)
        {
            m_newConcept = m_panelBoard.CreateNewPanel().GetComponent<Button>();
            FilePathHandler(img);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
