using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelisationTask: TaskController
{
    IModelisation_View m_view;
    ModelisationTask_Model m_model;

    public ModelisationTask(AssetManagerModel _assetManager, TaskState _state, TaskName[] _nextTasks): base(_assetManager, _state, _nextTasks)
    {

        m_taskName = TaskName.Modelisation;
        //init view 
        GameObject view = GameObject.Find("ModePanel");
        m_view = view.AddComponent<Modelisation_View>();
        m_view.onSelectSoftware += HandleStartSoftware;

        m_model = new ModelisationTask_Model(_assetManager, m_taskName);
        Debug.Log("Init mode panel");
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

    //handlers
    private void HandleStartSoftware(object _sender, StartSoftwareEvent _args)
    {
        m_model.OpenAsset(_args.SoftwareName);
    }
}

public interface IModelisation_View:ITask_View
{
    //events
    event EventHandler<StartSoftwareEvent> onSelectSoftware;

}

public class StartSoftwareEvent : EventArgs
{
    public string SoftwareName { get; private set; }

    public StartSoftwareEvent(string _softName)
    {
        SoftwareName = _softName;
    }
}

