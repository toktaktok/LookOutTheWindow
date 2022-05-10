using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GoogleSheetsToUnity.ThirdPary;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class TestData : ScriptableObject
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

    internal void UpdateStats(List<GSTU_Cell> list, string name)
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

}

[CustomEditor(typeof(TestData))]
public class DataEditor : Editor
{
    TestData vd;

    void OnEnable()
    {
        vd = (TestData)target;


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
        vd.UpdateStats(ss.rows[vd.name], vd.name);
        EditorUtility.SetDirty(target);
    }

}