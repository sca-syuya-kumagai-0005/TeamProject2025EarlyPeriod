using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj=GameObject.Find("SpawnManager");
        spawnManager = obj.GetComponent<SpawnManager>();
        shoot=false;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            collider.enabled = true;//�R���C�_�[��L����
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
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

        for(int i = 0;i < hitEnemies.Count; i++)
        {
            //�G���[�̉��C���B���܂��낵���Ȃ��̂Ńv���O�����̍Ē������Ђ悤�i�ڍׂ͏�j
            if (hitEnemies[i] == null)
            {
                continue;
            }

            ClickTest2D clickTest = hitEnemies[i].GetComponent<ClickTest2D>();
            clickTest.AlphaStart = true;
        }
        collider.enabled = false;
        
    }
}
