using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IConcept_View:ITask_View
{
    event EventHandler<AddConceptEvent> addConceptEvent;//spread Add panel signal
    event EventHandler<MainButtonEvent> selectConceptEvent;
    event EventHandler<MainButtonEvent> removeConceptEvent;

    void DisplayPanel(bool _val); //show or hide the concept panel
    void LoadConcepts(string[] _vals);
}

public class AddConceptEvent
{
    public string ConceptPath { get; private set; }
    public int IdButton { get; private set; }

    public AddConceptEvent(int _idButton, string _imgPath)
    {
        ConceptPath = _imgPath;
        IdButton = _idButton;
    }
}
