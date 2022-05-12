using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mSpeed;
    public float jSpeed;
    public float gForce;
    public float rSpeed;
    public float slideSpeed;
    public GameObject player;
    private float magnitude;
    private float gravity;
    private float originalGForce;
    private float originalJSpeed;
    private float originalMSpeed;
    public Transform cTransform;
    private Vector3 velocity;
    private CharacterController characterController;

    private bool isGrounded;
    private Vector3 hitNormal;

    public Material blue;
    public Material red;
    public Material rock;
    public Material neutral;

    private int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;
    public Text scoreKeeper;
    private int score = 0;
    private bool canMove;

    private Vector3 mDirection;
    private float jForce;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        scoreKeeper.text = "Slimes Absorbed: " + score.ToString();
        originalGForce = gForce;
        originalJSpeed = jSpeed;
        originalMSpeed = mSpeed;
        characterController = GetComponent<CharacterController>();
        player.GetComponent<Renderer>().enabled = true;
        player.GetComponent<Renderer>().material = neutral;

        //Debug.Log(blue + " " + red + " " + rock  + " " + neutral);
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            playerMovement();
        }    
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentHealth -= 20;
            healthBar.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            canMove = false;
            FindObjectOfType<GameManager>().EndGame();

        }
        else if (cTransform.position.y < -160f)
        {
            canMove=false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.GetComponent<Renderer>().material = neutral;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        //check for water collissions
        if (other.gameObject.name == "water_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd() == neutral.name)
        {
            gForce = originalGForce / 2;
            jSpeed = originalJSpeed / 2;
            mSpeed = originalMSpeed / 2;
            StartCoroutine(takeDamageOverTime(other.gameObject, 0.1f));
            //Debug.Log("mat neutral: " + player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }
        if (other.gameObject.name == "water_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(rock.name))
        {
            gForce = originalGForce / 4;
            jSpeed = originalJSpeed / 2;
            mSpeed = originalMSpeed / 2;
            StartCoroutine(takeDamageOverTime(other.gameObject, 0.1f));
            //Debug.Log("mat rock: " + player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }
        else if (other.gameObject.name == "water_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(red.name))
        {
            currentHealth = 0;
            healthBar.value = currentHealth;
            //Debug.Log("mat red: " + player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }
        else if (other.gameObject.name == "water_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(blue.name))
        {
            InvokeRepeating("restoreHealthOverTime", 0.1f, 1);
            //Debug.Log("mat blue: "+player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }

        //check for rock collisions
        if (other.gameObject.name == "rock_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd() == neutral.name)
        {
            gForce = originalGForce / 2;
            jSpeed = originalJSpeed / 2;
            mSpeed = originalMSpeed / 2;
            StartCoroutine(takeDamageOverTime(other.gameObject, 0.1f));
            //Debug.Log("mat neutral: " + player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }
        if (other.gameObject.name == "rock_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(red.name))
        {
            gForce = originalGForce / 2;
            jSpeed = originalJSpeed / 2;
            mSpeed = originalMSpeed / 2;
            StartCoroutine(takeDamageOverTime(other.gameObject, 0.1f));
            //Debug.Log("mat rock: " + player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }
        else if (other.gameObject.name == "rock_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(blue.name))
        {
            currentHealth = 0;
            healthBar.value = currentHealth;
            //Debug.Log("mat red: " + player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }
        /*else if (other.gameObject.name == "rock_collision" && player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(rock.name))
        {
            CancelInvoke();
            //Debug.Log("mat blue: "+player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd());
        }*/
        if (other.gameObject.name == "Capsule")
        {
            player.GetComponent<Renderer>().material = neutral;
            InvokeRepeating("restoreHealthOverTime", 0.1f, 1);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        CancelInvoke();
        StopAllCoroutines();
        gForce = originalGForce;
        jSpeed = originalJSpeed;
        mSpeed = originalMSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(neutral.name))
        {
            if (collision.gameObject.name == "slime_water")
            {
                Destroy(collision.gameObject);
                player.GetComponent<Renderer>().material = blue;
                score += 1;
                scoreKeeper.text = "Slimes Absorbed: " + score.ToString();
            }
            else if (collision.gameObject.name == "CubeRed")
            {
                Destroy(collision.gameObject);
                player.GetComponent<Renderer>().material = red;
                score += 1;
                scoreKeeper.text = "Slimes Absorbed: " + score.ToString();
            }
            else if (collision.gameObject.name == "slime_rock")
            {
                Destroy(collision.gameObject);
                player.GetComponent<Renderer>().material = rock;
                score += 1;
                scoreKeeper.text = "Slimes Absorbed: " + score.ToString();
            }
        }
        else if (player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(blue.name))
        {
            if (collision.gameObject.name == "CubeRed")
            {
                InvokeRepeating("takeDamage", 0.01f, 1);
            }
            else if (collision.gameObject.name == "slime_rock")
            {
                Destroy(collision.gameObject);
            }
        }
        else if (player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(rock.name))
        {
            if (collision.gameObject.name == "slime_water")
            {
                InvokeRepeating("takeDamage", 0.01f, 1);
            }
            else if (collision.gameObject.name == "CubeRed")
            {
                Destroy(collision.gameObject);
            }
        }
        else if (player.GetComponent<Renderer>().material.name.Replace("(Instance)", "").TrimEnd().Equals(red.name))
        {
            if (collision.gameObject.name == "slime_rock")
            {
                InvokeRepeating("takeDamage", 0.01f, 1);
            }
            else if (collision.gameObject.name == "slime_water")
            {
                Destroy(collision.gameObject);
            }
        }
        StopAllCoroutines();
    }

    private void OnCollisionExit(Collision collision)
    {
        CancelInvoke();
    }


    private IEnumerator takeDamageOverTime(GameObject other, float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (other.name == "rock_collision")
            {
                if (currentHealth > 0)
                {
                    currentHealth -= 5;
                    healthBar.value = currentHealth;
                    delay = 1.5f;
                }
            }
            else if (other.name == "water_collision")
            {
                if (currentHealth > 0)
                {
                    currentHealth -= 10;
                    healthBar.value = currentHealth;
                    delay = 0.5f;
                }
            }
        }
    }

    private void takeDamage()
    {
        currentHealth -= 25;
        healthBar.value = currentHealth;
    }

    private void restoreHealthOverTime()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 5;
            healthBar.value = currentHealth;
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth =  maxHealth;
            healthBar.value = currentHealth;
        }
        else if (currentHealth == maxHealth)
        {
            CancelInvoke();
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private void playerMovement()
    {
        mDirection = new Vector3(Input.GetAxis("Horizontal") * mSpeed, mDirection.y, Input.GetAxis("Vertical") * mSpeed);
        magnitude = Mathf.Clamp(mDirection.magnitude, 0, 50f);
        mDirection = Quaternion.AngleAxis(cTransform.rotation.eulerAngles.y, Vector3.up) * mDirection;
        mDirection.Normalize();

        gravity = Physics.gravity.y * gForce;
        jForce += gravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            jForce = -3f;
            if (Input.GetButtonDown("Jump"))
            {
                jForce = Mathf.Sqrt(jSpeed * jForce * gravity);
            }
        }

        velocity = mDirection * magnitude;
        velocity.y = jForce;


        if (!isGrounded)
        {
            velocity.y += gravity;
            velocity.x += ((1f - hitNormal.y) * hitNormal.x) * slideSpeed;
            velocity.z += ((1f - hitNormal.y) * hitNormal.z) * slideSpeed;
        }

        characterController.Move(velocity * Time.deltaTime);

        isGrounded = Vector3.Angle(Vector3.up, hitNormal) <= characterController.slopeLimit;

        if (mDirection != Vector3.zero)
        {
            Quaternion mRotation = Quaternion.LookRotation(mDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mRotation, rSpeed * Time.deltaTime);
        }
    }
}
