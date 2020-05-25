using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalWindows 
{

    private static ModalWindows m_ModalWindow;
    public static ModalWindows ModalWindow { 
        get { 
            if (m_ModalWindow == null) {
                m_ModalWindow = new ModalWindows();
            }
            return m_ModalWindow;
        }
    }

    private ErrorWindow_View m_errorWindow;

    public ModalWindows()
    {
        GameObject win = GameObject.FindWithTag("ModalWindows");
        if(win == null)
        {
            Debug.LogError("le tag 'ErrorWindow' n'as pas été trouvé, verifier si il ya une 'error window' avec le bon tag");
        }
        else
        {
            m_errorWindow = win.transform.Find("ErrorWindow").GetComponent<ErrorWindow_View>();
            m_errorWindow.Init();
            m_errorWindow.Hide();
        }
    }

    public void ThrowError(string _msg)
    {
        m_errorWindow.Display(_msg);
    }


}
