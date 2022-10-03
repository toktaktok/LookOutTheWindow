using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumTypes
{
    public enum GameFlowState
    {
        InitGame,
        CutScene,
        Setting,
        EventFlow,
        Ending,
    }
    
    public enum CurrentMiniGameState
    {
        Good,
        SoSo,
        Bad,
    }

    public enum UIState
    {
        Basic,
        NoteBook,
        Interacting,
    }

    public enum VillagerEnumData
    {
        None = 0,
        Mayor = 1,
        Zig,
        FlowerElephant,
        GumMan,
        JumpingKid,
        Toad,
        YoungMouse,
        OutsiderFox,
        OutsiderCrane,
        PlanetKids,
        KiwiBird = 11,
        RegularKitty,
        MuseumGuard,
        RabbitCouple,
        CollegeStudents,
        PriestCroco,
        GuardCroco,
        CandyMan,
        Cedgehog,
        Postman,
        AmusementGuard = 21,
        FishingRaccoon,
        Dogtor,
        AngelCaterpillar,
        LakeDinosaur,
        Goose,
        BabyBat
        
        //이후 오브젝트는 1001번부터
        
    }
    
}
