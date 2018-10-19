using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Extensions {
    public static class ExtensionsCore {
        public static void Init() {
            Logging.Init();
            NameSubjectToChange.Init();
            SpecialMoves.Init();
        }
    }
}
