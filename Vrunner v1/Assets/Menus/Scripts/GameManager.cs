using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { set; get; }

    public float DNA_SCORE_AMOUT = 0.04f;
    private bool death = false;

    public float score;
    public int randomPower;

    [Header("=======Death Menu=======")]
    public Text distanceText;
    public Text fragmentsDNAText, stashesDNAText, mutacaoLvlText, scoreText; // texto exibido
    [HideInInspector] public float distance = 0, fragDNA = 0, stashesDNA = 0, mutLvl = 1; // contadores death menu


    [Space(10)]
    [Header("=======Pause Menu=======")]
    [Space(5)]
    public Image mutacaobar;
    public Image febrebar; // barra grafica
    [HideInInspector] public float mutacao, febre; // valor logico da barra

    #region Pause Menu
    public Text distancePauseText, mutacaoLvlPauseText, febreText;
    private float lastScore, lastTime;
    public static bool pauseIsActive;
    #endregion

    [Space(20)]
    [Header("=======Hud Menu=======")]
    [Space(5)]
    #region Hud
    public Image mutacaobarG;
    public Image febrebarG;

    public Text distanceTxtG, mutacaoTextG, febreTextG;
    private bool mut100;
    private bool febre100;
    #endregion


    [Space(20)]
    public Text count;
    public static float timeSaindoDoPause;
    [HideInInspector] public bool powerUpActive = false; // variavel que ativa o powerup
    [HideInInspector] public bool active = false;

    public GameObject pauseMenu;
    public GameObject hud;
    public GameObject deathMenu;
    public GameObject powerupMenu;

    [Space(20)]
    public GameObject guia;
    public GameObject player;
    public GameObject pcamera;

    float incrementoDoTimeScale;

    [Space(20)]
    public float[] pontuacaoNecessaria = new float[0];
    public GameObject notificacao = null;


    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;


    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Instance = this;
        Time.timeScale = 1f;
        pauseIsActive = false;
        timeSaindoDoPause = -1f;

    }

    void Start()
    {
        pauseMenu.SetActive(false);
        powerupMenu.SetActive(false);
    }

    void Update()
    {
        SaidinhaDoPause();

        if (pauseIsActive == true || powerupMenu.activeSelf)
        {
            paused.TransitionTo(0);
        }
        else
        {
            unpaused.TransitionTo(0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            GetFebreBar();
        #region Update score
        UpdateScores();

        distance = death ? lastScore : distance += Time.deltaTime * 3;

        distancePauseText.text = "Distância: " + lastScore.ToString("0.00");

        if (lastScore != distance)
        {
            lastScore = distance;
        }// get lest score

        //animação da barra
        if (mutacaobarG.fillAmount < 1)
            ColorMutBar.Instance.Colorbar(100);
        else
            ColorMutBar.Instance.Colorbar(0.8f);

        if (febrebarG.fillAmount < 0.8f)
            ColorFebBar.Instance.Colorbar(100);
        else
            ColorFebBar.Instance.Colorbar(0.8f);

        #endregion

        #region Game = Hud
        distanceText.text = distanceTxtG.text = distancePauseText.text; mutacaobarG.fillAmount = mutacaobar.fillAmount; febrebarG.fillAmount = febrebar.fillAmount;
        mutacaoLvlText.text = mutacaoTextG.text = mutacaoLvlPauseText.text; febreTextG.text = febreText.text;
        #endregion

        #region Limit of lvls Bar

        if (febre >= 100)
        {
            febre100 = false;
            febre = 0;
            GameModeBH.modo = 4;
            GameObject.FindGameObjectWithTag("CameraShakeShake").GetComponent<Transform>().transform.position = player.transform.position;
            AudioPowerUpEffects.Instance.ads[6].Play();
        }
        else
        {
            febre100 = true;
        }

        febrebar.fillAmount = ((75 + febre * 0.75f) / 150);

        mut100 = mutacao >= 100 ? false : true;
        mutacaobar.fillAmount = ((75 + mutacao * 0.75f) / 150);

        #endregion

        if (powerUpActive && !mut100)
        {
            powerUpActive = false;
            mutLvl++;
            mutacaoLvlPauseText.text = "MUT: \nLVL " + mutLvl.ToString();
        } // upa de nivel quando usa power up

        DNA_SCORE_AMOUT = active ? 0.08f : 0.04f; // double coins


        if (CameraMotor.deadzone && pcamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity <= 0.5f)
        {
            pcamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity += 0.036f * Time.deltaTime * 30;
        }

        score = (lastScore + fragDNA * 4f) * mutLvl + (stashesDNA * 500);
        for (int i = 0; i < pontuacaoNecessaria.Length; i++)
        {
            if (score >= pontuacaoNecessaria[i] && PlayerPrefs.GetInt("Skin" + i) == 2)
            {
                StartCoroutine(WaitNotification(5));
                PlayerPrefs.SetInt("Skin" + i, 3);
            }
        }

        // Game Velocity
        Time_Scale();
    }
    void Time_Scale()
    {
        if (pauseIsActive || timeSaindoDoPause > 0)
            Time.timeScale = 0.00001f;
        else if (powerupMenu.activeSelf)
            Time.timeScale = 0.2f;
        else if (GameModeBH.emTransicao)
            Time.timeScale = lastTime;
        else if (GameModeBH.bateuNaFebre)
            Time.timeScale = 1f;
        else if (GameModeBH.skipCounter > 0)
        {
            if (GameModeBH.modo == 2)
                Time.timeScale = 12f; // 12
            else if (GameModeBH.modo == 1)
                Time.timeScale = lastTime;
            else
                Time.timeScale = 3f;
        }
        else if (LevelManager.Instace.somenteTransicoes)
        {
            if (GameModeBH.modo == 3)
            {
                if (GameModeBH.bateuNaFebre)
                    Time.timeScale = 3f; // 5
                else if (GameModeBH.contadorDeObstaculosFebre <= 0)
                    Time.timeScale = 0.5f;
                else if (!PlayerBehaviour.bulletTime)
                    Time.timeScale = lastTime * 2f; // *3
                else
                    Time.timeScale = lastTime * 0.5f;
            }
            else if (GameModeBH.modo == 2)
                Time.timeScale = lastTime * 2f;
        }
        else if (GameModeBH.modo == 1)
        {
            Time.timeScale = 1 + incrementoDoTimeScale;
            lastTime = Time.timeScale;
            if (incrementoDoTimeScale <= 1f)
                incrementoDoTimeScale += 0.0001f;
        }
    }


    void UpdateScores()
    {
        stashesDNAText.text = "DNA Estacionários: " + stashesDNA.ToString();
        fragmentsDNAText.text = "Frags. DNA: " + fragDNA.ToString();
        distancePauseText.text = "Distância: " + distance.ToString("0.00");
        febreText.text = "FEBRE: \n" + febre.ToString("0") + "%";
    }

    public void GetDNA()
    {
        if (mut100)
            mutacao += DNA_SCORE_AMOUT * 100;
        fragDNA++;
    }
    public void GetDNAStashes()
    {
        if (mut100)
            mutacao += DNA_SCORE_AMOUT * 500;
        stashesDNA++;
    }
    public void GetFebreBar()
    {
        if (febre100)
        {
            febre += DNA_SCORE_AMOUT * 1000;
            febreText.text = "MUT: \n" + febre.ToString() + "%";
        }
    }
    public void OnPauseButton()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        hud.SetActive(!hud.activeSelf);
        pauseIsActive = !pauseIsActive;

        notificacao.SetActive(false);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Premios()
    {
        PlayerPrefs.SetInt("RotacaoCamera", 1);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMutButtom()
    {
        if (!mut100)
        {
            powerupMenu.SetActive(true);
            mutacao = 0;
            powerUpActive = false;
        }
    }
    public void OnDeath()
    {
        AudioPlayer.Instance.ads[1].enabled = true; // reproduz o som
        GameModeBH.playerMesh.transform.localRotation = Quaternion.identity;

        if (MapMotor.Instance.speed > 0)
            MapMotor.Instance.speed -= 0.1f;

        guia.GetComponent<Accelerometer>().enabled = false;
        player.GetComponent<FreeFallBH>().enabled = false;
        player.GetComponent<PlayerBehaviour>().enabled = false;
        player.GetComponent<SphereCollider>().enabled = false;
        player.GetComponentInChildren<Animator>().enabled = false;

        hud.SetActive(false);
        deathMenu.SetActive(true);
        death = true;

        score = (lastScore + fragDNA * 4f) * mutLvl + (stashesDNA * 500);
        scoreText.text =  score.ToString();

        if (score > PlayerPrefs.GetFloat("HighScore", 0))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }

        StartCoroutine(WaitSeconds());
    }

    IEnumerator WaitSeconds() // lerp no tempo depois que morre
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0.05f;
    }
    IEnumerator WaitNotification(float time) // lerp no tempo depois que morre
    {
        notificacao.SetActive(true);
        yield return new WaitForSeconds(time * PowerUps.Instance.deltaTime);
        notificacao.SetActive(false);
    }

    void SaidinhaDoPause()
    {
        if (pauseIsActive)
            timeSaindoDoPause = 3 * Time.timeScale;

        if (!pauseIsActive && timeSaindoDoPause > 0)
        {
            timeSaindoDoPause -= Time.deltaTime;
            int _time = (int)(timeSaindoDoPause / Time.timeScale);
            count.enabled = true;
            count.text = _time.ToString();
        }
        else
            count.enabled = false;
    }
}
