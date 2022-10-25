// This script belongs to Okay Dinos, used for the Nimble project.
// This script contains the logic for the cooking minigame and inherits from MinigameBase.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using TMPro;

public class MinigameCooking : MinigameBase
{
    [SerializeField] List<MinigameRecipe> mr_Recipes = new List<MinigameRecipe>();

    List<MinigameObject> mr_SOObjects = new List<MinigameObject>();

    List<GameObject> mr_Objects = new List<GameObject>();

    MinigameRecipe mr_CurrentRecipe;

    MinigameStep mr_CurrentStep;

    [SerializeField] GameObject mg_BlankObject;

    [SerializeField] Camera mc_Cam;

    [SerializeField] GameObject mg_StepComplete;

    [SerializeField] LineRenderer mc_CutLine;

    [SerializeField] GameObject mc_Slider;

    [SerializeField] GameObject mc_HelpText;

    [SerializeField] GameObject mg_CutZoneStart;
    [SerializeField] GameObject mg_CutZoneEnd;

    [SerializeField] SpriteRenderer mc_OvenFront;

    [SerializeField] Sprite mc_OvenFrontDefault;
    [SerializeField] Sprite mc_OvenFrontFire;

    bool mb_StepComplete;

    bool mb_Cutting;
    Vector2 m2_CutStart;
    Vector2 m2_CutEnd;
    bool mb_GamepadCutting;
    bool mb_Mashing;
    float mf_MashLevel;
    bool mb_Stirring;
    bool mb_GamepadStirring;
    int mi_CurrentSide;
    float mf_StirLevel;
    bool mb_Mixing;
    bool mb_Frying;
    bool mb_GamepadFrying;
    float mf_FryLevel;
    float mf_FryLevelSiz;

    float mf_MinigameTimer;

    [SerializeField] Vector3 m3_MousePos;

    int mi_CurrentObject;
    int mi_CurrentObjectOther;

    GameObject mg_IntProRef;
    string ms_PreviousPrompt;
    ControlScheme me_PreviousScheme;

    public new void Start()
    {
        base.Start();

        mr_CurrentRecipe = mr_Recipes[GameManager.GetRecipeID()];

        mg_StepComplete.SetActive(false);

        mb_Mashing = false;
        mb_Cutting = false;
        mb_Stirring = false;
        mb_Mixing = false;
        mb_Frying = false;

        mb_StepComplete = false;

        mc_OvenFront.sprite = mc_OvenFrontDefault;

        mc_CutLine.enabled = false;
        mc_Slider.SetActive(false);
        mc_HelpText.SetActive(false);

        mg_CutZoneStart.SetActive(false);
        mg_CutZoneEnd.SetActive(false);

        ms_PreviousPrompt = "";
        me_PreviousScheme = GameManager.GetControlScheme();
    }

    public override void Begin()
    {
        mr_CurrentRecipe.Begin();

        mf_MinigameTimer = 0f;

        PreLoadStep();
    }

    public override void SkipStep()
    {
        NextStep();
    }

    void NextStep()
    {
        mc_Slider.SetActive(false);

        mg_CutZoneStart.SetActive(false);
        mg_CutZoneEnd.SetActive(false);

        if (mr_CurrentRecipe.IncrementStep())
        {
            StartCoroutine(FinishStepCR());
        }
        else
        {
            mc_HelpText.SetActive(false);

            DeletePrompt();

            StartCoroutine(FinishStepEndCR());
        }
    }

    IEnumerator FinishStepEndCR()
    {
        float lf_Timer = 0f;

        mg_StepComplete.SetActive(true);

        while (lf_Timer < 0.8f)
        {
            lf_Timer += Time.deltaTime;

            yield return null;
        }

        mg_StepComplete.SetActive(false);

        StartCoroutine(MoveCamCREnd(PosFromLoc(CookingLocation.TableTop, -5f), 0.4f));
    }

    IEnumerator FinishStepCR()
    {
        float lf_Timer = 0f;

        mg_StepComplete.SetActive(true);

        while (lf_Timer < 0.8f)
        {
            lf_Timer += Time.deltaTime;

            yield return null;
        }

        mg_StepComplete.SetActive(false);

        PreLoadStep();
    }

