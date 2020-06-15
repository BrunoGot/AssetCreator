using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public interface ITask_View
{
    //event
    event EventHandler<UpdateStateEvent> validTaskEvent;

    void DisplayPanel(bool _val); //show or hide the concept panel
    void Reload();//used to reload and update the view with new parameters, used when loading some new assets

}

public abstract class Task_View : MonoBehaviour, ITask_View
{
    public event EventHandler<UpdateStateEvent> validTaskEvent;

    public void DisplayPanel(bool _val) //show or hide the concept panel
    {
        CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = _val ? 1 : 0;
        cg.interactable = _val;
        cg.blocksRaycasts = _val;
    }

    protected virtual void InitGUI()
    {
        DisplayPanel(false);
        Button quitPanel = this.transform.Find("QuitButton").GetComponent<Button>();
        quitPanel.onClick.AddListener(QuitPanel);
        Button validButton = this.transform.Find("ValidButton").GetComponent<Button>();
        validButton.onClick.AddListener(ValidateStep);
    }

    public void ValidateStep()
    {
        Debug.Log("Validate the step");
        validTaskEvent(this, new UpdateStateEvent(TaskState.Done));
        QuitPanel();
    }

    public void Reload()
    {
        Debug.Log("Reload view");
    }

    private void QuitPanel()
    {
        Debug.Log("quit panel");
        DisplayPanel(false);
    }

}
