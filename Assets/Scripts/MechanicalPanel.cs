using UnityEngine;

public class MechanicalPanel : MonoBehaviour
{
    public MechanicalPanel mechanicalpanel;
    public Gashpon gashpon;
    public void OnFClick()
    {
        gashpon.SpawnObject();
        mechanicalpanel.gameObject.SetActive(false);
        
    }
    public void OnEClick()
    {
        gashpon.SpawnObject();
        mechanicalpanel.gameObject.SetActive(false);
    }
}