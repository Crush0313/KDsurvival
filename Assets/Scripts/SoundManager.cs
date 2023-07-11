using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    #region �̱���
    void Awake() //��ü ���� �� ���� ����
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion

    public string[] playSoundName;

    public AudioSource[] audioSorceEffects; //���� ��� ���� �迭��
    public AudioSource audioSourceBGM;

    public Sound[] effectSounds;
    public Sound[] BGMsound;

    private void Start()
    {
        playSoundName = new string[audioSorceEffects.Length]; //����� �ҽ� ����ŭ ����
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++) //���� ���̺귯������ �ش� �̸��� ���� ã��
        {
            if (_name == effectSounds[i].name)
            {
                Debug.Log("ã����"+ i);
                for (int j = 0; j < audioSorceEffects.Length; j++) //��� ������  ���� �� ������ҽ� ã��
                {
                    if (!audioSorceEffects[j].isPlaying)
                    {
                        Debug.Log("ã��" + j);
                        Debug.Log(_name);
                        Debug.Log(effectSounds[i].name);

                        playSoundName[j] = effectSounds[i].name;
                        audioSorceEffects[j].clip = effectSounds[i].clip; //Ŭ�� �ֱ�
                        audioSorceEffects[j].Play(); //���
                        return; //�Լ� ����
                    }
                }
                Debug.Log("��� ���� audioSorce�� ������Դϴ�."); //�� �ҽ��� ����
                return;
            }
        }
        Debug.Log(name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�."); //��ϵ� �̸��� ���尡 �ƴ�
    }
    public void StopAllSE()
    {
        for (int i = 0; i < audioSorceEffects.Length; i++) //��ȸ�ϸ� ���� ����
        {
            audioSorceEffects[i].Stop();
        }
    }
    public void StopSE(string _name) //Ư�� ���� ����
    {
        for (int i = 0; i < audioSorceEffects.Length; i++) //��ȸ�ϸ� ���� ����
        {
            if (playSoundName[i] == _name) //����ҽ��� �� ���¿����� �� ���ϴ�, ������ �ε����� �Ҵ�Ǵ� ���ڿ� ���� �迭�� �̿��� Ž��
            {
                audioSorceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("��� ����" + _name + "���尡 �����ϴ�.");
    }
}
