using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtask_View : MonoBehaviour, ISubtask_View
{
    //events
    public event EventHandler<EventArgs> onOpenSoftware;
    private Text m_softwareUsed;
    private Dropdown m_dropDownVersion;
    // Start is called before the first frame update
    void Awake()
    {
        m_softwareUsed = transform.Find("TextSoftware").GetComponent<Text>();
        m_dropDownVersion = transform.Find("DropdownVersion").GetComponent<Dropdown>();
        Button openSoftButton = transform.Find("OpenSoft_Button").GetComponent<Button>();
        openSoftButton.onClick.AddListener(OpenSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string _software, List<int> _versions, int _currentVersion)
    {
        m_softwareUsed.text = _software;
    }

    public void Clean()
    {
        m_softwareUsed.text = "";
        m_dropDownVersion.value = 0;
    }

    private void OpenSoftware()
    {
        onOpenSoftware(this, new EventArgs());
    }
}
