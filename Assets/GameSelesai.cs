using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSelesai : MonoBehaviour
{
    [SerializeField] private Text Text_Score;
    [SerializeField] private Text Text_TotalScore;

    void Awake()
    {
        // Hanya cari UI elements jika berada di scene GameSelesai
        if (SceneManager.GetActiveScene().name == "GameSelesai")
        {
            Debug.Log("[GameSelesai] Awake - Scene aktif adalah GameSelesai, mencari UI elements");
            
            // Fokus pada pencarian Text_Score
            if (Text_Score == null)
            {
                Text_Score = GameObject.Find("Text_Score")?.GetComponent<Text>();
                Debug.Log($"[GameSelesai] Awake - Text_Score Found via Find: {Text_Score != null}");
                
                // Jika masih null, coba metode lain
                if (Text_Score == null)
                {
                    var canvas = FindObjectOfType<Canvas>();
                    if (canvas != null)
                    {
                        Text_Score = canvas.transform.Find("Text_Score")?.GetComponent<Text>();
                        Debug.Log($"[GameSelesai] Awake - Text_Score Found via Canvas: {Text_Score != null}");
                    }
                }
            }

            // Pencarian biasa untuk Text_TotalScore
            if (Text_TotalScore == null)
            {
                Text_TotalScore = GameObject.Find("Text_TotalScore")?.GetComponent<Text>();
                Debug.Log($"[GameSelesai] Awake - Text_TotalScore Found: {Text_TotalScore != null}");
            }
            
            Debug.Log($"[GameSelesai] Awake - Data.DataScore: {Data.DataScore}");
        }
        else
        {
            // Jika tidak di scene GameSelesai, logging dan berhenti mencari UI elements
            Debug.Log($"[GameSelesai] Awake - Scene saat ini adalah {SceneManager.GetActiveScene().name}, bukan GameSelesai. Skip pencarian UI elements.");
            // Menonaktifkan script ini jika bukan di scene yang tepat
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        // Keluar jika bukan scene GameSelesai
        if (SceneManager.GetActiveScene().name != "GameSelesai")
        {
            Debug.Log($"[GameSelesai] Start - Scene saat ini bukan GameSelesai. Keluar dari method Start.");
            return;
        }
        
        // Simpan skor terlebih dahulu
        int savedHighScore = PlayerPrefs.GetInt("score", 0);
        
        Debug.Log($"[GameSelesai] Current Score: {Data.DataScore}");
        Debug.Log($"[GameSelesai] Saved High Score: {savedHighScore}");

        if (Data.DataScore > savedHighScore)
        {
            PlayerPrefs.SetInt("score", Data.DataScore);
            PlayerPrefs.Save();
        }

        // Animasi
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("in");
        }

        // Update UI text komponen dengan menunggu satu frame
        // Ini memastikan semua objek sudah terinisialisasi dengan benar
        Invoke("UpdateScoreText", 0.1f);
    }

    void UpdateScoreText()
    {
        // Keluar jika bukan scene GameSelesai
        if (SceneManager.GetActiveScene().name != "GameSelesai")
        {
            Debug.Log($"[GameSelesai] UpdateScoreText - Scene saat ini bukan GameSelesai. Keluar dari method.");
            return;
        }
        
        Debug.Log("[GameSelesai] UpdateScoreText called");

        // Final attempt untuk Text_Score jika masih null
        if (Text_Score == null)
        {
            // Coba metode alternatif - cari semua Text komponen
            Text[] allTexts = FindObjectsOfType<Text>();
            foreach (Text text in allTexts)
            {
                Debug.Log($"Found Text: {text.name}");
                if (text.name.Contains("Text_Score"))
                {
                    Text_Score = text;
                    Debug.Log($"[GameSelesai] Found Text_Score via FindObjectsOfType: {text.name}");
                    break;
                }
            }
        }

        // Set current score text - tambahkan pengecekan aktif/enabled
        if (Text_Score != null && Text_Score.gameObject.activeInHierarchy)
        {
            // Pastikan nilai yang ditampilkan adalah nilai statis terakhir
            Text_Score.text = Data.DataScore.ToString();
            Debug.Log($"[GameSelesai] Text_Score updated to: {Text_Score.text}");
        }
        else
        {
            Debug.LogError("[GameSelesai] Text_Score is NULL or inactive! Make sure it exists and is active in the scene.");
        }

        // Set high score text
        if (Text_TotalScore != null && Text_TotalScore.gameObject.activeInHierarchy)
        {
            int highScore = PlayerPrefs.GetInt("score", 0);
            Text_TotalScore.text = highScore.ToString();
            Debug.Log($"[GameSelesai] Text_TotalScore updated to: {Text_TotalScore.text}");
        }
        else
        {
            Debug.LogError("[GameSelesai] Text_TotalScore is NULL or inactive! Make sure it exists and is active in the scene.");
        }
    }
}