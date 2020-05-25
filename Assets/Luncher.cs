using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;

public class Luncher : MonoBehaviour
{
    private AssetManagerController m_assetManager;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        GameObject viewObject;
        viewObject = GameObject.Find("View");
        IAssetManagerView assetManagerView =  viewObject.GetComponent<AssetManager_View>();
        m_assetManager = new AssetManagerController(assetManagerView, new AssetManagerModel());


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
