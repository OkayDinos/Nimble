// This script belongs to Okay Dinos, used for the Nimble project.
// This script is for controlling the player character.
// Author(s): Emira Karaibrahimova, Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Apr 2022.

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Yarn.Unity;
using Yarn.Unity.Example;
using UnityEngine.Experimental.U2D.Animation;

public enum PlayerState
{
    Idle,
    Walking,
    WalkingVertical,
    Interacting,
}

public class Player : MonoBehaviour
{
    [SerializeField]
    Yarn.Unity.LineView mc_LineView;

    LevelManager mr_LevelManRef;

    InteractionPromptManager mr_IntProManRef;

    private Vector2 m2_velocity;

    bool mb_IsSprinting;

    private Rigidbody2D mc_body;

    Vector2 m2_ColliderSize;

    private Interactable mr_Interactable;

    private bool mb_IsInteractable;

    bool mb_IsInteracting;
    bool mb_IsTalking;

    [HideInInspector] public bool mb_AutoMoving;

    bool mb_IsFlipped;
    bool mb_IsFacingFront;
    [SerializeField] bool mb_IsWall;

    private PlayerState me_CurrentPlayerState;

    [SerializeField] LayerMask mc_InteractableLayer;

    [SerializeField] Animator mc_Animator;

    [SerializeField] GameObject mg_EyeBone;

    [SerializeField] GameObject mc_SpriteGO;

    [SerializeField] GameObject mg_PromptText;

    [SerializeField] LayerMask mc_GroundLayer;

    [SerializeField] float mf_MagsWalkSpeedMultiplierRuntime = 24, mf_MagsWalkSpeedMultiplierEditor = 120;

    [SerializeField] CapsuleCollider2D mc_Collider;

    float mf_MagsWalkSpeedMultiplier = 24;

    float mf_SprintMult;

    Coroutine mc_OldWalkCR;

    bool mb_FreeEyeMovement;

    List<SpriteRenderer> mc_SpriteRenders;

    float mf_BlinkTimer;
    float mf_BlinkCD;

    [SerializeField] List<SpriteResolver> mc_EyeResolvers = new List<SpriteResolver>();

    [SerializeField] List<SpriteResolver> mc_MouthResolvers = new List<SpriteResolver>();

    [SerializeField] List<SpriteResolver> mc_DirectionResolvers = new List<SpriteResolver>();

    [SerializeField] List<SpriteResolver> mc_ArmResolvers = new List<SpriteResolver>();

    [SerializeField] List<SpriteRenderer> mc_OrderChange = new List<SpriteRenderer>();

    [SerializeField] YarnCharacterView mc_YarnCharacter;

    [SerializeField] DialogueRunner mc_DialogueRunner;

    [SerializeField] GameObject mg_InHandItem;

    float mf_IdleTimer;
    bool mb_IsIdleAnim;

    Interactable mc_PreviousInteractable;

    GameObject mg_IntProRef;
    string ms_PreviousPrompt;
    ControlScheme me_PreviousScheme;

    void Start()
    {
#if UNITY_EDITOR
        mf_MagsWalkSpeedMultiplier = mf_MagsWalkSpeedMultiplierEditor;
#elif UNITY_STANDALONE
        mf_MagsWalkSpeedMultiplier = mf_MagsWalkSpeedMultiplierRuntime;
#endif
        m2_ColliderSize = GetComponent<CapsuleCollider2D>().size;

        mr_LevelManRef = LevelManager.mr_Instance;
        mr_IntProManRef = InteractionPromptManager.mr_Instance;

        m2_velocity = new Vector2(0, 0);
        mc_body = GetComponent<Rigidbody2D>();
        mb_IsInteractable = false;
        mb_IsSprinting = false;
        mf_SprintMult = 1;

        mc_Animator.Play("Idle");
        me_CurrentPlayerState = PlayerState.Idle;
        mb_IsFlipped = false;
        mb_IsFacingFront = true;
        mb_AutoMoving = false;
        mb_IsInteracting = false;
        mb_IsTalking = false;
        mb_IsWall = false;

        mf_IdleTimer = 0;
        mb_IsIdleAnim = false;
        ms_PreviousPrompt = "";
        me_PreviousScheme = GameManager.GetControlScheme();

        mb_FreeEyeMovement = true;
        mf_BlinkTimer = 0f;
        mf_BlinkCD = Random.Range(0f, 4f);

        mc_Collider = GetComponent<CapsuleCollider2D>();
        mc_SpriteRenders = this.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();

        SetPartOverride("Default", mc_ArmResolvers);
        SetPartOverride("Closed", mc_MouthResolvers);
        SetPartOverride("EyesOpen", mc_EyeResolvers);
        SetPartOverride("Front", mc_DirectionResolvers);

        mg_InHandItem.SetActive(false);
    }

