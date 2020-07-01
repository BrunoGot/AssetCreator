using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtask_Controller:TaskController
{
    private Subtask_Model m_model;
    private ISubtask_View m_view;

    public Subtask_Controller(ISubtask_View _view, Subtask_Model _model)
    {
        m_model = _model;
        m_view = _view;
        m_view.onOpenSoftware += OpenSoftwareHandler;
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

    private void OpenSoftwareHandler(object _sender, EventArgs _args)
    {
        m_model.OpenAsset();
    }

    public void Display()
    {
        m_view.Init(m_model.Software, m_model.Versions, m_model.CurrentVersion);
    }
}

public interface ISubtask_View
{
    //events
    event EventHandler<EventArgs> onOpenSoftware;

    void Init(string _software, List<int> _versions, int _currentVersion);
    void Clean();
}
