﻿using INSAT._4I4U.TryShare.MobileApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSAT._4I4U.TryShare.MobileApp.Services.ReturnZones
{
    public interface IReturnZonesService
    {
        public IEnumerable<ReturnZone> GetAllReturnZones();
    }
}
