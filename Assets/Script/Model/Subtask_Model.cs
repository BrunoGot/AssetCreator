using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtask_Model: TaskModel
{
    private string m_path;
    private string m_name;
    private string m_software;
    private List<int> m_versions;
    private string m_commentaire;
    private string m_todo;
    private string m_retakes;
    private MementoHandler m_mementoHandler;
    public string Software { get { return m_software; } }
    public List<int> Versions { get { return m_versions; } }
    public int CurrentVersion { get { return m_currentVersion; } }

    public Subtask_Model(AssetManagerModel _assetManager, TaskName _taskName, string _name, int _softwareIndex) : base(_assetManager, _taskName)
    {
        m_name = _name;
        Debug.Log("_softwareIndex = "+ _softwareIndex+" Taskname = "+_taskName);
        m_software = AssetSystem.System.GetSoftwareList(m_taskName)[_softwareIndex];
        //Debug.Log("Create a subtask");
        m_mementoHandler = new MementoHandler(); //using memento pattern to handle saving/loading and also undo/redo in the 

        //todo : m_versions = pipelineSystem.GetVersions(subtaskName, TaskName) 
        m_versions = new List<int>();
    }

    public override SavedState SaveState()
    {
        int[] versions = new int[m_versions.Count];
        for(int i =0; i<m_versions.Count; i++)
        {
            versions[i] = m_versions[i];
        }
        SubtaskState subtask = new SubtaskState(m_path, m_name, m_software, m_currentVersion, versions, m_commentaire, m_todo, m_retakes);
        Debug.Log("save subtask name = " + subtask.Name);
        m_mementoHandler.SetState(subtask); //will move when a new concept is added, to handle undo/redo functionality
        return m_mementoHandler.GetState();
    }
    public void OpenAsset()
    {
        string extension = PipelineSystem.System.GetExtension(m_software);
        string taskPath = PipelineSystem.System.GetPathTask(m_taskName);
        string versionPath = PipelineSystem.System.GetVersionPath(m_currentVersion);
        Debug.Log("m_assetManager.AssetName = " + m_assetManager.AssetName);
        Debug.Log("m_assetManager.AssetPipelineFolder = " + m_assetManager.AssetPath);
        Debug.Log("task path = " + taskPath);
        Debug.Log("software extention = " + extension);
        Debug.Log("asset version = " + m_currentVersion + " path = "+versionPath);
        string finalPath = m_assetManager.AssetPath + taskPath + "\\" + versionPath + "\\" + m_assetManager.AssetName + extension;
        Debug.Log("finalPath  = " + finalPath);
        Debug.Log("hard path = C:\\Users\\Natspir\\NatspirProd\\03_WORK_PIPE\\01_ASSET_3D\\Enviro\\DataTunnel\\3d\\scenes\\Mode\\mode\\work_v002\\untitled.blend");
        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.Arguments = "/C " + finalPath; //TODO : use asset path + add the task path + add the version path + open it in the software. If there is no file for the software trig a message to open in other soft or import from the file as obj
        cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.Start();
    }
    /*public override void Deserialize(SavedState _savedState)
    {
        //base.Deserialize(_savedState);
        //SubtaskState subtask = ((SubtaskState)_savedState);
        //Debug.Log(imgPaths);
        //m_view.LoadConcepts(conceptState.ImgPaths);
        //load the state of the task
        //HandleUpdateConcept(this, new UpdateStateEvent(conceptState.StateTask));
    }*/
}

[Serializable]
public class SubtaskState:SavedState
{
    public string Path;
    public string Name;
    public string Software;
    public int CurrentVersion;
    public int[] Versions;
    public string Comment;
    public string Todo;
    public string Retakes;

    public SubtaskState(string _path, string _name, string _software, int _currentVersion, int[] _versions, string _comment, string _todo, string _retakes)
    {
        Path = _path;
        Name = _name;
        Software = _software;
        CurrentVersion = _currentVersion;
        Versions = _versions;
        Comment = _comment;
        Todo = _todo;
        Retakes = _retakes;
    }
}
