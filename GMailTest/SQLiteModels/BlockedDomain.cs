﻿using System;
using System.Collections.Generic;

#nullable disable

namespace GMailTest.SQLiteModels
{
    public partial class BlockedDomain
    {
        public long PkId { get; set; }
        public String Domain { get; set; }
    }
}
