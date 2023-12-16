using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject wallPrefab, effect;
    public GameObject[] ball_list;
    public GameObject dead_screen,main_screen,medal;

    public bool canSpawned, isMoving, isDead;
    public int counter, score, bestScore;

    public TextMeshProUGUI best_score_text;
    public TextMeshProUGUI dead_score_text;

    TextMeshProUGUI score_text;
    GameObject newBall;
    GameObject spawnPoint;

    private void Start()
    {
        counter = 1;
        score = 0;
        canSpawned = false;
        isMoving = false;

        GameObject canvas = GameObject.Find("Canvas");
        spawnPoint = GameObject.Find("SpawnPoint");

        WallSpawner();
        newBall = Instantiate(ball_list[0], spawnPoint.transform.position, Quaternion.identity);

        if (canvas != null)
        {
            Transform score_object = canvas.transform.GetChild(0).GetChild(0);
            score_text = score_object.GetComponent<TextMeshProUGUI>();
        }

        bestScore = PlayerPrefs.GetInt(nameof(bestScore));
        best_score_text.text = PlayerPrefs.GetInt(nameof(bestScore)).ToString();

        medal.SetActive(false);

    }


    private void FixedUpdate()
    {
        Rigidbody2D ball_rb = newBall.GetComponent<Rigidbody2D>();

        if ((ball_rb.gravityScale != 0) && (!isMoving) && (!isDead))
        {
            canSpawned = true;
            StartCoroutine(BallSpawnerCoroutine());
            isMoving = true;
        }


        if (isDead)
        {
            GameObject[] ball_objects = new GameObject[0];

            for (int i = 0; i < 11; i++)
            {
                GameObject[] ball_tags_1 = GameObject.FindGameObjectsWithTag("Ball_0" + (i+1));

                int oldLenght = ball_objects.Length;
                System.Array.Resize(ref ball_objects, oldLenght + ball_tags_1.Length);

                System.Array.Copy(ball_tags_1, 0, ball_objects, oldLenght, ball_tags_1.Length);
            }


            foreach (GameObject item in ball_objects)
            {
                Vector3 effect_pos = new Vector3(item.transform.position.x, item.transform.position.y,item.transform.position.z-3);
                Instantiate(effect, effect_pos, Quaternion.identity);

                string index = item.tag.Substring(item.tag.Length - 2);
                int ball_number = int.Parse(index);

                score += ball_number * 3;
                score_text.text = score.ToString();

                if(score >= bestScore)
                {
                    PlayerPrefs.SetInt(nameof(bestScore), score);
                    best_score_text.text = PlayerPrefs.GetInt(nameof(bestScore)).ToString();
                }

                Destroy(item);
                
                StartCoroutine(AfterDead());

            }
        }

        score_text.text = score.ToString();
    }


    private void BallSpawner()
    {
        switch (counter)
        {
            case 1:
            case 2:
                BallFunctions(0);
                break;
            case 3:
                BallFunctions(1);
                break;
            case 4:
            case 5:
                BallFunctions(2);
                break;
            case 6:
                BallFunctions(3);
                break;
            default:
                BallFunctions(Random.Range(0, 5));
                break;
        }
    }
    private void BallFunctions(int index)
    {
        counter++;
        newBall = Instantiate(ball_list[index], spawnPoint.transform.position, Quaternion.identity);
        newBall.SetActive(true);
        canSpawned = false;
        isMoving = false;
    }
    private void WallSpawner()
    {
        // Ekranýn geniþlik ve yüksekliði
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Spawn konumlarý için temel vektörleri al
        Vector3 leftSpawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(0f, screenHeight / 2, -Camera.main.transform.position.z));
        Vector3 rightSpawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight / 2, -Camera.main.transform.position.z));
        Vector3 bottomSpawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth / 2, 12f, -Camera.main.transform.position.z));
        Vector3 topSpawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth / 2, screenHeight, -Camera.main.transform.position.z));

        // Duvar spawnlama fonksiyonlarý
        GameObject wall_left = Instantiate(wallPrefab, leftSpawnPosition, Quaternion.Euler(new Vector3(0, 0, 90)));
        GameObject wall_right = Instantiate(wallPrefab, rightSpawnPosition, Quaternion.Euler(new Vector3(0, 0, 90)));
        GameObject wall_bottom = Instantiate(wallPrefab, bottomSpawnPosition, Quaternion.identity);
        GameObject wall_top = Instantiate(wallPrefab, topSpawnPosition, Quaternion.identity);

        wall_left.tag = "WallLeft";
        wall_right.tag = "WallRight";
        wall_top.tag = "WallTop";
        wall_bottom.tag = "WallBottom";

        wall_left.GetComponent<SpriteRenderer>().enabled = false;
        wall_right.GetComponent<SpriteRenderer>().enabled = false;
        wall_top.GetComponent<SpriteRenderer>().enabled = false;

    }

    IEnumerator BallSpawnerCoroutine()
    {
        yield return new WaitForSeconds(2f);
        BallSpawner();
    }
    IEnumerator AfterDead()
    {
        dead_score_text.text = score.ToString();
        main_screen.SetActive(false);
        yield return new WaitForSecondsRealtime(2);
        dead_screen.SetActive(true);

        if(score >= bestScore)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            medal.SetActive(true);
        }
    }
}








