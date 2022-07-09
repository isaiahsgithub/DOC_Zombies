using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        //Hide mouse - to get it back press ESCAPE
        Cursor.lockState = CursorLockMode.Locked;
    }
}
