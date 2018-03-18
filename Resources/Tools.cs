using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace Resources {
    static class Tools {
        public static Regex alphaNumeric = new Regex(@"^[a-zA-Z0-9\s,]*$");
        /// <summary>
        /// if you specify an amount of iterations > 1 the initial delay will be 0
        /// </summary>
        /// <param name="todo"></param>
        /// <param name="initialDelay"></param>
        /// <param name="iterationDelay"></param>
        /// <param name="iterations"></param>
        public static void DoLater(Func<bool> todo, int delay, int iterations = 1) {
            Timer t = null;
            t = new Timer((obj) => {
                if (todo() || --iterations == 0) {
                    t.Dispose();
                }
            }, null, iterations == 1 ? delay : 0, delay);
        }
    }
}
