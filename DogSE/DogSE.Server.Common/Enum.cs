#region zh-CHS 2006 - 2010 DemoSoft 团队 | en 2006-2010 DemoSoft Team

//     NOTES
// ---------------
//
// This file is a part of the MMOSE(Massively Multiplayer Online Server Engine) for .NET.
//
//                              2006-2010 DemoSoft Team
//
//
// First Version : by H.Q.Cai - mailto:caihuanqing@hotmail.com

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published
 *   by the Free Software Foundation; either version 2.1 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

#region zh-CHS 包含名字空间 | en Include namespace

#endregion

namespace DogSE.Common
{
    #region zh-CHS 枚举 | en Enum

    /// <summary>
    /// 帐号的等级
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        /// 玩家
        /// </summary>
        Player,
        /// <summary>
        /// 
        /// </summary>
        VeryImportantPerson,
        /// <summary>
        /// GM
        /// </summary>
        GameMaster,
        /// <summary>
        /// 开发者
        /// </summary>
        Developer,
        /// <summary>
        /// 管理者
        /// </summary>
        Administrator,
        /// <summary>
        /// 拥有者
        /// </summary>
        Owner
    }

    /// <summary>
    /// Indicates the extent to which bounding volumes intersect or contain one another.
    /// </summary>
    public enum ContainmentType
    {
        /// <summary>
        /// Indicates that one bounding volume completely contains the other.
        /// </summary>
        Disjoint,
        /// <summary>
        /// Indicates there is no overlap between the bounding volumes.
        /// </summary>
        Contains,
        /// <summary>
        /// Indicates that the bounding volumes partially overlap.
        /// </summary>
        Intersects
    }

    /// <summary>
    /// Describes the intersection between a plane and a bounding volume.
    /// </summary>
    public enum PlaneIntersectionType
    {
        /// <summary>
        /// There is no intersection, and the bounding volume is in the negative half-space of the Plane.
        /// </summary>
        Front,
        /// <summary>
        /// There is no intersection, and the bounding volume is in the positive half-space of the Plane.
        /// </summary>
        Back,
        /// <summary>
        /// The Plane is intersected.
        /// </summary>
        Intersecting
    }

    #endregion
}
#endregion

