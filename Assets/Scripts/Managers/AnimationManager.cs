using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator fadePanelAnim;
    public Animator goalPanelAnim;

    public void OK()
    {
        if (fadePanelAnim != null && goalPanelAnim != null)
        {
            // Trigger both "Out" bools to true
            fadePanelAnim.SetBool("Out", true);
            goalPanelAnim.SetBool("Out", true);
        }
    }
}
