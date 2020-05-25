using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAssetManagerView
{
    event EventHandler<LoadAssetsArgs> loadAssetEvent;
    void UpdateTaskButton(TaskName _name, TaskState _state, string _warningMessage);
    void InitPipelineButtons(Dictionary<TaskName, ITasksController> _tasks);
}

public class LoadAssetsArgs : EventArgs
{
    public LoadAssetsArgs(string s)
    {
        m_path = s;
    }
    private string m_path;

    public string Path
    {
        get { return m_path; }
        set { m_path = value; }
    }

}