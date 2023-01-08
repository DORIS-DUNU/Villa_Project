using System.ComponentModel.DataAnnotations;

namespace VilllaParks.Model.Dto
{
    public class VillaParkNoUpdateDTO
    {
        public int VillaParkNo { get; set; }
        [Required]
        public int VillaParkID { get; set; }

        public string SpecialDetails { get; set; }
    }
}
