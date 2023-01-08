using System.ComponentModel.DataAnnotations;

namespace VilllaParks.Model.Dto
{
    public class VillaParkNoCreatDTO
    {
        public int VillaParkNo { get; set; }
        [Required]
        public int VillaParkID { get; set; }
        public string SpecialDetails { get; set; }
    }
}
