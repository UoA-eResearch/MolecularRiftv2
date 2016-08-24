using UnityEngine;
using System.Collections;

public class ToggleMenuButtonsDataBinder : MonoBehaviour
{

    public int associatedMenu;
    private PlayerController playerController;

    public bool GetCurrentData()
    {
        if (playerController == null)
        {
            playerController = transform.parent.parent.parent.GetComponent<PlayerController>();
        }
        return associatedMenu == playerController.menuShowing;
    }

    protected void setDataModel(bool value)
    {
        if (playerController == null)
        {
            playerController = transform.parent.parent.parent.GetComponent<PlayerController>();
        }

        playerController.SwitchToMenu(associatedMenu);
    }
}
