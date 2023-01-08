
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VilllaParks.Model
{
    public class VillaParkNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaParkNo { get; set; }

        [ForeignKey("VillaPark")]
        public int VillaParkID { get; set; }
        public VillaPark VillaPark { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate {get; set;}
    }
}
