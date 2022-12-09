﻿using ConvenienceStore.ViewModel.StaffVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model.Staff
{
    public class Bills : BaseViewModel
    {
        #nullable enable

        private int? _BillId;
        public int? BillId { get { return _BillId; } set { _BillId = value; OnPropertyChanged(); } }

        private string? _UserName;
        public string? UserName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }

        private string? _CustomerName;
        public string? CustomerName { get { return _CustomerName; } set { _CustomerName = value; OnPropertyChanged(); } }

        private System.DateTime? _BillDate;
        public System.DateTime? BillDate { get { return _BillDate; } set { _BillDate = value; OnPropertyChanged(); } }

        private int? _TotalPrice;
        public int? TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; OnPropertyChanged(); } }

        #nullable disable
    }
}
