using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSample : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        //���N���b�N���󂯕t����
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");

        //�E�N���b�N���󂯕t����
        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        //�~�h���N���b�N���󂯕t����
        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
    }
}