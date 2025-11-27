using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedRPG;

namespace TextBasedRPG
{ 
    // фабрика врагов
    public class EnemyFactory
    {
        private List<AbstractEnemy> regularEnemies = new List<AbstractEnemy>
            {
                new Goblin(),
                new Mag(),
                new Skeleton(),
                new Slime()
            };

        private List<AbstractEnemy> bosses = new List<AbstractEnemy>
            {
                new VVG(),
                new Kovalsky(),
                new ArchimagCpp(),
                new PestovCmm()
            };

    public AbstractEnemy GetRandomEnemy(bool forceBoss = false)
        {
            var enemiesToChoose = forceBoss ? bosses : regularEnemies;
            if (enemiesToChoose.Count > 0)
            {
                return enemiesToChoose[RandomHelper.Next(enemiesToChoose.Count)];
            }
            return regularEnemies[RandomHelper.Next(regularEnemies.Count)];
        }

        public AbstractEnemy GenerateEnemy() => GetRandomEnemy(false);

        public AbstractEnemy GenerateBoss() => GetRandomEnemy(true);
    }
}
