﻿using Application.Orders.Entities;
using Infrastructure.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Orders.Admins.Dto
{
    [AutoMapFrom(typeof(Order))]
    public class OrderForExportDto
    {
        public string Number { get; set; }

        public string Title { get; set; }

        public decimal Money { get; set; }

        public string StatusText { get; set; }

        public string NoteOfCustomer { get; set; }

        public string ProductInfo { get; set; }

        public OrderCustomerInfoForOrderForExportDto OrderCustomerInfo { get; set; }

        public ShipStatus ShipStatus { get; set; }

        public bool IsSpreader { get; set; }

        public DateTime CreationTime { get; set; }

        [NotMapped]
        public string ExpressCompany { get; set; }

        [NotMapped]
        public string TrackingNumber { get; set; }
    }
}
