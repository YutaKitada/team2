using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> seList;
    public static List<AudioClip> soundList_SE;
    [SerializeField]
    private List<AudioClip> bgmList;
    public static List<AudioClip> soundList_BGM;

    public static AudioSource se;
    public static AudioSource bgm;

    // Start is called before the first frame update
    void Start()
    {
        BGM_SE_Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //SEを再生
    public static void PlaySE(int seNumber)
    {
        se.PlayOneShot(soundList_SE[seNumber]);
    }

    //BGMを再生
    public static void PlayBGM(int bgmNumber)
    {
        bgm.Stop();
        bgm.volume = 1f;
        bgm.clip = soundList_BGM[bgmNumber];
        bgm.Play();
    }

    //BGMを止める
    public static void StopBGM()
    {
        bgm.Stop();
    }

    //現在のBGMが指定したBGMと一緒か否か
    public static bool CheckBGM(int bgmNumber)
    {
        if (bgm.clip == soundList_BGM[bgmNumber])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //BGMをフェードアウトさせる
    public static void FadeOut()
    {
        bgm.volume -= 0.005f;
    }
    
    //現在のBGMを取得
    public static AudioSource GetBGM()
    {
        return bgm;
    }

    //BGMとSEをロードする
    public void BGM_SE_Load()
    {
        soundList_SE = seList;
        soundList_BGM = bgmList;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        se = audioSources[0];
        bgm = audioSources[1];
    }
}
