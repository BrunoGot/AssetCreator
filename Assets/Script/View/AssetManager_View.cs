using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System;
using UnityEngine.UI;

public class AssetManager_View : MonoBehaviour, IAssetManagerView
{
	public event EventHandler<LoadAssetsArgs> loadAssetEvent;
	public event EventHandler<SaveAssetsArgs> saveAssetEvent;
	// Start is called before the first frame update

	//gui
	private Dictionary<TaskName, Button> m_pipelineButtons; //dic containing all the buttons of the 'pipeline' panel
    void Start()
    {
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
		FileBrowser.SetDefaultFilter(".jpg");
		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
		FileBrowser.AddQuickLink("Users", "C:\\Users", null);

	}

	public void InitPipelineButtons(Dictionary<TaskName, ITasksController> _tasks)
	{
		//get ui element
		GameObject pipelinePanel = GameObject.Find("PipelineProcess");
		Button[] buttons = pipelinePanel.GetComponentsInChildren<Button>();
		Button currentButton;
		m_pipelineButtons = new Dictionary<TaskName, Button>();
		int i = 0;
		TaskName currentTask;
		foreach (Button button in buttons)
		{
			currentTask = (TaskName)i;
			m_pipelineButtons.Add(currentTask, button);
			Debug.Log("(TaskName)i = " + currentTask + " Button i = " + button.GetInstanceID());

			m_pipelineButtons[currentTask].onClick.AddListener (_tasks[currentTask].OnSelect);
			i++;
		}
	}
    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTask(TaskName _taskName)
	{
		Debug.Log("Click on the task : " + _taskName);
	}

	public void OpenAssetButton()
    {
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	public void SaveAssetButton()
	{
		saveAssetEvent(this, new SaveAssetsArgs());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog(true, null, "Load File", "Load");

		// Dialog is closed
		// Print whether a file is chosen (FileBrowser.Success)
		// and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
		string path = FileBrowser.Result;
		Debug.Log("file = "+path);
		if (path != "")
		{
			OnLoadEvent(path);
		}
		//loadAssetEvent(this, new LoadAssetsArgs(FileBrowser.Result));

	}


	public void UpdateTaskButton(TaskName _name, TaskState _state, string _warningMessage)
	{
		
		Debug.Log("Update Task Buttons : name = "+_name+" state = "+_state);
		ColorBlock colBlock = m_pipelineButtons[_name].colors;
		Color col = colBlock.normalColor;
		switch (_state)
		{
			case TaskState.Todo:
				col = new Color(1f, 0.75f, 0.5f, 1.0f);
				break;
			case TaskState.Progressing:
				col = new Color(0.75f,0.75f,1.0f,1.0f);
				break;
			case TaskState.Done:
				col = Color.green;
				break;
		}
		Debug.Log("Warning msg = " + _warningMessage);
		if (string.IsNullOrEmpty(_warningMessage)==false)
		{
			Debug.Log("Warning msg1 = " + _warningMessage);
			m_pipelineButtons[_name].transform.Find("WarningButton").gameObject.SetActive(true);
		}
		colBlock.normalColor = col;
		m_pipelineButtons[_name].colors = colBlock;
	}
	public void IsAssetLoaded(bool val) //update view mode for when an asset is loaded in the system or not
	{
		//update pipeline buttons
		foreach(Button button in m_pipelineButtons.Values)
		{
			button.interactable = val;
		}
		//make valide the save button
		GameObject.Find("Save").GetComponent<Button>().interactable = val;
	}


	//handlers
	void OnLoadEvent(string _path)
	{
		loadAssetEvent(this, new LoadAssetsArgs(_path));
	}
}
