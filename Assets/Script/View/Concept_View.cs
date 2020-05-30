using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//manage the concept panel gui
public class Concept_View : MonoBehaviour, IConcept_View
{

    //events
    public event EventHandler<AddConceptEvent> addConceptEvent;
    public event EventHandler<MainButtonEvent> selectConceptEvent;

    //parameter
    private Button m_newConcept; //buffer used to save the id of the last button created

    PanelBoard_Controller m_panelBoard; //this module is a gui module so its linked directly to the view part. It's an independant module that can be used in other application
    // Start is called before the first frame update
    void Start()
    {
        m_panelBoard = new PanelBoard_Controller();
        m_panelBoard.addPanelEvent += HandleNewConcept;
        m_panelBoard.onMainButtonEvent += HandleSelectConcept;
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayPanel(bool _val) //show or hide the concept panel
    {
        gameObject.SetActive(_val);
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
        StartCoroutine(GetTextureFromPath(_path));
    }

    private IEnumerator GetTextureFromPath(string _path)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_path);
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
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

}
