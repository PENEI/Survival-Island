
//아이템 태그
public enum E_MaterialType
{
    None,

    Material,
    Tool,
    Armor,
    Use_Item,
    Medicene,

    Max
}

//유저 도구 
public enum E_UseTool
{
    None,

    Default,
    Bottle,
    Axe,
    Shovel,
    Knife,
    Pickaxe,
    Hammer,

    Max
}

public enum E_Object_Type
{
    None,

    N_Obj,
    A_Obj,

    Max
}

//슬롯 타입
public enum E_SlotType
{
    None,

    Default,        //기본
    Tool,            //도구
    Armor,         //방어구
    MakingResult, //제작결과
    Fuel,

    Max
}

//상호작용 오브젝트 타입
public enum E_InteractionType
{
    None,

    Gathering,      //채집
    Bed,            //침대
    Making,         //제작
    Ending,         //엔딩
    Build,           //건축

    Max
}



//제작 타입
public enum E_MakingType
{
    None,

    Cook,           //요리
    Workbench,    //작업대
    Inven,           //가방
    Brazier,        //화로

    Max
}


//자연재해
public enum E_Disaster_Type
{
    None,       
    Sunny,      //맑음
    Earthquake, //지진
    Heatwave,   //폭염
    Rain,       //폭우
    Tidalwaves, //해일
    Typhoon,    //태풍
    Eruption,   //분화
    Max         //enum 크기
}
public enum EWeader
{
    None,
    Sunny,
    Earthquake,
    Heatwave,
    Rain,
    Tidalwaves,
    Typhoon,
    Eruption,
    Max
}
//플레이어 행동
public enum E_Player_State
{
    None,
    Idle,       //대기
    Move,       //이동
    Attack,     //공격
    Work,       //작업
    Hit,        //일반 피격
    Dead,       //죽음
    Search,     //탐색
    Axe,        //도끼질
    Fire,       //요리
    Water,      //물떠기
    Shovel,     //삽질
    Ham,        //제작
    Galmuri,    //시채 채집
    Pickax,     //곡괭이
    Sham,       //건설
    Sleep,      //잠자기
    Max
}

public enum E_Debuff_Type
{
    None,
    Bodyache,
    Cold,
    Dehydration,
    Foodpoison,
    Stun,
    Swim,
    Wound,
    Max
}

//몬스터 행동 패턴
public enum E_Enemy_State
{
    None,
    Idle,       //대기
    Move,       //이동
    Attack,     //공격
    Hit,        //일반 피격
    Dead,       //죽음
    Comeback,    //복귀
    Max
}

public enum E_Enemy_Type
{
    None,
    Carnivore,  //육식 
    Herbivores  //초식
}
