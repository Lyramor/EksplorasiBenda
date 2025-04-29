using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Obj_Drag : MonoBehaviour
{
    public Vector2 SavePosisi;
    public bool IsDiAtasObject;

    Transform SaveObject;

    public int ID;

    public Text Teks;

    public UnityEvent OnDragBenar;

    void Start()
    {
        SavePosisi = transform.position;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnMouseUp()
    {
        if (IsDiAtasObject)
        {
            int ID_TempatDrop = SaveObject.GetComponent<Tempat_Drop>().ID; // Pastikan script Tempat_Drop ada

            if (ID == ID_TempatDrop)
            {
                // --- Jawaban Benar ---
                Vector3 skalaAsli = transform.localScale; // Simpan skala asli
                transform.SetParent(SaveObject);
                transform.localPosition = Vector3.zero;

                // Buat objek lebih besar dari aslinya (opsional, sesuai kode asli)
                transform.localScale = skalaAsli * 1.5f;

                SaveObject.GetComponent<SpriteRenderer>().enabled = false;

                OnDragBenar.Invoke();

                // Tingkatkan data saat ini dan skor
                GameSystem.instance.DataSaatIni++;
                Data.DataScore += 100;

                // Panggil suara benar
                KumpulanSuara.instance.Panggil_sfx(1);

                Data.DataWaktu += 10;
                Debug.Log("Jawaban Benar! Waktu ditambah 10 detik. Waktu sekarang: " + Data.DataWaktu); // Optional: untuk debugging
            }
            else
            {
                // --- Jawaban Salah ---
                transform.position = SavePosisi;

                // Kurangi darah
                Data.DataDarah--;

                // Panggil suara salah
                KumpulanSuara.instance.Panggil_sfx(2);

                Data.DataWaktu -= 10;
                Debug.Log("Jawaban Salah! Waktu dikurangi 10 detik. Waktu sekarang: " + Data.DataWaktu); // Optional: untuk debugging

                // Pastikan waktu tidak negatif secara visual (meskipun logic game over di GameSystem sudah menangani <= 0)
                // if (Data.DataWaktu < 0)
                // {
                //     Data.DataWaktu = 0;
                // }
            }
        }
        else
        {
            // Kembali ke posisi awal jika tidak di atas area drop
            transform.position = SavePosisi;
        }
    }

    private void OnMouseDown()
    {
        KumpulanSuara.instance.Panggil_sfx(0);
    }

    private void OnMouseDrag()
    {
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Pos;
    }

    private void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiAtasObject = true;
            SaveObject = trig.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiAtasObject = false;
            SaveObject = null; // Sebaiknya set ke null jika keluar dari area drop
        }
    }
}