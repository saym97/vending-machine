﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace dbContext.VendingMachine.Entities;

public partial class Products
{
    public Guid ProductId { get; set; }

    public string Name { get; set; }

    public int Cost { get; set; }

    public int AmountAvailable { get; set; }

    public Guid SellerId { get; set; }

    public virtual Users Seller { get; set; }
}