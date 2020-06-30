using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
//using UnityEditorInternal;
using UnityEngine;

/*
 * TODO for saving : 
 * Save the Concept by storing the imgPath and the index of the associated panel in a strucutre like dictionary or serialize it.
 * Get the signal from the panelboard module for each panle loaded with their index.
 * Get the index from this signal, use it to found the associated img path, stored in a dictionary (index, imgPath). 
 * Recreate or reload the concept art from these information
 * 
 */
public class ConceptTask : TaskController
{
    //  private ConceptTaskView m_view;
    IConcept_View m_view;

    //concept list assigned to their ID button
    Dictionary<int, string> m_concepts; //idbutton linked to the img path of the concept

    //events 
    public override event EventHandler<UpdateTaskEvent> updateTaskEvent; //sended to the asset manager part

    public ConceptTask(AssetManagerModel _assetManager, TaskState _state, TaskName[] _nextTasks) : base(_assetManager,_state, _nextTasks)
    {
        m_taskName = TaskName.Concepts;
        GameObject conceptPanel = GameObject.Find("ConceptPanel");
        //Debug.Log("Concept panel = " + conceptPanel);
        m_view = conceptPanel.AddComponent<Concept_View>();
        m_view.selectConceptEvent += HandleSelectConcept;
        m_view.addConceptEvent += HandleAddConcept;
        m_view.validTaskEvent += HandleUpdateConcept;
        m_view.removeConceptEvent += HandleRemoveConcept;
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
        if (m_state == TaskState.Todo)
        {
            m_state = TaskState.Progressing;
            updateTaskEvent(this, new UpdateTaskEvent(TaskName.Concepts, this));
        }
    }

    private void HandleSelectConcept(object _sender, MainButtonEvent _eventArg)
    {
        Debug.Log("try to open image");
        if (m_concepts.ContainsKey(_eventArg.ButtonId))
        {
            Debug.Log("Open image " + _eventArg.ButtonId);
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/C " + m_concepts[_eventArg.ButtonId];
            cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.Start();
        }
        //Process.Start("CMD.exe", "/C "+m_concepts[_eventArg.ButtonId]);
    }
    //trigged when remove a concept from view
    private void HandleRemoveConcept(object _sender, MainButtonEvent _eventArg)
    {
        Debug.Log("RemoveObject id = "+ _eventArg.ButtonId);
        m_concepts.Remove(_eventArg.ButtonId);
        //Process.Start("CMD.exe", "/C "+m_concepts[_eventArg.ButtonId]);
    }

    public override SavedState Serialize()
    {
        string[] imgPaths = new string[m_concepts.Count];
        int index = 0;
        foreach (string imgPath in m_concepts.Values)
        {
            imgPaths[index] = imgPath;
            index++;
        }
        Debug.Log("Saving concept");
        MementoHandler m = new MementoHandler(); //using memento pattern to handle saving/loading and also undo/redo in the 
        ConceptState cs = new ConceptState(imgPaths, m_state);

        m.SetState(cs); //will move when a new concept is added, to handle undo/redo functionality
        return m.GetState();
/*        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream("C:\\Users\\Natspir\\NatspirProd\\Test.assetProd", FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, m.GetState());
        stream.Close();*/
    }
    public override void Deserialize(SavedState _savedState)
    {
        base.Deserialize(_savedState);
        /*IFormatter formatter = new BinaryFormatter();

        Stream stream = new FileStream("C:\\Users\\Natspir\\NatspirProd\\Test.assetProd", FileMode.Open, FileAccess.Read);
        string[] imgPaths = ((ConceptState)formatter.Deserialize(stream)).ImgPaths;
        stream.Close();*/
        ConceptState conceptState = ((ConceptState)_savedState);
        //Debug.Log(imgPaths);
        m_view.LoadConcepts(conceptState.ImgPaths);
        //load the state of the task
        HandleUpdateConcept(this,  new UpdateStateEvent(conceptState.StateTask));
    }

    public void HandleUpdateConcept(object _sender, UpdateStateEvent _args)
    {
        Debug.Log("Valid the step !");
        m_state = _args.State;
        updateTaskEvent(this, new UpdateTaskEvent(TaskName.Concepts, this));
    }
}



[Serializable]
public class ConceptState : SavedState
{
    public string[] ImgPaths; //path of the loaded images
    public TaskState StateTask;
    public ConceptState(string[] _imgPaths, TaskState _state)
    {
        ImgPaths = _imgPaths; //the differents image path of the concepts
        StateTask = _state; //save the state of the task
    }
}



