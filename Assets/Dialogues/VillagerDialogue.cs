using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using GoogleSheetsToUnity;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "VillagerDialogue", menuName = "VillagerDialogue", order = 0)]
public class VillagerDialogue : ScriptableObject
{
    [HideInInspector]
    public string associatedSheet = "11o0ElV8iRc7YUpkbGWrqCG4TbfYfJ_w4vF1_Zvb3yPc";
    [HideInInspector]
    public string associatedWorksheet = "TestBasicDialogue";

    public string villagerId;       //변하지 않음 - 주민 별 번호

    public string villagerName;
    public List<string> dialogues1 = new List<string>();
    public List<string> dialogues2 = new List<string>();

    //public List<string> Ids = new List<string>();

    internal void UpdateStats(List<GSTU_Cell> list)
    {
        dialogues1.Clear();
        dialogues2.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {

                case "VillagerName":
                    {
                        villagerName = list[i].value;
                        break;
                    }
                //case "Dialogues1":
                //    {
                //        dialogues1.Add(list[i].value.ToString());
                //        break;
                //    }
                //case "Dialogues2":
                //    {
                //        dialogues2.Add(list[i].value.ToString());
                //        break;
                //    }
            }
        }

        Debug.Log($"{villagerName} ({name}) : {dialogues1[1]}");
    }

    //internal void UpdateStats(GstuSpreadSheet ss)
    //{
    //    dialogues1.Clear();
    //    dialogues2.Clear();

    //    villagerName = ss[name, "VillagerName"].value;
    //    dialogues1.Add(ss[name, "Dialogues1"].value.ToString());
    //    dialogues2.Add(ss[name, "Dialogues2"].value.ToString());
    //}

    //internal void UpdateStats(GstuSpreadSheet ss, bool mergedCells)
    //{
    //    items.Clear();
    //    villagerId = ss[name, "VillagerId"].value;
    //    dialogues1 = ss[name, "Dialogues1"].value.Split("^");
    //    dialogues2 = ss[name, "Dialogues2"].value.Split("^");

    //    //I know that my items column may contain multiple values so we run a for loop to ensure they are all added
    //    foreach (var value in ss[name, "Items", true])
    //    {
    //        items.Add(value.value.ToString());
    //    }
    //}

}

#if UNITY_EDITOR
[CustomEditor(typeof(VillagerDialogue))]
public class VillagerDialogueEditor : Editor
{
    VillagerDialogue vd;

    void OnEnable()
    {
        vd = (VillagerDialogue)target;
       
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update"))
        {
            Debug.Log(vd.name);
            UpdateStats(UpdateMethodOne);
        }

        //if (GUILayout.Button("Pull Data With merged Cells"))
        //{
        //    UpdateStats(UpdateMethodMergedCells, true);
        //}
    }

    //GSTU_Search 객체를 생성하는 부분
    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(vd.associatedSheet, vd.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        vd.UpdateStats(ss.rows[vd.name]);
        EditorUtility.SetDirty(target);
    }

    //void UpdateMethodMergedCells(GstuSpreadSheet ss)
    //{
    //    foreach (string dataName in vd.Ids)
    //        vd.UpdateStats(ss.rows[dataName],dataName, true);

    //    EditorUtility.SetDirty(target);
    //}

}
#endif