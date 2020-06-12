using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Modelisation_View : Task_View , IModelisation_View
{

    PanelBoard_Controller m_panelBoard;

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
        
        IPanelBoardView panelboard = GameObject.Find("ModePanelBoard").AddComponent<PanelBoard_View>();
        m_panelBoard = new PanelBoard_Controller(panelboard);
        m_panelBoard.addPanelEvent += HandleAddSoftware;
        m_panelBoard.onMainButtonEvent += HandleOpenAsset;
        m_panelBoard.removePanelEvent += HandleRemoveSoftware;
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
