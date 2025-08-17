/** メインメニュー関連APIルート */
export default class ApiRouteAttendanceConsts {

    /** 親ルート */
    private static MAIN_MENU_ROUTE: string = "/mainmenu";

    /** 出勤登録状況機能 */
    static readonly AttendanceClockInState: string = ApiRouteAttendanceConsts.MAIN_MENU_ROUTE + "/AttendanceClockInState";

    /** 出勤登録機能 */
    static readonly AttendanceClockIn: string = ApiRouteAttendanceConsts.MAIN_MENU_ROUTE + "/attendanceclockin";

    /** 退勤登録機能 */
    static readonly AttendanceClockOut: string = ApiRouteAttendanceConsts.MAIN_MENU_ROUTE + "/attendanceclockout";
    
    /** 勤怠取得機能 */
    static readonly AttendanceGet: string = ApiRouteAttendanceConsts.MAIN_MENU_ROUTE + "/attendanceget";

    /** 勤怠取得機能 */
    static readonly AttendanceGetForMonth: string = ApiRouteAttendanceConsts.MAIN_MENU_ROUTE + "/attendancegetformonth";

    /** 勤怠編集機能 */
    static readonly AttendanceUpdate: string = ApiRouteAttendanceConsts.MAIN_MENU_ROUTE + "/attendanceupdate";

    /** 勤怠削除機能 */
    static readonly AttendanceDelete: string = ApiRouteAttendanceConsts.MAIN_MENU_ROUTE + "/attendancedelete";
}