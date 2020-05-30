﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConceptTask : TaskController
{
    //  private ConceptTaskView m_view;
    IConcept_View m_view;

    //concept list assigned to their ID button
    Dictionary<int, string> m_concepts; //idbutton linked to the img path of the concept

    public ConceptTask(TaskState _state, TaskName[] _nextTasks) : base(_state, _nextTasks)
    {
        m_taskName = TaskName.Concepts;
        GameObject conceptPanel = GameObject.Find("ConceptPanel");
        m_view = conceptPanel.AddComponent<Concept_View>();
        m_view.selectConceptEvent += HandleSelectConcept;
        m_view.addConceptEvent += HandleAddConcept;

        m_concepts = new Dictionary<int, string>();

    }

    public override void OnSelect()
    {
        base.OnSelect();
        m_view.DisplayPanel(true);
       
    }

    private void HandleAddConcept(object _sender, AddConceptEvent _eventArgs)
    {
        Debug.Log("Add a button with id = " + _eventArgs.IdButton);
        m_concepts.Add(_eventArgs.IdButton, _eventArgs.ConceptPath);
    }

    private void HandleSelectConcept(object _sender, MainButtonEvent _eventArg)
    {
        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.Arguments = "/C "+m_concepts[_eventArg.ButtonId];
        cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.Start();
        //Process.Start("CMD.exe", "/C "+m_concepts[_eventArg.ButtonId]);
    }
}