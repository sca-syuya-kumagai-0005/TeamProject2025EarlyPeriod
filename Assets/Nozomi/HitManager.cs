using UnityEngine;
using System.Collections.Generic;

//���ڈȍ~�̏����ŃG���[����������͗l�B�i���C���ς݁j
/*�G���[�����ƃ\�[�X�̖��A�����āi�x�z�搶����j
 ����
    hitEnemies(List)�̗v�f����Enemy�����������Ɍ���Ȃ��B
    53�s�ڂ�for�����񂵂����ɑz�����������遨null�ŃG���[
�\�[�X�̖��
    �����������ɂ�Enemy���������ɗv�f���������K�v������
    �����A�����_�̃\�[�X���Ɨv�f���̏����ꏊ���w��ł��Ȃ�
    ���R�FEnemy�������Ă���̂�HitManager�ł͂Ȃ�ClickTest2D������
    ���ꂾ��Enemy�̎�ނ𑝂₵������List�̒[�ȊO�����������ɍ���
������
    Enemy������ClickTest2D�ł͂Ȃ�HitManager�ɔC����
    ClickTest2D�Ńt���O�𗧂āAHitManager�ŏ�����List��remove������
 */
public class HitManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> hitEnemies=new List<GameObject>();
    SpawnManager spawnManager;
    private bool shoot;
    private Collider2D collider;
    bool clickFarst;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj=GameObject.Find("SpawnManager");
        spawnManager = obj.GetComponent<SpawnManager>();
        shoot=false;
        clickFarst=true;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.enabled=Input.GetMouseButton(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool haveEnemy = false;
        for (int i = 0; i < hitEnemies.Count; ++i)
        {
            if (hitEnemies[i] == collision.gameObject)
            {
                haveEnemy = true;
            }
        }

        if (!haveEnemy)//���łɎ擾���Ă���Object�����O
        {
            hitEnemies.Add(collision.gameObject);
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheak clickTest = hitEnemies[i].GetComponent<HitCheak>();
            clickTest.AlphaStart = true;
        }
         hitEnemies=new List<GameObject>();
    }
}
