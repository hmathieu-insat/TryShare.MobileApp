﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSAT._4I4U.TryShare.MobileApp.Services.User
{
    public class UserSubscriptionMockService : IUserSubscriptionService
    {
        public bool CheckSubscriptionValidity()
        {
            return true;
        }
    }
}
