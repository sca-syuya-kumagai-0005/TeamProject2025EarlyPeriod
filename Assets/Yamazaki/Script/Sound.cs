using UnityEngine;

public class Sound : SoundPlayer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SEPlayer("SE_8_Door_Open", false);
        BGMPlayer("Test");
    }
}
