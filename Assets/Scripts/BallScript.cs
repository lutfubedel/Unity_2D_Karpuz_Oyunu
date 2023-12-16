using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallScript : MonoBehaviour
{

    public GameObject[] balls;
    public GameObject effect;

    public float moveSpeed,stayTime;
    public bool canMove,canMerge,isMainSceneActive;

    GameObject spawnPoint;
    Rigidbody2D rb;
    AudioSource source;
    GameManager gameManager;
    Transform mainScene;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnPoint = GameObject.Find("SpawnPoint");
        mainScene = GameObject.Find("Canvas").transform.GetChild(0);

        canMerge = false;

        if (transform.position.x == spawnPoint.transform.position.x)
        {
            rb.gravityScale = 0;
            canMove = true;
            canMerge = true;
        }
        else
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            rb.gravityScale = 1;
            StartCoroutine(MergeTimer());
        }

        stayTime = 0f;
    }

    void FixedUpdate()
    {
        float newSize = Mathf.Lerp(transform.localScale.x, 0.9f, 5 * Time.deltaTime);
        transform.localScale = new Vector3(newSize, newSize, newSize);

        isMainSceneActive = mainScene.gameObject.activeInHierarchy;

        print(source.isPlaying);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            bool touchUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

            if((!touchUI) && (isMainSceneActive))
            {
                if ((touch.phase == TouchPhase.Began) && (canMove))
                {
                    Vector3 touchPoint = Input.GetTouch(0).position;
                    touchPoint = Camera.main.ScreenToWorldPoint(touchPoint);

                    transform.position = new Vector3(touchPoint.x, transform.position.y, transform.position.z);
                }

                if ((touch.phase == TouchPhase.Moved) && (canMove))
                {
                    rb.velocity = new Vector2(touch.deltaPosition.x * moveSpeed * Time.deltaTime, rb.velocity.y);
                }

                if ((touch.phase == TouchPhase.Ended))
                {
                    moveSpeed = 0;
                    canMove = false;
                    rb.gravityScale = 1;
                }
            }  
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb_collision = collision.gameObject.GetComponent<Rigidbody2D>();

        if ((gameObject.tag == collision.gameObject.tag) && (rb.velocity.magnitude < rb_collision.velocity.magnitude))
        {
            if (canMerge == true)
            {
                string index = gameObject.tag.Substring(gameObject.tag.Length - 2);
                int ball_number = int.Parse(index);

                Destroy(collision.gameObject);
                Instantiate(effect, new Vector3(transform.position.x, transform.position.y, transform.position.z - 3), Quaternion.identity);

                print("a");
                source.Play();
                
                print("b");

                if (ball_number != 11)
                {
                    Instantiate(balls[ball_number], transform.position, Quaternion.identity);
                }

                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;

                gameManager.score += ball_number * 6;

                Destroy(gameObject, 5f);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            stayTime += Time.deltaTime;

            if (stayTime >= 2)
            {
                gameManager.isDead = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stayTime = 0f;
    }


    IEnumerator MergeTimer()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        canMerge = true;
    }


}
