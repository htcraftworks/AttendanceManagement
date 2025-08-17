using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContexts
{
    /// <summary>
    /// データベース定義コンテキスト
    /// NOTE:MigrationについてはReadMe.txt参照
    /// </summary>
    public class MyDbContext : IdentityDbContext<Users>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="options">構成オプション</param>
        public MyDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        #region Entity

        /// <summary>
        /// 勤怠記録
        /// </summary>
        public DbSet<AttendanceRecords> AttendanceRecords { get; set; }

        /// <summary>
        /// 勤怠記録履歴
        /// </summary>
        public DbSet<AttendanceRecordsHistory> AttendanceRecordsHistory { get; set; }

        #endregion

        #region model

        /// <summary>
        /// 戻り値型：ストアド「usp_GetAttendanceListWithHistory」
        /// </summary>
        public DbSet<ResultModel_GetAttendanceListWithHistory> ResultModel_GetAttendanceListWithHistory { get; set; }

        #endregion

        /// <summary>
        ///  エンティティのカスタマイズ
        /// </summary>
        /// <param name="builder">構成</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 戻り値型定義（データベースには登録されない空のEntity）
            builder.Entity<ResultModel_GetAttendanceListWithHistory>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
        }
    }
}