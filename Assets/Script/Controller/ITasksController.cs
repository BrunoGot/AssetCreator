using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum TaskName
{
    Concepts, Modelisation, UVs, Animation, FX,  Texturing, Rigging, LookDev
}
public enum TaskState
{
    Todo, Progressing,Done
}
public interface ITasksController
{
    //events
    event EventHandler<UpdateTaskEvent> updateTaskEvent;

    TaskName GetTaskName();
    TaskState GetState();
    void SetState(TaskState _state);

    TaskName[] GetNextTasks(); //link to the next task
    string GetWarning();
    void SetWarning(string _msg);
    void OnSelect();

    SavedState Serialize(); //used to save task
    void Deserialize(SavedState _savedState); //used to load task
    void Reload(TaskState _state);

}

public abstract class TaskController : ITasksController
{
    protected TaskName m_taskName;
    protected TaskName[] m_nextTasks; //list of the next pipeline tasks
    protected TaskState m_state; //move it to the model part. Add a model part to the concept task
    protected string m_warningMsg;
    
    public virtual event EventHandler<UpdateTaskEvent> updateTaskEvent;

    public TaskController(AssetManagerModel _assetManager, TaskState _state, TaskName[] _nextTasks)
    {
        m_state = _state;//m_state = read a saved file with last saved state
        m_nextTasks = _nextTasks;
        m_warningMsg = "";
    }

    public TaskController()
    {

    }

    public TaskName GetTaskName()
    {
        return m_taskName;
    }

    public TaskState GetState()
    {
        return m_state;
    }
    public void SetState(TaskState _state)
    {
        m_state=_state;
    }

    public TaskName[] GetNextTasks()
    {
        return m_nextTasks;
    }

    public void SetWarning(string _warningMsg)
    {
        m_warningMsg = _warningMsg;
    }
    public string GetWarning()
    {
        return m_warningMsg;
    }

    public virtual void OnSelect()
    {
        Debug.Log("click on " + m_taskName);
    }

    public virtual SavedState Serialize()
    {
        return new ConceptState(new string[1], TaskState.Todo) as SavedState; //smell, temporary
    }
   
    public virtual void Deserialize(SavedState _savedState)
    {
        Debug.Log("deserialize " + m_taskName);
    }

    public void Reload(TaskState _state)
    {
        m_state = _state;
        Debug.Log("Reload Task");
    }

}

public abstract class TaskModel
{
    //maping paths
    protected Dictionary<string, string> m_softwareList; //(buttonId, Software path/extension) will be initialized from the future system singleton class
 //   protected Dictionary<TaskName, string> m_mappingTaskPath; //should move to the system manager class and have something like System.GetPath(TaskName)
    

    //properties & link with Asset datas
    protected AssetManagerModel m_assetManager;
    protected TaskName m_taskName;

    //parameters
    protected int m_currentVersion;
    
    public TaskModel(AssetManagerModel _assetManager, TaskName _taskName)
    {
        m_assetManager = _assetManager;
        m_taskName = _taskName;
        m_currentVersion = 1;
    }

    public virtual SavedState SaveState()
    {
        return new ConceptState(new string[1], TaskState.Todo) as SavedState; //smell, temporary
    }
    /*
    public virtual void Deserialize(SavedState _savedState)
    {
        Debug.Log("deserialize " + m_taskName);
    }*/
}

//memento pattern implementation
[Serializable]
public abstract class SavedState
{

}

//mother class to save all stuf commons to the taskModels classes
[Serializable]
public class TaskModelState : SavedState
{
    public SavedState[] SubtasksDatas;
    public TaskModelState(List<SavedState> _subtasks )
    {
        SubtasksDatas = new SubtaskState[_subtasks.Count];
        for(int i = 0; i < _subtasks.Count; i++)
        {
            SubtasksDatas[i] = _subtasks[i];
        }
    }
}

public class MementoHandler
{
    CareTaker m_carTaker;
    Originator m_originator;

    public MementoHandler()
    {
        m_carTaker = new CareTaker();
        m_originator = new Originator();
    }
    public void SetState(SavedState _savedState)// string[] _imgList, TaskState _stateTask)
    {
        m_originator.SetState(_savedState);
        m_carTaker.Add(m_originator.SaveToMemento());
    }

    public SavedState GetState()
    {
        m_originator.RestoreToMemento(m_carTaker.Get(1));
        return m_originator.GetState();
    }
}

//save the last state and pack/unpack state into a memento
public class Originator
{
    private SavedState m_state;

    public void SetState(SavedState _state)
    {
        m_state = _state;
    }

    public SavedState GetState()
    {
        return m_state;
    }

    public Memento SaveToMemento()
    {
        Debug.Log("Originator : sauvegardé dans le mémento.");
        return new Memento(m_state);
    }

    public void RestoreToMemento(Memento _m)
    {
        m_state = _m.GetSavedState();
    }
}

//Mementos has the state of a class or set of datas
public class Memento
{
    private SavedState m_state;
    public Memento(SavedState _state)
    {
        m_state = _state;
    }

    public SavedState GetSavedState()
    {
        return m_state;
    }

}

//manage a list of mementos
public class CareTaker
{
    private List<Memento> m_mementos;
    public CareTaker()
    {
        m_mementos = new List<Memento>();
    }

    public void Add(Memento _m)
    {
        m_mementos.Add(_m);
    }
    public Memento Get(int _index)
    {
        return m_mementos[_index % m_mementos.Count];
    }
}

//event class
public class UpdateStateEvent : EventArgs //used to transport an update signal on the state of a task
{
    public TaskState State;

    public UpdateStateEvent(TaskState _state)
    {
        State = _state;
    }
}
