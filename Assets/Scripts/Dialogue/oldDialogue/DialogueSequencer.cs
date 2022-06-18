//
// public class DialogueException : System.Exception
// {
//     public DialogueException(string message) : base(message)
//     {
//     }
// }
//
// public class DialogueSequencer
// {
//
//     public delegate void DialogueCallback(Dialogue dialogue);
//     public delegate void DialogueNodeCallback(DialogueNode node);
//
//     public DialogueCallback OnDialogueStart;
//     public DialogueCallback OnDialogueEnd;
//     public DialogueNodeCallback OnDialogueNodeStart;
//     public DialogueNodeCallback OnDialogueNodeEnd;
//     
//     //현재 다이얼로그와 노드
//     private Dialogue currentDialogue;
//     private DialogueNode currentNode;
//
//     public void StartDialogue(Dialogue dialogue)
//     {
//         if (currentDialogue == null)
//         {
//             currentDialogue = dialogue;
//             OnDialogueStart?.Invoke(currentDialogue);
//             StartDialogueNode(dialogue.FirstNode);
//         }
//         else
//         {
//             throw new DialogueException("Can't start a dialogue when another is already running.");
//         }
//     }
//
//     public void EndDialogue(Dialogue dialogue)
//     {
//         if (currentDialogue == dialogue)
//         {
//             StopDialogueNode(currentNode);
//             OnDialogueEnd?.Invoke(currentDialogue);
//             currentDialogue = null;
//         }
//         else
//         {
//             throw new DialogueException("Trying to stop a dialogue that isn't running.");
//         }
//     }
//
//     private bool CanStartNode(DialogueNode node)
//     {
//         return (currentNode == null || node == null || currentNode.CanBeFollowedByNode(node));
//     }
//
//     public void StartDialogueNode(DialogueNode node)
//     {
//         if (CanStartNode(node))
//         {
//             StopDialogueNode(currentNode);
//             currentNode = node;
//
//             if (currentNode != null)
//             {
//                 OnDialogueNodeStart?.Invoke(currentNode);
//             }
//             else
//             {
//                 EndDialogue(currentDialogue);
//             }
//         }
//         else
//         {
//             throw new DialogueException("failed to start dialogue node.");
//         }
//     }
//
//     private void StopDialogueNode(DialogueNode node)
//     {
//         if (currentNode == node)
//         {
//             OnDialogueNodeEnd?.Invoke(currentNode);
//             currentNode = null;
//         }
//         else
//         {
//             throw new DialogueException("trying to stop a dialogue node that isn't running.");
//         }
//     }
//     
//     
// }
