﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ServerDatabase
    {
        public static Dictionary<int, Laptop> Laptopovi = new Dictionary<int, Laptop>();
        public static DirektorijumKorisnika direktorijumKorisnika = new DirektorijumKorisnika();
    }
}
