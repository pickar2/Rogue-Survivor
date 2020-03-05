using System;

namespace RogueSurvivor.Data
{
    [Serializable]
    enum Weather
    {
        _FIRST,

        CLEAR = _FIRST,
        CLOUDY,
        RAIN,
        HEAVY_RAIN,

        _COUNT
    }
}
