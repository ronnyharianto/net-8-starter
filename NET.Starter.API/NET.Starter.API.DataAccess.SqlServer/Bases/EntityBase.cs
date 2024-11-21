using System.ComponentModel.DataAnnotations;

namespace NET.Starter.API.DataAccess.SqlServer.Bases
{
    public class EntityBase
    {
        /// <summary>
        /// Primary Key for all entity
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Date and Time when data was created
        /// </summary>
        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Date and Time when data was lastest modified
        /// </summary>
        public DateTime? Modified { get; set; }
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// 0 -> Active Data
        /// 1 -> Soft Delete Data
        /// </summary>
        public int RowStatus { get; set; } = 0;
    }
}
