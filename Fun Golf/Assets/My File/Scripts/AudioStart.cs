using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStart : MonoBehaviour
{
    //Ses Kayna�� Ve Di�er Ses Dosyalar�
    [SerializeField] public AudioSource AllAudios;

    [SerializeField] public AudioClip AudioFailed;

    [SerializeField] public AudioClip AudioSuccess;

    [SerializeField] public AudioClip AudioHit;
    [SerializeField] public AudioClip AudioHit2;
    [SerializeField] public AudioClip AudioHit3;
    [SerializeField] public AudioClip AudioHit4;
    [SerializeField] public AudioClip AudioHit5;
    [SerializeField] public AudioClip AudioHit6;
    [SerializeField] public AudioClip AudioHit7;

    [SerializeField] public AudioClip AudioButtons;
    


    
  
    public void FailedFun()
    {
        AllAudios.PlayOneShot(AudioFailed, 0.7F);// Ba�ar�s�z olununca b�l�m sonu sesi
    }
    public void SuccessFun()
    {
        AllAudios.PlayOneShot(AudioSuccess, 0.7F); // Ba�ar�l� olunca b�l�m sonu sesi
    }

    public void HitFun()
    {
        //Vuru� sonras� rastgele 7 farkl� sesten birisi �al�yor
        int xcount = Random.Range(1, 8);

        if (xcount == 1) { AllAudios.PlayOneShot(AudioHit, 0.7F); }
        else if (xcount == 2) { AllAudios.PlayOneShot(AudioHit2, 0.7F); }
        else if (xcount == 3) { AllAudios.PlayOneShot(AudioHit3, 0.7F); }
        else if (xcount == 4) { AllAudios.PlayOneShot(AudioHit4, 0.7F); }
        else if (xcount == 5) { AllAudios.PlayOneShot(AudioHit5, 0.7F); }
        else if (xcount == 6) { AllAudios.PlayOneShot(AudioHit6, 0.7F); }
        else if (xcount == 7) { AllAudios.PlayOneShot(AudioHit7, 0.7F); }
    }

    public void ButtonFun()
    {
        //Butona t�klama sesi
        AllAudios.PlayOneShot(AudioButtons, 0.7F);
    }
}
