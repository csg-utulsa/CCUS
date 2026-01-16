//This script goes on tiles that become partially transparent when the mouse hovers over them
using UnityEngine;

public class MouseHoverHideTile : MonoBehaviour
{
    public bool fadeThisTilesTransparency = true;
    private GameObject currentUIPopUp;
    public GameObject CurrentUIPopUp {
        get{
            return currentUIPopUp;
        }
        set{
            currentUIPopUp = value;
            SetVisibilityOfTileUIPopUp();
        }
    }

    private GameObject tileModel;
    private GameObject activatedTileGraphic;
    private TileMaterialHandler tileMaterialHandler;
    //public float hoverTransparency = .5f;
    public float timeToFade = .1f;
    private float percentageOfActivationGraphicLeftHide = .5f;

    public bool IsHidden{ get; set; } = false;

    public bool UIPopUpsAreHidden {get; set;} = false;

    public float popUpHiddenTransparency = .1f;

    void Awake(){
        if(GetComponent<TileMaterialHandler>() != null){
            tileMaterialHandler = GetComponent<TileMaterialHandler>();
        }
        if(GetFirstActiveMeshRenderer() != null){
            tileModel = GetFirstActiveMeshRenderer();
        }

        ActivatableBuilding activatableBuilding = GetComponent<ActivatableBuilding>();
        if(activatableBuilding != null && activatableBuilding.buildingActivatedGraphic != null){
            activatedTileGraphic = activatableBuilding.buildingActivatedGraphic;
        }
    }

    private float fadeTimer = 0f;
    private bool isFading = false;
    private float previousTransparency = 1f;
    private float targetTransparency = .5f;
    //private float currentTransparency = 1f;
    void Update(){
        if(isFading){
            fadeTimer += Time.deltaTime;
            float percentageFaded = fadeTimer/timeToFade;
            float newTransparency = previousTransparency + (percentageFaded * (targetTransparency - previousTransparency));
            if(percentageFaded < 1f){
                tileMaterialHandler.SetDitherTransparency(newTransparency);
            } else{
                tileMaterialHandler.SetDitherTransparency(targetTransparency);
                isFading = false;
                fadeTimer = 0;
                previousTransparency = targetTransparency;
            }
        }
        
        

    }

    private void FadeToTransparency(float _targetTransparency){
        if(fadeThisTilesTransparency){
            fadeTimer = 0f;
            previousTransparency =  tileMaterialHandler.GetCurrentTransparency();
            targetTransparency = _targetTransparency;
            isFading = true;
        }else{
            tileMaterialHandler.SetDitherTransparency(_targetTransparency);
        }
        
        
    }

    
    public void HideTile(float hoverTransparency){
        string tileName = GetComponent<Tile>().tileScriptableObject.Name;
        // if(activatedTileGraphic != null)
        //     activatedTileGraphic.SetActive(false);
        IsHidden = true;
        FadeToTransparency(hoverTransparency);
        if(activatedTileGraphic != null){
            SpriteRenderer[] spriteRenderers = activatedTileGraphic.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer spriteRenderer in spriteRenderers){
                Color prevColor = spriteRenderer.color;
                spriteRenderer.color = new Color(prevColor.r, prevColor.g, prevColor.b, hoverTransparency * percentageOfActivationGraphicLeftHide);
            }
        }
        
        //tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.PartiallyTransparentPlaced);
        // if(tileModel != null)
        //     tileModel.SetActive(false);
    }
    public void UnhideTile(){
        string tileName = GetComponent<Tile>().tileScriptableObject.Name;

        IsHidden = false;
        // if(GetComponent<ActivatableBuilding>() != null)
        //     GetComponent<ActivatableBuilding>().UpdateBuildingActivation();
        FadeToTransparency(1f);
        // if(tileModel != null)
        //     tileModel.SetActive(true);

        if(activatedTileGraphic != null){
            SpriteRenderer[] spriteRenderers = activatedTileGraphic.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer spriteRenderer in spriteRenderers){
                Color prevColor = spriteRenderer.color;
                spriteRenderer.color = new Color(prevColor.r, prevColor.g, prevColor.b, 1f);
            }
        }
        
        tileMaterialHandler.MaterialSet(TileMaterialHandler.matState.Placed);
    }

    public void SetVisibilityOfTileUIPopUp(){
        if(UIPopUpsAreHidden){
            HideTileUIPopUps(popUpHiddenTransparency);
        }else{
            UnHideTileUIPopUps();
        }
    }
    public void HideTileUIPopUps(float hoverTransparencyOfUIPopUps){
        UIPopUpsAreHidden = true;
        popUpHiddenTransparency = hoverTransparencyOfUIPopUps;
        if(CurrentUIPopUp == null) return;
        UIPopUpHideOnMouseOver popUpHider = CurrentUIPopUp.GetComponent<UIPopUpHideOnMouseOver>();
        if(popUpHider != null){
            popUpHider.HidePopUp(hoverTransparencyOfUIPopUps);
        }
    }
    public void UnHideTileUIPopUps(){
        UIPopUpsAreHidden = false;
        if(CurrentUIPopUp == null) return;
        UIPopUpHideOnMouseOver popUpHider = CurrentUIPopUp.GetComponent<UIPopUpHideOnMouseOver>();
        if(popUpHider != null){
            popUpHider.UnHidePopUp();
        }
    }

    public GameObject GetFirstActiveMeshRenderer(){
        MeshRenderer[] allMeshRenderersInChildren = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < allMeshRenderersInChildren.Length; i++)
        {
            if(allMeshRenderersInChildren[i].gameObject.activeSelf == true)
            {
                return allMeshRenderersInChildren[i].gameObject;
            }
        }
        return null;
        
    }
}
