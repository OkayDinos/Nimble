using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BirdState
{

    IDLE,
    FLYING,

};
public class SeagullAI : MonoBehaviour
{


    [SerializeField] AudioClip audio;
    Animator mc_Animator;
    LevelManager mr_LevelMan;
    SpriteRenderer spriteRenderer;
    //MusicManager mc_MusicManager;
    BirdState me_CurrentState;
    bool mb_IsFacingRight;
    float mf_speed = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        mc_Animator = GetComponent<Animator>();
        mc_Animator.Play("BlinkingSeagull");
        mr_LevelMan = LevelManager.mr_Instance;
        me_CurrentState = BirdState.IDLE;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }



    // Update is called once per frame
    void Update()
    {

        if ((mr_LevelMan.mg_PlayerRef.transform.position - transform.position).magnitude < 2.0f && me_CurrentState == BirdState.IDLE)
        {
            if ((mr_LevelMan.mg_PlayerRef.transform.position - transform.position).x < 0.0f)
            {
                transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
                if (spriteRenderer.flipX == false) 
                {

                    transform.Rotate(0, 180, 0);
                    transform.Rotate(0, 0, -40);

                }
                transform.Rotate(0, 0, 30);
                mb_IsFacingRight = true;


            }
            else
            {
                transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);


                if (spriteRenderer.flipX == false)
                {

                    transform.Rotate(0, 180, 0);
                    transform.Rotate(0, 0, 40);

                }

                transform.Rotate(0, 0, -30);
                mb_IsFacingRight = false;
            }



            me_CurrentState = BirdState.FLYING;

            mc_Animator.Play("FlyingSeagull");
            GetComponent<AudioSource>().PlayOneShot(audio);



        }

        if (me_CurrentState == BirdState.FLYING)
        {
            if (mb_IsFacingRight == true)
            {

                transform.position += ((Vector3.right + Vector3.up) * (Time.deltaTime * mf_speed));


            }

            else
            {

              transform.position += ((Vector3.left + Vector3.up) * (Time.deltaTime * mf_speed));

            }

            if (transform.position.y > 90)
            {

                Destroy(gameObject);

            }

        }


    }

    


}


