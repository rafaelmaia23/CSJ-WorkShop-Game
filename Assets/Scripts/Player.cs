using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rig;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public float health;
    public float velocity;
    public float jumpForce;
    public bool isJumping;
    public bool invencible;
    private GameControler gc;
    float direction;

    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameControler>();   
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxis("Horizontal");

        
        
        if(direction > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);            
        }

        if(direction < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);            
        }

        if(direction != 0 && isJumping == false)
        {
            anim.SetInteger("Transition", 1);
        }

        if(direction == 0 && isJumping == false)
        {
            anim.SetInteger("Transition", 0);
        }

        Jump();
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(direction * velocity, rig.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetInteger("Transition", 2);
            isJumping = true;
        }
    }

    public void Damage()
    {
        if(invencible == false)
        {
            gc.LoseHealth(health);
            health--;
            invencible = true;
            StartCoroutine(Respawn());
        }
    }

    float invencibleTime  = 0.3f;
    IEnumerator Respawn()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(invencibleTime);
        spriteRenderer.enabled = true;        
        yield return new WaitForSeconds(invencibleTime);
        spriteRenderer.enabled = false;        
        yield return new WaitForSeconds(invencibleTime);
        spriteRenderer.enabled = true;        
        yield return new WaitForSeconds(invencibleTime);        
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(invencibleTime);
        spriteRenderer.enabled = true;        
        yield return new WaitForSeconds(invencibleTime);
        
        invencible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }
}
