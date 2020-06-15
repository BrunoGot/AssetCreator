using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVTask: TaskController
{
    public UVTask(TaskState _state, TaskName[] _nextTasks) : base(null, _state, _nextTasks)
    {
        m_taskName = TaskName.UVs;

    }
}
