using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class Modelisation_View : Task_View , IModelisation_View
{

    //PanelBoard_Controller m_panelBoard; maybe not use this widget
    
    //events
    public event EventHandler<StartSoftwareEvent> onSelectSoftware;

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

        /*
         * Desactivated temporarly because it's not a major functionality
         * IPanelBoardView panelboard = GameObject.Find("ModePanelBoard").AddComponent<PanelBoard_View>();
        m_panelBoard = new PanelBoard_Controller(panelboard);
        m_panelBoard.addPanelEvent += HandleAddSoftware;
        m_panelBoard.onMainButtonEvent += HandleOpenAsset;
        m_panelBoard.removePanelEvent += HandleRemoveSoftware;*/

        //temporary used while the panel nutton module is not implemented
        Button blenderButton = GameObject.Find("ButtonPanelBlender").transform.Find("MainButton").GetComponent<Button>();
        blenderButton.onClick.AddListener(OpenBlender);
        Button houdiniButton = GameObject.Find("ButtonPanelHoudini").transform.Find("MainButton").GetComponent<Button>();
        houdiniButton.onClick.AddListener(OpenHoudini);

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

    private void HandleAddSoftware(object _sender, AddPanelEvent _args)
    {
        Debug.Log("Add panel : " + _args.ButtonId);
    }
    private void HandleOpenAsset(object _sender, MainButtonEvent _args)
    {
        Debug.Log("Add panel : " + _args.ButtonId);
    }
    private void HandleRemoveSoftware(object _sender, MainButtonEvent _args)
    {
        Debug.Log("Add panel : " + _args.ButtonId);
    }
}
