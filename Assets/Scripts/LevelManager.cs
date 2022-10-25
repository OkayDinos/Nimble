// This script belongs to Okay Dinos, used for the Nimble Project.
// This script is used to track events throughout a level.
// Author(s): Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public enum LocationLayer
{
    DoveStreet = 0,
    MainStreet,
    LowerHarbour,
    HarbourWall,
    Inside,
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager mr_Instance;

    public GameObject mg_PlayerRef;

    public Player mr_PlayerRef;

    InteractionPromptManager mr_IntProManRef;

    public CameraController mc_MainCamera;

    ProgressSystemManager mr_ProgressSystemRef;

    public static List<MinigameManager> mr_MinigameMans = new List<MinigameManager>();

    public static List<Interactable> mg_Interactables = new List<Interactable>();

    List<List<GameObject>> mg_CollisionLayers = new List<List<GameObject>>();

    LocationLayer me_CurrentLayer;

    [SerializeField]
    Light2D[] mc_ExternalLights;

    GameObject mg_CurrentNestedHouse;

    public Camera mc_Camera;

    int mi_MinigameFinishesQuestID;

    bool mb_MinigameCompleted;

    public bool mb_AutoMoveNext;
    public bool mb_AutoMoveBack;

    [SerializeField]
    GameObject mg_House;

    private void Awake()
    {
        if (mr_Instance != null)
            Destroy(mr_Instance);
        mr_Instance = this;

        mg_PlayerRef = GameObject.FindGameObjectWithTag("Player");

        mr_PlayerRef = mg_PlayerRef.GetComponent<Player>();

        ItemDatabase.DestroyExistingDatabase();

        mc_Camera = Camera.main;

        GameManager.SetCurrentGameState(GameState.PlayerNormal);

        StartCoroutine(PreLoadNestedSpace());
    }

    IEnumerator PreLoadNestedSpace()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

        yield return new WaitUntil(() => asyncLoad.isDone == true);

        UpdateLayers();

        MovePlayer(LocationLayer.Inside, mg_PlayerRef.transform.position, "DoveHousesPlayer");

        GetSetNestedHouse(mg_House);

        ToggleLight(0);
        GetSetNestedHouse().GetComponentInChildren<SpriteRenderer>().sharedMaterial.SetFloat("_Opacity", 0);
    }
    private void OnDestroy()
    {
        if (mr_Instance == this)
            mr_Instance = null;
    }

    private void Start()
    {
        mr_IntProManRef = InteractionPromptManager.mr_Instance;

        me_CurrentLayer = LocationLayer.MainStreet;
        mr_ProgressSystemRef = ProgressSystemManager.mr_ProgressSystemManagerInstance;

        int li_LayerCount = transform.GetChild(1).childCount;
        for (int j = 0; j < li_LayerCount; j++)
        {
            List<GameObject> lg_CollisionLayer = new List<GameObject>();

            int li_InterCount = transform.GetChild(1).transform.GetChild(j).childCount;
            for (int i = 0; i < li_InterCount; i++)
            {
                GameObject lg_Collision = transform.GetChild(1).transform.GetChild(j).transform.GetChild(i).gameObject;

                lg_CollisionLayer.Add(lg_Collision);
            }

            mg_CollisionLayers.Add(lg_CollisionLayer);
        }

        UpdateLayers();

        StartCoroutine(DelayedUpdate());
    }

    IEnumerator DelayedUpdate() // HACK: calling UpdateLayers() in Start() is too early since not all interactables are linked, this adds a delay (also needs to be called in Start() cause collisions need updated instantly)
    {
        float lf_Timer = 0;

        if (lf_Timer < 0.5f)
        {
            lf_Timer += Time.deltaTime;
            yield return null;
        }

        UpdateLayers();
    }

    void ToggleLight(float af_LightValue)
    {
        foreach (Light2D ac_Light in mc_ExternalLights)
        {
                ac_Light.intensity = af_LightValue;
        }
    }

    public GameObject GetSetNestedHouse(GameObject ag_House = null)
    {
        if (ag_House == null)
            return mg_CurrentNestedHouse;
        else
        {
            mg_CurrentNestedHouse = ag_House;
            return mg_CurrentNestedHouse;
        }
    }

    public void QuestFinished(int ai_QuestID)
    {
        foreach (Interactable i in mg_Interactables)
        {
            i.TryUnlock(ai_QuestID);

            UpdateLayers();
        }
    }

    public IEnumerator MovePlayerMultiple(LocationLayer ae_NewLayer, List<Transform> mc_Targets, string as_NewPlayerSortingLayerName)
    {
        if (mc_Targets.Count > 1)
        {
            mb_AutoMoveBack = false;
            if (mc_Targets[0].position.z < mc_Targets[1].position.z)
            {
                mb_AutoMoveBack = true;
            }

            for (int i = 0; i < mc_Targets.Count - 1; i++)
            {
                mb_AutoMoveNext = false;
                MovePlayer(ae_NewLayer, mc_Targets[i].position, as_NewPlayerSortingLayerName, false);
                while (!mb_AutoMoveNext)
                {
                    yield return null;
                }
            }
        }
        MovePlayer(ae_NewLayer, mc_Targets[mc_Targets.Count - 1].position, as_NewPlayerSortingLayerName, true);
    }

    public void MovePlayer(LocationLayer ae_NewLayer, Vector3 a3_NewPos, string as_NewPlayerSortingLayerName, bool ab_IsLast = true)
    {
        if (ab_IsLast == true)
        {
            me_CurrentLayer = ae_NewLayer;

            UpdateLayers();

            mr_PlayerRef.SetCollision(true);
        }
        else
        {
            mr_PlayerRef.SetCollision(false);
        }
       
        mr_PlayerRef.GoToPosition(a3_NewPos, true, as_NewPlayerSortingLayerName, ab_IsLast);
    }

    public void FinishedInteracting()
    {
        mr_PlayerRef.FinishedInteracting();
    }

    public void UpdateLayers()
    {
        foreach (Interactable i in mg_Interactables)
        {
            i.Hide(!(i.GetLayer() == me_CurrentLayer));
        }

        int li_C = 0;
        foreach (List<GameObject> j in mg_CollisionLayers)
        {
            foreach (GameObject i in j)
            {
                i.gameObject.SetActive((LocationLayer)li_C == me_CurrentLayer);
            }
            li_C++;
        }
    }

    public void StartMinigame(MinigameType ae_Type)
    {
        GameManager.SetMinigame(ae_Type);
        StartCoroutine(LoadMinigame());
    }

    IEnumerator LoadMinigame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MinigameTestScene", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        StartCoroutine(MinigameWait());
    }

    IEnumerator MinigameWait()
    {
        while (mr_MinigameMans[0].mb_Leave == false)
        {
            yield return null;
        }

        EndMinigame();
    }

    public void EndMinigame()
    {
        mb_MinigameCompleted = mr_MinigameMans[0].mb_MinigameCompleted;

        StartCoroutine(UnloadMinigame());
    }

    IEnumerator UnloadMinigame()
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync("MinigameTestScene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        OnMinigameClosed();
    }

    public void SetMGFinishesQuestID(int ai_ID)
    {
        mi_MinigameFinishesQuestID = ai_ID;
    }
    
    void OnMinigameClosed()
    {
        mr_IntProManRef.ClearAllPrompts();

        FinishedInteracting();
        if (mb_MinigameCompleted)
            mr_ProgressSystemRef.CompleteQuest(mi_MinigameFinishesQuestID);
    }
}
