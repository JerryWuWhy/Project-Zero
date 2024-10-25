using UnityEngine;

public class MechanicalPanel : MonoBehaviour
{
    public MechanicalPanel mechanicalpanel;
    public Gashpon gashpon;
    public MechanicalParts mechanicalparts;
    public HousePanel housepanel;
    public void OnFClick()
    {
        if (mechanicalparts.countM >= mechanicalparts.Fcost)
        {
            mechanicalparts.countM -= mechanicalparts.Fcost;
            gashpon.SpawnObject(gashpon.GetWeightedPrizeIndex());
            mechanicalpanel.gameObject.SetActive(false);
        }
        else
        {
            housepanel.log.gameObject.SetActive(true);
        }
        
    }
    public void OnEClick()
    {
        if (mechanicalparts.countM >= mechanicalparts.Ecost)
        {
            mechanicalparts.countM -= mechanicalparts.Ecost;
            gashpon.SpawnObject(gashpon.GetWeightedPrizeIndex());
            mechanicalpanel.gameObject.SetActive(false);
        }
        else
        {
            housepanel.log.gameObject.SetActive(true);
        }
    }
}