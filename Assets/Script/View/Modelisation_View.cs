using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Modelisation_View : Task_View , IModelisation_View
{

    //gui
    private PanelBoard_Controller m_panelBoard; //use it to handle custom subtasks

    //buffers
    private int m_lastPanel; //buffer saving the last subtask panel created

    //events
    public event EventHandler<StartSoftwareEvent> onSelectSoftware;
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

        InitSubtaskForm();
        DisplaySubtaskForm(false);
        //temporary used while the panel nutton module is not implemented
        /*Button blenderButton = GameObject.Find("ButtonPanelBlender").transform.Find("MainButton").GetComponent<Button>();
        blenderButton.onClick.AddListener(OpenBlender);
        Button houdiniButton = GameObject.Find("ButtonPanelHoudini").transform.Find("MainButton").GetComponent<Button>();
        houdiniButton.onClick.AddListener(OpenHoudini);*/

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
    //temporary
    private void OpenBlender()
    {
        Debug.Log("try to open blender");
        onSelectSoftware(this, new StartSoftwareEvent("blender"));
    }

    private void OpenHoudini()
    {
        onSelectSoftware(this, new StartSoftwareEvent("houdini"));

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
    }
    private void HandleRemoveSubtask(object _sender, MainButtonEvent _args)
    {
        Debug.Log("remove panel : " + _args.ButtonId);
        onRemoveSubtask(this, new RemoveSubtaskEvent(_args.ButtonId));
    }

    public int LoadSubtask(string _subtaskName, string _softwareName)
    {
        Debug.Log("load subtask");
        m_lastPanel = m_panelBoard.CreateNewPanel();
        //convert softwareName to software index with the dropdown
        Debug.Log("subtask name = " + _subtaskName);
        Debug.Log("Software name = " + _softwareName);

        int indexSoftware = AssetSystem.System.GetSoftwareList(TaskName.Modelisation).IndexOf(_softwareName);
        InitSubtaskGUI(_subtaskName, indexSoftware);
        return m_lastPanel; //return the index of the assigned panel
    }

    public void Clean()
    {
        m_panelBoard.RemoveAll();
    }
}
