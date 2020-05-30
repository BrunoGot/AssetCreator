﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetManagerController
{
    private IAssetManagerView m_view;
    private AssetManagerModel m_model;
    //add the model

    public AssetManagerController(IAssetManagerView _view,AssetManagerModel _model)
    {
        m_view = _view;
        m_model = _model;
        m_view.loadAssetEvent += LoadAsset;
        m_model.updateTaskEvent += UpdateTaskView;
        m_view.InitPipelineButtons(m_model.GetTasks());

    }

    //trigged when click on load asset button
    private void LoadAsset(object _sender, LoadAssetsArgs _args)
    {
        string parentFolder = Path.GetDirectoryName(_args.Path);
        parentFolder = Path.GetDirectoryName(parentFolder);
        parentFolder = Path.GetFileName(parentFolder);
        Debug.Log("Asset loaded : " + parentFolder+ " m_model.AssetPipelineFolder = "+ m_model.AssetPipelineFolder);
        //Debug.Log("folder = " + );
        if (parentFolder == m_model.AssetPipelineFolder)
            m_model.LoadAsset(_args.Path);
        else 
        {
            ModalWindows.ModalWindow.ThrowError("error loading asset");
        }
    }

    private void UpdateTaskView(object _sender, UpdatTaskEvent _args)
    {
        Debug.Log("UpdateTaskView");
        foreach(TaskName taskName in _args.Tasks.Keys)
        {
            m_view.UpdateTaskButton(taskName, _args.Tasks[taskName].GetState(), _args.Tasks[taskName].GetWarning()); //send state value of the task to the view part
        }
    }
    //add here the interaction with the button
    //send it to the model

}
