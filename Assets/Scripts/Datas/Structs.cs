using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Structs
{
    [SerializeField]
    public struct DialogueData
    {

    }

    [SerializeField]
    public struct MiniGameData
    {
        public int[] UISize;

    }

    //대사 묶음(숫자, 텍스트로 이뤄짐)
    [System.Serializable]
    public struct DialogueInfo
    {
        public int id;
        public string text;
    }

    //전체 대사 리스트
    [System.Serializable]
    public struct BasicDialogueList
    {
        public List<DialogueInfo> dialogueData;
    }
    
    //전체 대사 리스트
    [System.Serializable]
    public struct GossipDialogueList
    {
        public List<DialogueInfo> dialogueData;
    }


}
