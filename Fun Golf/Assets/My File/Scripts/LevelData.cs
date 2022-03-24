using UnityEngine;


[System.Serializable]
public class LevelData
{
    public int shotCountEasy;
    public int shotCountMedium;
    public int shotCountHard;
    public int timeCountEasy;
    public int timeCountMedium;
    public int timeCountHard; //Kolay Zor Orta Vuruş Ve Süre Sınırları
    public GameObject levelPrefab;  //referans
}
