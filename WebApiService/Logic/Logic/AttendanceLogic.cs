using System.Text;
using Data.Accessor;
using Data.Entities;
using Library;
using WebApiService.Common.Const;
using WebApiService.Logic.Model;

namespace WebApiService.Logic.Logic
{
    /// <summary>
    /// 勤怠ロジック
    /// </summary>
    public static class AttendanceLogic
    {
        /// <summary>
        /// 出勤登録状況を取得します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <returns>結果</returns>
        public static async Task<ServiceResult<bool>> GetClockInState(string userCode)
        {
            TimeZoneInfo timeZone = TimeZoneConsts.Tokyo.TimeZoneInfo;
            var workDate = DateTimeUtility.ConvertDateTimeToDateOnly(DateTimeUtility.GetUserDateTimeNow(timeZone));

            var attendance = await AttendanceAccessor.SelectAttendanceForWorkDate(userCode, workDate);
            if (attendance != null && attendance.ClockOutDateTime == null)
            {
                // 本日未退勤データあり
                return ServiceResult<bool>.CreateSuccessWithError(true, ApplicationErrorCodes.NotRegistAttendanceClockOutError);
            }

            return ServiceResult<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// 出勤データを登録します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="param">パラメータ</param>
        /// <returns>結果</returns>
        public static async Task<ServiceResult<bool?>> RegisterClockIn(string userCode, AttendanceClockInOutParam param)
        {
            var clockInUtcDateTime = DateTimeUtility.ConvertUnixTimeToUtcDateTime(param.ClockInOutDateTimeUserUnixTimeSec);
            if (!clockInUtcDateTime.HasValue)
            {
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.ValidateParamError);
            }

            TimeZoneInfo timeZone = TimeZoneConsts.Tokyo.TimeZoneInfo;
            var clockInUserDateTime = DateTimeUtility.ConvertUtcDateTimeToUserDateTime(clockInUtcDateTime.Value, timeZone);

            // TODO:仮で固定データとする（オフライン等未考慮）
            var attendance = new InsertAttendanceParam
            {
                AttendanceId = Guid.NewGuid(),
                UserCode = userCode,
                WorkDate = DateTimeUtility.ConvertDateTimeToDateOnly(clockInUserDateTime),
                ClockInDateTime = clockInUtcDateTime.Value,
                BreakInDateTime = DateTimeUtility.ConvertUserDateTimeToUtcDateTIme(DateTimeUtility.ConvertToDateToAddTime(clockInUserDateTime, 12, 0), timeZone),
                BreakOutDateTime = DateTimeUtility.ConvertUserDateTimeToUtcDateTIme(DateTimeUtility.ConvertToDateToAddTime(clockInUserDateTime, 13, 0), timeZone),
                WorkingStyleState = WorkingStyleStates.Office.State
            };

            var result = await AttendanceAccessor.CreateAttendance(attendance);
            if (result == 0)
            {
                Logger.Warn($"CreateAttendance error. AttendanceId:{attendance.AttendanceId}");
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.NotDataAttendanceError);
            }

            return ServiceResult<bool?>.CreateSuccess(true);
        }

        /// <summary>
        /// 退勤データを登録します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="param">パラメータ</param>
        /// <returns>結果</returns>
        public static async Task<ServiceResult<bool?>> RegisterClockOut(string userCode, AttendanceClockInOutParam param)
        {
            var clockOutUtcDateTime = DateTimeUtility.ConvertUnixTimeToUtcDateTime(param.ClockInOutDateTimeUserUnixTimeSec);
            if (!clockOutUtcDateTime.HasValue)
            {
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.ValidateParamError);
            }

