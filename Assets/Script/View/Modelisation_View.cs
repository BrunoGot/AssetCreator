using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class Modelisation_View : Task_View , IModelisation_View
{

    //events
    public event EventHandler<EventArgs> onOpenSoftware;

    //gui
    private PanelBoard_Controller m_panelBoard; //use it to handle custom subtasks
    private Color m_selectedColorPanel; //color to switch when a panel is selected

    //buffers
    private int m_lastPanel; //buffer saving the last subtask panel created
    private GameObject m_selectedPanel; //saving the curret selected subtask 
    private Color m_normalPanelColor; //saving the normal button panel color

    //events
    public event EventHandler<SelectSubtaskEvent> onSelectSubtask;
    public event EventHandler<CreateSubtaskEvent> onCreateSubtask;
    public event EventHandler<RemoveSubtaskEvent> onRemoveSubtask;

    // Start is called before the first frame update
    void Start()
    {
        InitGUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void InitGUI()
    {
        base.InitGUI();

         // Desactivated temporarly because it's not a major functionality
        IPanelBoardView panelboard = GameObject.Find("ModePanelBoard").AddComponent<PanelBoard_View>();
        panelboard.Init("GUI/ButtonPanelSubtask");
        m_panelBoard = new PanelBoard_Controller(panelboard);
        m_panelBoard.addPanelEvent += HandleAddSubtask;
        m_panelBoard.onMainButtonEvent += HandleSelectSubtask;
        m_panelBoard.removePanelEvent += HandleRemoveSubtask;
        Button openSoftButton = GameObject.Find("SubtaskGUI").transform.Find("OpenSoft_Button").GetComponent<Button>();
        openSoftButton.onClick.AddListener(OpenSoftware);

        InitSubtaskForm();
        DisplaySubtaskForm(false);
        //temporary used while the panel nutton module is not implemented
        /*Button blenderButton = GameObject.Find("ButtonPanelBlender").transform.Find("MainButton").GetComponent<Button>();
        blenderButton.onClick.AddListener(OpenBlender);
        Button houdiniButton = GameObject.Find("ButtonPanelHoudini").transform.Find("MainButton").GetComponent<Button>();
        houdiniButton.onClick.AddListener(OpenHoudini);*/
        m_selectedColorPanel = new Color(0.5707547f, 0.5707547f, 1f);
    }

    private void InitSubtaskForm()
    {
        m_subtaskForm = GameObject.Find("SubtaskForm");
        Transform panelButton = m_subtaskForm.transform.Find("Panel");
        Button create = panelButton.Find("ButtonCreate").GetComponent<Button>();
        create.onClick.AddListener(CreateSubTask);
        Button cancel = panelButton.Find("ButtonCancel").GetComponent<Button>();

        cancel.onClick.AddListener(CancelForm);
        Dropdown dpDown = panelButton.Find("DropdownSoftware").GetComponent<Dropdown>();
        List<string> softwares = AssetSystem.System.GetSoftwareList(TaskName.Modelisation);
        dpDown.ClearOptions();
        Dropdown.OptionData data = new Dropdown.OptionData();
        foreach (string software in softwares)
        {
            data = new Dropdown.OptionData();
            data.text = software;
            dpDown.options.Add(data);
        }
        dpDown.value = 1; 
    }

    //check the field of the form and create a subtask with that if correct
    private void CreateSubTask()
    {
        m_subtaskForm = GameObject.Find("SubtaskForm");
        Transform panelButton = m_subtaskForm.transform.Find("Panel");
        InputField infield = panelButton.Find("InputFieldSubtaskName").GetComponent<InputField>();
        //string subTaskName = infield.text;
        Dropdown dpDown = panelButton.Find("DropdownSoftware").GetComponent<Dropdown>();
        int index = dpDown.value;
        Debug.Log("Index software = " + index);
        ColorBlock cols = infield.colors;
        bool errors = false;
        if (infield.text.Length == 0)
        {
            
            cols.normalColor = Color.red;
           
            Debug.Log("Inputs fields invalids");
            errors = true;
        }
        else
        {
            cols.normalColor = Color.white;
        }
        infield.colors = cols;
        if(errors == false)
        {
            DisplaySubtaskForm(false);
            InitSubtaskGUI(infield.text, index);
        }
        Debug.Log("Create Subtask : Name = "+ infield.text + " soft index = "+index);
    }

    private void CancelForm()
    {
        m_panelBoard.RemovePanel(m_lastPanel);
        DisplaySubtaskForm(false);
    }

    private void InitSubtaskGUI(string _subtaskName, int _indexSoftware)
    {
        GameObject SubtaskPanel = m_panelBoard.GetPanel(m_lastPanel);

        SubtaskPanel.transform.Find("Player").Find("Text").GetComponent<Text>().text = _subtaskName;
        ISubtask_View subtaskGUI = GameObject.Find("SubtaskGUI").GetComponent<ISubtask_View>();
        onCreateSubtask(this, new CreateSubtaskEvent(_subtaskName, _indexSoftware, m_lastPanel, subtaskGUI));
    }

    private void HandleAddSubtask(object _sender, AddPanelEvent _args)
    {
        m_lastPanel = _args.ButtonId;
        Debug.Log("Add subtask : " + m_lastPanel);
        DisplaySubtaskForm(true);
    }
    private void HandleSelectSubtask(object _sender, MainButtonEvent _args)
    {
        Debug.Log("select panel : " + _args.ButtonId);
        if(m_selectedPanel != null)
        {
            Release(m_selectedPanel);
        }
        m_selectedPanel = m_panelBoard.GetPanel(_args.ButtonId);
        Debug.Log("selectedPanel = " + m_selectedPanel);
        m_selectedPanel.GetComponent<Image>().color = m_selectedColorPanel;
        onSelectSubtask(this, new SelectSubtaskEvent(_args.ButtonId)) ;
        
    }
    private void HandleRemoveSubtask(object _sender, MainButtonEvent _args)
    {
        Debug.Log("remove panel : " + _args.ButtonId);
        onRemoveSubtask(this, new RemoveSubtaskEvent(_args.ButtonId));
    }

    private void Release(GameObject _panelButton)
    {
        _panelButton .GetComponent<Image>().color = m_normalPanelColor;
    }

    public int LoadSubtask(string _subtaskName, string _softwareName)
    {
        Debug.Log("load subtask");
        m_lastPanel = m_panelBoard.CreateNewPanel();
        //convert softwareName to software index with the dropdown
        Debug.Log("subtask name = " + _subtaskName);
        Debug.Log("Software name = " + _softwareName);

        int indexSoftware = AssetSystem.System.GetSoftwareList(TaskName.Modelisation).IndexOf(_softwareName);
        Debug.Log("Loading : indexSoftware = " + indexSoftware);
        InitSubtaskGUI(_subtaskName, indexSoftware);
        return m_lastPanel; //return the index of the assigned panel
    }

    public void Clean()
    {
        m_panelBoard.RemoveAll();
    }

    private void OpenSoftware()
    {
        onOpenSoftware(this, new EventArgs());
    }
    /*moving to subtask View
    public void DisplaySubtask(string comments, string _software)//load the subtask information from the model part and displays them on the GUI
    {
        //m_
    }*/
}
