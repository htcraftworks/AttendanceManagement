using Data.DbContexts;
using Data.Entities;
using Library;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Accessor
{
    /// <summary>
    /// 勤怠アクセサ
    /// </summary>
    public static class AttendanceAccessor
    {
        /// <summary>
        /// 勤怠を登録します。
        /// </summary>
        /// <param name="attendance">勤怠データ</param>
        /// <returns>更新件数</returns>
        public static async Task<int> CreateAttendance(InsertAttendanceParam attendance)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                var newAttendance = new AttendanceRecords();

                newAttendance.AttendanceId = attendance.AttendanceId;
                newAttendance.UserCode = attendance.UserCode;
                newAttendance.WorkDate = attendance.WorkDate;
                newAttendance.ClockInDateTime = attendance.ClockInDateTime;
                newAttendance.ClockOutDateTime = attendance.ClockOutDateTime ?? null;
                newAttendance.BreakInDateTime = attendance.BreakInDateTime ?? null;
                newAttendance.BreakOutDateTime = attendance.BreakOutDateTime ?? null;
                newAttendance.WorkingStyleState = attendance.WorkingStyleState;
                newAttendance.GenerateDateTime = DateTimeUtility.GetUtcDateTimeNow();

                context.AttendanceRecords.Add(newAttendance);

                return await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 対象日の勤怠を取得します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="targetDate">指定月</param>
        /// <returns>勤怠データ</returns>
        public static async Task<AttendanceRecords?> SelectAttendanceForWorkDate(string userCode, DateOnly targetDate)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                return await context.AttendanceRecords
                    .AsNoTracking()
                    .Where(r =>
                        r.UserCode == userCode
                        && r.WorkDate == targetDate
                        && !r.IsDelete)
                    .OrderByDescending(r => r.GenerateDateTime)
                    .FirstOrDefaultAsync<AttendanceRecords>();
            }
        }

        /// <summary>
        /// 指定月の勤怠を取得します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="dateTimeFrom">開始日</param>
        /// <param name="dateTimeTo">終了日</param>
        /// <returns>勤怠データ</returns>
        public static async Task<List<ResultModel_GetAttendanceListWithHistory>> SelectAttendanceForMonth(string userCode, DateOnly dateTimeFrom, DateOnly dateTimeTo)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                // HACK:EF COREは複雑なクエリのGroupJoinはサポートしてない（SelectManyによる平坦化が必要、実体化しないとソートできない等コストがかかる）
                var parameters = new[]
                {
                    new SqlParameter("@UserCode", userCode),
                    new SqlParameter("@DateTimeFrom", dateTimeFrom.ToDateTime(TimeOnly.MinValue)),
                    new SqlParameter("@DateTimeTo", dateTimeTo.ToDateTime(TimeOnly.MinValue))
                };

                return await context.ResultModel_GetAttendanceListWithHistory
                    .FromSqlRaw("usp_GetAttendanceListWithHistory @UserCode, @dateTimeFrom, @dateTimeTo", parameters)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// 勤怠ID指定の勤怠を取得します。
        /// </summary>
        /// <param name="attendanceId">勤怠ID</param>
        /// <param name="userCode">ユーザーコード</param>
        /// <returns>勤怠データ</returns>
        public static async Task<AttendanceRecords?> SelectAttendanceForId(Guid attendanceId, string userCode)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                return await context.AttendanceRecords
                    .AsNoTracking()
                    .Where(r =>
                        !r.IsDelete
                        && r.AttendanceId == attendanceId
                        && r.UserCode == userCode)
                    .SingleOrDefaultAsync<AttendanceRecords>();
            }
        }

        /// <summary>
        /// 最後の勤怠データを取得します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <returns>勤怠データ</returns>
        public static async Task<AttendanceRecords?> SelectAttendanceLastData(string userCode)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                return await context.AttendanceRecords
                    .AsNoTracking()
                    .Where(r =>
                        !r.IsDelete
                        && r.UserCode == userCode)
                    .OrderByDescending(r => 
                        r.GenerateDateTime)
                    .FirstOrDefaultAsync<AttendanceRecords>();
            }
        }

        /// <summary>
        /// 指定の勤怠を更新します。
        /// </summary>
        /// <param name="attendance">勤怠データ</param>
        /// <param name="reason">理由</param>
        /// <returns>更新件数</returns>
        public static async Task<int?> UpdateAttendance(AttendanceRecords attendance, string reason)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                // 勤怠データ論理更新
                var dateTime = DateTimeUtility.GetUtcDateTimeNow();
                attendance.UpdateDateTime = dateTime;

                context.AttendanceRecords.Attach(attendance);

                context.Entry(attendance).Property(e => e.ClockInDateTime).IsModified = true;
                context.Entry(attendance).Property(e => e.ClockOutDateTime).IsModified = true;
                context.Entry(attendance).Property(e => e.BreakInDateTime).IsModified = true;
                context.Entry(attendance).Property(e => e.BreakOutDateTime).IsModified = true;
                context.Entry(attendance).Property(e => e.WorkingStyleState).IsModified = true;
                context.Entry(attendance).Property(e => e.UpdateDateTime).IsModified = true;

                // 履歴追加
                var newHistory = new AttendanceRecordsHistory
                {
                    AttendanceId = attendance.AttendanceId,
                    UserCode = attendance.UserCode,
                    GenerateDateTime = dateTime,
                    Reason = reason,
                    IsDelete = false
                };

                context.AttendanceRecordsHistory.Add(newHistory);

                return await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 退勤日時を更新します。
        /// </summary>
        /// <param name="attendance">勤怠データ</param>
        /// <returns>結果</returns>
        public static async Task<int?> UpdateAttendanceClockOut(AttendanceRecords attendance)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                attendance.UpdateDateTime = DateTimeUtility.GetUtcDateTimeNow();
                context.AttendanceRecords.Attach(attendance);

                // 参照なしで直接更新
                context.Entry(attendance).Property(e => e.ClockOutDateTime).IsModified = true;
                context.Entry(attendance).Property(e => e.UpdateDateTime).IsModified = true;

                return await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 勤怠を削除します。
        /// </summary>
        /// <param name="attendance">勤怠データ</param>
        /// <returns>勤怠情報</returns>
        public static async Task<int?> DeleteAttendance(DeleteAttendanceParam attendance)
        {
            using (var context = new MyDbContextFactory().CreateDbContext())
            {
                // 勤怠データ論理削除
                var attendanceDelete = new AttendanceRecords 
                { 
                    AttendanceId = attendance.AttendanceId,
                    UserCode = attendance.UserCode,
                    IsDelete = true
                };

                context.AttendanceRecords.Attach(attendanceDelete);
                context.Entry(attendanceDelete).Property(e => e.IsDelete).IsModified = true;

                // 勤怠履歴データ論理削除
                var historyDelete = await context.AttendanceRecordsHistory
                    .Where(h =>
                        h.AttendanceId == attendance.AttendanceId
                        && h.UserCode == attendance.UserCode)
                    .ToListAsync<AttendanceRecordsHistory>();

                if (historyDelete.Any())
                {
                    historyDelete.ForEach(h => h.IsDelete = true);
                }

                // 勤怠履歴データの削除履歴登録
                var newHistory = new AttendanceRecordsHistory
                {
                    AttendanceId = attendance.AttendanceId,
                    UserCode = attendance.UserCode,
                    GenerateDateTime = DateTimeUtility.GetUtcDateTimeNow(),
                    Reason = attendance.Reason,
                    IsDelete = true
                };

                context.AttendanceRecordsHistory.Add(newHistory);

                return await context.SaveChangesAsync();
            }
        }
    }
}