    void PreLoadStep()
    {
        mr_CurrentStep = mr_CurrentRecipe.GetCurrentStep();
        UpdateLocation(mr_CurrentStep.GetStepLocation());
    }

    void LoadStep()
    {
        LoadAllObjects();
        LoadAction();
    }

    void LoadAction()
    {
        mb_Mashing = false;
        mb_Cutting = false;
        mb_Stirring = false;
        mb_Mixing = false;
        mb_Frying = false;

        //mc_HelpText.SetActive(true);
        //mc_HelpText.GetComponent<TextMeshProUGUI>().text = mr_CurrentStep.GetHelpText(0);

        SetPrompt(mr_CurrentStep.GetHelpText(0), mr_CurrentStep.GetHelpText(1), mr_CurrentStep.GetHelpIcon(0), mr_CurrentStep.GetHelpIcon(1));

        if (mr_CurrentStep.GetStepType() == CookingType.Slicing || mr_CurrentStep.GetStepType() == CookingType.Mashing)
        {
            mb_Mashing = true;
            mf_MashLevel = 0f;

            mc_Slider.SetActive(true);
            mc_Slider.GetComponent<Slider>().value = 0;
        }
        else if (mr_CurrentStep.GetStepType() == CookingType.Cutting)
        {
            mb_Cutting = true;

            mg_CutZoneStart.SetActive(true);
            mg_CutZoneEnd.SetActive(true);

            mg_CutZoneStart.transform.position = new Vector3((mr_CurrentStep.GetCutZoneOffset(false).x) * Screen.width, (mr_CurrentStep.GetCutZoneOffset(false).y) * Screen.height, 0f);
            mg_CutZoneEnd.transform.position = new Vector3((mr_CurrentStep.GetCutZoneOffset(true).x) * Screen.width, (mr_CurrentStep.GetCutZoneOffset(true).y) * Screen.height, 0f);
        }
        else if (mr_CurrentStep.GetStepType() == CookingType.Stir)
        {
            mb_Stirring = true;
            mf_StirLevel = 0f;

            mb_GamepadStirring = false;
            mc_Slider.SetActive(true);
            mc_Slider.GetComponent<Slider>().value = 0;
        }
        else if (mr_CurrentStep.GetStepType() == CookingType.Mixing)
        {
            mb_Mixing = true;
            m3_MousePos = mc_Cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height * 0.2f, 0f));

