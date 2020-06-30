using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineSystem
{
    private Dictionary<string, string> m_softwareExtensions; //link software name with their extensions
    Dictionary<TaskName, string> m_mappingTaskPath; //link taskname with their path on the computer
    Dictionary<int, string> m_mappingVersionPath; //(versionNumber, versionPath)

    //singleton
    private static PipelineSystem m_system;
    static public PipelineSystem System
    {
        get
        {
            if (m_system == null)
            {
                m_system = new PipelineSystem();
            }
            return m_system;
        }
    }

    public PipelineSystem()
    {
        m_softwareExtensions = new Dictionary<string, string>();//will be initialized from the future system singleton class
        m_softwareExtensions["Blender"] = ".blend";
        m_softwareExtensions["Houdini"] = ".hou";
        m_mappingTaskPath = new Dictionary<TaskName, string>();
        m_mappingTaskPath[TaskName.Modelisation] = "\\3d\\scenes\\Mode\\mode"; //will move to the system class or even a singleton pipelineSystem class that make mapping between some asset preferences and pipeline path location
        m_mappingVersionPath = new Dictionary<int, string>();
        m_mappingVersionPath[1] = "work_v001";

    }

    public string GetExtension(string _softwareName)//string will be replace by enum SoftName
    {
        return m_softwareExtensions[_softwareName];
    }

    public string GetPathTask(TaskName _taskName)
    {
        return m_mappingTaskPath[_taskName];
    }
    public string GetVersionPath(int _version)
    {
        return m_mappingVersionPath[_version];
    }
}
