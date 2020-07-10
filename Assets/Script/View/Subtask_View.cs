using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtask_View : MonoBehaviour, ISubtask_View
{
    private Text m_softwareUsed;
    private Dropdown m_dropDownVersion;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Awake subtask");
        m_softwareUsed = transform.Find("TextSoftware").GetComponent<Text>();
        m_dropDownVersion = transform.Find("DropdownVersion").GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string _software, List<string> _versions, int _currentVersion)
    {
        m_softwareUsed.text = _software;
        UpdateDropdownVersion(_versions);
    }
    
    private void UpdateDropdownVersion(List<string> _versions)
    {
        m_dropDownVersion.ClearOptions();
        m_dropDownVersion.AddOptions(_versions);
    }

    public void Clean()
    {
        m_softwareUsed.text = "";
        m_dropDownVersion.value = 0;
    }

}
