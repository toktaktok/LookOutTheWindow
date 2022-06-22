using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;
using System.Linq;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeDrawer : NodeEditor
{
    private DialogueNode dialogueNode;

    private bool showEntryNode = true;
    
    private bool showNodeSettings = false;
    
    private bool showDialogueSettings = true;
    
    private string newDialogueOption = "";
     
    private string newDialogueOptionOutput = "";
    
    private int currentNodeTab = 0;

    private int nodePortToDelete = 0;

    
    public override void OnBodyGUI()
    {
        //Dialogue에 적용되는 node ui.
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
        
        
        //기본 npc 대사 -> 어떤 행동을 취할지 선택.
        Color prev = GUI.backgroundColor;

        GUI.backgroundColor = (dialogueNode.IsBasicState()) ? Color.cyan : prev;
            
        if (GUILayout.Button("Basic statement"))
        {
            dialogueNode.ToggleBasicState();
        }
        GUI.backgroundColor = prev;

        //노드 세팅(fold)
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

        showDialogueSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showDialogueSettings, "Dialogue Settings");
        
        if (showDialogueSettings)
        {
            float prevWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 150;
            
            dialogueNode.speakerName = EditorGUILayout.TextField("Speaker", dialogueNode.speakerName);
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("dialogue"));
            
            EditorGUIUtility.labelWidth = 150;

  
            
            dialogueNode.dialogueOptions = EditorGUILayout.Toggle("Show dialogue options", dialogueNode.dialogueOptions);
            EditorGUIUtility.labelWidth = prevWidth;


            
            if (dialogueNode.dialogueOptions)
            {
                foreach (DialogueNode.DialogueOption d in dialogueNode.dialogueOptionList)
                {
                    EditorGUILayout.PrefixLabel(d.dialogue);
                    d.dialogue = EditorGUILayout.TextField(d.dialogue);
                    EditorGUILayout.TextField(d.option);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                foreach (NodePort p in dialogueNode.DynamicOutputs)
                {
                    NodeEditorGUILayout.PortField(p);
                }
            }
        } 
        EditorGUILayout.EndFoldoutHeaderGroup();

    }

}
