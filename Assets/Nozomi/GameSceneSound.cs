using Unity.VisualScripting;
using UnityEngine;

public class GameSceneSound : SoundPlayer
{
    private GameObject hitObj;
    HitManager hitManager;

    private GameObject flashObj;
    Flash flashSc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        BGMPlayer("BGM_1");
        hitObj = GameObject.Find("Hit");
        hitManager = hitObj.GetComponent<HitManager>();
        flashObj = GameObject.Find("Flash");
        flashSc = flashObj.GetComponent<Flash>();

    }

    // Update is called once per frame
    void Update()
    {

        if (hitManager.Mode == HitManager.modeChange.cameraMode)
        {

            if (Input.GetMouseButton(0))
            {
                if (hitManager.HitCoolUp == true)
                {
                    Debug.Log("‰¹‚P");
                    SEPlayer("SE_6_Camera", false);
                }

            }

        }

        if (hitManager.Mode == HitManager.modeChange.flashMode)
        {

            if (Input.GetMouseButton(0))
            {
                if (flashSc.FlashCoolUp == true)
                {
                    Debug.Log("‰¹2");
                    SEPlayer("SE_6_Camera", false);
                }

            }

        }
    }
}
