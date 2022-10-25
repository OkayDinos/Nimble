using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class OutlineController : MonoBehaviour
{
    // Reference to the shader material defined in the next section
    [SerializeField] Material mc_OutlineMaterial;
    [SerializeField] float mf_OutlineSize = 1f;

    bool mb_IsActive;

    private List<Material> mc_OutlineMats = new List<Material>();
    private List<SpriteRenderer> mc_OutlineSpriteRenderers = new List<SpriteRenderer>();

    void Start()
    {
        mb_IsActive = true;
        foreach (var s in GetComponentsInChildren<SpriteRenderer>())
        {
            AddOutline(s);
        }

        SetOutline(false);
    }

    //void OnMouseEnter()
    //{
    //    StartCoroutine(Animate(
    //        (m, progress) => m.SetFloat("_Alpha", progress)));
    //}

    //void OnMouseExit()
    //{
    //    StartCoroutine(Animate(
    //        (m, progress) => m.SetFloat("_Alpha", 1 - progress)));
    //}

    public void SetOutline(bool ab_Activate)
    {
        foreach (SpriteRenderer ac_SpriteRenderer in mc_OutlineSpriteRenderers)
        {
            ac_SpriteRenderer.enabled = ab_Activate;
        }

        //if (!ab_Activate && mb_IsActive)
        //{
        //    FadeAnim(ab_Activate);
        //}
        //else if (ab_Activate && !mb_IsActive)
        //{
        //    FadeAnim(ab_Activate);
        //}
        mb_IsActive = ab_Activate;
    }

    async void FadeAnim(bool ab_Activate)
    {
        float lf_Time = 0.4f;
        float lf_End = 0f, lf_Start = 1f, lf_Timer = lf_Time;
        if (ab_Activate)
        {
            lf_End = 1f;
            lf_Start = 0f;
        }

        while (lf_Timer > 0)
        {
            lf_Timer -= Time.deltaTime;
            float lf_Progress = lf_Timer / lf_Time;
            foreach (var m in mc_OutlineMats)
            {
                m.SetFloat("_Alpha", Mathf.Lerp(lf_Start, lf_End, lf_Progress));
            }
            await Task.Yield();
        }
    }

    //private IEnumerator Animate(Action<Material, float> updateAction)
    //{
    //    for (int i = 0; i < 20; i++)
    //    {
    //        var progress = Mathf.SmoothStep(0f, 1f, (i + 1) / 20f);
    //        foreach (var m in attachedMaterials)
    //        {
    //            updateAction(m, progress);
    //        }
    //        yield return new WaitForSeconds(0.02f);
    //    }
    //}

    private void AddOutline(SpriteRenderer sprite)
    {
        var width = sprite.bounds.size.x;
        var height = sprite.bounds.size.y;

        var widthScale = 1 / width;
        var heightScale = 1 / height;
        
        // Add child object with sprite renderer
        var outline = new GameObject("Outline");
        outline.transform.parent = sprite.gameObject.transform;
        outline.transform.localScale = new Vector3(1f, 1f, 1f);
        outline.transform.localPosition = new Vector3(0f, 0f, 0f);
        outline.transform.localRotation = Quaternion.identity;
        var outlineSprite = outline.AddComponent<SpriteRenderer>();
        if (sprite.gameObject.TryGetComponent(out SpriteSkin mb_HasSkin))
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            SpriteSkin lc_OldSpriteSkin = sprite.gameObject.GetComponent<SpriteSkin>();
            FieldInfo[] mc_Fields = lc_OldSpriteSkin.GetType().GetFields(flags);
            SpriteSkin lc_NewSpriteSkin = outlineSprite.gameObject.AddComponent<SpriteSkin>();
            foreach (FieldInfo mc_Field in mc_Fields)
            {
                mc_Field.SetValue(lc_NewSpriteSkin, mc_Field.GetValue(lc_OldSpriteSkin));
            }
        }
        outlineSprite.sprite = sprite.sprite;
        outlineSprite.sortingLayerID = sprite.sortingLayerID;
        outlineSprite.material = mc_OutlineMaterial;
        // The UV coordinates of the texture is always from 0..1 no matter
        // what the aspect ratio is so we need to specify both the
        // horizontal and vertical size of the outline
        //outlineSprite.material.SetFloat(
        //    "_HSize", 0.03f * widthScale * mf_OutlineSize);
        //outlineSprite.material.SetFloat(
        //    "_VSize", 0.03f * heightScale * mf_OutlineSize);
        outlineSprite.sortingOrder = -10;
        mc_OutlineMats.Add(outlineSprite.material);
        mc_OutlineSpriteRenderers.Add(outlineSprite);

        outline.SetActive(false);
        outline.SetActive(true);
    }
}