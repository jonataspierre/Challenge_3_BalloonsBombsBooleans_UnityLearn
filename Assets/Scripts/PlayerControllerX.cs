using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private GameManager gameManager;

    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private float upBound = 14.5f;
    private int pointMoneyValue = 10;

    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip impulseSound;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;

        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5f);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetButton("Jump") && !gameOver)            
        {            
            if (transform.position.y < upBound)
            {
                playerRb.AddForce(Vector3.up * floatForce);
            }            
        }

        if (transform.position.y > upBound && playerRb.velocity.y > 0)
        {
            playerRb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;            
            Destroy(other.gameObject);

            gameManager.GameOver();
        }        

        //if player collides with ground, impulse
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerRb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            playerAudio.PlayOneShot(impulseSound, 1.0f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // if player trigger collider with money, fireworks
        if (other.gameObject.CompareTag("Money"))
        {
            gameManager.UpdateScore(pointMoneyValue);
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}
