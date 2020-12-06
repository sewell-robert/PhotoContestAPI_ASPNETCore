using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContestAPI.Classes
{
    public class MiscCalculations
    {
        public string contestStartDt = "2020-11-29 00:00:00";

        public string GetContestWeek()
        {
            DateTime.TryParse(this.contestStartDt, out var startDt);

            //For testing purposes only
            //DateTime.TryParse("2020-12-19", out var testDt);
            //TimeSpan interval = testDt - startDt;

            TimeSpan interval = DateTime.Today - startDt;

            double numberOfDays = Convert.ToDouble(interval.Days);

            int contestWeek = ((int)Math.Floor(numberOfDays / 7)) + 1;

            return contestWeek.ToString();
        }
    }
}
