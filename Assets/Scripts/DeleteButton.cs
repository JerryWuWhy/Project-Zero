// using UnityEngine;
// using UnityEngine.UI;
//
// public class DeleteButton : MonoBehaviour
// {
//     public Outline outline;
//     private bool _on;
//
//     public void OnClick()
//     {
//         var on = !_on;
//         Hud.Inst.DeselectAllButtons();
//         Hud.Inst.SetState(on ? Hud.State.Remove : Hud.State.None);
//         _on = on;
//         outline.enabled = on;
//     }
//
//     public void Deselect()
//     {
//         _on = false;
//         outline.enabled = false;
//     }
// }