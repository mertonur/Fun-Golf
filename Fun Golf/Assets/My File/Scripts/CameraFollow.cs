using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField] private ActiveVectors activeVectors;   //ekseni takip etmesine izin verilen sınıf

    private GameObject followTarget;                        //referans
    private Vector3 offset;                                 //kamera ve top arası ofset
    private Vector3 changePos;                              //kamera pozisyonu takip için

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

    }

    public void BaslangicKamera()
    {
        instance.changePos.x = 10;
    }
    /// <summary>
    /// Method to set target from other scripts
    /// </summary>
    public void SetTarget(GameObject target)
    {
        followTarget = target;                                          //hedefi ayarla
        offset = followTarget.transform.position - transform.position;  //ofseti ayarla
        changePos = transform.position;                                 //pozisyonu güncelle
    }

   
    private void LateUpdate()
    {
        if (followTarget)                                               //hedef varsa
        {
            if (activeVectors.x)                                        //x için takip aktifse
            {                                                           //x pozisyonu ayarla
                changePos.x = followTarget.transform.position.x - offset.x;
            }
            if (activeVectors.y)                                        //y için takip aktifse
            {                                                           //y pozisyonu ayarla
                changePos.y = followTarget.transform.position.y - offset.y;
            }
            if (activeVectors.z)                                        //z için takip aktifse
            {                                                           //z pozisyonu ayarla
                changePos.z = followTarget.transform.position.z - offset.z;
            }
            transform.position = changePos;                             //belirtilen konuma geçir kamerayı
        }
    }
}

[System.Serializable]
public class ActiveVectors
{
    public bool x, y, z;
}
