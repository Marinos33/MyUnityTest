using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms; //index 0 = LR, index 1 = LRB, index 2 = LRT, index 3 = LRTB

    public float moveAmount; // determine l'ecart entre chaque case (de milieu a milieu)
    public float startTimeBtwRoom = 0.25f;
    public float minX;
    public float maxX;
    public float minY;
    public LayerMask room;


    private float timeBtwRoom;
    private int direction; //determine la prochaine case pour etre sur qu'il y ait toujours un chemin possible
    public bool stopGenaration = false;
    private int downCounter;

    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[0], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (timeBtwRoom <= 0 && stopGenaration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move()
    {
        
        if (direction == 1 || direction == 2) // move right
        {

            if (transform.position.x < maxX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = new Vector3(newPos.x, newPos.y, 0);

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) // move left
        {
            if (transform.position.x > maxX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = new Vector3(newPos.x, newPos.y, 0);

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 5) // move down
        {
            downCounter++;
            if (transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room); // collider pour determiner le type de piece qui a etait generé
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {

                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().roomDestruction();
                        Instantiate(rooms[3], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().roomDestruction();

                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    }
                }


                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = new Vector3(newPos.x, newPos.y, 0);


                int rand = Random.Range(0, 4);
                Instantiate(rooms[rand], new Vector3(transform.position.x,transform.position.y,0), Quaternion.identity);


                direction = Random.Range(1, 6);
            }
            else
            {
                stopGenaration = true;
            }
        }
    }
}
