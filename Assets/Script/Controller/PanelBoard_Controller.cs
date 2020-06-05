using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * TODO for saving : 
 * Save state of the buttonlist in the view part
 * Recreate this list when loading. Send signal for each panel loaded with the index of the panel (1st, 2nd, 3rd...)
 */
//as it a simple element, the controller part also implement the model part
public class PanelBoard_Controller
{
    //view part
    IPanelBoardView m_view;

    //events
    public event EventHandler<AddPanelEvent> addPanelEvent;
    public event EventHandler<MainButtonEvent> onMainButtonEvent;

    //params
    public PanelBoard_Controller()
    {
        Debug.Log("start panel button");
        GameObject panelButon = GameObject.Find("PanelBoard");
        m_view = panelButon.AddComponent<PanelBoard_View>();
        m_view.Init();
        m_view.addPanelEvent += HandleAddNewButton;
        m_view.onMainButtonEvent += HandleMainButton;
    }

    void HandleAddNewButton(object _sender, AddPanelEvent _eventArgs)
    {
        Debug.Log("click on add panel in controller");
        //spread the signal to the rest of the application
        addPanelEvent(this, _eventArgs);
    }

    void HandleMainButton(object _sender, MainButtonEvent _eventArgs)
    {
        onMainButtonEvent(this, _eventArgs); //spread the signal/connection to the application part
    }

    public void RemovePanel(GameObject _panel)
    {
        m_view.RemovePanel(_panel);
    }

    public GameObject CreateNewPanel()//used to create a new panel from the system 
    {
        return m_view.CreatePanel();
    }
}

