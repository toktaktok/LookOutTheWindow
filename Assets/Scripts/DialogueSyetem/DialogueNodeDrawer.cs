using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;
using System.Linq;
using EnumTypes;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeDrawer : NodeEditor
{
    private VillagerName _villagerName;
    
    private DialogueNode dialogueNode;

    private bool showEntryNode = true;
    
    private bool showNodeSettings = false;
    
    private bool showDialogueSettings = true;
    
    private string newDialogueOption = "";
     
    private string newDialogueOptionOutput = "";
    
    private int currentNodeTab = 0;

    private int nodePortToDelete = 0;

    private string testDialogueText; //manager 통해서 대사 받을 수 있는가?
    
    // private int dialogueId = 0;


    
    public override void OnBodyGUI()//Dialogue에 적용될 node ui 설정.

    {
        if (dialogueNode == null)
        {
            dialogueNode = target as DialogueNode;
        } 
        serializedObject.Update();

        //entry 노드를 토글한다.
        if (showEntryNode)
        {
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("entry")) ;
        }
        //exit 노드
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("exit"));
        
        //버튼 누를 시 showEntryNode toggle
        if (GUILayout.Button("Toggle Entry Node"))
        {
            showEntryNode = !showEntryNode;
        }
        
        //기본 npc 상태 -> 선택 옵션 윈도우를 띄울 것인지, 아닌지
        Color prev = GUI.backgroundColor;

        GUI.backgroundColor = (dialogueNode.IsBasicState()) ? Color.cyan : prev;
            
        if (GUILayout.Button("Basic statement"))
        {
            dialogueNode.ToggleBasicState(); //누를 시 상태 toggle.
        }
        GUI.backgroundColor = prev;

        //노드 세팅(fold되어 있음. 뒤에 새 port 추가할 때 사용할 듯)
        showNodeSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showNodeSettings, "Node Settings");
        if (showNodeSettings)
        {
            currentNodeTab = GUILayout.Toolbar(currentNodeTab, new string[] { "Add Output", "Remove Output" });
            switch (currentNodeTab)
            {
                case 0:
                    //새로 추가할 output 포트 - 대사
                    EditorGUILayout.PrefixLabel("Dialogue");
                    newDialogueOption = EditorGUILayout.TextField(newDialogueOption);
                    //새로 추가할 output 포트 - 이름
                    EditorGUILayout.PrefixLabel("Output");
                    newDialogueOption = EditorGUILayout.TextField(newDialogueOptionOutput);
            
                    //포트 등록 과정. 내용이 없거나 포트 이름이 중복되는지 체크한다.
                    if (GUILayout.Button("Create new option"))
                    {
                        bool noDialogue = (newDialogueOption.Length == 0);
                        bool noDialogueOption = (newDialogueOptionOutput.Length == 0);

                        if (noDialogue)
                        {
                            EditorUtility.DisplayDialog("Error creating port", "no dialogues entered.", "ok");
                            return;
                        }
                        if (noDialogueOption)
                        {
                            EditorUtility.DisplayDialog("Error creating port", "no port was specified.", "ok");
                            return; 
                        }

                        bool matchesExistingOutput = false;
                        foreach (NodePort p in dialogueNode.DynamicOutputs)
                        {
                            if (p.fieldName == newDialogueOptionOutput)
                            {
                                matchesExistingOutput = true;
                                break;
                            }
                        }

                        if (matchesExistingOutput)
                        {
                            EditorUtility.DisplayDialog("Error creating port", "The port name is already in use.", "ok");
                            return; 
                        }
                
                    }
                    dialogueNode.AddDynamicOutput(typeof(int), Node.ConnectionType.Multiple, Node.TypeConstraint.None, newDialogueOptionOutput);
                    dialogueNode.dialogueOptionList.Add(new DialogueNode.DialogueOption(newDialogueOption, newDialogueOptionOutput));
                    break;
                
                case 1:
                    if (dialogueNode.DynamicOutputs.Count() == 0)
                    {
                        EditorGUILayout.HelpBox("There is no port to delete!", MessageType.Error);
                    }

                    else
                    {
                        EditorGUILayout.PrefixLabel("Choose Port");

                        List<string> outputs = new List<string>();
                        foreach (NodePort p in dialogueNode.DynamicOutputs)
                        {
                            outputs.Add(p.fieldName);
                        }

                        nodePortToDelete = EditorGUILayout.Popup(nodePortToDelete, outputs.ToArray());

                        if (GUILayout.Button("Delete selected node"))
                        {
                            foreach (DialogueNode.DialogueOption d in dialogueNode.dialogueOptionList)
                            {
                                if (d.option == dialogueNode.DynamicOutputs.ElementAt(nodePortToDelete).fieldName)
                                {
                                    dialogueNode.dialogueOptionList.Remove(d);
                                    break;
                                }
                            }

                            dialogueNode.RemoveDynamicPort(dialogueNode.DynamicOutputs.ElementAt(nodePortToDelete));
                        }
                    }
                    break;
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //대사 세팅
        showDialogueSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showDialogueSettings, "Dialogue Settings");
        if (showDialogueSettings)
        {
            float prevWidth = EditorGUIUtility.labelWidth; //가로 크기
            EditorGUIUtility.labelWidth = 150;
        
            _villagerName = dialogueNode.CurVillagerName;
            dialogueNode.CurVillagerName = (VillagerName)EditorGUILayout.EnumPopup("현재 주민", _villagerName); //enum 체크로 주민 특정
   
            GUILayout.Space(10);
            EditorGUIUtility.labelWidth = 150;
            if (dialogueNode.IsBasicState()) // 현재 기본 대사 노드인지?
            {

            }
            else //기본 대사가 아닌 의뢰, 스토리 관련 대사 노드
            {
                dialogueNode.dialogueId = EditorGUILayout.IntField("dialogue id",dialogueNode.dialogueId);
                // dialogueId = EditorGUILayout.IntField("dialogue id",dialogueId);
            //         // GUILayout.TextField(DialogueManager.Instance.GetDialogue(dialogueId));
            //         // NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("dialogue"));
            }

            // dialogueNode.dialogueOptions = EditorGUILayout.Toggle("Show dialogue options", dialogueNode.dialogueOptions);
            // EditorGUIUtility.labelWidth = prevWidth;
            //
            // if (dialogueNode.dialogueOptions)
            // {
            //     foreach (var d in dialogueNode.dialogueOptionList)
            //     {
            //         EditorGUILayout.PrefixLabel(d.dialogue);
            //         d.dialogue = EditorGUILayout.TextField(d.dialogue);
            //         EditorGUILayout.TextField(d.option);
            //         EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            //     }
            //     foreach (NodePort p in dialogueNode.DynamicOutputs)
            //     {
            //         NodeEditorGUILayout.PortField(p);
            //     }
            // }
        } 
        EditorGUILayout.EndFoldoutHeaderGroup();

    }

}
