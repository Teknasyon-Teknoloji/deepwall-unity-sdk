using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepWallModule
{
    public enum ProrationType
    {
        UNKNOWN_SUBSCRIPTION_UPGRADE_DOWNGRADE_POLICY = 0,
        IMMEDIATE_WITH_TIME_PRORATION = 1,
        IMMEDIATE_AND_CHARGE_PRORATED_PRICE = 2,
        IMMEDIATE_WITHOUT_PRORATION = 3,
        DEFERRED = 4,
        NONE = 5,
    }
}