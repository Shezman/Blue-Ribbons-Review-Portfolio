using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BlueRibbonsReview.Models
{
    public enum Categories
    {
        Clothing,
        [Display(Name ="Food & Drink")]
        FoodAndDrink,
        [Display(Name = "Health & Beauty")]
        HealthAndBeauty,
        [Display(Name = "Home Products")]
        HomeProducts,
        Children,
        Technology,
        Miscellaneous
    }
}