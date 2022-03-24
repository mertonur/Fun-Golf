using UnityEngine;


public class InputManager : MonoBehaviour
{  
    //Top Etrafında Vurulabilir alan sınırı
    [SerializeField]
    private float distanceBetweenBallAndMouseClickLimit = 1.5f; 
    private float distanceBetweenBallAndMouseClick;             
    private bool canRotate = false;                             //Döndürülebilir mi değeri

    void Update()
    {
        if (GameManager.singleton.gameStatus != GameStatus.Playing) return; //Eğer oyunda değilse fonksyondan çık

        if (Input.GetMouseButtonDown(0) && !canRotate)          //eğer tıkladıysa döndürebilmeyi kapat
        {
            GetDistance();                                      //Top ile mouse tıkı arası uzaklık
            canRotate = true;                                   //döndürebilmeyi aç

            if (distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
            {
                BallControl.instance.MouseDownMethod();         //top kontrolu
            }
        }

        if (canRotate)                                          //eğer döndürebilme açıksa
        {
            if (Input.GetMouseButton(0))                        //ve mouse tıklandıysa
            {                                                   //ve vuruş limitinden küçükse
                if (distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
                {
                    BallControl.instance.MouseNormalMethod();   //normal methodu çağır
                }
                else
                {                                               //değilse kamera methodunu çağır
                    CameraRotation.instance.RotateCamera(Input.GetAxis("Mouse X"));
                }
            }

            if (Input.GetMouseButtonUp(0))                      //eğer tıklandıysa
            {
                canRotate = false;                              //döndürebilmeyi kapat
                                                                //mesafe limitden azsa
                if (distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
                {
                    BallControl.instance.MouseUpMethod();       //topla ilgili methodu çağır
                }
            }
        }
    }

    
    void GetDistance()
    {
        //Topa normal çiziyoruz
        var plane = new Plane(Camera.main.transform.forward, BallControl.instance.transform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //ray oluştur
        float dist;                                                     //uzaklık
        if (plane.Raycast(ray, out dist))
        {
            var v3Pos = ray.GetPoint(dist);                             //verilen mesafedeki nokta
            //uzaklık hesaplama
            distanceBetweenBallAndMouseClick = Vector3.Distance(v3Pos, BallControl.instance.transform.position);
        }
    }

}
