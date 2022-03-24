using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Image powerBar;        //Uİ Vuruş Gücü
    [SerializeField] public Text shotText;         // Uİ Atış Hakkı
    [SerializeField] public Text timeText;          //Uİ Zaman
    [SerializeField] public Text timeText2;         //Uİ Zaman2
    [SerializeField] public Text timeText3;         //Uİ Zaman3
    [SerializeField] public Text timeText4;         //Uİ Zaman :
    [SerializeField] public Text timeText5;         //Uİ Zaman :
    [SerializeField] public Text textİnfo;       //Uİ Mod ve zorluk bilgisi
    [SerializeField] public  Text textMod;      //Uİ Mod
    [SerializeField] public  Text textDif;      //Uİ Zorluk
    [SerializeField] public Text textFinal;     //Final yazılım
    [SerializeField] public Text textTotal;     //Skor
    [SerializeField] public Text textTotalPoint;   // toplam skoru yazdırma
    [SerializeField] private GameObject mainMenu, gameMenu, gameOverPanel, retryBtn, nextBtn, homeBtn, homeBtnGame, quitBtn;   //Butonlar
    [SerializeField] public  Slider sliderMod, sliderDif; //Sliderlar

   [SerializeField] private GameObject container, lvlBtnPrefab;    //prefablar

    public static string Mod = "Atıs Hakkı";
    public static string Dif = "Kolay";

    [SerializeField] public AudioStart audiostart;


    public Text ShotText { get { return shotText; } }   //get komutu şut sayısı için
    public Text TimeText { get { return timeText; } }   //get komutu zaman için
    public Text ModText { get { return textMod; } }   //get komutu mod texti için
    public Text DifText { get { return textDif; } }   //get komutu zorluk texti için
    public Image PowerBar { get => powerBar; }          //get komutu güç barı için

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

        powerBar.fillAmount = 0;                        //güç barını sıfırla
    }

    void Start()
    {
        

        if (GameManager.singleton.gameStatus == GameStatus.None)    //eğer statü none ise
        {
            CreateLevelButtons();                       //level butonlarını üret
        }     //statü kontrolü
        else if (GameManager.singleton.gameStatus == GameStatus.Failed ||
            GameManager.singleton.gameStatus == GameStatus.Complete)
        {
            mainMenu.SetActive(false);                                      //main menüyü gösterme
            gameMenu.SetActive(true);                                       //oyun menüsünü göster
            LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex);  //level üret
        }
    }

    private void FixedUpdate()
    {
        //Ana menüye dönüş sonrası static değerlerden textleri düzenleme
        if (Mod== "Süreye Karsı") {
            sliderMod.value = 1;
            textMod.text = "Süreye Karsı";
           
        }
       else if (Mod == "Atıs Hakkı")
        {
            sliderMod.value = 0;
            textMod.text ="Atıs Hakkı";
            
        }
        if (Dif == "Kolay")
        {
            sliderDif.value = 0;
            textDif.text = "Kolay";
        }
        else if (Dif == "Orta")
        {
            sliderDif.value = 1;
            textDif.text = "Orta";
        }
        else if (Dif == "Zor")
        {
            sliderDif.value = 2;
            textDif.text = "Zor";
        }
    }

    void CreateLevelButtons()
    {
        //toplam level datası kadar for
        for (int i = 0; i < LevelManager.instance.levelDatas.Length; i++)
        {
            GameObject buttonObj = Instantiate(lvlBtnPrefab, container.transform);   //butonları container içine oluştur
            buttonObj.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1);   //level sayisini buton içine yaz
            Button button = buttonObj.GetComponent<Button>();                           
            button.onClick.AddListener(() => OnClick(button));                          //butona listener ekle
        }
    }

  
    void OnClick(Button btn)
    {
        audiostart.ButtonFun();                                                          //buton sesi çıkar
        mainMenu.SetActive(false);                                                      //main menüyü gizle
        gameMenu.SetActive(true);                                                       //oyun menüsünü göster
        GameManager.singleton.currentLevelIndex = btn.transform.GetSiblingIndex(); ;    //butonun sayısını güncel levele ata
        LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex);      //leveli oluştur
    }

    public void GameResult()
    {
        switch (GameManager.singleton.gameStatus)
        {
            case GameStatus.Complete:                       //bölüm başarıyla biterse
                gameOverPanel.SetActive(true);              //Bitiş panelini göster
                nextBtn.SetActive(true);                    //ileriki bölüm ve ana sayfa butonları aktifleştir
                homeBtn.SetActive(true);
                                                
                audiostart.SuccessFun();

                break;
            case GameStatus.Failed:                         //bölüm başarısız olursa
                gameOverPanel.SetActive(true);              //bölüm bitiş menüsünü aç
                retryBtn.SetActive(true);       //yeniden dene butonunu aç
                homeBtn.SetActive(true);  //anasayfa butonunu aç
               
                audiostart.FailedFun();

                break;
        }
    }

   
    public void HomeBtn()
    {       //anasayfaya Dönüş
        audiostart.ButtonFun();
        GameManager.singleton.gameStatus = GameStatus.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }

   
    public void NextRetryBtn()
    {
        //yeniden başlat
        audiostart.ButtonFun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitBtn()
    {
        //çıkış butonu
        audiostart.ButtonFun();
        Application.Quit();
    }

    public void ModChange()
    {
        //mod değiştirince menü yazıları ve mod değiştirilmesi
        if (sliderMod.value == 0)
        {
            textMod.text = "Atıs Hakkı";
            Mod = "Atıs Hakkı";
        }
        else if (sliderMod.value == 1)
        {
            textMod.text = "Süreye Karsı";
            Mod = "Süreye Karsı";
        }
        
    }
    public void DifChange()
    {
        //zorluk değiştirince menü yazıları ve zorluk değiştirilmesi
        if (sliderDif.value == 0)
        {
            textDif.text = "Kolay";
            Dif = "Kolay";
        }
        else if (sliderDif.value == 1)
        {
            textDif.text = "Orta";
            Dif = "Orta";
        }
        else if (sliderDif.value == 2)
        {
            textDif.text = "Zor";
            Dif = "Zor";
        }
    }
}