            Vector2 l2_WarpPos = new Vector2((Screen.width * 0.2f), (Screen.height * 0.2f));
            Mouse.current.WarpCursorPosition(l2_WarpPos);
            InputState.Change(Mouse.current.position, l2_WarpPos);
        }
        else if (mr_CurrentStep.GetStepType() == CookingType.Fry)
        {
            mb_Frying = true;
            mf_FryLevel = 0f;
            mb_GamepadFrying = false;

            mc_Slider.SetActive(true);
            mc_Slider.GetComponent<Slider>().value = 0;
        }
    }

    int GetSide(Vector2 a2_Dir)
    {
        if (a2_Dir.x > 0 && a2_Dir.y > 0)
        { 
            return 0; // top right
        }
        else if (a2_Dir.x > 0)
        {
            return 1; // bottom right
        }
        else if (a2_Dir.y > 0)
        {
            return 3; // top left
        }

        return 2; // bottom left
    }

    private void Update()
    {
        if (GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            if (me_PreviousScheme != GameManager.GetControlScheme() && (mb_Frying || mb_Cutting || mb_Mashing || mb_Mixing || mb_Stirring))
            {
                SetPrompt(mr_CurrentStep.GetHelpText(0), mr_CurrentStep.GetHelpText(1), mr_CurrentStep.GetHelpIcon(0), mr_CurrentStep.GetHelpIcon(1));
            }
            
            mf_MinigameTimer += Time.deltaTime;

            if (mb_Frying)
            {
                if (GameManager.GetControlScheme() == ControlScheme.Keyboard)
                {
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        mf_FryLevelSiz = 0f;
                    }

                    if (Mouse.current.leftButton.wasReleasedThisFrame)
                    {
                        mf_FryLevel = 0f;
                    }         
                }

                if (Mouse.current.leftButton.isPressed || mb_GamepadFrying)
                {
                    mf_FryLevel += Time.deltaTime;

                    if (mf_FryLevel > mf_FryLevelSiz)
                    {
                        mf_FryLevelSiz += 0.15f;
                        StartCoroutine(ShakeAnim());
                    }

                    if (mf_FryLevel > 3f)
                    {
                        mb_Frying = false;
                        mb_StepComplete = true;

                        StartCoroutine(PulseAnim());
                        mr_SOObjects[mi_CurrentObject].mb_IsFried = true;
                        mr_Objects[mi_CurrentObject].GetComponent<SpriteRenderer>().sprite = mr_SOObjects[mi_CurrentObject].GetSprite();
                    }
                }

                mc_Slider.GetComponent<Slider>().value = mf_FryLevel / 3f;
            }

            if (mb_Mixing)
            {
                if (GameManager.GetControlScheme() == ControlScheme.Keyboard)
                {
                    m3_MousePos = mc_Cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                }

                m3_MousePos.z = 0f;

                mr_Objects[mi_CurrentObject].transform.position = m3_MousePos;
                
                Vector2 l2_Pos = (m3_MousePos - mc_Cam.transform.position);

                if (l2_Pos.magnitude < 2f)
                {
                    mb_Mixing = false;
                    mb_StepComplete = true;

                    mr_Objects[mi_CurrentObject].transform.position = PosFromLoc(mr_CurrentStep.GetStepLocation(), 0f);

                    StartCoroutine(ShrinkAnim());

                    if (mr_CurrentStep.mb_AddFire)
                    {
                        mc_OvenFront.sprite = mc_OvenFrontFire;
                    }
                    
                    if (mr_CurrentStep.mf_SoupLevel > 0f)
                    {
                        mr_Objects[mi_CurrentObjectOther].GetComponent<SpriteRenderer>().sprite = mr_SOObjects[mi_CurrentObjectOther].GetSprite(mr_CurrentStep.mf_SoupLevel / 8f);
                    }
                }
            }

            if (mb_Stirring)
            {
                if (GameManager.GetControlScheme() == ControlScheme.Keyboard)
                {
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        Vector2 l2_RotVec = (mc_Cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - mc_Cam.transform.position).normalized;

                        mi_CurrentSide = GetSide(l2_RotVec);
                    }

                    if (Mouse.current.leftButton.isPressed)
                    {
                        Vector2 l2_RotVec = (mc_Cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - mc_Cam.transform.position).normalized;

                        int li_NewSide = GetSide(l2_RotVec);

                        if (mi_CurrentSide != li_NewSide)
                        {
                            if (mi_CurrentSide < li_NewSide || (mi_CurrentSide == 3 && li_NewSide == 0))
                            {
                                mf_StirLevel++;

                                StartCoroutine(RotateAnim());

                                StartCoroutine(PulseAnim());
                            }

                            mi_CurrentSide = li_NewSide;
                        }
                    }

                    if (Mouse.current.leftButton.wasReleasedThisFrame)
                    {
                        mf_StirLevel = 0f;
                    }
                }

                if (mf_StirLevel > 6f)
                {
                    mb_Stirring = false;
                    mb_StepComplete = true;

                    if (mr_CurrentStep.mb_IsLastStir == true)
                    {
                        mr_Objects[mi_CurrentObject].GetComponent<SpriteRenderer>().sprite = mr_SOObjects[mi_CurrentObjectOther].GetSprite(1);
                    }
                }

                mc_Slider.GetComponent<Slider>().value = mf_StirLevel / 6f;
            }

            if (mb_Cutting)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.GetControlScheme() == ControlScheme.Keyboard)
                {
                    m2_CutStart = mc_Cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                    mc_CutLine.SetPosition(0, new Vector3(m2_CutStart.x, m2_CutStart.y, -1));

                    mc_CutLine.SetPosition(1, new Vector3(m2_CutStart.x, m2_CutStart.y, -1));

                    mc_CutLine.enabled = true;
                }

                if (Mouse.current.leftButton.isPressed && GameManager.GetControlScheme() == ControlScheme.Keyboard)
                {
                    m2_CutEnd = mc_Cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                    mc_CutLine.SetPosition(1, new Vector3(m2_CutEnd.x, m2_CutEnd.y, -1));
                }

                if (mb_GamepadCutting)
                {
                    m2_CutEnd = m3_MousePos;

                    mc_CutLine.SetPosition(1, new Vector3(m2_CutEnd.x, m2_CutEnd.y, -1));
                }

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    m2_CutEnd = mc_Cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                    mc_CutLine.enabled = false;

                    Vector3 l3_CutZoneStart = mc_Cam.ScreenToWorldPoint(mg_CutZoneStart.transform.position);
                    Vector3 l3_CutZoneEnd = mc_Cam.ScreenToWorldPoint(mg_CutZoneEnd.transform.position);

                    Vector2 l2_CutZoneStart = new Vector2(l3_CutZoneStart.x, l3_CutZoneStart.y);
                    Vector2 l2_CutZoneEnd = new Vector2(l3_CutZoneEnd.x, l3_CutZoneEnd.y);

                    if ((m2_CutStart - l2_CutZoneStart).magnitude < 1f && (m2_CutEnd - l2_CutZoneEnd).magnitude < 1f)
                    {
                        mb_Cutting = false;
                        mb_StepComplete = true;

                        StartCoroutine(PulseAnim());

                        if (mr_SOObjects[mi_CurrentObject].mb_IsCut == true)
                        {
                            mr_SOObjects[mi_CurrentObject].mb_IsCut2 = true;
                        }
                        else
                        {
                            mr_SOObjects[mi_CurrentObject].mb_IsCut = true;
                        }
                        mr_Objects[mi_CurrentObject].GetComponent<SpriteRenderer>().sprite = mr_SOObjects[mi_CurrentObject].GetSprite();
                    }
                }

                mg_CutZoneStart.transform.Rotate(0, 0, 45f * Time.deltaTime, Space.Self);
                mg_CutZoneEnd.transform.Rotate(0, 0, 45f * Time.deltaTime, Space.Self);
            }

            if (mb_StepComplete)
            {
                NextStep();
                mb_StepComplete = false;
            }
        }
    }

    private void OnFry(InputValue ar_InputValue)
    {
        if (GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            if (GameManager.GetControlScheme() == ControlScheme.Gamepad)
            {
                if (mb_Frying)
                {
                    mf_FryLevelSiz = 0f;
                    mb_GamepadFrying = true;
                }

                if (mb_Cutting)
                {
                    mb_GamepadCutting = true;
                    m3_MousePos = mc_Cam.ScreenToWorldPoint(mg_CutZoneStart.transform.position);

                    m2_CutStart = mc_Cam.ScreenToWorldPoint(mg_CutZoneStart.transform.position);

                    mc_CutLine.SetPosition(0, new Vector3(m2_CutStart.x, m2_CutStart.y, -1));

                    mc_CutLine.SetPosition(1, new Vector3(m2_CutStart.x, m2_CutStart.y, -1));

                    mc_CutLine.enabled = true;
                }
            }
        }
    }

    private void OnFryStop(InputValue ar_InputValue)
    {
        if (GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            if (GameManager.GetControlScheme() == ControlScheme.Gamepad)
            {
                if (mb_Frying)
                {
                    mb_GamepadFrying = false;
                    mf_FryLevel = 0f;
                }

                if (mb_Cutting)
                {
                    mb_GamepadCutting = false;

                    m2_CutEnd = m3_MousePos;

                    m3_MousePos.z = 0f;

                    mc_CutLine.enabled = false;

                    Vector3 l3_CutZoneStart = mc_Cam.ScreenToWorldPoint(mg_CutZoneStart.transform.position);
                    Vector3 l3_CutZoneEnd = mc_Cam.ScreenToWorldPoint(mg_CutZoneEnd.transform.position);

                    Vector2 l2_CutZoneStart = new Vector2(l3_CutZoneStart.x, l3_CutZoneStart.y);
                    Vector2 l2_CutZoneEnd = new Vector2(l3_CutZoneEnd.x, l3_CutZoneEnd.y);

                    if ((m2_CutStart - l2_CutZoneStart).magnitude < 1f && (m2_CutEnd - l2_CutZoneEnd).magnitude < 1f)
                    {
                        mb_Cutting = false;
                        mb_StepComplete = true;

                        StartCoroutine(PulseAnim());
                        if (mr_SOObjects[mi_CurrentObject].mb_IsCut == true)
                        {
                            mr_SOObjects[mi_CurrentObject].mb_IsCut2 = true;
                        }
                        else
                        {
                            mr_SOObjects[mi_CurrentObject].mb_IsCut = true;
                        }
                        mr_Objects[mi_CurrentObject].GetComponent<SpriteRenderer>().sprite = mr_SOObjects[mi_CurrentObject].GetSprite();
                    }
                }
            }
        }

    }

    private void OnMash(InputValue ar_InputValue)
    {
        if (GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            if (mb_Mashing)
            {
                mf_MashLevel += 10f;

                StartCoroutine(PulseAnim());

                mr_Objects[mi_CurrentObject].GetComponent<SpriteRenderer>().sprite = mr_SOObjects[mi_CurrentObject].GetSprite(mf_MashLevel / 100f);

                mc_Slider.GetComponent<Slider>().value = mf_MashLevel / 100f;

                if (mf_MashLevel >= 100f)
                {
                    mb_Mashing = false;
                    mb_StepComplete = true;

                    mr_SOObjects[mi_CurrentObject].mb_IsMashed = true;
                    mr_Objects[mi_CurrentObject].GetComponent<SpriteRenderer>().sprite = mr_SOObjects[mi_CurrentObject].GetSprite();
                }
            }
        }
    }
    
    private void OnJoystick(InputValue ar_InputValue)
    {
        if (GameManager.GetControlScheme() == ControlScheme.Gamepad && GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            Vector2 l2_InputVector = ar_InputValue.Get<Vector2>();

            if (mb_Mixing || mb_Cutting)
            {
                m3_MousePos += new Vector3(l2_InputVector.x / 3f, l2_InputVector.y / 3f, 0f);

                m3_MousePos.x = Mathf.Clamp(m3_MousePos.x, mc_Cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x, mc_Cam.ScreenToWorldPoint(new Vector3(Screen.width * 1f, Screen.height * 1f, 0f)).x);
                m3_MousePos.y = Mathf.Clamp(m3_MousePos.y, mc_Cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y, mc_Cam.ScreenToWorldPoint(new Vector3(Screen.width * 1f, Screen.height * 1f, 0f)).y);
            }

            if (mb_Stirring)
            {
                if (l2_InputVector.magnitude > 0.1f && !mb_GamepadStirring)
                {
                    Vector2 l2_RotVec = l2_InputVector.normalized;

                    mi_CurrentSide = GetSide(l2_RotVec);

                    mb_GamepadStirring = true;
                }

                if (l2_InputVector.magnitude > 0.1f && mb_GamepadStirring)
                {
                    Vector2 l2_RotVec = l2_InputVector.normalized;

                    int li_NewSide = GetSide(l2_RotVec);

                    if (mi_CurrentSide != li_NewSide)
                    {
                        if (mi_CurrentSide < li_NewSide || (mi_CurrentSide == 3 && li_NewSide == 0))
                        {
                            mf_StirLevel++;

                            StartCoroutine(RotateAnim());

                            StartCoroutine(PulseAnim());
                        }

                        mi_CurrentSide = li_NewSide;
                    }
                }

                if (l2_InputVector.magnitude <= 0.1f)
                {
                    mb_GamepadStirring = false;
                    mf_StirLevel = 0f;
                }
            }
        }
    }

    IEnumerator ShakeAnim()
    {
        float lf_Timer = 0f;
        float lf_Time = 0.1f;

        Vector3 l3_Pos = mr_Objects[mi_CurrentObject].transform.localPosition;

        while (lf_Timer < lf_Time)
        {
            lf_Timer += Time.deltaTime;

            mr_Objects[mi_CurrentObject].transform.localPosition = l3_Pos + new Vector3(Mathf.Sin((lf_Time/ lf_Timer) * 0.05f), 0, 0);

            yield return null;
        }

        mr_Objects[mi_CurrentObject].transform.localPosition = l3_Pos;
    }

    IEnumerator ShrinkAnim()
    {
        float lf_Timer = 0f;
        float lf_Time = 0.3f;

        float lf_Size = mr_SOObjects[mi_CurrentObject].GetDefaultSize();

        while (lf_Timer < lf_Time)
        {
            lf_Timer += Time.deltaTime;

            mr_Objects[mi_CurrentObject].transform.localScale = new Vector3(lf_Size - (lf_Size * (lf_Timer / lf_Time)), lf_Size - (lf_Size * (lf_Timer / lf_Time)), 1);

            mr_Objects[mi_CurrentObject].transform.Rotate(0f, 0f, 360f * 1/0.3f * Time.deltaTime, Space.Self);

            yield return null;
        }

        mr_Objects[mi_CurrentObject].transform.localScale = new Vector3(0, 0, 1);
    }

    IEnumerator PulseAnim()
    {
        float lf_Timer = 0f;
        float lf_Time = 0.1f;

        float lf_Size = mr_SOObjects[mi_CurrentObject].GetDefaultSize();

        while (lf_Timer < lf_Time)
        {
            lf_Timer += Time.deltaTime;

            mr_Objects[mi_CurrentObject].transform.localScale = new Vector3(lf_Size - 0.4f + (0.4f * (lf_Timer/lf_Time)), lf_Size - 0.4f + (0.4f * (lf_Timer / lf_Time)), 1);

            yield return null;
        }

        mr_Objects[mi_CurrentObject].transform.localScale = new Vector3(lf_Size, lf_Size, 1);
    }

    IEnumerator RotateAnim()
    {
        float lf_Timer = 0f;
        float lf_Time = 0.1f;
        float lf_AmountRotated = 0f;

        while (lf_Timer < lf_Time)
        {
            lf_Timer += Time.deltaTime;

            mr_Objects[mi_CurrentObject].transform.Rotate(0f, 0f, -((lf_Timer / lf_Time) * 90f), Space.Self);

            lf_AmountRotated += (lf_Timer / lf_Time) * 90f;

            yield return null;
        }

        mr_Objects[mi_CurrentObject].transform.Rotate(0f, 0f, -(90f - lf_AmountRotated), Space.Self);
    }

    private void LoadAllObjects()
    {
        foreach (GameObject go in mr_Objects)
        {
            go.SetActive(false);
        }

        MinigameObject lr_Object = mr_CurrentStep.GetObject();

        LoadObject(lr_Object, true);

        if (mr_CurrentStep.GetExtra() != null)
        {
            LoadObject(mr_CurrentStep.GetExtra(), false);
        }

        if (mr_CurrentStep.GetExtra2() != null)
        {
            LoadObject(mr_CurrentStep.GetExtra2(), false, true);
        }
    }

    private void LoadObject(MinigameObject ar_Object, bool ab_IsMain, bool ab_IsExtra = false)
    {
        int li_Counter = 0;

        foreach (MinigameObject mgo in mr_SOObjects)
        {
            if (ar_Object == mgo)
            {

                mr_Objects[li_Counter].SetActive(true);
                mr_Objects[li_Counter].transform.position = PosFromLoc(mr_CurrentStep.GetStepLocation(), 0f) + new Vector3(mr_SOObjects[li_Counter].m2_ScreenOffset.x, mr_SOObjects[li_Counter].m2_ScreenOffset.y, 0);
                if (ab_IsMain)
                    mi_CurrentObject = li_Counter;

                return;
            }

            li_Counter++;
        }

        mr_SOObjects.Add(ar_Object);
        
        ar_Object.Reset();

        if (ab_IsMain)
            mi_CurrentObject = li_Counter;

        if (ab_IsExtra)
            mi_CurrentObjectOther = li_Counter;

        NewObject(ar_Object.GetSprite(), ar_Object.mi_Layer, PosFromLoc(mr_CurrentStep.GetStepLocation(), 0f) + new Vector3(ar_Object.m2_ScreenOffset.x, ar_Object.m2_ScreenOffset.y, 0), ar_Object.GetDefaultSize());
    }

    void NewObject(Sprite ac_Sprite, int ai_SortLayer, Vector3 a3_Pos, float af_Size)
    {
        GameObject lg_Object = Instantiate(mg_BlankObject);
        lg_Object.GetComponent<SpriteRenderer>().sprite = ac_Sprite;
        lg_Object.transform.parent = transform;
        lg_Object.transform.position = a3_Pos;
        lg_Object.transform.localScale = new Vector3(af_Size, af_Size, 1);
        lg_Object.GetComponent<SpriteRenderer>().sortingOrder = ai_SortLayer;

        mr_Objects.Add(lg_Object);
    }

    private void UpdateLocation(CookingLocation ae_Location)
    {
        float lf_MoveTime = 0.4f;
        StartCoroutine(MoveCamCR(PosFromLoc(ae_Location, -5f), lf_MoveTime));
    }

    private Vector3 PosFromLoc(CookingLocation ae_Loc, float af_Z)
    {
        switch(ae_Loc)
        {
            case CookingLocation.TableTop:
                return new Vector3(-6f, 0.25f, af_Z);
            case CookingLocation.OvenFront:
                return new Vector3(14.5f, -8f, af_Z);
            case CookingLocation.StoveTop:
                return new Vector3(14.5f, 0.25f, af_Z);
            case CookingLocation.CuttingBoard:
                return new Vector3(34f, 0.25f, af_Z);
            default:
                return new Vector3(-6f, 0.25f, af_Z);
        }
    }

    IEnumerator MoveCamCR(Vector3 a3_EndPos, float af_Time)
    {
        Vector3 l3_StartPos = mc_Cam.transform.position;

        float lf_Timer = 0f;

        while (lf_Timer < af_Time)
        {
            lf_Timer += Time.deltaTime;

            mc_Cam.transform.position = Vector3.Lerp(l3_StartPos, a3_EndPos, lf_Timer / af_Time);

            yield return null;
        }

        LoadStep();
    }

    IEnumerator MoveCamCREnd(Vector3 a3_EndPos, float af_Time)
    {
        Vector3 l3_StartPos = mc_Cam.transform.position;

        float lf_Timer = 0f;

        while (lf_Timer < af_Time)
        {
            lf_Timer += Time.deltaTime;

            mc_Cam.transform.position = Vector3.Lerp(l3_StartPos, a3_EndPos, lf_Timer / af_Time);

            yield return null;
        }

        Vector3 l3_FinalPos = mc_Cam.transform.position;

        l3_FinalPos.z += 5f;
        l3_FinalPos.y -= 1.5f;
        l3_FinalPos.x -= 0.5f;

        NewObject(mr_CurrentRecipe.GetFinal(), 5, l3_FinalPos, 2.5f);

        l3_FinalPos.x += 4f;
        l3_FinalPos.y -= 1.5f;

        if (mr_CurrentRecipe.GetFinalExtra() != null)
        {
            NewObject(mr_CurrentRecipe.GetFinalExtra(), 5, l3_FinalPos, 2.5f);
        }

        mr_MinigameManRef.MinigameComplete(2f - (mf_MinigameTimer / 35f));
    }

    void SetPrompt(string as_TextKeyboard, string as_TextGamepad, InteractionControl ae_ControlKeyboard, InteractionControl ae_ControlGamepad)
    {
        if (ms_PreviousPrompt != as_TextKeyboard)
        {
            if (mg_IntProRef != null)
            {
                mr_IntProManRef.mg_InteractionPrompts.Remove(mg_IntProRef);
                Destroy(mg_IntProRef);
                mg_IntProRef = null;
            }
        }

        if (me_PreviousScheme != GameManager.GetControlScheme())
        {
            if (mg_IntProRef != null)
            {
                mr_IntProManRef.mg_InteractionPrompts.Remove(mg_IntProRef);
                Destroy(mg_IntProRef);
                mg_IntProRef = null;
            }
        }

        if (mg_IntProRef == null)
        {
            if (GameManager.GetControlScheme() == ControlScheme.Keyboard)
            {
                mg_IntProRef = mr_IntProManRef.AddInteractionPrompt(as_TextKeyboard, ae_ControlKeyboard);
            }
            else
            {
                mg_IntProRef = mr_IntProManRef.AddInteractionPrompt(as_TextGamepad, ae_ControlGamepad);
            }
            mr_IntProManRef.mg_InteractionPrompts.Add(mg_IntProRef);
            ms_PreviousPrompt = as_TextKeyboard;
            me_PreviousScheme = GameManager.GetControlScheme();
        }
    }

    void DeletePrompt()
    {
        if (mg_IntProRef != null)
        {
            mr_IntProManRef.mg_InteractionPrompts.Remove(mg_IntProRef);
            Destroy(mg_IntProRef);
        }
    }
}
