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
    //model part
    private List<GameObject> m_panels;

    //events
    public event EventHandler<AddPanelEvent> addPanelEvent;
    public event EventHandler<MainButtonEvent> onMainButtonEvent;
    public event EventHandler<MainButtonEvent> removePanelEvent;

    //params
    public PanelBoard_Controller(IPanelBoardView _view)
    {
        Debug.Log("start panel button");
        GameObject panelButon = GameObject.Find("PanelBoard");

        m_panels = new List<GameObject>(); //the list containing the created buttons

        m_view = _view; // 
        m_view.addPanelEvent += HandleAddNewButton;
        m_view.onMainButtonEvent += HandleMainButton;
        m_view.removePanelEvent += HandleRemoveButton;


    }

    void HandleAddNewButton(object _sender, AddPanelEvent _eventArgs)
    {
        Debug.Log("click on add panel in controller");
        //spread the signal to the rest of the application
        _eventArgs.ButtonId = CreateNewPanel();
        addPanelEvent(this, _eventArgs);
    }

    void HandleMainButton(object _sender, MainButtonEvent _eventArgs)
    {
        onMainButtonEvent(this, _eventArgs); //spread the signal/connection to the application part
    }
    void HandleRemoveButton(object _sender, MainButtonEvent _eventArgs)
    {
        removePanelEvent(this, _eventArgs); //pass the remove signal to the rest of the system    
        int id = _eventArgs.ButtonId;
        Debug.Log("id = "+ id);
        RemovePanel(m_panels[id]);
        m_panels.RemoveAt(id);
    }

    public void RemovePanel(GameObject _panel)
    {
        m_view.RemovePanel(_panel);
    }
    public void RemovePanel(int _panelID)
    {
        m_view.RemovePanel(m_panels[_panelID]);
    }

    public void RemoveAll()
    {
        for(int i=0; i < m_panels.Count; i++)
        {
            removePanelEvent(this, new MainButtonEvent(i));
            RemovePanel(i);
        }
    }

    public int CreateNewPanel()//used to create a new panel from the system 
    {
        int id = m_panels.Count;
        GameObject panel = m_view.CreatePanel(id); //create a new panel with a new id*
        m_panels.Add(panel);
        return id;
    }

    public GameObject GetPanel(int _idPanel)
    {
        return m_panels[_idPanel];
    }
}

