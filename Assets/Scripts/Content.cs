using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public Text text_;

    public void UpdateContent(string message)
    {
        text_.text = message;
        text_.SetNativeSize();
    }
}
