﻿using System.ComponentModel.DataAnnotations;

namespace DevInsight.Core.Attributes;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            return date.Date >= DateTime.UtcNow.Date;
        }
        return false;
    }
}