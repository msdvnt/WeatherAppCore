using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAppCore.Models
{
    /// <summary>
    /// Дата	Время	Т	Отн. влажность	Td	Атм. давление,	Направление	Скорость	Облачность,	h	VV	Погодные явления
    /// </summary>
    public class WeatherData
    {
        //[Column("Id")]
        //public int _id;
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }
        [Column(TypeName = "decimal(5, 2)")]        
        public decimal T{ get; set; }

        public int Humidity { get; set; }


        [Column(TypeName = "decimal(5, 2)")]        
        public decimal Td { get; set; }

        public int Pressure { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Direction { get; set; }
        public int Speed{ get; set; }

        public int Cloudiness { get; set; }

        public int h { get; set; }
        public int VV { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Comment { get; set; }
    }
}
