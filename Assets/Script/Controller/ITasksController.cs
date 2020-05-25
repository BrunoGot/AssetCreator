using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    TaskName GetTaskName();
    TaskState GetState();
    void SetState(TaskState _state);

    TaskName[] GetNextTasks(); //link to the next task
    string GetWarning();
    void SetWarning(string _msg);
    void OnSelect();
}

public abstract class TaskController : ITasksController
{
    protected TaskName m_taskName;
    protected TaskName[] m_nextTasks; //list of the next pipeline tasks
    protected TaskState m_state;
    protected string m_warningMsg;
    public TaskController(TaskState _state, TaskName[] _nextTasks)
    {
        m_state = _state;//m_state = read a saved file with last saved state
        m_nextTasks = _nextTasks;
        m_warningMsg = "";
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

}
