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
        GameObject obj=GameObject.Find("SpawnManager");//�G�𐶐�����X�|�i�[���������đ��
        spawnManager = obj.GetComponent<SpawnManager>();//spawnManager�ɁA��Ō��������I�u�W�F�N�g��Inspector����SpawnManager���擾
        shoot=false;
        clickFarst=true;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.enabled=Input.GetMouseButton(0);//�}�E�X����������R���C�_�[��L����
    }

    ///Unity�ŗp�ӂ���Ă���֐��@�����蔻��@�p�r�ɉ����Ďg���֐����Ⴄ���璍�Ӂ@�ڂ����͎����Œ��ׂ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool haveEnemy = false;
        for (int i = 0; i < hitEnemies.Count; ++i)
        {
            if (hitEnemies[i] == collision.gameObject)
            {
                haveEnemy = true;//���łɓ����I�u�W�F�N�g���擾���Ă��邩�ǂ���
            }
        }

        if (!haveEnemy)//���łɎ擾���Ă���I�u�W�F�N�g�����O
        {
            hitEnemies.Add(collision.gameObject);//�܂��ǉ����Ă��Ȃ��I�u�W�F�N�g�Ȃ�List�ɒǉ�����
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheak clickTest = hitEnemies[i].GetComponent<HitCheak>();//�e�G�ɂ��Ă���HitCheckScript���擾
            clickTest.AlphaStart = true;//HitCheckScript��AlphaStart��True�ɕύX
        }
         hitEnemies=new List<GameObject>();//hitEnemies��������
    }
}
