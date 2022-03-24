using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [HideInInspector]
    public int currentLevelIndex; //geçerli level
    [HideInInspector]
    public GameStatus gameStatus = GameStatus.None; //oyun statüsü tanımı

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject); //destroy edilmesini engelle
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public enum GameStatus //Statüler
{
    None,
    Playing,
    Failed,
    Complete
}