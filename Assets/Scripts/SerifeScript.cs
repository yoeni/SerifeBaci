using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SerifeScript : MonoBehaviour
{
    public GameObject MainButton;
    public int Km=1;
    public float SerifeHp = 1;
    public float BoyHp = 1;
    public Image BoyHpImage;
    public Image SerifeHpImage;
    public float deneme = 1.5f;
    Animator animator,boyAnimator;
    public float Speed = 0.03f;
    public GameObject kagni;
    public GameObject boy;
    public ParticleSystem Snowing;
    public bool Follower = false;
    private bool FollowerState = false;
    public GameObject paper,partycle;
    public sinifim.Mode Zorluk=sinifim.Mode.Kolay;
    public float KarYogunluk = 100;
    public AudioSource boySource, SerifeSource;
    private void Awake()
    {
        switch (PlayerPrefs.GetInt("mode"))
        {
            case 0: Zorluk = sinifim.Mode.Kolay; break;
            case 1: Zorluk = sinifim.Mode.Orta; break;
            case 2: Zorluk = sinifim.Mode.Zor; break;
            default: Zorluk = sinifim.Mode.Kolay; break;
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        boyAnimator = boy.GetComponent<Animator>();
    }
    bool Game = true, boyWarm = false, hardRangeState=false;
    // Update is called once per frame
    void Update()
    {
        if (Game)
        {
            partycle.transform.position = Vector3.Lerp(partycle.transform.position, new Vector3(transform.position.x, transform.position.y + 9f, transform.position.z), 0.5f * Time.deltaTime);
            PlayGame(Zorluk);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (FollowerState)
                {
                    Follower = true;
                    PaperClose();
                }
                else
                {
                    if (Follower)
                    {
                        Follower = false;
                        FollowerState = false;
                        kagni.GetComponent<Animator>().SetFloat("Speed", 0);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (FollowerState)
                {
                    boyWarm = WarmBaby(boyWarm);
                }
            }
            if (Follower)
            {
                kagni.transform.position = Vector3.Lerp(kagni.transform.position, new Vector3(transform.position.x - 5f, transform.position.y + 0.25f, transform.position.z), 5f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Snowing_(0, -5, 1);
                transform.position = new Vector3(transform.position.x + Speed / 3, transform.position.y);
                transform.rotation = new Quaternion(0, 0, 0, 0);
                if (Follower)
                {
                    kagni.GetComponent<Animator>().SetFloat("Speed", 1);
                }
                if (!SerifeSource.isPlaying)
                {
                    SerifeSource.Play();
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Snowing_(0, 5, 1);
                transform.position = new Vector3(transform.position.x - Speed / 3, transform.position.y);
                transform.rotation = new Quaternion(0, 180, 0, 0);
                if (Follower)
                {
                    kagni.GetComponent<Animator>().SetFloat("Speed", 1);
                }
                if (!SerifeSource.isPlaying)
                {
                    SerifeSource.Play();
                }
            }
            else
            {
                Snowing_(0, 0, 0);
                kagni.GetComponent<Animator>().SetFloat("Speed", 0);
                SerifeSource.Stop();
            }
        }
    }
    bool WarmBaby(bool state)
    {
        if (!state)
        {
            boy.transform.localPosition = new Vector3(0.85F, 0F, 0F);// Vector3.Lerp(boy.transform.localPosition, new Vector3(0.85F, 0F,0F), 0.1F * Time.deltaTime);
            state = true;
        }
        else
        {
            boy.transform.localPosition = new Vector3(0.00719201F, 0.1133533F, 0F);// Vector3.Lerp(new Vector3(0.85F, 0F, 0F), new Vector3(0.00719201F, 0.1133533F, 0F), 0.1F * Time.deltaTime);
            state = false;
        }
        return state;
    }
    float hardRange = 0;
    void PlayGame(sinifim.Mode Zorluk)
    {
        float karYogunlukPower = 1;
        float lowTo = Random.Range(0,0.00005F);
        if (Zorluk==sinifim.Mode.Kolay)
        {
            KarYogunluk = 100;
            karYogunlukPower = 2;
        }
        else if (Zorluk==sinifim.Mode.Orta)
        {
            lowTo += 0.00005F;
            KarYogunluk = 400;
            karYogunlukPower = 3;
        }
        else
        {
            lowTo += 0.0001F;
            KarYogunluk = 600;
            karYogunlukPower = 3.5F;
        }

        if (!hardRangeState&&(Km==5||Km==10||Km==15))
        {
            hardRange = Random.Range(Km , Km + 3);
            hardRangeState = true;
        }
        if (Km == hardRange)
        {
            KarYogunluk *= karYogunlukPower;
        }
        if (Km>hardRange)
        {
            hardRangeState = false;
        }

        if (boyWarm)
        {
            BoyHp += 0.0005F;
        }

        if (BoyHp <= 0)
        {
            EndGame();
        }
        else
        {
            BoyHp -= lowTo;
        }
        if (SerifeHp <= 0)
        {
            EndGame(true);
        }
        else
        {
            SerifeHp -= (lowTo / 2);
        }
        syncHpToUi();
    }
    void EndGame(bool notDest=false)
    {
        if (!notDest)
        {
            PaperInfo("Oyun Bitti. Hedefe ulaşamadan bebek öldü!");
        }
        else
        {
            if (Km<=25)
            {
                
                PaperInfo("Oyun Bitti. Hedefe ulaşamadan bebek öldü!");
            }
            else
            {
                PaperInfo("Hikaye Tamamlandı! Teslimat noktasına çoktan vardın.");
            }
        }
        boyWarm = WarmBaby(true);
        Game = false;
        var main = Snowing.main;
        main.startColor = Color.red;
        MainButton.SetActive(true);
        kagni.GetComponent<Animator>().SetFloat("Speed", 0);
    }
    void syncHpToUi()
    {
        SerifeHp = SerifeHp >= 1 ? 1 : SerifeHp;
        BoyHp = BoyHp >= 1 ? 1 : BoyHp;
        if (BoyHp<=0.2)
        {
            if (!boySource.isPlaying)
            {
                boySource.Play();
            }
        }
        else
        {
            boySource.Stop();
        }
        animator.SetFloat("hp", SerifeHp);
        boyAnimator.SetFloat("hp",BoyHp);
        SerifeHpImage.fillAmount = SerifeHp;
        BoyHpImage.fillAmount = BoyHp;
    }
    void Snowing_(float time,float value,int speed)
    {
        var fo = Snowing.forceOverLifetime;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(time, value);
        fo.x = new ParticleSystem.MinMaxCurve(0,value );

        var em = Snowing.emission;
        em.rateOverTime = KarYogunluk;
        Debug.Log(KarYogunluk);
        animator.SetFloat("Speed", speed);
    }
    void PaperInfo(string message)
    {
        if (paper.activeSelf)
        {
            PaperClose();
        }
        paper.SetActive(true);
        paper.GetComponent<Animator>().SetBool("State", true);
        paper.transform.GetChild(0).gameObject.GetComponent<Text>().text = message;
    }
    void PaperClose()
    {
        paper.GetComponent<Animator>().SetBool("State", false);
        paper.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kagni")
        {
            if (!Follower)
            {
                PaperInfo("Kağnıyı kontrol etmek için F, Bebeği ısıtmak için B tuşunu kullan.");
            }
            FollowerState = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kagni")
        {
            if (!Follower)
            {
                PaperClose();
            }
            boyWarm = WarmBaby(true);
            FollowerState = false;
        }
    }
}