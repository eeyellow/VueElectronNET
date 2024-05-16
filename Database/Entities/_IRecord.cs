using ElectronApp.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronApp.Database.Entities
{
    /// <summary>
    /// 介面 - 紀錄欄位
    /// </summary>
    public interface IRecord
    {
        /// <summary> 新增日期 </summary>
        DateTime CreateDatetime { get; set; }
        /// <summary> 更新日期 </summary>
        DateTime UpdateDatetime { get; set; }
    }

    /// <summary>
    /// 抽象 - 紀錄欄位
    /// </summary>
    public abstract class ARecord : IEntity, IDelete, IRecord
    {
        /// <inheritdoc/>
        [Key]            
        [Comment("流水號")]
        public long ID { get; set; }

        /// <inheritdoc/>       
        [Comment("是否刪除")]
        [SqlDefaultValue("0")]
        public virtual int IsDelete { get; set; } = 0;

        /// <inheritdoc/>        
        [Column(TypeName = "timestamp")]
        [Comment("新增日期")]
        [SqlDefaultValue("DateTime('now')")]
        public virtual DateTime CreateDatetime { get; set; }

        /// <inheritdoc/>             
        [Column(TypeName = "timestamp")]
        [Comment("更新日期")]
        [SqlDefaultValue("DateTime('now')")]
        public virtual DateTime UpdateDatetime { get; set; }
    }
}
