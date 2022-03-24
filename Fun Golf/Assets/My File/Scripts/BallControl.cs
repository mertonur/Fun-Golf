using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallControl : MonoBehaviour
{
    public static BallControl instance;                 

    [SerializeField] private LineRenderer lineRenderer;     //referans
    [SerializeField] private float MaxForce;                //maksimum güç
    [SerializeField] private float forceModifier = 0.5f;    //kuvvet göstergesi çarpanı
    [SerializeField] private GameObject areaAffector;       //topun etrafındaki alan
    [SerializeField] private LayerMask rayLayer;            //rayin algılanmasını sağlıyan alan

    private float force;                                    //güç
    private Rigidbody rgBody;                               //referans
   
    private Vector3 startPos, endPos;
    private bool canShoot = false, ballIsStatic = true;    //topu durdurmayı kolaylaştırmak için
    private Vector3 direction;                              //topun atılacağı yön

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rgBody = GetComponent<Rigidbody>();                
    }

    
    void Update()
    {
        if (rgBody.velocity == Vector3.zero && !ballIsStatic)   //gerekli kontroller
        {
            ballIsStatic = true;                                //bs yi doğru yap
            LevelManager.instance.ShotTaken();                  //level managera bilgi ver
            rgBody.angularVelocity = Vector3.zero;              //açısal hızı 0 a ayarla
            areaAffector.SetActive(true);                       //arae yı aktif et
        }
    }

    private void FixedUpdate()
    {
        if (canShoot)                                               //eğer vurabilirse
        {
            canShoot = false;                                       //vurabiliri yanlış ya
            ballIsStatic = false;                                   //bs yi yanlış olarak güncelle
            direction = startPos - endPos;                          //yönü hesapla
            rgBody.AddForce(direction * force, ForceMode.Impulse);  //yöne güç uygula
            areaAffector.SetActive(false);                          //areanın aktifliğini kapat
            UIManager.instance.PowerBar.fillAmount = 0;             //güç barını sıfırla
            force = 0;                                              //gücü sıfırla
            startPos = endPos = Vector3.zero;                       //vektörleri sıfırla
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Destroyer")                              //Eğer destroyer alana düşerse bölümü bitir
        {
            LevelManager.instance.LevelFailed();                   
        }
        else if (other.name == "Hole")                              //eğer deliğe girerse bölümü bitir
        {
            LevelManager.instance.LevelComplete();                  
        }
    }

    public void MouseDownMethod()                                           
    {
        if(!ballIsStatic) return;                                           //top hareketliyse girme
        startPos = ClickedPoint();                                          
        lineRenderer.gameObject.SetActive(true);                            
        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition); 
    }

    public void MouseNormalMethod()                                         
    {
        if(!ballIsStatic) return;                                           //top hareketliyse girme
        endPos = ClickedPoint();                                                
        force = Mathf.Clamp(Vector3.Distance(endPos, startPos) * forceModifier, 0, MaxForce);   //gücü hesapla
        UIManager.instance.PowerBar.fillAmount = force / MaxForce;              //güç barını çiz
        
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(endPos));   
    }

    public void MouseUpMethod()                                           
    {
        if(!ballIsStatic) return;                                          //top hareketliyse girme
        UIManager.instance.audiostart.HitFun();                             //topa vuruş sesi çıkar
        canShoot = true;                                                    //vurabiliri doğrula
        lineRenderer.gameObject.SetActive(false);                           //linerendererı kapat
    }

  
    Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;                                //yeni vektör
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //kameradan mouse yönüne ray oluştur
        RaycastHit hit = new RaycastHit();                              //raycast oluştur
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer))    //vurdu mu
        {
            position = hit.point;                                       //pozisyona kaydet
        }
        return position;                                                //pozisyon değerini döndür
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

#endif

}
