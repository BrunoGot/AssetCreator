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
       //Connect to the view part, will maybe move to the parent class
        m_view.onCreateSubtask += HandleCreateSubtask;
        m_view.onRemoveSubtask += HandleRemoveSubtask;
        m_view.onSelectSubtask += HandleSelectSubtask;
        m_model = new ModelisationTask_Model(_assetManager, m_taskName);
        //m_model.onLoadSubtask += m_view.HandleLoadSubtask;//HandleSubtask;
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
   /* private void HandleStartSoftware(object _sender, StartSoftwareEvent _args)
    {
        m_model.OpenAsset(_args.SoftwareName);
    }*/

    private void HandleCreateSubtask(object _sender, CreateSubtaskEvent _args)
    {
        m_model.CreateSubtask(_args.SubtaskName,_args.SoftwareIndex, _args.PanelID, _args.ViewPart);
    }

    public void HandleRemoveSubtask(object _sender, RemoveSubtaskEvent _args) //called when removing a subtask button panel, remove the associated subtask
    {
        m_model.RemoveSubtask(_args.IdButton); //remove the button by its ID
    }

    public void HandleSelectSubtask(object _sender, SelectSubtaskEvent  _args)
    {
        m_model.SelectSubtask(_args.ID);
    }

    public override SavedState Serialize()
    {
        Debug.Log("save modelisation task");
        return m_model.SaveState();  

    }
    public override void Deserialize(SavedState _savedState)
    {
        base.Deserialize(_savedState);
        CleanDatas();
        TaskModelState state = ((TaskModelState)_savedState);
        SubtaskState subtask;
        for (int i = 0; i < state.SubtasksDatas.Length; i++)
        {
            subtask = ((SubtaskState)state.SubtasksDatas[i]);
            m_view.LoadSubtask(subtask.Name, subtask.Software);
            //onLoadSubtask(this, new LoadSubtaskEvent(subtask)); //send the subtask datas to the view part to update it
        }
        m_model.Load(state);
    }

    private void CleanDatas()
    {
        m_view.Clean();
        m_model.Clean();
    }
}

public interface IModelisation_View : ITask_View
{
    //events
    event EventHandler<SelectSubtaskEvent > onSelectSubtask;
    event EventHandler<CreateSubtaskEvent> onCreateSubtask;
    event EventHandler<RemoveSubtaskEvent> onRemoveSubtask;

    int LoadSubtask(string _subtaskName, string _softwwareName);
    void Clean();
}

public class CreateSubtaskEvent
{
    public string SubtaskName;
    public int SoftwareIndex;
    public int PanelID;
    public ISubtask_View ViewPart;
    public CreateSubtaskEvent(string _subtaskName, int _softwareIndex, int _panelID, ISubtask_View _viewPart)
    {
        SubtaskName = _subtaskName;
        SoftwareIndex = _softwareIndex;
        PanelID = _panelID;
        ViewPart = _viewPart;
    }
}

public class RemoveSubtaskEvent : EventArgs
{
    public int IdButton;
    public RemoveSubtaskEvent(int _id)
    {
        IdButton = _id;
    }
}
public class SelectSubtaskEvent : EventArgs
{
    public int ID { get; private set; }

    public SelectSubtaskEvent(int _id)
    {
        ID = _id;
    }
}

