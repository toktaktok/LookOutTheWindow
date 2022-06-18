// using UnityEngine;
//
// [CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Dialogue Channel")]
// public class DialogueChannel : ScriptableObject
// {
//     public delegate void DialogueCallBack(Dialogue dialogue);
//
//     public DialogueCallBack OnDialogueRequested;
//     public DialogueCallBack OnDialogueStart;
//     public DialogueCallBack OnDialogueEnd;
//
//     public delegate void DialogueNodeCallBack(DialogueNode node);
//
//     public DialogueNodeCallBack OnDialogueNodeRequested;
//     public DialogueNodeCallBack OnDialogueNodeStart;
//     public DialogueNodeCallBack OnDialogueNodeEnd;
//
//     public void RaiseRequestDialogue(Dialogue dialogue)
//     {
//         OnDialogueRequested?.Invoke(dialogue);
//     }
//
//     public void RaiseDialogueStart(Dialogue dialogue)
//     {
//         OnDialogueStart?.Invoke(dialogue);
//     }
//     
//     public void RaiseDialogueEnd(Dialogue dialogue)
//     {
//         OnDialogueEnd?.Invoke(dialogue);
//     }
//
//     public void RaiseRequestDialogueNode(DialogueNode node)
//     {
//         OnDialogueNodeRequested?.Invoke(node);
//     }
//
//     public void RaiseDialogueNodeStart(DialogueNode node)
//     {
//         OnDialogueNodeStart?.Invoke(node);
//     }
//     
//     public void RaiseDialogueNodeEnd(DialogueNode node)
//     {
//         OnDialogueNodeEnd?.Invoke(node);
//     }
//     
//
// }
