using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Control : MonoBehaviour
{
    public bool IsTransisi, IsTidakPerlu;
    string SaveNamaScene;

    public void Awake()
    {
        if(IsTransisi && IsTidakPerlu){
            gameObject.SetActive(false);
        }
    }
    
    public void Btn_Suara(int id){
        KumpulanSuara.instance.Panggil_sfx(0);
    }

    public void Btn_Pindah(string nama)
    {
        // Ganti pemanggilan animasi dengan langsung pindah scene
        SaveNamaScene = nama;
        // GetComponent<Animator>().Play("end"); // Comment ini sementara
        SceneManager.LoadScene(nama); // Langsung pindah
    }

    // Fungsi ini dipanggil oleh animation event
    public void pindah(){
        Debug.Log("Pindah ke scene: " + SaveNamaScene);
        SceneManager.LoadScene(SaveNamaScene);
    }

    // Jika fungsi di atas tidak bekerja, coba tambahkan ini
    // Nama fungsi harus persis sama dengan yang dipanggil oleh animation event
    public void OnAnimationEnd(){
        Debug.Log("Animation End Event dipanggil");
        if (!string.IsNullOrEmpty(SaveNamaScene)) {
            SceneManager.LoadScene(SaveNamaScene);
        }
    }

    public void Btn_KeluarGame(){
        Application.Quit();
    }
}