﻿namespace Web.Services.InventoryAPI.Models.Dto
{
    public class StockCreate
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public long StockQuantity { get; set; }
        public long ReservedQuantity { get; set; }
    }
}
