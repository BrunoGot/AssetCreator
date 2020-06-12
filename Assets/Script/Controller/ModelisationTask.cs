using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelisationTask: TaskController
{
    IModelisation_View m_view;
    public ModelisationTask(TaskState _state, TaskName[] _nextTasks): base(_state, _nextTasks)
    {
        Debug.Log("Init mode panel");
        m_taskName = TaskName.Modelisation;
        GameObject view = GameObject.Find("ModePanel");
        m_view = view.AddComponent<Modelisation_View>();
    }
    
    public void Reload(TaskState _state)
    {
        m_state = _state;
        m_view.Reload();
    }

    public override void OnSelect()
    {
        base.OnSelect();
        m_view.DisplayPanel(true);
    }
}

public interface IModelisation_View:ITask_View
{
}

