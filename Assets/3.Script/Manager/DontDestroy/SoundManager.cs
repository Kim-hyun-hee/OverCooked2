using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // �� �̸�
    public AudioClip clip;
}


public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [Header("ȿ���� �÷��̾�")]
    public AudioSource[] audioSourcesEffects;
    [Header("BGM �÷��̾�")]
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    [Header("���� ���")]
    public Sound[] effectSound;     //AudioCilp
    public Sound[] bgmSound;

    private bool underOneMinute = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playSoundName = new string[audioSourcesEffects.Length];
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSound.Length; i++)
        {
            if (_name == bgmSound[i].name)
            {
                audioSourceBgm.clip = bgmSound[i].clip;
                audioSourceBgm.Play();
                return;
            }
        }
    }

    public void StopBGM()
    {
        audioSourceBgm.Stop();
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSound.Length; i++)    //����Ʈ������ �迭�� �ش�� ���� �˻�
        {
            if (_name == effectSound[i].name)    //���� for������ �̸��� ��ġ�ϴ� ����� �ҽ� ã�� �����Ű��
            {
                for (int j = 0; j < audioSourcesEffects.Length; j++)    //������� �帧�� ������ �ʰ�, ������ҽ�����Ʈ�� �Ҵ�� ����� Ŭ���� �˻��ϰ� ��������� �ʴ°� ã�´�.
                {
                    if (!audioSourcesEffects[j].isPlaying)   //������ҽ�����Ʈ �迭���� ��������� ���� �뷡�� ã�� ���ǹ�
                    {
                        playSoundName[j] = effectSound[i].name;
                        audioSourcesEffects[j].clip = effectSound[i].clip;  // j��° Ŭ���� effectSound[i]�� �ǰ� ����ǰ� �ȴ�.
                        audioSourcesEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� AudioSource�� ������Դϴ�");        //�� �αװ� �����ٴ°��� ����� �ҽ� ����
                return;
            }
        }
        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�");     //�� �αװ� �����ٴ� ���� �̸��� Ʋ�Ȱų� ���� ����
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourcesEffects[i].Stop();
                return;
            }
        }
        Debug.Log("�������" + _name + "���尡 �����ϴ�");
    }


    // �ٸ� Ŭ�������� ����� Ŭ�� �ҷ����� ��
    // ��ũ��Ʈ�� �������� - [SerializeField] private string �뷡��;
    // SoundManager.instance.PlaySE(�뷡��);
}

