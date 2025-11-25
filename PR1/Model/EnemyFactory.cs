using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class EnemyGenerator
{
    public abstract void CreateEnemy();
}

public class Goblin : EnemyGenerator
{
    public override void CreateEnemy()
    {
        new Enemy("Гоблин", 40, 8, 2, 0.2, 0, false, false);
    }
}

public class Skeleton : EnemyGenerator 
{
    public override void CreateEnemy()
    {
        new Enemy("Скелет", 45, 7, 3, 0, 0, true, false);
    }
}

public class Mage : EnemyGenerator 
{
    public override void CreateEnemy()
    {
        new Enemy("Маг", 35, 9, 1, 0, 0.15, false, false);
    }
}

public class Slime : EnemyGenerator
{
    public override void CreateEnemy()
    {
        new Enemy("Слизень", 30, 6, 3, 0, 0, false, true);
    }
}

//class VVG : Goblin
//{
//    public override void CreateEnemy()
//    {
//        //new Enemy("ВВГ", (int)(40 * 2.0), (int)(8 * 1.5), (int)(2 * 1.2), 0.2 + 0.1, 0, false, false);
//    }
//}

internal class EnemyFactory
// Фабрика для генерации врагов и боссов
{
    public Enemy GenerateEnemy(Enemy enemy, Enemy slime)
    {
        //Slime slime = new Slime();
        int type = RandomProvider.Next(4);
        switch (type)
        {
            case 0: return new Enemy("Гоблин" , 40, 8, 2, 0.2, 0   , false, false);
            case 1: return new Enemy("Скелет" , 45, 7, 3, 0  , 0   , true , false);
            case 2: return new Enemy("Маг"    , 35, 9, 1, 0  , 0.15, false, false);
            case 3: return new Enemy("Слизень", 30, 6, 3, 0  , 0   , false, true );
            //case 4: return slime;
            default: return null;
        }
    }

    public Enemy GenerateBoss()
    {
        int type = RandomProvider.Next(4);
        switch (type)
        {
            case 0: return new Enemy("ВВГ"        , (int)(40 * 2.0), (int)(8 * 1.5), (int)(2 * 1.2), 0.2 + 0.1, 0          , false, false);
            case 1: return new Enemy("Ковальский" , (int)(45 * 2.5), (int)(7 * 1.3), (int)(3 * 1.4), 0        , 0          , true , false);
            case 2: return new Enemy("Архимаг C++", (int)(35 * 1.8), (int)(9 * 1.6), (int)(1 * 1.1), 0        , 0.15 + 0.10, false, false);
            case 3: return new Enemy("Пестов С--" , (int)(45 * 1.3), (int)(7 * 1.8), (int)(3 * 0.6), 0        , 0.15 + 0.15, true , false);
            default: return null;
        }
    }
}
