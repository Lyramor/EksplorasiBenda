using UnityEngine;

public class KumpulanSuara : MonoBehaviour
{
    public static KumpulanSuara instance;

    public AudioClip[] clip;

    public AudioSource source_sfx;

    public AudioSource source_bgm;

  void Awake()
  {
    if(instance == null){
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }else{
        Destroy(gameObject);
    }
  } 

  public void Panggil_sfx(int id){
    source_sfx.PlayOneShot(clip[id]);
  }
}
