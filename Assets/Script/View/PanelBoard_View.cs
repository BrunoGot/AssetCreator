using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//view part for the board handling the button list
public class PanelBoard_View : MonoBehaviour, IPanelBoardView
{
    GameObject m_buttonPanel;
    //events
    public event EventHandler<AddPanelEvent> addPanelEvent;
    public event EventHandler<MainButtonEvent> onMainButtonEvent;
    public event EventHandler<MainButtonEvent> removePanelEvent;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Init() //used to init manually beofre the "start" of a unityscript
    {
        transform.Find("AddPanelButton").GetComponent<Button>().onClick.AddListener(AddButtonHandler);
        m_buttonPanel = Resources.Load<GameObject>("GUI/ButtonPanelTemplate") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //create a new panel, assign the new id and return the gameobject to the controller that called it
    public GameObject CreatePanel(int _newId)
    {
        int idPanel = _newId;// m_panels.Count; //use the button position in the list as button id. Default ID button can change with some update so its not very stable
        GameObject panel = Instantiate(m_buttonPanel, this.transform);
        GameObject mainButton = panel.transform.Find("MainButton").gameObject;
        mainButton.GetComponent<Button>().onClick.AddListener(() => MainButtonHandler(idPanel));
        GameObject deleteButton = mainButton.transform.Find("DeleteButton").gameObject;
        deleteButton.GetComponent<Button>().onClick.AddListener(()=>DeleteHandler(idPanel));
        return panel;
    }
    public void AddButtonHandler() //add a new panelon cliced
    {
        addPanelEvent(this, new AddPanelEvent());//spread signal that a new panel has been added to initialise it in the application part if needed
    }

    private void DeleteHandler(int _idButton)
    {
        removePanelEvent(this, new MainButtonEvent(_idButton));
    }

    public void PlayButtonHandler()
    {
        Debug.Log("playbutton");
    }

    private void MainButtonHandler(int _idButton)
    {
        Debug.Log("click on button : " + _idButton);
        //#todo : send the signal to the controller part and to the rest of the application via the controller
        onMainButtonEvent(this, new MainButtonEvent(_idButton));
    }
    public void RemovePanel(GameObject _panel)
    {
        Destroy(_panel);
    }

}
