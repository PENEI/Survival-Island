
//������ �±�
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

//���� ���� 
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

//���� Ÿ��
public enum E_SlotType
{
    None,

    Default,        //�⺻
    Tool,            //����
    Armor,         //��
    MakingResult, //���۰��
    Fuel,

    Max
}

//��ȣ�ۿ� ������Ʈ Ÿ��
public enum E_InteractionType
{
    None,

    Gathering,      //ä��
    Bed,            //ħ��
    Making,         //����
    Ending,         //����
    Build,           //����

    Max
}



//���� Ÿ��
public enum E_MakingType
{
    None,

    Cook,           //�丮
    Workbench,    //�۾���
    Inven,           //����
    Brazier,        //ȭ��

    Max
}


//�ڿ�����
public enum E_Disaster_Type
{
    None,       
    Sunny,      //����
    Earthquake, //����
    Heatwave,   //����
    Rain,       //����
    Tidalwaves, //����
    Typhoon,    //��ǳ
    Eruption,   //��ȭ
    Max         //enum ũ��
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
//�÷��̾� �ൿ
public enum E_Player_State
{
    None,
    Idle,       //���
    Move,       //�̵�
    Attack,     //����
    Work,       //�۾�
    Hit,        //�Ϲ� �ǰ�
    Dead,       //����
    Search,     //Ž��
    Axe,        //������
    Fire,       //�丮
    Water,      //������
    Shovel,     //����
    Ham,        //����
    Galmuri,    //��ä ä��
    Pickax,     //���
    Sham,       //�Ǽ�
    Sleep,      //���ڱ�
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

//���� �ൿ ����
public enum E_Enemy_State
{
    None,
    Idle,       //���
    Move,       //�̵�
    Attack,     //����
    Hit,        //�Ϲ� �ǰ�
    Dead,       //����
    Comeback,    //����
    Max
}

public enum E_Enemy_Type
{
    None,
    Carnivore,  //���� 
    Herbivores  //�ʽ�
}
