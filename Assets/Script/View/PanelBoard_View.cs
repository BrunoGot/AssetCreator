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
    private List<GameObject> m_panels;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Init() //used to init manually beofre the "start" of a unityscript
    {
        transform.Find("AddPanelButton").GetComponent<Button>().onClick.AddListener(AddButtonHandler);
        m_buttonPanel = Resources.Load<GameObject>("GUI/ButtonPanelTemplate") as GameObject;
        m_panels = new List<GameObject>(); //just the list containing the created buttons
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CreatePanel()
    {
        GameObject button = Instantiate(m_buttonPanel, this.transform);
        m_panels.Add(button);
        GameObject mainButton = button.transform.Find("MainButton").gameObject;
        mainButton.GetComponent<Button>().onClick.AddListener(() => MainButtonHandler(mainButton.GetInstanceID()));
        return mainButton;
    }
    public void AddButtonHandler() //add a new panelon cliced
    {
        addPanelEvent(this, new AddPanelEvent(CreatePanel()));//spread signal that a new panel has been added to initialise it in the application part if needed
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
        m_panels.Remove(_panel);
        Destroy(_panel);
    }

}