    private void Update()
    {
        if (mb_FreeEyeMovement && !mb_IsIdleAnim && GameManager.GetControlScheme() == ControlScheme.Keyboard)
            MoveEyes(true, Vector2.up);
        else if (mb_FreeEyeMovement && !mb_IsIdleAnim && GameManager.GetControlScheme() == ControlScheme.Gamepad)
        {
            if (Mathf.Abs(m2_velocity.x) < 0.01f)
            {
                MoveEyes(false, Vector2.zero);
            }
            else
            {
                MoveEyes(false, Vector2.right);
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.GetCurrentGameState() == GameState.PlayerNormal) // Point & Click movement - CP
        {
            if (!mb_IsInteracting)
            {
                if (Mouse.current.position.ReadValue().y > 0 &&
                    Mouse.current.position.ReadValue().y < Screen.height &&
                    Mouse.current.position.ReadValue().x > 0 &&
                    Mouse.current.position.ReadValue().x < Screen.width)
                {
                    Ray lc_Ray = mr_LevelManRef.mc_Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    float lf_Distance;
                    Plane mc_InteractionPlane = new Plane(new Vector3(0, 0, 1), transform.position);
                    if (mc_InteractionPlane.Raycast(lc_Ray, out lf_Distance))
                        GoToPosition(lc_Ray.GetPoint(lf_Distance), false);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!(me_CurrentPlayerState == PlayerState.WalkingVertical) && !(me_CurrentPlayerState == PlayerState.Interacting))
            PlayerMovement();

        if (me_CurrentPlayerState == PlayerState.Idle && !mb_IsIdleAnim && !mb_IsInteracting && !mb_IsTalking)
        {
            mf_IdleTimer += Time.deltaTime;
            if (mf_IdleTimer > 5f)
            {
                mb_IsIdleAnim = true;
                IdleAnim();
            }
        }
        else if (!(me_CurrentPlayerState == PlayerState.Idle))
        {
            mf_IdleTimer = 0;
            mb_IsIdleAnim = false;
        }

        if (!mb_IsInteracting)
        {
            mb_IsInteractable = InteractableCast();
        }
        else
        {
            mb_IsInteractable = false;
        }

        if (mb_IsInteractable == true)
        {
            if (mc_PreviousInteractable != mr_Interactable)
            {
                if (mc_PreviousInteractable)
                    if (mc_PreviousInteractable.GetOutlineObject())
                        mc_PreviousInteractable.GetOutlineObject().SendMessage("SetOutline", false);

                if (mr_Interactable.GetOutlineObject())
                    mr_Interactable.GetOutlineObject().SendMessage("SetOutline", true);

                mc_PreviousInteractable = mr_Interactable;
            }

            if (ms_PreviousPrompt != mr_Interactable.GetPrompt())
            {
                if (mg_IntProRef != null)
                {
                    mr_IntProManRef.mg_InteractionPrompts.Remove(mg_IntProRef);
                    Destroy(mg_IntProRef);
                }
            }

            if (me_PreviousScheme != GameManager.GetControlScheme())
            {
                if (mg_IntProRef != null)
                {
                    mr_IntProManRef.mg_InteractionPrompts.Remove(mg_IntProRef);
                    Destroy(mg_IntProRef);
                }
            }

            if (mg_IntProRef == null)
            {
                if (GameManager.GetControlScheme() == ControlScheme.Keyboard)
                {
                    if ((mr_Interactable.GetType() == typeof(MoveLocation)))
                    {
                        if (mr_Interactable.GetComponent<MoveLocation>().GetInteractionDirectionType() == InteractDirectionType.TowardsCam)
                        {
                            mg_IntProRef = mr_IntProManRef.AddInteractionPrompt(mr_Interactable.GetPrompt(), InteractionControl.S);
                        }
                        else if (mr_Interactable.GetComponent<MoveLocation>().GetInteractionDirectionType() == InteractDirectionType.AwayCam)
                        {
                            mg_IntProRef = mr_IntProManRef.AddInteractionPrompt(mr_Interactable.GetPrompt(), InteractionControl.W);
                        }
                        else
                        {
                            mg_IntProRef = mr_IntProManRef.AddInteractionPrompt(mr_Interactable.GetPrompt(), InteractionControl.E);
                        }
                    }
                    else
                    {
                        mg_IntProRef = mr_IntProManRef.AddInteractionPrompt(mr_Interactable.GetPrompt(), InteractionControl.E);
                    }


                }
                else
                {
                    mg_IntProRef = mr_IntProManRef.AddInteractionPrompt(mr_Interactable.GetPrompt(), InteractionControl.GamepadWest);
                }
                mr_IntProManRef.mg_InteractionPrompts.Add(mg_IntProRef);
                ms_PreviousPrompt = mr_Interactable.GetPrompt();
                me_PreviousScheme = GameManager.GetControlScheme();
            }
        }
        else
        {
            //mg_PromptText.SetActive(false);

            if (mg_IntProRef != null)
            {
                mr_IntProManRef.mg_InteractionPrompts.Remove(mg_IntProRef);
                Destroy(mg_IntProRef);
            }
        }

        if (mb_IsFacingFront) BlinkCheck();

        if (!mb_IsTalking && mc_YarnCharacter.GetSpeaker() == "Mags")
        {
            mb_IsTalking = true;
            Talk();
        }

        if (GameManager.GetCurrentGameState() == GameState.Paused || GameManager.GetCurrentGameState() == GameState.InventoryOpen)
        {
            m2_velocity = Vector2.zero;
        }
    }

    async void IdleAnim()
    {
        SetPartOverride("Knitting", mc_ArmResolvers);
        MoveEyes(false, Vector2.down);
        mc_Animator.Play("Knitting");
        while (mb_IsIdleAnim)
        {
            await Task.Yield();
        }
        SetPartOverride("Default", mc_ArmResolvers);
    }

    async void Talk()
    {
        SetPart("Talk", mc_MouthResolvers);
        float lf_TalkTimer = 0f;
        float lf_TalkTime = 0.15f;
        bool lb_Open = true;

        while (mc_YarnCharacter.GetSpeaker() == "Mags" && mc_DialogueRunner.IsDialogueRunning)
        {
            lf_TalkTimer += Time.deltaTime;

            if (lf_TalkTimer > lf_TalkTime)
            {
                lf_TalkTimer = 0f;
                lb_Open = !lb_Open;
                if (lb_Open)
                {
                    if (this != null) SetPart("Talk", mc_MouthResolvers);
                    lf_TalkTime = Random.Range(0.05f, 0.2f);
                }
                else
                {
                    if (this != null) SetPart("Closed", mc_MouthResolvers);
                    lf_TalkTime = Random.Range(0.15f, 0.3f);
                }
            }

            await Task.Yield();
        }

        mb_IsTalking = false;
        if (this != null) SetPart("Closed", mc_MouthResolvers);
    }

    protected void BlinkCheck()
    {
        mf_BlinkTimer += Time.deltaTime;
        if (mf_BlinkTimer > mf_BlinkCD)
        {
            mf_BlinkTimer = 0f;
            mf_BlinkCD = Random.Range(3f, 4f);

            Blink();
        }
    }

    async void Blink()
    {
        SetPartOverride("EyesClosed", mc_EyeResolvers);
        float lf_BlinkTimer = 0f;

        while (lf_BlinkTimer < 0.15f)
        {
            lf_BlinkTimer += Time.deltaTime;
            await Task.Yield();
        }
        if (mb_IsFacingFront && this != null) SetPartOverride("EyesOpen", mc_EyeResolvers);
    }

    protected void SetPart(string as_ToSet, List<SpriteResolver> ac_Resolvers)
    {
        foreach (var lc_Res in ac_Resolvers)
        {
            lc_Res.SetCategoryAndLabel(lc_Res.GetCategory(), as_ToSet);
        }
    }

    protected void SetPartOverride(string as_ToSet, List<SpriteResolver> ac_Resolvers)
    {
        foreach (var lc_Res in ac_Resolvers)
        {
            var lb_Exists = lc_Res.spriteLibrary.GetSprite(lc_Res.GetCategory(), as_ToSet);
            lc_Res.gameObject.SetActive(lb_Exists);
            if (lb_Exists) lc_Res.SetCategoryAndLabel(lc_Res.GetCategory(), as_ToSet);
        }
    }

    public void FinishedInteracting()
    {
        mb_FreeEyeMovement = true;
        me_CurrentPlayerState = PlayerState.Idle;
        mb_IsInteracting = false;
        mg_InHandItem.SetActive(false);

        mb_IsSprinting = false;
        mf_SprintMult = 1f;

        m2_velocity = Vector2.zero;
    }

    void PlayerMovement()
    {
        if ((mb_IsFlipped && SideCheck(mc_GroundLayer, Vector2.left)) || (!mb_IsFlipped && SideCheck(mc_GroundLayer, Vector2.right)))
        {
            mb_IsWall = true;
        }
        else
        {
            mb_IsWall = false;
        }

        Vector2 l2_SlopePerp = SlopeCheck(mc_GroundLayer);

        if (!mb_IsWall)
            mc_body.AddForce(new Vector2(m2_velocity.x * Mathf.Abs(l2_SlopePerp.x), m2_velocity.x * -l2_SlopePerp.y).normalized * mf_MagsWalkSpeedMultiplier * mf_SprintMult);

        if (m2_velocity.x != 0)
        {
            Face(m2_velocity.x < 0);

            if (mb_IsWall)
            {
                if (me_CurrentPlayerState != PlayerState.Idle)
                {
                    mc_Animator.CrossFadeInFixedTime("Idle", 0.2f);
                }
                else if (!mb_IsIdleAnim)
                {
                    mc_Animator.Play("Idle");
                }

                me_CurrentPlayerState = PlayerState.Idle;
            }
            else
            {
                me_CurrentPlayerState = PlayerState.Walking;

                if (me_CurrentPlayerState != PlayerState.Walking)
                {
                    mc_Animator.CrossFadeInFixedTime("Walking", 0.2f); // TODO: this doesnt work yet :( - CP
                }
                else
                {
                    if (mb_IsSprinting)
                    {
                        mc_Animator.Play("Sprinting");
                    }
                    else
                    {
                        mc_Animator.Play("Walking");
                    }
                }
            }
        }
        else
        {
            if (me_CurrentPlayerState != PlayerState.Idle)
            {
                mc_Animator.CrossFadeInFixedTime("Idle", 0.2f);
            }
            else if (!mb_IsIdleAnim)
            {
                mc_Animator.Play("Idle");
            }

            me_CurrentPlayerState = PlayerState.Idle;
        }
    }

    public void Face(bool ab_Left)
    {
        if (ab_Left)
        {
            mb_IsFlipped = true;
            mc_SpriteGO.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            mb_IsFlipped = false;
            mc_SpriteGO.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Does a circular cast to check if player is in range of an interactable, if there are multiple, picks the closest one - CP
    bool InteractableCast()
    {
        RaycastHit2D[] lc_Hit = Physics2D.CircleCastAll(transform.position, 1f, Vector2.up, Mathf.Infinity, mc_InteractableLayer);

        bool lb_Result = false;

        if (lc_Hit.Length > 0)
        {

            GameObject lg_Closest = lc_Hit[0].transform.gameObject;

            if (lc_Hit.Length > 1)
            {
                float lf_Closest = float.MaxValue;

                foreach (RaycastHit2D l in lc_Hit)
                {
                    bool lb_BadInteractable = false;

                    foreach (Interactable ac_Interactable in l.transform.gameObject.GetComponents<Interactable>())
                    {
                        if ((ac_Interactable.GetType() == typeof(MapLocation)))
                        {
                            lb_BadInteractable = true;
                        }

                    }

                    float dist = (l.transform.position - transform.position).magnitude;

                    if (dist < lf_Closest && lb_BadInteractable == false)
                    {

                        lg_Closest = l.transform.gameObject;
                        lf_Closest = dist;

                    }
                }
            }

            //* mr_Interactable = lg_Closest.transform.gameObject.GetComponent<Interactable>();

            Interactable[] lc_Interactables = lg_Closest.transform.gameObject.GetComponents<Interactable>();

            foreach (Interactable ac_Interactable in lc_Interactables)
            {
                if (!(ac_Interactable.GetType() == typeof(MapLocation)) && !(ac_Interactable.GetType() == typeof(Proxim)))
                    if (ac_Interactable.GetUnlockStatus())
                    {
                        mr_Interactable = ac_Interactable;
                        lb_Result = true;
                    }
            }
        }

        return lb_Result;
    }

    private void OnMove(InputValue ar_InputValue)
    {
        if (!mb_IsInteracting && GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            Vector2 l2_InputVector = ar_InputValue.Get<Vector2>();

            m2_velocity = l2_InputVector;

            if (mc_OldWalkCR != null)
                this.StopCoroutine(mc_OldWalkCR);

            mb_AutoMoving = false;

            //if (mb_IsInteractable == true && Mathf.Abs(l2_InputVector.y) > 0.5f)
            //{
            //    int li_VWalk = mr_Interactable.TryWalkInt();
            //    if ((li_VWalk == 1 && l2_InputVector.y > 0.5f) || (li_VWalk == 2 && l2_InputVector.y < -0.5f))
            //    {
            //        OnInteract(null);
            //    }
            //}
        }
        //else if (GameManager.GetCurrentGameState() == GameState.Paused || GameManager.GetCurrentGameState() == GameState.InventoryOpen)
        //{
        //    m2_velocity = Vector2.zero;
        //}
    }

    private void OnInteract(InputValue ar_InputValue)
    {
        if (mb_IsInteractable == true && GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            bool lb_DoIt = false;

            if (mr_Interactable.GetType() == typeof(MoveLocation))
            {
                if (mr_Interactable.GetComponent<MoveLocation>().GetInteractionDirectionType() == InteractDirectionType.Default)
                    lb_DoIt = true;
            }
            else
                lb_DoIt = true;

            if (lb_DoIt)
            {
                mc_Animator.Play("Idle");

                mb_IsInteracting = true;

                mb_FreeEyeMovement = false;
                MoveEyes(false, new Vector2(0.8f, 0.3f));

                mr_Interactable.OnInteract();
            }
        }
    }

    private void OnInteractBack(InputValue ar_InputValue)
    {
        if (mb_IsInteractable == true && GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            bool lb_DoIt = false;

            if (mr_Interactable.GetType() == typeof(MoveLocation))
            {
                if (mr_Interactable.GetComponent<MoveLocation>().GetInteractionDirectionType() == InteractDirectionType.AwayCam)
                    lb_DoIt = true;
            }
            else
                lb_DoIt = false;

            if (lb_DoIt)
            {
                mc_Animator.Play("Idle");

                mb_IsInteracting = true;

                mb_FreeEyeMovement = false;
                MoveEyes(false, new Vector2(0.8f, 0.3f));

                mr_Interactable.OnInteract();
            }
        }
    }
     void OnTalk()
    {
        Debug.Log("PregressMe2");
        mc_LineView.UserRequestedViewAdvancement();
    }


    private void OnInteractFront(InputValue ar_InputValue)
    {
        if (mb_IsInteractable == true && GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            bool lb_DoIt = false;

            if (mr_Interactable.GetType() == typeof(MoveLocation))
            {
                if (mr_Interactable.GetComponent<MoveLocation>().GetInteractionDirectionType() == InteractDirectionType.TowardsCam)
                    lb_DoIt = true;
            }
            else
                lb_DoIt = false;

            if (lb_DoIt)
            {
                mc_Animator.Play("Idle");

                mb_IsInteracting = true;

                mb_FreeEyeMovement = false;
                MoveEyes(false, new Vector2(0.8f, 0.3f));

                mr_Interactable.OnInteract();
            }
        }
    }

    public void ProximInteract()
    {
        mc_Animator.Play("Idle");

        mb_IsInteracting = true;

        mb_FreeEyeMovement = false;
        MoveEyes(false, new Vector2(0.8f, 0.3f));
    }

    void OnSprint(InputValue ar_InputValue)
    {
        if (!mb_IsInteracting && GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            mb_IsSprinting = true;
            mf_SprintMult = 1.5f;
        }
    }

    void OnSprintRelease(InputValue ar_InputValue)
    {
        if (!mb_IsInteracting && GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            mb_IsSprinting = false;
            mf_SprintMult = 1f;
        }
    }

    public void PickUp(Sprite ac_ItemSprite)
    {
        mg_InHandItem.SetActive(true);
        if (ac_ItemSprite != null)
        {
            mg_InHandItem.GetComponent<SpriteRenderer>().sprite = ac_ItemSprite;
        }
        me_CurrentPlayerState = PlayerState.Interacting;
        mc_Animator.Play("PickUp");
        MoveEyes(false, new Vector2(0.6f, -0.5f));
    }

    void MoveEyes(bool ab_FollowMouse, Vector2 a2_Pos) // Move eyes (Vector 2 goes from -1 to 1)
    {
        Vector2 l2_EyeCentre = new Vector2(0.42f, 0.115f);

        Vector2 l2_MaxOffset = new Vector2(0.05f, 0.05f);

        if (ab_FollowMouse)
        {
            Vector2 l2_MousePos = Mouse.current.position.ReadValue();

            l2_MousePos = new Vector2(l2_MousePos.x * 2 / Screen.width, l2_MousePos.y * 2 / Screen.height);

            l2_MousePos.x = Mathf.Clamp(l2_MousePos.x, 0f, 2f);
            l2_MousePos.y = Mathf.Clamp(l2_MousePos.y, 0f, 2f);

            l2_MousePos -= new Vector2(1, 1);

            if (mb_IsFlipped)
                l2_MousePos.x *= -1;

            mg_EyeBone.transform.localPosition = l2_EyeCentre + new Vector2(l2_MousePos.y * l2_MaxOffset.y, -l2_MousePos.x * l2_MaxOffset.x);
        }
        else
        {
            mg_EyeBone.transform.localPosition = l2_EyeCentre + new Vector2(a2_Pos.y * l2_MaxOffset.y, -a2_Pos.x * l2_MaxOffset.x);
        }
    }

    // Moves the player to new position from current position - CP
    public void GoToPosition(Vector3 a3_newPos, bool ab_Vertical, string as_NewPlayerVislayer = "", bool ab_IsLast = true)
    {
        float lf_Distance = (transform.position - a3_newPos).magnitude;

        if (mc_OldWalkCR != null)
            this.StopCoroutine(mc_OldWalkCR);

        mb_AutoMoving = true;

        if (ab_Vertical)
            mc_OldWalkCR = StartCoroutine(MoveToVerticalCR(transform.position, a3_newPos, lf_Distance / 5f, as_NewPlayerVislayer, ab_IsLast));
        else
            mc_OldWalkCR = StartCoroutine(MoveToCR(transform.position.x, a3_newPos.x));
    }

    IEnumerator MoveToCR(float af_StartPos, float af_EndPos)
    {
        bool lb_DirLeft = af_StartPos > af_EndPos;

        mc_Animator.Play("Walking");

        while (((lb_DirLeft && transform.position.x > af_EndPos) || (!lb_DirLeft && transform.position.x < af_EndPos)) && mb_AutoMoving == true)
        {
            if (lb_DirLeft)
                m2_velocity.x = -1;
            else
                m2_velocity.x = 1;

            yield return null;
        }

        m2_velocity.x = 0f;
        mc_Animator.Play("Idle");
        mb_AutoMoving = false;
    }

    // Coroutine that moves player from start pos to end pos in a set amount of time - CP
    IEnumerator MoveToVerticalCR(Vector3 a3_StartPos, Vector3 a3_EndPos, float af_Time, string as_NewPlayerSortingLayer, bool ab_IsLast)
    {
        float lf_Timer = 0f;

        bool lb_EndSorting = false;

        mc_Animator.Play("WalkingVertical");

        me_CurrentPlayerState = PlayerState.WalkingVertical;

        if (a3_StartPos.z > a3_EndPos.z) // Moving towards cam
        {
            MoveEyes(false, new Vector2(0f, -0.5f));
            if (ab_IsLast || !mr_LevelManRef.mb_AutoMoveBack)
            {
                foreach (SpriteRenderer ac_SpriteRender in mc_SpriteRenders)
                {
                    ac_SpriteRender.sortingLayerName = as_NewPlayerSortingLayer;
                }
            }
        }
        else if (a3_StartPos.z == a3_EndPos.z)
        {
            foreach (SpriteRenderer ac_SpriteRender in mc_SpriteRenders)
            {
                ac_SpriteRender.sortingLayerName = "DoveHousesDepthPlayer";
            }
        }
        else // Moving away from cam
        {
            mb_IsFacingFront = false;
            SetPartOverride("Back", mc_DirectionResolvers);
            SetPartOverride("Back", mc_MouthResolvers);
            SetPartOverride("Back", mc_EyeResolvers);
            mc_OrderChange[0].sortingOrder = 13;
            mc_OrderChange[1].sortingOrder = 15;
            mc_OrderChange[2].sortingOrder = 14;
            mc_OrderChange[3].sortingOrder = 8;
            mc_OrderChange[4].sortingOrder = 9;
            lb_EndSorting = true;
        }

        if (a3_StartPos.z > a3_EndPos.z)
        {
            SetPartOverride("Front", mc_DirectionResolvers);
            SetPartOverride("Closed", mc_MouthResolvers);
            SetPartOverride("EyesOpen", mc_EyeResolvers);
            mc_OrderChange[0].sortingOrder = 32;
            mc_OrderChange[1].sortingOrder = 9;
            mc_OrderChange[2].sortingOrder = 8;
            mc_OrderChange[3].sortingOrder = 15;
            mc_OrderChange[4].sortingOrder = 14;
            mb_IsFacingFront = true;
        }

        while (lf_Timer < af_Time)
        {
            lf_Timer += Time.deltaTime;

            transform.position = Vector3.Lerp(a3_StartPos, a3_EndPos, lf_Timer / af_Time);

            yield return null;
        }

        if (a3_StartPos.z == a3_EndPos.z)
        {
            foreach (SpriteRenderer ac_SpriteRender in mc_SpriteRenders)
            {
                ac_SpriteRender.sortingLayerName = "DoveHousesPlayer";
            }
        }

        if (lb_EndSorting && (ab_IsLast || !mr_LevelManRef.mb_AutoMoveBack))
        {
            foreach (SpriteRenderer ac_SpriteRender in mc_SpriteRenders)
            {
                ac_SpriteRender.sortingLayerName = as_NewPlayerSortingLayer;
            }
        }

        if (ab_IsLast)
        {
            FinishedInteracting();

            mb_AutoMoving = false;

            if (!mb_IsFacingFront)
            {
                SetPartOverride("Front", mc_DirectionResolvers);
                SetPartOverride("Closed", mc_MouthResolvers);
                SetPartOverride("EyesOpen", mc_EyeResolvers);
                mc_OrderChange[0].sortingOrder = 32;
                mc_OrderChange[1].sortingOrder = 9;
                mc_OrderChange[2].sortingOrder = 8;
                mc_OrderChange[3].sortingOrder = 15;
                mc_OrderChange[4].sortingOrder = 14;
                mb_IsFacingFront = true;
            }
        }
        else
        {
            mr_LevelManRef.mb_AutoMoveNext = true;
        }
    }

    private Vector2 SlopeCheck(LayerMask layerCheck)
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, -m2_ColliderSize.y / 4);
        RaycastHit2D lc_HitV = Physics2D.Raycast(checkPos, Vector2.down, 1f, layerCheck);

        if (lc_HitV)
        {
            Vector2 l2_SlopeNormPerp = Vector2.Perpendicular(lc_HitV.normal).normalized;

            Debug.DrawRay(lc_HitV.point, l2_SlopeNormPerp, Color.red);
            Debug.DrawRay(lc_HitV.point, lc_HitV.normal, Color.green);

            return l2_SlopeNormPerp;
        }
        return new Vector2(1, 0);
    }

    bool SideCheck(LayerMask layerCheck, Vector2 a2_Dir)
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, -m2_ColliderSize.y / 2);
        RaycastHit2D lc_HitV = Physics2D.Raycast(checkPos, a2_Dir, 0.5f, layerCheck);

        if (lc_HitV)
        {
            return true;
        }
        return false;
    }

    public void SetCollision(bool ab_Enabled)
    {
        mc_Collider.enabled = ab_Enabled;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(transform.position, 1);
    //}
}
