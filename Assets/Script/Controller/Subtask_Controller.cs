using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Subtask_Controller:TaskController
{
    private Subtask_Model m_model;
    private ISubtask_View m_view;

    public Subtask_Controller(ISubtask_View _view, Subtask_Model _model)
    {
        m_model = _model;
        m_view = _view;
        m_view.addNewVersionEvent += HandleAddNewVersion;
        m_view.changeVersionEvent += HandleChangeVersion;
        m_view.Init(m_model.Software, m_model.Versions, m_model.CurrentVersion);
        
    }

    public override SavedState Serialize()
    {
        return m_model.SaveState();
    }

    public void Remove()
    {
        m_model = null;
        m_view.Clean();
        m_view = null;
    }

    /*private void OpenSoftwareHandler(object _sender, EventArgs _args)
    {
        //m_model.OpenAsset();
    }*/


    public void Select()
    {
        m_model.IsSelected = true;
        Display();
    }

    public void DeSelect()
    {
        m_model.IsSelected = false;     
    }

    public void Display()
    {
        m_view.Init(m_model.Software, m_model.Versions, m_model.CurrentVersion);
    }

    public void OpenAsset()
    {
        Debug.Log("openAsset");
        m_model.OpenAsset();
    }

    public void HandleAddNewVersion(object _sender, EventArgs _args)
    {
        Debug.Log("click on add new Version");
        if (m_model.IsSelected)
        {
            m_model.AddNewVersion();
            Display();//update the view with new datas
        }
    }

    public void HandleChangeVersion(object _sender, ChangeVersionEvent _args)
    {
        if (m_model.IsSelected)
        {
            m_model.SetCurrentVersion(_args.Version);
            Debug.Log("selected version = " + _args.Version);
        }
            
    }
}

public interface ISubtask_View
{
    //events
    event EventHandler<EventArgs> addNewVersionEvent;
    event EventHandler<ChangeVersionEvent> changeVersionEvent;

    void Init(string _software, List<string> _versions, int _currentVersion);
    void Clean();
}

public class ChangeVersionEvent : EventArgs
{
    public int Version { get; private set; }
    public ChangeVersionEvent(int _version)
    {
        Version = _version;
    }
}