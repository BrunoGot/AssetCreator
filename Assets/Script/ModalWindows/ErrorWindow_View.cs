using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ErrorWindow_View : MonoBehaviour
{
    private Text m_text;

    // Start is called before the first frame update
    public void Init()
    {
        m_text = transform.Find("Text").GetComponent<Text>();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Display(string _msg)
    {
        m_text.text = _msg;
        gameObject.SetActive(true);
    }

    public void OKButton()
    {
        Hide();
    }
}
