using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject ballPrefab;           
    public Vector3 ballSpawnPos;            //Topun Başlangıç Konumu

    public LevelData[] levelDatas;          //Tüm Leveller Ve Dataları

    public int shotCount = 0;              //Bölümde Kullanılacak Vuruş ve Süre Sınırları
    public float timeCount = 0;
    private int totalpoint =0;
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
    }

   
    public void SpawnLevel(int levelIndex)
    {
       
        Instantiate(levelDatas[levelIndex].levelPrefab, Vector3.zero, Quaternion.identity);
        if (UIManager.Dif == "Kolay")
        {
            shotCount = levelDatas[levelIndex].shotCountEasy; //Kolay Zorluk Atış Sayısını Ayarla
            timeCount = levelDatas[levelIndex].timeCountEasy; //Kolay Zorluk Süreyi Ayarla
        }
        else if (UIManager.Dif == "Orta")
        {
            shotCount = levelDatas[levelIndex].shotCountMedium; //Orta Zorluk Atış Sayısını Ayarla
            timeCount = levelDatas[levelIndex].timeCountMedium; //Orta Zorluk Süreyi Ayarla
        }
        else if (UIManager.Dif == "Zor")
        {
            shotCount = levelDatas[levelIndex].shotCountHard; //Zor Zorluk Atış Sayısını Ayarla
            timeCount = levelDatas[levelIndex].timeCountHard; //Zor Zorluk Süreyi Ayarla
        }
        if (UIManager.Mod == "Süreye Karsı")
        {
            UIManager.instance.timeText.gameObject.SetActive(true); //Bu Modda Açılacak olan Textler
            UIManager.instance.timeText2.gameObject.SetActive(true);
            UIManager.instance.timeText3.gameObject.SetActive(true);
            UIManager.instance.timeText4.gameObject.SetActive(true);
            UIManager.instance.timeText5.gameObject.SetActive(true);
            UIManager.instance.shotText.gameObject.SetActive(false);

            


        }
        else if(UIManager.Mod == "Atıs Hakkı")
        {
            UIManager.instance.timeText.gameObject.SetActive(false); //Bu Modda Açılacak olan Textler
            UIManager.instance.timeText2.gameObject.SetActive(false);
            UIManager.instance.timeText3.gameObject.SetActive(false);
            UIManager.instance.timeText4.gameObject.SetActive(false);
            UIManager.instance.timeText5.gameObject.SetActive(false);
            UIManager.instance.shotText.gameObject.SetActive(true);
            
        }
        UIManager.instance.textİnfo.text = UIManager.Mod.ToString() + "-" + UIManager.Dif.ToString(); //Mod Bilgisi Ve Zorluğu

        UIManager.instance.ShotText.text = shotCount.ToString();                        //Kalan Vuruş
                                                                   
        GameObject ball = Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity); //Topu Başlangıç Yerinde Yaratma
        CameraFollow.instance.SetTarget(ball);        //Kameranın hedefini top olarak gösterme
        CameraFollow.instance.BaslangicKamera();
       GameManager.singleton.gameStatus = GameStatus.Playing;      //Oyun Statüsünü Playing e çevirme
    }

    public void FixedUpdate()
    {
        if (GameManager.singleton.gameStatus == GameStatus.Playing) //Eğer Oyundaysa Süre Akıcak
        {
            if (UIManager.Mod=="Süreye Karsı") {
                if (timeCount <= 0f)
                {
                    UIManager.instance.timeText.text = "00"; // Bitiş Süresini 00 gösterme
                    UIManager.instance.timeText2.text = "00";
                    UIManager.instance.timeText3.text = "00";
                    LevelFailed();
                }
                else
                {
                    timeCount -= Time.deltaTime;
                    int min = Mathf.FloorToInt(timeCount / 60f); // Kalan Süreyi Dakika saniye salise olarak gösterme
                    int sec = Mathf.FloorToInt(timeCount % 60f);
                    int mil = Mathf.FloorToInt((timeCount * 100f) % 100f);
                    UIManager.instance.timeText.text = min.ToString("00");
                    UIManager.instance.timeText2.text = sec.ToString("00");
                    UIManager.instance.timeText3.text = mil.ToString("00");
                    
                }
            }
        }
      
    }
  
    public void ShotTaken()
    {
        if (shotCount > 0)                                          //Şut hakkı var mı
        {
            shotCount--;                                            //kalan şut hakkını düşür
            UIManager.instance.ShotText.text = "" + shotCount;      //Kalan şut hakkını yazdırma
            if (UIManager.Mod == "Atıs Hakkı")
            {
                if (shotCount <= 0)                                     //eğer hakkımız kalmadıysa level fail
                {
                    LevelFailed();                                         
                }
            }
        }
    }

  
    public void LevelFailed()
    {
        if (GameManager.singleton.gameStatus == GameStatus.Playing) //Oynadığını kontrol et
        {
            GameManager.singleton.gameStatus = GameStatus.Failed;   //Statüyü Düzelt
            UIManager.instance.textFinal.text = "Tekrar Dene";
            UIManager.instance.textTotalPoint.text = "0 Puan";
            UIManager.instance.GameResult();                        //Bitiş Yazılarını yazdır ve ekranı göster
        }
    }

 
    public void LevelComplete()
    {
       

        if (GameManager.singleton.gameStatus == GameStatus.Playing) //Oynadığını Kontrol et
        {    
            if (GameManager.singleton.currentLevelIndex < levelDatas.Length-1)    
            {
                Debug.Log(levelDatas.Length+" "+ GameManager.singleton.currentLevelIndex);

                GameManager.singleton.currentLevelIndex++;  //üst levele geç
            }
            else
            {
              
                GameManager.singleton.currentLevelIndex = 0;
            }
            GameManager.singleton.gameStatus = GameStatus.Complete; //Oyun statüsünü güncelle
            UIManager.instance.textFinal.text = "Bölüm Tamamlandı";

            //modlara ve zorluğa göre puanlama yap

            if (UIManager.Mod == "Süreye Karsı")
            {                
                 if (UIManager.Dif == "Kolay")
                    { totalpoint = ((int)timeCount) * 100 * 1; }
                else if (UIManager.Dif == "Orta")
                { totalpoint = ((int)timeCount) * 100 * 3; }
                else if (UIManager.Dif == "Zor")
                { totalpoint = ((int)timeCount) * 100 * 5; }
            }
            if (UIManager.Mod == "Atıs Hakkı")
            {
                if (UIManager.Dif == "Kolay")
                { totalpoint = ((int)shotCount) * 1000 * 1; }
                else if (UIManager.Dif == "Orta")
                { totalpoint = ((int)shotCount) * 1000 * 3; }
                else if (UIManager.Dif == "Zor")
                { totalpoint = ((int)shotCount) * 1000 * 5; }
            }
            UIManager.instance.textTotalPoint.text =totalpoint +" Puan";
            UIManager.instance.GameResult();                    //Puanı Yazdır ve Bitiş Ekranını Göster
        }
    }
}
