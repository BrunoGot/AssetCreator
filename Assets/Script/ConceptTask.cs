using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConceptTask : TaskController
{
    //  private ConceptTaskView m_view;
    GameObject conceptPanel;

    public ConceptTask(TaskState _state, TaskName[] _nextTasks) : base(_state, _nextTasks)
    {
        m_taskName = TaskName.Concepts;
        conceptPanel = GameObject.Find("ConceptPanel");
        conceptPanel.SetActive(false);

    }

    public override void OnSelect()
    {
        base.OnSelect();
        conceptPanel.SetActive(true);
    }
}
