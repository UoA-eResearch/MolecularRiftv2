﻿using UnityEngine;
using System.Collections;

public class ToggleLigandButtonDataBinder : MonoBehaviour
{

    public int LigandIndex { get; set; }
    private AddAtoms addAtoms;

    public GameObject TextLabel;
    private UnityEngine.UI.Text Text;
    public Color LabelOnColor = new Color(0, 255, 255);
    public Color LabelOffColor = new Color(0, 128, 128);

    public bool GetCurrentData()
    {
        if (addAtoms == null)
        {
            addAtoms = transform.parent.parent.gameObject.GetComponent<PopulateLigandMenu>().addAtoms;
        }

        bool value = addAtoms.LigandsShowing[LigandIndex];

        if (Text == null)
        {
            Text = TextLabel.GetComponent<UnityEngine.UI.Text>();
        }

        if (value)
        {
            Text.color = LabelOnColor;
        }
        else
        {
            Text.color = LabelOffColor;
        }

        return value;
    }

    protected void setDataModel(bool value)
    {
        addAtoms.ToggleLigandDisplay(LigandIndex, value);
    }
}
