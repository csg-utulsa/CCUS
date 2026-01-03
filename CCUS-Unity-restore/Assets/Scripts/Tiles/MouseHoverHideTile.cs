using UnityEngine;

public class MouseHoverHideTile : MonoBehaviour
{
    private GameObject tileModel;
    private GameObject activatedTileGraphic;

    void Awake(){
        if(GetFirstActiveMeshRenderer() != null){
            tileModel = GetFirstActiveMeshRenderer();
        }

        ActivatableBuilding activatableBuilding = GetComponent<ActivatableBuilding>();
        if(activatableBuilding != null && activatableBuilding.buildingActivatedGraphic != null){
            activatedTileGraphic = activatableBuilding.buildingActivatedGraphic;
        }
    }
    public void HideTile(){
        if(activatedTileGraphic != null)
            activatedTileGraphic.SetActive(false);
        if(tileModel != null)
            tileModel.SetActive(false);
    }
    public void UnhideTile(){
        if(GetComponent<ActivatableBuilding>() != null)
            GetComponent<ActivatableBuilding>().UpdateBuildingActivation();
        if(tileModel != null)
            tileModel.SetActive(true);
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
