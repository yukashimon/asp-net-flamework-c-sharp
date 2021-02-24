using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnimalCrossingNewHorizons.Models
{
    public class ComModel
    {
        [DisplayName("商品ID")]
        public long Com_Id { get; set; }

        [DisplayName("商品名")]
        [Required(ErrorMessage = "商品名は必須入力です。")]
        public string ComName { get; set; }

        [DisplayName("詳細")]
        public string ComDetail { get; set; }
    }
}