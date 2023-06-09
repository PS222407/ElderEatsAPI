﻿using Newtonsoft.Json;

namespace ElderEatsAPI.ViewModels;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ProductViewModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string? Brand { get; set; }

    public string? QuantityInPackage { get; set; }

    public string? Barcode { get; set; }

    public string? Image { get; set; }

    public string full_name
    {
        get
        {
            string val = Name;
            if (!string.IsNullOrEmpty(Brand))
            {
                val += " - ";
                val += Brand;
            }
            if (!string.IsNullOrEmpty(QuantityInPackage))
            {
                val += " - ";
                val += QuantityInPackage;
            }
            return val;
        }
    }

    /*    public List<AccountProduct>? AccountProducts { get; set; }

        public bool ShouldSerializeAccountProducts()
        {
            return AccountProducts != null && AccountProducts.Count > 0;
        }*/
}
