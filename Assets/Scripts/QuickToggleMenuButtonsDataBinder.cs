﻿using UnityEngine;
using System.Collections;

public class QuickToggleMenuButtonsDataBinder : MonoBehaviour
{

    public enum PropertiesToToggle
    {
        Protein, Ribbons, AlphaTrace, Water, Ions, HBond, Ligand
    }

    private AddAtoms addAtoms;
    public PropertiesToToggle propertyToggled;
    public GameObject TextLabel;
    private UnityEngine.UI.Text Text;
    private Color LabelOnColor = Color.white;
    private Color LabelOffColor = Color.red;

    void Start()
    {
        Text = GetComponent<UnityEngine.UI.Text>();
        addAtoms = transform.root.GetComponent<AddAtoms>();
    }

    void OnTriggerEnter(Collider col)
    {
        bool current = GetCurrentData();
        bool new_value = !current;
        setDataModel(new_value);
        if (new_value)
        {
            Text.color = LabelOnColor;
        } else
        {
            Text.color = LabelOffColor;
        }
    }

    public bool GetCurrentData()
    {
        bool value = false;

        switch (propertyToggled)
        {
            case PropertiesToToggle.Protein:
                value = addAtoms.proteinShowing;
                break;

            case PropertiesToToggle.Ribbons:
                value = addAtoms.ribbonShowing;
                break;

            case PropertiesToToggle.AlphaTrace:
                value = addAtoms.alphaShowing;
                break;

            case PropertiesToToggle.Water:
                value = addAtoms.waterShowing;
                break;

            case PropertiesToToggle.Ions:
                value = addAtoms.ionsShowing;
                break;

            case PropertiesToToggle.HBond:
                value = addAtoms.HBondShowing;
                break;

            case PropertiesToToggle.Ligand:
                value = addAtoms.hetatmStick || addAtoms.hetatmBS;
                break;
        }
        return value;
    }

    protected void setDataModel(bool value)
    {

        switch (propertyToggled)
        {
            case PropertiesToToggle.Protein:
                if (addAtoms.proteinShowing)
                {
                    addAtoms.proteinShowing = addAtoms.resetProtein("bonds");
                    if (addAtoms.ballAndStickShowing)
                        addAtoms.resetProtein("balls");
                }
                else {
                    addAtoms.proteinShowing = addAtoms.showMode("bonds");
                    if (addAtoms.ballAndStickShowing)
                        addAtoms.showMode("balls");
                }
                break;

            case PropertiesToToggle.Ribbons:
                if (addAtoms.alphaShowing)
                {
                    addAtoms.alphaShowing = addAtoms.resetProtein("alpha");
                }
                if (addAtoms.ribbonShowing)
                    addAtoms.ribbonShowing = addAtoms.resetProtein("ribbons");
                else
                    addAtoms.ribbonShowing = addAtoms.showMode("ribbons");
                break;

            case PropertiesToToggle.AlphaTrace:
                if (addAtoms.ribbonShowing)
                {
                    addAtoms.ribbonShowing = addAtoms.resetProtein("ribbons");
                }
                if (addAtoms.alphaShowing)
                    addAtoms.alphaShowing = addAtoms.resetProtein("alpha");
                else
                    addAtoms.alphaShowing = addAtoms.showMode("alpha");
                break;

            case PropertiesToToggle.Water:
                if (addAtoms.waterShowing)
                    addAtoms.waterShowing = addAtoms.resetProtein("water");
                else
                    addAtoms.waterShowing = addAtoms.showMode("water");
                break;

            case PropertiesToToggle.Ions:
                if (addAtoms.ionsShowing)
                    addAtoms.ionsShowing = addAtoms.resetProtein("ion");
                else
                    addAtoms.ionsShowing = addAtoms.showMode("ion");
                break;

            case PropertiesToToggle.HBond:
                if (addAtoms.HBondShowing)
                    addAtoms.HBondShowing = addAtoms.resetProtein("hbond");
                else
                    addAtoms.HBondShowing = addAtoms.showMode("hbond");
                break;

            case PropertiesToToggle.Ligand:
                if (addAtoms.hetatmStick || addAtoms.hetatmBS)
                {
                    if (PlayerPrefs.GetString("hetatmBS") == "True")
                    {
                        addAtoms.hetatmBS = addAtoms.resetProtein("hetatmbs");
                    }
                    else if (addAtoms.sphere)
                    {
                        addAtoms.hetatmBS = addAtoms.resetProtein("hetatmbs");
                    }
                    addAtoms.hetatmStick = addAtoms.resetProtein("hetatms");
                }
                else
                {
                    if (PlayerPrefs.GetString("hetatmBS") == "True")
                    {
                        addAtoms.hetatmBS = addAtoms.showMode("hetatmbs");
                    }
                    else if (addAtoms.sphere)
                    {
                        addAtoms.hetatmBS = addAtoms.showMode("hetatmbs");
                    }
                    addAtoms.hetatmStick = addAtoms.showMode("hetatms");
                }
                break;
        }


    }

}
