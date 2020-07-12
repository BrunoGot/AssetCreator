using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtask_View : MonoBehaviour, ISubtask_View
{
    //events
    public event EventHandler<EventArgs> addNewVersionEvent;
    public event EventHandler<ChangeVersionEvent> changeVersionEvent;
    private Text m_softwareUsed;
    private Dropdown m_dropDownVersion;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Awake subtask");
        //init GUI
        m_softwareUsed = transform.Find("TextSoftware").GetComponent<Text>();
        m_dropDownVersion = transform.Find("DropdownVersion").GetComponent<Dropdown>();
        Button AddVersion = this.transform.Find("AddVersionButton").GetComponent<Button>();
        AddVersion.onClick.AddListener(AddNewVersion);
        m_dropDownVersion.onValueChanged.AddListener(ChangeVersion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string _software, List<string> _versions, int _currentVersion)
    {
        m_softwareUsed.text = _software;
        UpdateDropdownVersions(_versions);
        Debug.Log("UpdateView : _currentVersion = " + _currentVersion);
        Dropdown.OptionData option = new Dropdown.OptionData(_currentVersion.ToString().PadLeft(3, '0'));
        //
        m_dropDownVersion.value = m_dropDownVersion.options.FindIndex((i) => { return i.text.Equals(_currentVersion.ToString().PadLeft(3, '0')); });
    }
    
    private void UpdateDropdownVersions(List<string> _versions)
    {
        m_dropDownVersion.ClearOptions();
        m_dropDownVersion.AddOptions(_versions);
    }

    public void Clean()
    {
        m_softwareUsed.text = "";
        m_dropDownVersion.value = 0;
    }
    private void AddNewVersion()
    {
        addNewVersionEvent(this, new EventArgs());
    }

    private void ChangeVersion(int _version)
    {
        Debug.Log("ChangeVersion : version = " + _version);
        int version = int.Parse(m_dropDownVersion.options[_version].text); //sale trouver un moyen pour gerer les version juste avec du texte et non avec des int pour eviter de parser
        changeVersionEvent(this, new ChangeVersionEvent(version));
    }
}
