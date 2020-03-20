﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;

namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DashBoard : UserControl { 

        ViewModels.DashBoardViewModel dashboardVM;

        public DashBoard(Model.AircraftModel model)
        {
            InitializeComponent();
            this.dashboardVM = new ViewModels.DashBoardViewModel(model);
            DataContext = dashboardVM;
            //Console.WriteLine("View"+dashboardVM.VM_Indicated_heading_deg);
        }
    }

}