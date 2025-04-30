using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Dashboard.MVC.ViewModels
{
  


    // Example nested ViewModels for table data
    public class LowStockViewModel
    {
        public string Name_en { get; set; }

   
        public string? ActiveSubstance { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }


    }
}
