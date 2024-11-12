using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Refrences")]
    private Rigidbody rb;

    private float currentTime;

    private bool smash, invincible;

    private int currentBrokenStacks, totalStacks;

    public GameObject invincibleObj;
    public Image invincibleFill;

    [Header("Effects")]
    public ParticleSystem fireEffect;
    public ParticleSystem winEffect;
    public GameObject splashEffect;
    public float splashYOffset = .22f;

    public enum PlayerState
    {
        Preperation,
        Playing,
        Dead,
        Finish
    }

    [HideInInspector]
    public PlayerState state = PlayerState.Preperation;

    public AudioClip bounceClip, deadClip, winClip, destroyClip, iDestroyClip;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
    }

    private void Start()
    {
        totalStacks = FindObjectsOfType<StackController>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                smash = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                smash = false;
            }

            if (invincible)
            {
                currentTime -= Time.deltaTime * .35f;
                if (!fireEffect.gameObject.activeInHierarchy)
                {
                    fireEffect.gameObject.SetActive(true);
                }
            }
            else
            {

                if (fireEffect.gameObject.activeInHierarchy)
                    fireEffect.gameObject.SetActive(false);
                if (smash)
                    currentTime += Time.deltaTime * .8f;
                else
                    currentTime -= Time.deltaTime * .5f;

            }

            if (currentTime >= .15f || invincibleFill.color == Color.red)
                invincibleObj.SetActive(true);
            else
                invincibleObj.SetActive(false);

            if (currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
                invincibleFill.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
                invincibleFill.color = Color.white;
            }

            if (invincibleObj.activeInHierarchy)
                invincibleFill.fillAmount = currentTime / 1;
        }

        if (state == PlayerState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
                GameObject.FindAnyObjectByType<LevelManager>().NextLevel();
        }
    }

    private void FixedUpdate()
    {
        if ((state == PlayerState.Playing))
        {
            if (Input.GetMouseButton(0))
            {
                smash = true;
                rb.linearVelocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }

        if (rb.linearVelocity.y > 5)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 5, rb.linearVelocity.z);
        }
    }
    #region Collison Check
    private void OnCollisionEnter(Collision collision)
    {
        if (!smash)
        {
            rb.linearVelocity = new Vector3(0, 50 * Time.fixedDeltaTime * 5, 0);

            if (!collision.gameObject.CompareTag("Finish"))
            {
                GameObject splash = Instantiate(splashEffect);
                splash.gameObject.transform.SetParent(collision.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);

                float randomScale = Random.Range(.18f, .25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y - splashYOffset, transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            }

            SoundManager.instance.PlaySoundFX(bounceClip, .6f);
        }
        else
        {

            if (invincible)
            {
                if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("plane"))
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("enemy"))
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

                if (collision.gameObject.CompareTag("plane"))
                {
                    rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    state = PlayerState.Dead;
                    //ScoreManager.instance.ResetScore();
                    SoundManager.instance.PlaySoundFX(deadClip, .5f);
                }
            }

        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks / (float)totalStacks);

        if (collision.gameObject.CompareTag("Finish") && state == PlayerState.Playing)
        {
            state = PlayerState.Finish;
            SoundManager.instance.PlaySoundFX(winClip, .7f);
            GameObject win = Instantiate(winEffect.gameObject);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!smash || collision.gameObject.CompareTag("Finish"))
        {
            rb.linearVelocity = new Vector3(0, 50 * Time.fixedDeltaTime * 5, 0);
        }
    }
    #endregion

    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;
        if (!invincible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(destroyClip, .5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(iDestroyClip, .5f);

        }
    }
}
