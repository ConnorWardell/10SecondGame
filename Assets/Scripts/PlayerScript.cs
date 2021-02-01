using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public float timeLeft;

    public Text time;

    public Text score;

    public Text winText;

    public Text life;

    public Text start;

    private int scoreValue = 0;

    private int lifeValue = 1;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioClip musicClipThree;

    public AudioClip collectedClip;

    public AudioClip startClip;

    public AudioSource musicSource;

    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        StartCoroutine(MyCoroutine());
        score.text = "Score: " + scoreValue.ToString();
        start.text = "WASD to move. Press escape to quit.";
        winText.text = "";
        life.text = "Lives: " + lifeValue.ToString();
        time.text = "Time Left: " + timeLeft.ToString();
        
    }

    IEnumerator MyCoroutine()
    {
        musicSource.clip = startClip;
        musicSource.loop = false;
        musicSource.Play();
        yield return new WaitForSeconds(2);
        musicSource.Stop();
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 10)
        {
            float hozMovement = Input.GetAxis("Horizontal");
            float vertMovement = Input.GetAxis("Vertical");
            rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
            if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
            else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }
        }

        if (timeLeft >= 10)
        {
            time.text = "Time Left: 10";
        }
        else
        {
            time.text = "Time Left: " + Mathf.Round(timeLeft);
        }
        if (timeLeft <= 0)
        {
            winText.text = "You Lose!";
            time.text = "Time Left: 0";
            Destroy(this);
            musicSource.Stop();
            musicSource.clip = musicClipThree;
            musicSource.loop = false;
            musicSource.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            AudioSource.PlayClipAtPoint(collectedClip, transform.position);
            Destroy(collision.collider.gameObject);
           
        }

        if (scoreValue >= 1)
        {
            winText.text = "You Win!";
            Destroy(this);
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.loop = false;
            musicSource.Play();
        }

        if (collision.collider.tag == "Enemy")
        {
            lifeValue -= 1;
            life.text = lifeValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (lifeValue <= 0)
        {
            winText.text = "You Lose!";
            Destroy(this);
            musicSource.Stop();
            musicSource.clip = musicClipThree;
            musicSource.loop = false;
            musicSource.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (timeLeft <= 10)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                }
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

}