using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Space(10)]
    [Header("--------------AudioSource------------")]
    public AudioSource Sound;
    public AudioSource Music;

    /*  [Space(10)]
      [Header("----------Images On & Off------------")]
      public Image Sound_OnOff;
      public Image Music_OnOff;
      public Image Virbaration_OnOff;

      [Space(10)]
      [Header("----------Sprite On & Off------------")]
      public Sprite[] S;
      public Sprite[] M;
      public Sprite[] H;

      [Space(10)]
      [Header("-------------Bool--------------")]
      public bool IsSound = false;
      public bool IsMusic = false;
      public bool IsV = false;*/

    public AudioClip ClueFound;
    public AudioClip ClueWrong;
    public AudioClip Win;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("M"))
        {
            PlayerPrefs.SetInt("M", 0);
          //  IsMusic = PlayerPrefs.GetInt("M") == 1;
        }
        else
        {
           // IsMusic = PlayerPrefs.GetInt("M") == 1;
        }
        if (!PlayerPrefs.HasKey("S"))
        {
            PlayerPrefs.SetInt("S", 0);
          //  IsSound = PlayerPrefs.GetInt("S") == 1;
        }
        else
        {
           // IsSound = PlayerPrefs.GetInt("S") == 1;
        }

        if (!PlayerPrefs.HasKey("V"))
        {
            PlayerPrefs.SetInt("V", 0);
           // IsV = PlayerPrefs.GetInt("V") == 1;
        }
        else
        {
           // IsV = PlayerPrefs.GetInt("V") == 1;
        }
        ChangeIconSound();
        ChangeIconMusic();
        ChangeIconV();
       // Sound.mute = IsSound;
       // Music.mute = IsMusic;
    }
    public void Sound_Click()
    {
      //  GameManager.instance.audioSource.PlayOneShot(GameManager.instance.clickSound);
        //if (IsSound == false)
        //{
        //    IsSound = true;
        //    Sound.mute = true;
        //}
        //else
        //{
        //    IsSound = false;
        //    Sound.mute = false;
        //}
        //PlayerPrefs.SetInt("S", IsSound ? 1 : 0);
        ChangeIconSound();
    }

    public void Music_Click()
    {// GameManager.instance.audioSource.PlayOneShot(GameManager.instance.clickSound);
        //if (IsMusic == false)
        //{
        //    IsMusic = true;
        //    Music.mute = true;
        //}
        //else
        //{
        //    IsMusic = false;
        //    Music.mute = false;
        //}
        //PlayerPrefs.SetInt("M", IsMusic ? 1 : 0);
        ChangeIconMusic();
    }
    public void Virbaration()
    {
      //  GameManager.instance.audioSource.PlayOneShot(GameManager.instance.clickSound);
        //if (IsV == false)
        //{
        //    IsV = true;
        //}
        //else
        //{
        //    Vibration.Vibrate(100, 200);
        //    IsV = false;
        //}
        //PlayerPrefs.SetInt("V", IsV ? 1 : 0);
        ChangeIconV();
    }

    private void ChangeIconSound()
    {
    //    if (IsSound == false)
    //    {
    //        Sound_OnOff.sprite = S[0];
    //        // Sound_OnOff.enabled = true;
    //    }
    //    else
    //    {
    //        Sound_OnOff.sprite = S[1];
    //        // Sound_OnOff.enabled = false;
    //    }
    }
    private void ChangeIconMusic()
    {
        //if (IsMusic == false)
        //{
        //    Music_OnOff.sprite = M[0];
        //    // Music_OnOff.enabled = true;
        //}
        //else
        //{
        //    Music_OnOff.sprite = M[1];
        //    //  Music_OnOff.enabled = false;
        //}
    }
    private void ChangeIconV()
    {
        //if (IsV == false)
        //{
        //    Virbaration_OnOff.sprite = H[0];
        //    // Music_OnOff.enabled = true;
        //}
        //else
        //{
        //    Virbaration_OnOff.sprite = H[1];
        //    //  Music_OnOff.enabled = false;
        //}
    }

    public void BackGroudMusic_On_Off(bool IsOn)
    {
        if (IsOn == false)
        {
            Music.enabled = false;
        }
        else if (IsOn == true)
        {
            Music.enabled = true;
        }
    }
    public void Btn_Click_Sound()
    {
        // Sound.PlayOneShot(Click);
    }

    public void Exits()
    {
        Application.Quit();
    }
    /* if (!IsV)
     {
         Vibration.Vibrate(100, 200);
     }*/
}

