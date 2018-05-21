﻿//******************************************************************************************************
//  PanicException.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/07/2018 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;

namespace goutil
{
    /// <summary>
    /// Represents an exception for the "panic" keyword.
    /// </summary>
    [DebuggerNonUserCode]
    public class PanicException : Exception
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public object State { get; }

        public PanicException(object state) : base(state?.ToString() ?? "nil") => State = state;
    }
}