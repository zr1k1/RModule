using System;
using UnityEngine;
using RModule.Runtime.Data.Configs;

// Create your "CustomGameItem" enum with placements for app
// Create a class and inherit from AppEconomics<CustomGameItem>
// For additional conditions to get price value create SomeHardPriceConfig and inherit PriceConfig. drug in inspector
public class AppEconomicsConfig<TReceivedGameItem> : BaseConfig<TReceivedGameItem> where TReceivedGameItem : Enum {
}