using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectXWindows.Core
{
    class Helper
    {
        public float ConvertAngle(float num, bool radian)
        {
            float ratio = 57.295779513082320876798154814105f;
            if (radian)
            {
                return num * ratio;
            }
            else
            {
                return num / ratio;
            }
        }

        public float ChangeRotationByDegrees(float degrees)
        {
            return ConvertAngle(degrees, false);
        }

        public int OddEvenChoice(int number)
        {
            //Convert.ToInt32(Regex.Replace(System.Guid.NewGuid().ToString(), "[^0-9]", "").Substring(0, 1))
            if (number % 2 == 0)
                return 1;
            return 0;
        }
    }
}
