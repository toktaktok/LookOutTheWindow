using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ScriptableObjects
{
    //에디터 타임에 수정됨, 게임 타임에는 변하지 않는 초기 데이터 값들
    [Serializable]
    [CreateAssetMenu(fileName = "Data", menuName ="ScriptableObjects/GamePrefabs", order = 1)]
    public class GamePrefabs : ScriptableObject
    {
        //편하게 데이터에 접근할 수 있도록 데이터 덩어리 객체를 정의 
        #region Using
        public ScriptableObjects.SpriteGroup spriteGroup;

        public ScriptableObjects.BuildingGroup buildingGroup;

        //public ScriptableObjects.ItemGroup itemGroup;

        //public ScriptableObjects.VillagerGroup villagerGroup;

        //public ScriptableObjects.QuestGroup questGroup;

        #endregion
    }

    [Serializable]
    [CreateAssetMenu(fileName = "ObjectInfo", menuName = "ScriptableObjects/Interactive/ObjectInfo", order = 1)]
    public class ObjectInfo : ScriptableObject
    {
        //클래스 안의 property : 앞에 _를 붙여 구별 
        [SerializeField]
        private Sprite _thumbnail;

        [SerializeField]
        private string _id; //아이템의 고유 번호 

        [SerializeField]
        private int _minigameId = 0;


        //그 외 아이템에 포함될 것들

        public Sprite Thumbnail
        {
            get { return _thumbnail; }
        }

        public string Name
        {
            get { return _id; }
        }

        public int Minigameid
        {
            get { return _minigameId; }
        }
        

        void CheckMinigame()
        {
            if(0 < Minigameid)
            {
            }
        }
    }

    [Serializable]
    public class Villager : ScriptableObject
    {
        [SerializeField]
        private string _id; //주민의 고유 번호

        [SerializeField]
        private enum _state
        {
            IDLE,
            TALK,
        }

        public string Id
        {
            get { return _id; }
        }
    }

    [Serializable]
    public class Minigame : ScriptableObject
    {

        [SerializeField]
        private int _id;

        [SerializeField]
        private int _score;

    }

    [Serializable]
    public class Quest : ScriptableObject
    {
        [SerializeField]
        private string _id; //의뢰별 고유 번호

        [SerializeField]
        private string _subject; //퀘스트 이

        [SerializeField]
        private List<string> _villagerIds; //관련된 주민들의 id 모음

        [SerializeField]
        private enum _state
        {

            IN_PROGRESS,
            COMPLETE,
        }
    }


    //데이터를 종류별로 묶어둔 데이터 덩어리 정의
    [Serializable]
    [CreateAssetMenu(fileName = "SpriteData", menuName = "ScriptableObjects/CommonGroup/SpriteGroup", order = 1)]
    public class SpriteGroup : ScriptableObject
    {
        public List<GameObject> sprites;
    }

    [Serializable]
    [CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/CommonGroup/BuildingGroup", order = 2)]
    public class BuildingGroup : ScriptableObject
    {
        public List<GameObject> buildings;
    }

    //[Serializable]
    //[CreateAssetMenu(fileName = "VillagerData", menuName = "ScriptableObjects/CommonGroup/VillagerGroup", order = 2)]
    //public class VillagerGroup : ScriptableObject
    //{ }

    //[Serializable]
    //[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/CommonGroup/ItemGroup", order = 3)]
    //public class ItemGroup : ScriptableObject
    //{ }

    //[Serializable]
    //[CreateAssetMenu(fileName ="QuestData", menuName = "criptableObjects/CommonGroup/QuestGroup", order = 4)]
    //public class QuestGroup : ScriptableObject
    //{ }

}
