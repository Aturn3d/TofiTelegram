﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    public class CurrentPaymentInfo
    {
        public int Id { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
    }
}