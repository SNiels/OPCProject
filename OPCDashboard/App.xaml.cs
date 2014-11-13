﻿using OPCLib.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OPCDashboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private WCFRESTService _service;
        public App()
        {
            _service = new WCFRESTService();
            _service.StartService();
        }
    }
}
