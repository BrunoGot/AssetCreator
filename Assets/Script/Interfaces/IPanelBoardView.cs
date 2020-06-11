using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddPanelEvent : EventArgs
{
    public int ButtonId { get; set; }
    public AddPanelEvent()
    {
        //ButtonConcept = _buttonId;
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
    event EventHandler<MainButtonEvent> removePanelEvent;

    void Init(); //used to init manually beofre the "start" of a unityscript
    // void MainButtonHandler(int _idButton); pas besoin en public finalement
    void PlayButtonHandler();
    void RemovePanel(GameObject _panel); //delete a panel from the view
    GameObject CreatePanel(int _id);//create a new panel from the controller side. Return the panel created
}
