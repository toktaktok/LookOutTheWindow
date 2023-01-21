using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    public struct Evidence
    {
        public int id;
        public string name;
        public string description;
    }

    [System.Serializable]
    public struct QuestInfo
    {
        public int id;
        public string title;
        public string requestedVillager;
        public string description;
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
    public struct DialogueList
    {
        public List<DialogueInfo> dialogueData;
    }
    
    //전체 대사 리스트
    [System.Serializable]
    public struct QuestList
    {
        public List<QuestInfo> data;
    }
    


}
