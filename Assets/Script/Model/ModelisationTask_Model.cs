using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Idea : 
 * Use a singleton to handle system stuff. And file systeme of the pipeline. make a kind of mapping between task and pipeline path of the tasks
 * 
 * */
public class ModelisationTask_Model:TaskModel
{
    private Dictionary<int, Subtask_Controller> m_Subtasks; //dictionary of the subtask linked to their button id

    //public event EventHandler<LoadSubtaskEvent> onLoadSubtask;

    public ModelisationTask_Model(AssetManagerModel _assetModel, TaskName _taskName) : base(_assetModel, _taskName)
    {
        m_Subtasks = new Dictionary<int, Subtask_Controller>();

    }

    /*public void OpenAsset(string _softwareId)
    {
        Debug.Log("m_assetManager.AssetName = " + m_assetManager.AssetName);
        Debug.Log("m_assetManager.AssetPipelineFolder = " + m_assetManager.AssetPath);
        Debug.Log("task path = " + m_mappingTaskPath[m_taskName]);
        Debug.Log("software extention = " + m_softwareList[_softwareId]);
        Debug.Log("asset version = " + m_mappingVersionPath[m_currentVersion]);
        string finalPath = m_assetManager.AssetPath+ m_mappingTaskPath[m_taskName]+"\\"+m_mappingVersionPath[m_currentVersion]+"\\"+ m_assetManager.AssetName + m_softwareList[_softwareId];
        Debug.Log("finalPath  = " + finalPath);
        Debug.Log("hard path = C:\\Users\\Natspir\\NatspirProd\\03_WORK_PIPE\\01_ASSET_3D\\Enviro\\DataTunnel\\3d\\scenes\\Mode\\mode\\work_v002\\untitled.blend");
        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.Arguments = "/C "+ finalPath; //TODO : use asset path + add the task path + add the version path + open it in the software. If there is no file for the software trig a message to open in other soft or import from the file as obj
        cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.Start();
    }*/

    public void CreateSubtask(string _subtaskName, int _softwareIndex, int _panelID, ISubtask_View viewPart)
    {
        Subtask_Model subTaskModel = new Subtask_Model(m_assetManager,m_taskName, _subtaskName, _softwareIndex);
        Subtask_Controller subTask = new Subtask_Controller(viewPart, subTaskModel);
        m_Subtasks[_panelID] = subTask;
    }

    public void SelectSubtask(int _id)
    {
        m_Subtasks[_id].Display();
    }

    public override SavedState SaveState()
    {
        List<SavedState> subtaskStates = new List<SavedState>();
        //saving all the subtasks
        int i = 0;
        foreach(Subtask_Controller subtask in m_Subtasks.Values)
        {
            subtaskStates.Add(subtask.Serialize());
            i++;
        }
        TaskModelState taskState = new TaskModelState(subtaskStates);
        return taskState;
    }
    public void Load(SavedState _savedState)
    {
        //todo:
        /*
         * for i in m_subtasks : 
         *  removeSubtask(i)
         */
         
        
        //Debug.Log("nombre de soustaches = " + state.SubtasksDatas.Length);

    }

    public void RemoveSubtask(int _idButton)
    {
        m_Subtasks[_idButton].Remove();//clean and delete view and model
        m_Subtasks.Remove(_idButton);//rempve the subtask controller
    }

    public void Clean()
    {
        //cleanstuff
    }
}

public class LoadSubtaskEvent:EventArgs
{
    public SubtaskState SubtasksData;
    public LoadSubtaskEvent(SubtaskState _subtaskState)
    {
        SubtasksData = _subtaskState;
    }
}