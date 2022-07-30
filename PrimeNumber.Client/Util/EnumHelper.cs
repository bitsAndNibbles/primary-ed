﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace PrimeNumber.Client.Util;

/// <summary>
/// from https://stackoverflow.com/a/12430331
/// </summary>
public static class EnumHelper
{
    public static string Description(this Enum value)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Any())
            return (attributes.First() as DescriptionAttribute).Description;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // If no description is found, the least we can do is replace underscores with spaces
        // You can add your own custom default formatting logic here
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
    }

    public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type t)
    {
        if (!t.IsEnum)
            throw new ArgumentException($"{nameof(t)} must be an enum type");

        return Enum.GetValues(t).Cast<Enum>().Select((e) => new ValueDescription() { Value = e, Description = e.Description() }).ToList();
    }
}
