using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtask_Controller
{
    private Subtask_Model m_model;
    private ISubtask_View m_view;

    public Subtask_Controller(ISubtask_View _view, Subtask_Model _model)
    {
        m_model = _model;
        m_view = _view;
        m_view.Init(m_model.Software, m_model.Versions, m_model.CurrentVersion);
    }
}

public interface ISubtask_View
{
    void Init(string _software, List<int> _versions, int _currentVersion);
}
