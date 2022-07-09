using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    void Update()
    {
        //Allows the gun to continuously rotate
        transform.Rotate(Vector3.up * 30.0f * Time.deltaTime);
    }
}
