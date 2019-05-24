using UnityEngine;

public class BGMStopper : MonoBehaviour
{
    enum BGMStopType {FADEOUT,CUT}
    [SerializeField]
    private BGMStopType bgmStopType = BGMStopType.CUT;
    
    //[HideInInspector]
    public bool bgmStop;

    private bool stop;

    // Start is called before the first frame update
    void Start()
    {
        bgmStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bgmStop && !stop)
        {
            if (bgmStopType == BGMStopType.CUT)
            {
                stop = true;
                SoundManager.StopBGM();
            }
            else if(bgmStopType == BGMStopType.FADEOUT)
            {
                if(SoundManager.BGMVolumeCheck() > 0)
                {
                    SoundManager.FadeOut();
                }
                else
                {
                    stop = true;
                }
            }
        }
    }
}
