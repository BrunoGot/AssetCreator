using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAssetManagerView
{
    //events
    event EventHandler<LoadAssetsArgs> loadAssetEvent;
    event EventHandler<SaveAssetsArgs> saveAssetEvent; //trigged when clicked on save button

    void Init(string _lastassetPath, Dictionary<TaskName, ITasksController> _tasks); //used to initialize some default values and preferences
    void UpdateTaskButton(TaskName _name, TaskState _state, string _warningMessage);
    void InitPipelineButtons(Dictionary<TaskName, ITasksController> _tasks);
    void IsAssetLoaded(bool val); //update view mode for when an asset is loaded in the system or not
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

public class SaveAssetsArgs : EventArgs
{
    public SaveAssetsArgs()
    {
    }
}