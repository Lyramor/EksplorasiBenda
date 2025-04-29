using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Data{
    public static int DataLevel,DataScore,DataWaktu,DataDarah;
}

public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;

    int MaxLevel = 5;

    [Header("Data Permainan")]
    public bool GameAktif;
    public bool GameSelesai;
    public bool SistemAcak;
    public int Target, DataSaatIni;
    


    [Header("Komponen UI")]
    public Text Teks_Level;
    public Text Teks_Waktu,Teks_Score;
    public RectTransform Ui_Darah;


    [Header("Obj GUI")]
    public GameObject Gui_Pause;
    public GameObject Gui_Transisi;


    [System.Serializable]
    public class DataGame
    {
        public string Nama;
        public Sprite Gambar;
        public string Sifat;
    }


    [Header("Settingan Standar")]
    public DataGame[] DataPermainan;

    [Space]
    [Space]
    [Space]
    [Space]
    [Space]

    public Obj_TempatDrop[] Drop_Tempat;
    public Obj_Drag[] Drag_Obj;


    public void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    float s;

    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Space))
        AcakSoal();

    if (GameAktif)  
    {
        if(Data.DataWaktu > 0)
        {
            s += Time.deltaTime;
            if(s >= 1)
            {
                Data.DataWaktu--;
                s = 0;
            }
        }

        if(Data.DataWaktu <= 0){
            GameAktif = false;
            GameSelesai = true;

            //game kalah
            //GetComponent<Animator>().Play("end");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameSelesai");
        }

        if(Data.DataDarah <= 0){
            GameAktif = true;
            GameSelesai = false;

            //fungsi kalah
            //GetComponent<Animator>().Play("end");
            KumpulanSuara.instance.Panggil_sfx(4);
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameSelesai");
        }
        
        if(DataSaatIni >= Target){
            Debug.Log($"Game Selesai! DataSaatIni: {DataSaatIni}, Target: {Target}, Level: {Data.DataLevel}, Score: {Data.DataScore}");
            
            GameSelesai = true;
            GameAktif = false;
            
            // Pastikan nilai skor tidak hilang saat transisi
            PlayerPrefs.SetInt("currentScore", Data.DataScore);
            PlayerPrefs.Save();
            
            if(Data.DataLevel < (MaxLevel - 1)){
                Data.DataLevel++;
                Debug.Log($"ATTEMPTING TO LOAD: Game{Data.DataLevel}");
                UnityEngine.SceneManagement.SceneManager.LoadScene($"Game{Data.DataLevel}");

                KumpulanSuara.instance.Panggil_sfx(3);
            }else{
                Debug.Log($"Loading GameSelesai with Score: {Data.DataScore}");
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameSelesai");
                KumpulanSuara.instance.Panggil_sfx(5);
            }
        }
    }

    SetInfoUI();
    }

    public void OnCorrectDrop()
    {
        // Tambahkan hanya jika jawaban benar
        GameSystem.instance.DataSaatIni++;
        
        // Tambahkan debug log untuk memantau
        Debug.Log($"Correct Drop: DataSaatIni = {GameSystem.instance.DataSaatIni}, Target = {GameSystem.instance.Target}");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     ResetData();
    //     Target = Drop_Tempat.Length;
    //     if(SistemAcak)
    //         AcakSoal();

    //     GameAktif = true;
    // }
    void Start()
    {
        GameAktif = true;
        GameSelesai = false;
        ResetData();
        Target = Drop_Tempat.Length;
        AcakSoal();

        DataSaatIni = 0;
        GameAktif = true;
    }

    void ResetData(){
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game0"){
            Data.DataWaktu = 60 * 3;
            Data.DataScore = 0;
            Data.DataDarah = 5;
            Data.DataLevel = 0;
        }
    }

    [HideInInspector] public List<int> _AcakSoal = new List<int>();
    [HideInInspector] public List<int> _AcakPos = new List<int>();
    int rand;
    int rand2;

    public void AcakSoal()
    {
        // Cek null untuk mencegah NullReferenceException
        if (Drag_Obj == null || Drop_Tempat == null || DataPermainan == null)
        {
            Debug.LogError("Reference tidak boleh null: Drag_Obj, Drop_Tempat, atau DataPermainan");
            return;
        }

        _AcakSoal.Clear();
        _AcakPos.Clear();

        // Inisialisasi _AcakSoal dengan ukuran Drag_Obj.Length
        _AcakSoal = new List<int>(new int[Drag_Obj.Length]);

        for (int i = 0; i < _AcakSoal.Count; i++)
        {
            rand = Random.Range(1, DataPermainan.Length);
            while (_AcakSoal.Contains(rand))
                rand = Random.Range(1, DataPermainan.Length);

            _AcakSoal[i] = rand;

            // Cek null sebelum mengakses properti
            if (Drag_Obj[i] != null)
            {
                Drag_Obj[i].ID = rand - 1;

                // Pastikan Teks tidak null sebelum mengakses text
                if (Drag_Obj[i].Teks != null)
                {
                    // Tampilkan Sifat bukan Nama
                    Drag_Obj[i].Teks.text = DataPermainan[rand - 1].Sifat;
                }
                else
                {
                    Debug.LogError("Drag_Obj[" + i + "].Teks is null!");
                }
            }
        }

        // Kode untuk _AcakPos tetap sama
        _AcakPos = new List<int>(new int[Drop_Tempat.Length]);

        for (int i = 0; i < _AcakPos.Count; i++)
        {
            rand2 = Random.Range(1, _AcakSoal.Count + 1);
            while (_AcakPos.Contains(rand2))
                rand2 = Random.Range(1, _AcakSoal.Count + 1);

            _AcakPos[i] = rand2;

            Drop_Tempat[i].Drop.ID = _AcakSoal[rand2 - 1] - 1;
            Drop_Tempat[i].Gambar.sprite = DataPermainan[Drop_Tempat[i].Drop.ID].Gambar;
        }
    }

    
    public void SetInfoUI()
    {
        Teks_Level.text = (Data.DataLevel + 1).ToString();

        int Menit = Mathf.FloorToInt(Data.DataWaktu / 60); // 01
        int Detik = Mathf.FloorToInt(Data.DataWaktu % 60); // 30
        Teks_Waktu.text = Menit.ToString("00") + ":" + Detik.ToString("00");

        Teks_Score.text = Data.DataScore.ToString();

        Ui_Darah.sizeDelta = new Vector2(57f * Data.DataDarah, 51f);
    }

    public void Btn_Pause(bool pause){
        if(pause){
            GameAktif = false;
            Gui_Pause.SetActive(true);
        }else{
            GameAktif = true;
            Gui_Pause.SetActive(false);
        }
    }


}
