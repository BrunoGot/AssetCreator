using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddPanelEvent : EventArgs
{
    public GameObject ButtonConcept { get; protected set; }
    public AddPanelEvent(GameObject _buttonId)
    {
        ButtonConcept = _buttonId;
    }
}

public class MainButtonEvent : EventArgs
{
    public int ButtonId { get; private set; }
    public MainButtonEvent(int _buttonId)
    {
        ButtonId = _buttonId;
    }
}

public interface IPanelBoardView
{
    event EventHandler<AddPanelEvent> addPanelEvent;
    event EventHandler<MainButtonEvent> onMainButtonEvent;

    // void MainButtonHandler(int _idButton); pas besoin en public finalement
    void PlayButtonHandler();
    void RemovePanel(GameObject _panel); //delete a panel from the view
}
