using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

//todo : 
//use a interface system to plug different configuration of pipeline and then adapt it to different studios easily
public class PipelineSystem
{
    private Dictionary<string, string> m_softwareExtensions; //link software name with their extensions
    Dictionary<TaskName, string> m_mappingTaskPath; //link taskname with their path on the computer
    Dictionary<int, string> m_mappingVersionPath; //(versionNumber, versionPath)
    private string m_pipelinePath;
    private string m_assetPath;
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
        m_mappingTaskPath[TaskName.Modelisation] = "Mode"; //make mapping between some asset preferences and pipeline path location
        
        m_mappingVersionPath = new Dictionary<int, string>();
//        m_mappingVersionPath[1] = "work_v001";

//        m_pipelinePath = "C:\\Users\\Natspir\\NatspirProd\\03_WORK_PIPE";
    }


    public void SetAssetPath(string _assetPath)
    {
        m_assetPath = _assetPath;
    }

    public string GetAssetPath()
    {
        return m_assetPath;
    }

    public string GetExtension(string _softwareName)//string will be replace by enum SoftName
    {
        return m_softwareExtensions[_softwareName];
    }

    public string GetPathTask(TaskName _taskName)
    {
        return Path.Combine(m_assetPath, m_mappingTaskPath[_taskName]);
    }
    public string GetVersionPath(int _version)
    {
        return m_mappingVersionPath[_version];
    }

    public void CreateSubtask(TaskName _taskName, string _subtask)
    {
        string path = Path.Combine(PipelineSystem.System.GetPathTask(_taskName), _subtask);
        if (ConformToPipeline(path) == true)//if the path have been created so create the workV1 by default
        {
            path = Path.Combine(path, m_mappingVersionPath[1]);
            ConformToPipeline(path);
        }
    }
    //check if a directory exist in the pipeline, create it if not return true if the folder have been created
    public bool ConformToPipeline(string _path)
    {
        bool folderCreated = false;
        Debug.Log("check if directory " + _path + " exists");
        if (Directory.Exists(_path) == false)
        {
            Directory.CreateDirectory(_path);
            folderCreated = true;
        }
        return folderCreated;
    }

    //todo : make a load function that reading the .assetProd file and rebuild the pipeline asset file. And signal if conflicts are detected

    //just read the pipeline and load the different version of the asset
    public List<string> GetVersionList(TaskName _taskName, string _subtaskName)
    {

        string path = Path.Combine(GetPathTask(_taskName), _subtaskName);
        Debug.Log("path = "+path);
        string[] listDir = Directory.GetDirectories(path);
        List<string> listVersions = new List<string>();
        for (int i=0; i < listDir.Length; i++)
        {
            // Debug.Log("folder = " + listDir[i]);
            string versionFolder = Directory.GetParent(listDir[i]+"\\").Name;
            // Debug.Log("parent folder = " + versionFolder);
            string version = versionFolder.Split('v')[1];
            m_mappingVersionPath[int.Parse(version)] = versionFolder; //revoir la gestion des versions peut etre ne pas y stocker dans un dico(int, string)
            //Debug.Log("mappingversion[" + int.Parse(version) + "] = " + versionFolder);
            listVersions.Add(version);
        }
        //Debug.Log("Folder list = " + Directory.GetDirectories(path));
        return listVersions;
    }

    public int CreateVersion(TaskName _taskName, string _subtaskName, int _version)
    {
        int version = _version;
        Debug.Log("count m_mappingVersionPath = " + m_mappingVersionPath.Count);
        Debug.Log("current version = " + version);
        int highestVersion = m_mappingVersionPath.Keys.Max();
        bool succes = int.TryParse(m_mappingVersionPath[highestVersion].Split('v')[1], out version); //un peu degeu, voir si _version ne devrait pas etre un string, au lieu de parser a chaque fois ladepuis le dictionnaire, ce qui peut etre source d'erreur.
        if (succes)
        {
            version += 1; //nom de la version
            Debug.Log("version = " + version);
            Debug.Log("_version = " + _version);
            m_mappingVersionPath[_version] = "work_v" + version.ToString().PadLeft(3,'0');
            string path = Path.Combine(new string[] { m_assetPath, m_mappingTaskPath[_taskName], _subtaskName, m_mappingVersionPath[_version] });
            Directory.CreateDirectory(path);
            Debug.Log("new path = " + path);
        }
        else
        {
            ModalWindows.ModalWindow.ThrowError("Error : the version has not been created. Probleme parsing previous version name");
        }

        return version;
    }
}
