using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetManagerModel 
{
    // Start is called before the first frame update
    public string AssetName { get; private set; }
    public string AssetPipelineFolder { get; private set; }

    private string m_assetPath;
    private Dictionary<TaskName,ITasksController> m_tasks;

    //events
    public event EventHandler<UpdatTaskEvent> updateTaskEvent;

    public AssetManagerModel()
    {
        AssetPipelineFolder = "01_ASSET_3D";
        InitPipelineTasks();
    }

    private void InitPipelineTasks() //init pipeline tasks with empty value
    {
        m_tasks = new Dictionary<TaskName, ITasksController>();
        m_tasks.Add(TaskName.Concepts, new ConceptTask(TaskState.Todo, new TaskName[] { TaskName.Modelisation })); //replace with new Concept Task
        m_tasks.Add(TaskName.Modelisation, new ModelisationTask(TaskState.Todo, new TaskName[] { TaskName.UVs, TaskName.Rigging })); 
        m_tasks.Add(TaskName.UVs, new UVTask(TaskState.Todo, new TaskName[] { TaskName.Texturing }));
        m_tasks.Add(TaskName.Texturing, new ModelisationTask(TaskState.Todo, new TaskName[] { TaskName.FX })); //replace with new Texturing Task
        m_tasks.Add(TaskName.Rigging, new ModelisationTask(TaskState.Todo, new TaskName[] { TaskName.Animation })); //replace with new Rigging Task
        m_tasks.Add(TaskName.Animation, new ModelisationTask(TaskState.Todo, new TaskName[] { TaskName.FX })); //replace with new Animation Task
        m_tasks.Add(TaskName.FX, new ModelisationTask(TaskState.Todo, new TaskName[] { TaskName.LookDev })); //replace with new FX Task
        m_tasks.Add(TaskName.LookDev, new ModelisationTask(TaskState.Todo, new TaskName[] {})); //replace with new LookDev Task

    }

    public void LoadAsset(string _path) //algorthms to get and parse file/folder to load asset into the manager
    {
        Debug.Log("AssetName = " + Path.GetFileName(_path));
        m_assetPath = _path;
        AssetName = Path.GetFileName(m_assetPath);//todo : update name
        ParseTasks(m_assetPath);
        UpdateTasks();
    }

    public Dictionary<TaskName, ITasksController> GetTasks()
    {
        return m_tasks;
    }

    private void ParseTasks(string _path)
    {

        //InitPipelineTasks()=> desactivé pour l'instnat, provoque une erreur lorsqu'on charge un projet, prevoir une fonction de reinitialisation pour chaque mode ? ;//reinitialise the tasks of the asset
        _path +="/3d/scenes/";
        Debug.Log(Directory.GetDirectories(_path).Length);
        string[] pathsDir = Directory.GetDirectories(_path);
        string folder = "";
        foreach (string path in pathsDir)
        {
            folder = Path.GetFileName(path);
            Debug.Log("folder = " + folder);
            switch(folder.ToLower()){
                case "mode":
                    Debug.Log("Mode Folder detected");
                    m_tasks[TaskName.Modelisation] = new ModelisationTask(TaskState.Progressing, new TaskName[]{ TaskName.UVs, TaskName.Rigging }); //m_state = read a saved file with last saved state
                    break;
                case "UVs":
                    Debug.Log("UV Folder detected");
                    m_tasks[TaskName.UVs] = new UVTask(TaskState.Progressing, new TaskName[] { TaskName.Texturing });
                    break;
                case "Texturing":
                    Debug.Log("Texturing Folder detected");
                    m_tasks[TaskName.Texturing] = new ModelisationTask(TaskState.Progressing, new TaskName[] { TaskName.FX});
                    break;
            }
        }
        CheckStateValues();
    }

    private void CheckStateValues() //check if states values between task are correct
    {
        TaskName currentTask = TaskName.Concepts;
        CheckStateValuesRec(currentTask);
    }

    private void CheckStateValuesRec(TaskName _currentTask) //check if states values between task are correct recursively
    {
        /*
         * rule : 
         *  - a task must be in 'Done' mode to start the nex task
         *  - if the previous task is not setted to Done, the current task can't be validated
         *  - if a task is not in done mode, the next task not in todo mode has a warning message
         *  - a state can be in Todo, InProgress, Done
        /*
         CurrentStep = Firststep
         check currentStep. 
            If not in Done state :
                if Next Sate is not in Todo Mode :
                    set Warning message to the currentStep "This step is not valid"
                    if Next State in Done Mode : 
                        switch to Progressing Mode
            else pass the next task
         */
        
        TaskName[] nextTasks = { };
        if (m_tasks[_currentTask].GetState() != TaskState.Done)
        {
            nextTasks = m_tasks[_currentTask].GetNextTasks();
            foreach(TaskName nextTask in nextTasks)
            {
                if (m_tasks[nextTask].GetState() != TaskState.Todo)
                {
                    m_tasks[nextTask].SetWarning("the task before is not done yet");
                    if (m_tasks[nextTask].GetState() == TaskState.Done)
                    {
                        m_tasks[nextTask].SetState(TaskState.Progressing); //retrograding the state to progressing if some problem has been detected
                    }
                }
                CheckStateValuesRec(nextTask); //recursively check for the next task state
                
            }

        }
    }

    private void UpdateTasks()
    {
        Debug.Log("Update tasks");
        updateTaskEvent(this, new UpdatTaskEvent(m_tasks));
    }

}

public class UpdatTaskEvent : EventArgs
{
    public Dictionary<TaskName,ITasksController> Tasks { get; private set; }
    public UpdatTaskEvent(Dictionary<TaskName, ITasksController> _tasks)
    {
        Tasks = _tasks;
    }
}