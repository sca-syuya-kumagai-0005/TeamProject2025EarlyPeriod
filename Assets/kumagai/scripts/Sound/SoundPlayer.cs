
using UnityEngine;
public class SoundPlayer : SoundManager
{
    /// <summary>
    /// SE���Đ�����֐�
    /// </summary>
    /// <param name="name">�Đ��������t�@�C���̖��O</param>
    /// <param name="loop">�Đ��ニ�[�v���邩�ǂ���</param>
    protected void SEPlayer(string name, bool loop)
    {
      
        AudioClip clip = SetSound(name,sePath);

       
;        if (clip == null)//�V�񂾍��Ձ@�G���[���O���J���t���ɕ\������
        {
            string[] color = new string[4] { "cyan", "yellow", "lime", "fuchsia" };
            string check = this.gameObject.name + "�ŌĂ΂�Ă���SEPlayer�ɑΉ�����SE���������Ă��܂���B";
            string output = null;
            for (int i = 0; i < name.Length; i++)
            {
                output += $"<color={color[i % color.Length]}>{name[i]}</color>";
            }
            Debug.LogError(output);
            return;
        }
        GameObject seObj = Resources.Load<GameObject>(seAudioSource);//Resources�t�H���_�[�ɓ����Ă���SE�p�̃I�[�f�B�I�\�[�X���擾
        GameObject obj = Instantiate(seObj);//�擾�������̂𐶐����āA���������I�u�W�F�N�g���擾
        
        AudioSource se = obj.GetComponent<AudioSource>();//���������I�u�W�F�N�g����AudioSource�R���|�[�l���g���擾

        se.clip = clip;//�擾����AudioSource�Ɉ����œn�����I�[�f�B�I�t�@�C������
        se.loop = loop;//���[�v���邩�ǂ�����ݒ�

        se.Stop();
        se.Play();
        float length=se.clip.length;
        if (!loop)
        {
            StartCoroutine(DestroySE(obj, clip.length));
        }
    }

    protected void BGMPlayer()
    {
        GameObject obj;
        obj = GameObject.Find("BGMPlayer").gameObject;
        if (obj==null)
        {
            obj=(GameObject)Instantiate(Resources.Load(bgmAudioSource));
        }
        AudioClip clip = SetSound(name,bgmPath);
        AudioSource bgm=obj.GetComponent<AudioSource>();
        bgm.clip = clip;
        bgm.Stop();
        bgm.Play();
    }
}
