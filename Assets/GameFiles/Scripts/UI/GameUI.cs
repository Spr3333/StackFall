using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [Header("Ui Properties")]
    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject inGameUi;
    [SerializeField] private GameObject finishUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject allBtn;


    [Header("Sound Ui")]
    [SerializeField] private Button soundBtn;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;


    [Header("InGame")]
    [SerializeField] private Image levelSlider;
    [SerializeField] private Image currentLevelImg;
    [SerializeField] private Image nextLevelImg;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;


    [Header("Finish")]
    public TextMeshProUGUI finishLevelText;


    [Header("Gameover Text")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverBestScoreText;

    private bool btns;
    //Helpers
    private Material playerMat;
    private PlayerController player;


    void Awake()
    {
        playerMat = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color + Color.gray;
        levelSlider.color = playerMat.color;

        currentLevelImg.color = playerMat.color;
        nextLevelImg.color = playerMat.color;

        soundBtn.onClick.AddListener(() => SoundManager.instance.SoundOnOFF());
    }

    private void Start()
    {
        currentLevelText.text = FindObjectOfType<LevelManager>().level.ToString();
        nextLevelText.text = FindObjectOfType<LevelManager>().level + 1 + "";
    }

    // Update is called once per frame
    void Update()
    {
        if (player.state == PlayerController.PlayerState.Preperation)
        {
            if (SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOn)
            {
                soundBtn.GetComponent<Image>().sprite = soundOn;
            }
            else if (!SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOff)
            {
                soundBtn.GetComponent<Image>().sprite = soundOff;
            }
        }

        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && player.state == PlayerController.PlayerState.Preperation)
        {
            player.state = PlayerController.PlayerState.Playing;
            homeUI.SetActive(false);
            inGameUi.SetActive(true);
        }

        if (player.state == PlayerController.PlayerState.Finish)
        {
            homeUI.SetActive(false);
            inGameUi.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level " + FindObjectOfType<LevelManager>().level;
        }

        if (player.state == PlayerController.PlayerState.Dead)
        {
            homeUI.SetActive(false);
            inGameUi.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);

            gameOverScoreText.text = ScoreManager.instance.score.ToString();
            gameOverBestScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();

            if (Input.GetMouseButtonDown(0))
            {
                ScoreManager.instance.ResetScore();
                SceneManager.LoadScene(0);
            }
        }

    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResult = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResult);

        for (int i = 0; i < raycastResult.Count; i++)
        {
            if (raycastResult[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResult.RemoveAt(i);
                i--;
            }
        }

        return raycastResult.Count > 0;
    }

    public void LevelSliderFill(float fillAmt)
    {
        levelSlider.fillAmount = fillAmt;
    }

    public void Settimgs()
    {
        btns = !btns;
        allBtn.SetActive(true);
    }
}
