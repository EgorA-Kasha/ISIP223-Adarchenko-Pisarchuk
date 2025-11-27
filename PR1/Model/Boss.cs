using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedRPG
{
    public class VVG : AbstractEnemy
    {
        public VVG() : base("ВВГ", 55, 8, 2, 0.2, true) { }
    }

    public class Kovalsky : AbstractEnemy
    {
        public Kovalsky() : base("Ковальский", 103, 9, 3, 0, true, true) { }
    }

    public class ArchimagCpp : AbstractEnemy
    {
        public ArchimagCpp() : base("Архимаг C++", 60, 10, 1, 0, true) { }
    }

    public class PestovCmm : AbstractEnemy
    {
        public PestovCmm() : base("Пестов С--", 61, 13, 1, 0, true, true) { }
    }
}
