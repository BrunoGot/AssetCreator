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
    
    public ModelisationTask_Model(AssetManagerModel _assetModel, TaskName _taskName) : base(_assetModel, _taskName)
    {
        m_softwareList = new Dictionary<string, string>();//will be initialized from the future system singleton class
        m_softwareList["blender"] = ".blend";
        m_softwareList["houdini"] = ".hou";

        m_mappingTaskPath = new Dictionary<TaskName, string>();
        m_mappingTaskPath[m_taskName] = "\\3d\\scenes\\Mode\\mode"; //will move to the system class or even a singleton pipelineSystem class that make mapping between some asset preferences and pipeline path location

        m_mappingVersionPath = new Dictionary<int, string>();
        m_mappingVersionPath[1] = "work_v001";

    }

    public void OpenAsset(string _softwareId)
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
    }
}
