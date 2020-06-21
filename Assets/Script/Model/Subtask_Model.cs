using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtask_Model
{
    private string m_path;
    private string m_name;
    private string m_software;
    private int m_currentVersion;
    private List<int> m_versions;
    private string m_commentaire;
    private string m_todo;
    private string m_retakes;

    public string Software { get { return m_software; } }
    public List<int> Versions { get { return m_versions; } }
    public int CurrentVersion { get { return m_currentVersion; } }

    public Subtask_Model(string _name, int _softwareIndex)
    {
        //m_versions = pipelineSystem.GetVersions(subtaskName, TaskName) 
    }


}
