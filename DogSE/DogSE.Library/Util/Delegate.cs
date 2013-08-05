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

namespace DogSE.Library.Util
{
    #region zh-CHS 委托 | en Delegate
    /// <summary>
    /// Delegate for calling a method that is not known at runtime.
    /// </summary>
    /// <param name="target">the object to be called or null if the call is to a static method.</param>
    /// <param name="parameters">the parameters to the method.</param>
    /// <returns>the return value for the method or null if it doesn't return anything.</returns>
    public delegate object FastInvokeHandler( object target, object[] parameters );

    /// <summary>
    /// Delegate for creating and object at runtime using the default constructor.
    /// </summary>
    /// <returns>the newly created object.</returns>
    public delegate object FastCreateInstanceHandler();

    /// <summary>
    /// Delegate to get an arbitraty property at runtime.
    /// </summary>
    /// <param name="target">the object instance whose property will be obtained.</param>
    /// <returns>the property value.</returns>
    public delegate object FastPropertyGetHandler( object target );

    /// <summary>
    /// Delegate to set an arbitrary property at runtime.
    /// </summary>
    /// <param name="target">the object instance whose property will be modified.</param>
    /// <param name="parameter"></param>
    public delegate void FastPropertySetHandler( object target, object parameter );
    #endregion
}
#endregion

