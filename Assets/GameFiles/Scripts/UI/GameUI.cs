using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [Header("Ui Properties")]
    [SerializeField] private Image levelSlider;
    [SerializeField] private Image currentLevelImg;
    [SerializeField] private Image nextLevelImg;

    //Helpers
    private Material playerMat;



    void Awake()
    {
        playerMat = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<MeshRenderer>().material;

        levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color + Color.gray;
        levelSlider.color = playerMat.color;

        currentLevelImg.color = playerMat.color;
        nextLevelImg.color = playerMat.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelSliderFill(float fillAmt)
    {
        levelSlider.fillAmount = fillAmt;
    }
}
