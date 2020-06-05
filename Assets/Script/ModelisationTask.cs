using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelisationTask: TaskController
{

    public ModelisationTask(TaskState _state, TaskName[] _nextTasks): base(_state, _nextTasks)
    {
        m_taskName = TaskName.Modelisation;
    }


}
