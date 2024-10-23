using UnityEngine;
using UnityEngine.UI;

public class HouseButton : MonoBehaviour
{
    public Outline outline;

    private bool _on;

    public void OnClick()
    {
        var on = !_on;
        Hud.Inst.SetState(Hud.State.None);
        Hud.Inst.DeselectAllButtons();
        Hud.Inst.housePanel.gameObject.SetActive(on);
        _on = on;
        outline.enabled = on;
    }

    public void Deselect()
    {
        _on = false;
        outline.enabled = false;
        Hud.Inst.housePanel.gameObject.SetActive(false);
    }
}