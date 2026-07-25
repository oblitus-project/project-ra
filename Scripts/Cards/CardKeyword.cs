using System;

namespace ProjectRA.Cards;

[Flags]
public enum CardKeyword
{
    None            = 0,
    Upgrade         = 1 << 0,
    Retain          = 1 << 1,
    Exhaust         = 1 << 2,
    Void            = 1 << 3,
    CannotPlay      = 1 << 4,
    Eternal         = 1 << 5,
    Transform       = 1 << 6,
    Steadfast       = 1 << 7,
    CannotClash     = 1 << 8,
    CannotTarget    = 1 << 9,
    FixedTarget     = 1 << 10,
    RandomAttack    = 1 << 11,
    WideBarrage     = 1 << 12,
    Indestructible  = 1 << 13,
    Consumption     = 1 << 14,
}
