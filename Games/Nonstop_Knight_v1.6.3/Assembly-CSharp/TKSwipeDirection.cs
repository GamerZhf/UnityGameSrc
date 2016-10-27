using System;

[Flags]
public enum TKSwipeDirection
{
    All = 0xff,
    BottomSide = 0xa8,
    Cardinal = 15,
    Diagonal = 240,
    DiagonalDown = 160,
    DiagonalLeft = 0x30,
    DiagonalRight = 0xc0,
    DiagonalUp = 80,
    Down = 8,
    DownLeft = 0x20,
    DownRight = 0x80,
    Horizontal = 3,
    Left = 1,
    LeftSide = 0x31,
    Right = 2,
    RightSide = 0xc2,
    TopSide = 0x54,
    Up = 4,
    UpLeft = 0x10,
    UpRight = 0x40,
    Vertical = 12
}

