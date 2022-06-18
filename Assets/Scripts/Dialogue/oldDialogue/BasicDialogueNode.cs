// using UnityEngine;
//
// [CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Node/Basic")]
//
// public class BasicDialogueNode : DialogueNode
// {
//     [SerializeField] private DialogueNode nextNode;
//     public DialogueNode NextNode => nextNode;
//
//     public override bool CanBeFollowedByNode(DialogueNode node)
//     {
//         return nextNode == node;    //다음 노드가 존재하는가?
//     }
//
//     public override void Accept(DialogueNodeVisitor visitor)
//     {
//         visitor.Visit(this);    //NodeVisitor이 자신을 방문하게 한다.
//     }
//     
// }
