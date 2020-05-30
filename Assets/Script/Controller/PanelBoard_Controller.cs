using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//as it a simple element, the controller part also implement the model part
public class PanelBoard_Controller
{
    //view part
    IPanelBoardView m_view;

    //events
    public event EventHandler<AddPanelEvent> addPanelEvent;
    public event EventHandler<MainButtonEvent> onMainButtonEvent;

    //params
    List<IPannelButton> m_buttonList;
    public PanelBoard_Controller()
    {
        Debug.Log("start panel button");
        GameObject panelButon = GameObject.Find("PanelBoard");
        m_view = panelButon.AddComponent<PanelBoard_View>();
        m_view.addPanelEvent += HandleAddNewButton;
        m_view.onMainButtonEvent += HandleMainButton;
        m_buttonList = new List<IPannelButton>();
    }

    void HandleAddNewButton(object _sender, AddPanelEvent _eventArgs)
    {
        Debug.Log("click on add panel in controller");
        //spread the signal to the rest of the application
        addPanelEvent(this, _eventArgs);
        //m_buttonList.Add()
    }

    void HandleMainButton(object _sender, MainButtonEvent _eventArgs)
    {
        onMainButtonEvent(this, _eventArgs); //spread the signal/connection to the application part
    }
}

public interface IPannelButton
{

}
