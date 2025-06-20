using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class NormalEnemy : StayEnemyTemplate
{
    float nextMoveTime=0.0f;
    const float nextMoveTimeMax=15.0f;
    const float nextMoveTimeMin=7.0f;
    const float half=0.5f;
    GameObject flashImage;
    enum State
    {
        HIDE = 0, //�������Ȃ�
        MOVE,//�ړ���
        OPEN,//��o����
        RUN,//�t���b�V�����ꂽ��
        EXIT,//�ʐ^���B��ꂽ��
    }

    State state=State.HIDE;

    private void Awake()
    {
        flashImage = GameObject.Find("FlashImage").gameObject;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ExectionCoroutine ()) ;
    }

     protected override IEnumerator ExectionCoroutine()
     {
        yield return null;
     }


}