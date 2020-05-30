using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System;

//mix of a singleton and a façade pattern
public class FileReader: MonoBehaviour
{
	//singleton unity style
	private static FileReader m_instance; //= new FileReader()
	public static FileReader Instance {
		get { if (m_instance == null)
			{
				m_instance = GameObject.Find("ModalWindows").AddComponent<FileReader>();
			}
			return m_instance;
		} 
	}
	//parameters
	private bool m_folderMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OpenFile(Action<string> _callback)
	{
		m_folderMode = false;
		StartCoroutine(ShowLoadDialogCoroutine(_callback));
	}

	public void OpenAsset(Action<string> _callback)
	{
		m_folderMode = true;
		StartCoroutine(ShowLoadDialogCoroutine(_callback));
	}

	IEnumerator ShowLoadDialogCoroutine(Action<string> _callback)
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog(m_folderMode, null, "Load File", "Load");

		// Dialog is closed
		// Print whether a file is chosen (FileBrowser.Success)
		// and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
		string path = FileBrowser.Result;
		Debug.Log("file = " + path);
		_callback(path);//giveback the path in a function as parameter
		//loadAssetEvent(this, new LoadAssetsArgs(FileBrowser.Result));

	}
}
