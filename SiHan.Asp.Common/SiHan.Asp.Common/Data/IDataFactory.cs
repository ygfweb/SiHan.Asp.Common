﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SiHan.Asp.Common.Data
{
    public interface IDataFactory
    {
        IDbConnection CreateConnection();
    }
}
