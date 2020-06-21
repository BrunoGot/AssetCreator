using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//Todo for application:
/*
 * Clean all before reload
 * bug detected in the concpet panel. First panel desapear ?
 */

//singleton handeling application preferences and parameter
public class AssetSystem : MonoBehaviour
{
    private Dictionary<TaskName, List<string>> m_taskSoftwares; //list of softwares used by tasks

    private static AssetSystem m_system;
    static public AssetSystem System { 
        get { 
            if (m_system == null) 
            {
                m_system = new AssetSystem();
            }
            return m_system;
        } 
    }

    public AssetSystem()
    {
        m_taskSoftwares = new Dictionary<TaskName, List<string>>();
        List<string> modelSoftware = new List<string>();
        modelSoftware.Add("Blender");
        modelSoftware.Add("Houdini");
        m_taskSoftwares[TaskName.Modelisation] = modelSoftware;
    }

    public List<string> GetSoftwareList(TaskName _taskName)
    {
        return m_taskSoftwares[_taskName];
    }
}
