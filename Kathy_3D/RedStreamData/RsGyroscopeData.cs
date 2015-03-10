using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsGyroscopeData : RsActorData // RsDamageableData when Arsenic makes collisions between Gyro and Ship
    {
        public int MaxTowersPerGyro;
        public float GyroAlignmentSpeed, GyroRotationSpeed;
    }
}