            var lastAttendance = await AttendanceAccessor.SelectAttendanceLastData(userCode);
            if (lastAttendance == null
                || lastAttendance.ClockOutDateTime != null)
            {
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.NotDataAttendanceError);
            }

            lastAttendance.ClockOutDateTime = clockOutUtcDateTime.Value;

            var result = await AttendanceAccessor.UpdateAttendanceClockOut(lastAttendance);
            if (result == 0)
            {
                Logger.Warn($"UpdateAttendanceClockOut error. AttendanceId:{lastAttendance.AttendanceId}");
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.UnexpectedError);
            }

            return ServiceResult<bool?>.CreateSuccess(true);
        }

        /// <summary>
        /// 勤怠データを取得します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="param">パラメータ</param>
        /// <returns>True:結果</returns>
        public static async Task<ServiceResult<AttendanceRecordsResult?>> GetAttendance(string userCode, AttendanceGetParam param)
        {
            if (param.AttendanceId == Guid.Empty)
            {
                return ServiceResult<AttendanceRecordsResult?>.CreateError(ApplicationErrorCodes.ValidateParamError);
            }

            var attendance = await AttendanceAccessor.SelectAttendanceForId(param.AttendanceId, userCode);
            if (attendance == null)
            {
                Logger.Warn($"Attendance not found. AttendanceId:{param.AttendanceId}");
                return ServiceResult<AttendanceRecordsResult?>.CreateError(ApplicationErrorCodes.NotDataAttendanceTargetError);
            }

            return ServiceResult<AttendanceRecordsResult?>.CreateSuccess(ConvertToAttendanceRecordResult(attendance));
        }

        /// <summary>
        /// 指定月の勤怠を取得します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="param">パラメータ</param>
        /// <returns>結果</returns>
        public static async Task<ServiceResult<List<AttendanceRecordsResult>?>> GetMonthlyAttendances(string userCode, AttendanceGetForMonthParam param)
        {
            TimeZoneInfo timeZone = TimeZoneConsts.Tokyo.TimeZoneInfo;
            var targetUserDateTime = DateTimeUtility.ConvertUnixTimeToUserDateTime(param.TargetDateUtcUnixTimeSec, timeZone);
            if (!targetUserDateTime.HasValue)
            {
                return ServiceResult<List<AttendanceRecordsResult>?>.CreateError(ApplicationErrorCodes.ValidateParamError);
            }

            var (startDate, endDate) = DateTimeUtility.ConvertToTargetMonthWithStartEnd(targetUserDateTime.Value);
            var records = await AttendanceAccessor.SelectAttendanceForMonth(userCode, startDate, endDate);

            if (records == null
                || records.Count == 0)
            {
                return ServiceResult<List<AttendanceRecordsResult>?>.CreateError(ApplicationErrorCodes.NotDataAttendanceError);
            }

            // GroupByは順番保障されるのでソートしない
            var groupedResults = records.GroupBy(r => new { r.AttendanceId, r.UserCode, r.WorkDate, r.GenerateDateTime });
            var attendanceList = new List<AttendanceRecordsResult>();

            foreach (var group in groupedResults)
            {
                var recordResult = ConvertGroupToAttendanceRecord(group);
                attendanceList.Add(recordResult);
            }

            return ServiceResult<List<AttendanceRecordsResult>?>.CreateSuccess(attendanceList);
        }

        /// <summary>
        /// 勤怠を更新します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="param">パラメータ</param>
        /// <returns>結果</returns>
        public static async Task<ServiceResult<bool?>> UpdateAttendance(string userCode, AttendanceUpdateParam param)
        {
            StringBuilder validateSb = new StringBuilder();
            if (param.AttendanceId == Guid.Empty)
            {
                validateSb.AppendLine("AttendanceId is required.");
            }

            if (!string.IsNullOrEmpty(param.UserCode) && param.UserCode != userCode)
            {
                validateSb.AppendLine("UserCode does not match.");
            }

            // 入力異常
            if (validateSb.Length > 0)
            {
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.ValidateParamError.Id, validateSb.ToString());
            }

            var attendance = await AttendanceAccessor.SelectAttendanceForId(param.AttendanceId, userCode);
            if (attendance == null)
            {
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.NotDataAttendanceError);
            }

            TimeZoneInfo timeZone = TimeZoneConsts.Tokyo.TimeZoneInfo;
            AttendanceRecords updateRecord;
            var validationErrors = ValidateUpdateParam(userCode, param, attendance, timeZone, out updateRecord);
            if (!string.IsNullOrEmpty(validationErrors))
            {
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.ValidateParamError.Id, validationErrors);
            }

            var updateResult = await AttendanceAccessor.UpdateAttendance(updateRecord, param.Reason);
            if (updateResult == null || updateResult.Value == 0)
            {
                Logger.Warn($"UpdateAttendance failed. AttendanceId:{param.AttendanceId}");
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.UnexpectedError);
            }

            return ServiceResult<bool?>.CreateSuccess(true);
        }

        /// <summary>
        /// 勤怠を削除します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="param">パラメータ</param>
        /// <returns>結果</returns>
        public static async Task<ServiceResult<bool?>> DeleteAttendance(string userCode, AttendanceDeleteParam param)
        {
            if (!Guid.TryParse(param.AttendanceId, out Guid attendanceId))
            {
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.ValidateParamError);
            }

            var deleteParam = new DeleteAttendanceParam
            {
                AttendanceId = attendanceId,
                UserCode = userCode,
                Reason = param.Reason
            };

            var result = await AttendanceAccessor.DeleteAttendance(deleteParam);
            if (result == null || result.Value == 0)
            {
                Logger.Warn($"DeleteAttendance failed. AttendanceId:{attendanceId}");
                return ServiceResult<bool?>.CreateError(ApplicationErrorCodes.NotDataAttendanceUpdateError);
            }

            return ServiceResult<bool?>.CreateSuccess(true);
        }

        #region private helper methods

        /// <summary>
        /// 勤怠データを応答データに変換します
        /// </summary>
        /// <param name="attendance">勤怠データ</param>
        /// <returns>応答データ</returns>
        private static AttendanceRecordsResult ConvertToAttendanceRecordResult(AttendanceRecords attendance)
        {
            return new AttendanceRecordsResult
            {
                AttendanceId = attendance.AttendanceId,
                UserCode = attendance.UserCode,
                WorkDateUserUnixTimeSec = DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(DateTimeUtility.ConvertDateOnlyToDate(attendance.WorkDate), false).ToString(),
                ClockInDateUtcUnixTimeSec = DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(attendance.ClockInDateTime, false).ToString(),
                ClockOutDateUtcUnixTimeSec = attendance.ClockOutDateTime.HasValue ? DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(attendance.ClockOutDateTime.Value, false).ToString() : null,
                BreakInDateUtcUnixTimeSec = attendance.BreakInDateTime.HasValue ? DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(attendance.BreakInDateTime.Value, false).ToString() : null,
                BreakOutDateUtcUnixTimeSec = attendance.BreakOutDateTime.HasValue ? DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(attendance.BreakOutDateTime.Value, false).ToString() : null,
                WorkingStyleState = attendance.WorkingStyleState
            };
        }

        /// <summary>
        /// 勤怠履歴データを応答データに変換します
        /// </summary>
        /// <param name="group">勤怠履歴データ</param>
        /// <returns>応答データ</returns>
        private static AttendanceRecordsResult ConvertGroupToAttendanceRecord(IGrouping<dynamic, ResultModel_GetAttendanceListWithHistory> group)
        {
            var attendance = new AttendanceRecordsResult
            {
                AttendanceId = group.Key.AttendanceId,
                UserCode = group.Key.UserCode
            };

            bool isFirst = true;
            var historyList = new List<AttendanceRecordsHistoryResult>();

            foreach (var row in group)
            {
                if (isFirst)
                {
                    isFirst = false;
                    attendance.WorkDateUserUnixTimeSec = DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(DateTimeUtility.ConvertDateOnlyToDate(row.WorkDate), false).ToString();
                    attendance.ClockInDateUtcUnixTimeSec = DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(row.ClockInDateTime, false).ToString();
                    attendance.ClockOutDateUtcUnixTimeSec = row.ClockOutDateTime.HasValue ? DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(row.ClockOutDateTime.Value, false).ToString() : null;
                    attendance.BreakInDateUtcUnixTimeSec = row.BreakInDateTime.HasValue ? DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(row.BreakInDateTime.Value, false).ToString() : null;
                    attendance.BreakOutDateUtcUnixTimeSec = row.BreakOutDateTime.HasValue ? DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(row.BreakOutDateTime.Value, false).ToString() : null;
                    attendance.WorkingStyleState = row.WorkingStyleState;
                }

                if (row.HistoryGenerateDateTime.HasValue)
                {
                    historyList.Add(new AttendanceRecordsHistoryResult
                    {
                        GenerateDateUtcUnixTimeSec = DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(row.HistoryGenerateDateTime.Value, false).ToString(),
                        Reason = row.Reason ?? string.Empty
                    });
                }
            }

            attendance.History = historyList.Count > 0 ? historyList : null;

            return attendance;
        }

        /// <summary>
        /// 更新データを検証します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <param name="param">パラメータ</param>
        /// <param name="attendance">勤怠データ</param>
        /// <param name="timeZone">タイムゾーン</param>
        /// <param name="updateRecord">補完された最終的な更新データ</param>
        /// <returns>検証結果</returns>
        private static string ValidateUpdateParam(string userCode, AttendanceUpdateParam param, AttendanceRecords attendance, TimeZoneInfo timeZone, out AttendanceRecords updateRecord)
        {
            var sb = new StringBuilder();

            if (param.AttendanceId == Guid.Empty)
            {
                sb.AppendLine("AttendanceId is required.");
            }

            if (!string.IsNullOrEmpty(param.UserCode) && param.UserCode != userCode)
            {
                sb.AppendLine("UserCode does not match.");
            }

            if (WorkingStyleStates.GetState(param.WorkingStyleState) == null)
            {
                sb.AppendLine("WorkingStyleState is invalid.");
            }

            DateTime clockInUtcDateTime = param.ClockInUtcDateUnixTimeSec != null ? DateTimeUtility.ConvertUnixTimeToUtcDateTime(param.ClockInUtcDateUnixTimeSec) ?? DateTime.MinValue : DateTime.MinValue;
            if (clockInUtcDateTime == DateTime.MinValue)
            {
                sb.AppendLine($"ClockInDateUnixTimeSec:{param.ClockInUtcDateUnixTimeSec} is invalid value.");
            }

            DateTime? clockOutUtcDateTime = param.ClockOutUtcDateUnixTimeSec != null ? DateTimeUtility.ConvertUnixTimeToUtcDateTime(param.ClockOutUtcDateUnixTimeSec) ?? null : null;
            if (param.ClockOutUtcDateUnixTimeSec != null
                && !clockOutUtcDateTime.HasValue)
            {
                sb.AppendLine($"ClockOutDateUnixTimeSec:{param.ClockOutUtcDateUnixTimeSec} is invalid value.");
            }

            DateTime? breakInUtcDateTime = param.BreakInDateUtcUnixTimeSec != null ? DateTimeUtility.ConvertUnixTimeToUtcDateTime(param.BreakInDateUtcUnixTimeSec) ?? null : null;
            if (param.BreakInDateUtcUnixTimeSec != null
                && !breakInUtcDateTime.HasValue)
            {
                sb.AppendLine($"BreakInDateUnixTimeSec:{param.BreakInDateUtcUnixTimeSec} is invalid value.");
            }

            DateTime? breakOutUtcDateTime = param.BreakOutDateUtcUnixTimeSec != null ? DateTimeUtility.ConvertUnixTimeToUtcDateTime(param.BreakOutDateUtcUnixTimeSec) ?? null : null;
            if (param.BreakOutDateUtcUnixTimeSec != null
                && !breakOutUtcDateTime.HasValue)
            {
                sb.AppendLine($"BreakOutDateUnixTimeSec:{param.BreakOutDateUtcUnixTimeSec} is invalid value.");
            }

            if (string.IsNullOrWhiteSpace(param.Reason))
            {
                sb.AppendLine($"Reason:{param.Reason} is invalid value.");
            }

            // 更新用データに補完
            updateRecord = new AttendanceRecords()
            {
                AttendanceId = attendance.AttendanceId,
                UserCode = attendance.UserCode,
                WorkDate = attendance.WorkDate,
                ClockInDateTime = clockInUtcDateTime,
                ClockOutDateTime = clockOutUtcDateTime ?? null,
                BreakInDateTime = breakInUtcDateTime ?? null,
                BreakOutDateTime = breakOutUtcDateTime ?? null,
                WorkingStyleState = param.WorkingStyleState,
            };

            DateTime workDay = DateTimeUtility.ConvertDateOnlyToDate(updateRecord.WorkDate);

            // 出勤時刻
            DateTime clockInUserDateTime = DateTimeUtility.ConvertUtcDateTimeToUserDateTime(updateRecord.ClockInDateTime, timeZone);
            if (clockInUserDateTime < workDay)
            {
                sb.AppendLine($"ClockInDateTime:{DateTimeUtility.ConvertToString(clockInUserDateTime, DateTimeUtility.FormatInputDateTime)} is invalid value.");
            }

            // 退勤時刻
            DateTime? clockOutUserDateTime = updateRecord.ClockOutDateTime.HasValue ? DateTimeUtility.ConvertUtcDateTimeToUserDateTime(updateRecord.ClockOutDateTime.Value, timeZone) : null;
            if (clockOutUserDateTime.HasValue
                && clockOutUserDateTime.Value < clockInUserDateTime)
            {
                sb.AppendLine($"ClockOutDateTime:{DateTimeUtility.ConvertToString(clockOutUserDateTime.Value, DateTimeUtility.FormatInputDateTime)} is invalid value.");
            }

            // 開始時刻
            DateTime? breakInUserDateTime = updateRecord.BreakInDateTime.HasValue ? DateTimeUtility.ConvertUtcDateTimeToUserDateTime(updateRecord.BreakInDateTime.Value, timeZone) : null;
            if (breakInUserDateTime.HasValue
                && ((breakInUserDateTime.Value < clockInUserDateTime)
                    || (clockOutUserDateTime.HasValue && clockOutUserDateTime.Value < breakInUserDateTime.Value)))
            {
                sb.AppendLine($"BreakInDateTime:{DateTimeUtility.ConvertToString(breakInUserDateTime.Value, DateTimeUtility.FormatInputDateTime)} is invalid value.");
            }

            // 終了時刻
            DateTime? breakOutUserDateTime = updateRecord.BreakOutDateTime.HasValue ? DateTimeUtility.ConvertUtcDateTimeToUserDateTime(updateRecord.BreakOutDateTime.Value, timeZone) : null;
            if (breakOutUserDateTime.HasValue
                && ((breakOutUserDateTime.Value < clockInUserDateTime)
                    || (breakInUserDateTime.HasValue && breakOutUserDateTime.Value < breakInUserDateTime.Value)
                    || (breakInUserDateTime.HasValue && breakOutUserDateTime.Value < breakInUserDateTime.Value)
                    || (clockOutUserDateTime.HasValue && clockOutUserDateTime.Value < breakOutUserDateTime.Value)))
            {
                sb.AppendLine($"BreakOutDateTime:{DateTimeUtility.ConvertToString(breakOutUserDateTime.Value, DateTimeUtility.FormatInputDateTime)} is invalid value.");
            }

            return sb.ToString();
        }

        #endregion
    }
}
