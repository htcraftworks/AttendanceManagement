/** utils */
import DateUtility from "../../../utils/dateUtility";

/** consts */
import HttpStatusCodeConsts from "../../../consts/httpStatusCodeConsts";
import TimeZoneConsts from "../../../consts/timeZoneConsts"

/** services */
import apisService from "../../../services/api/apisService";

/** types */
import { type AttendanceUpdateParam } from "../../../services/api/types/param/AttendanceUpdateParam";
import { type AttendanceClockInOutParam } from "../../../services/api/types/param/AttendanceClockInOutParam";
import { type AttendanceDeleteParam } from "../../../services/api/types/param/AttendanceDeleteParam";
import { type AttendanceGetForMonthParam } from "../../../services/api/types/param/AttendanceGetForMonthParam";
import { type AttendanceGetParam } from "../../../services/api/types/param/AttendanceGetParam";
import { type UILogicResultModel } from "../../App/types/UILogicResultModel";
import { type AttendanceHistoryModel } from "../types/AttendanceHistoryModel";
import { type AttendanceListModel } from "../types/AttendanceListModel";
import { type AttendanceUpdateInputModel } from "../types/AttendanceUpdateInputModel";
import { type ServiceResult } from "../../../services/api/types/result/ServiceResult";
import { type AttendanceRecordsResult } from "../../../services/api/types/result/AttendanceRecordsResult";

/** class */
import LogicBase from "../../App/logic/logicBase";

/** 
 * 勤怠ロジック
 * NOTE:メッセージリソースがhookを使用してるので動的クラスとする
 *  */
export default class AttendanceLogic extends LogicBase {

    /**
     * 勤怠データ取得
     * @param attendanceGetParam 取得条件
     * @returns 結果
     */
    async AttendanceGet(attendanceGetParam: AttendanceGetParam): Promise<UILogicResultModel<AttendanceUpdateInputModel>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";
        let textError = this._msg.getMsg("TextError");
        let attendance: AttendanceUpdateInputModel | null = null;

        /** 勤怠データ取得 */
        await apisService.Attendances.attendanceGet(attendanceGetParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess && resultData.value) {
                    isSuccess = true;

                    attendance = {
                        attendanceId: resultData.value.attendanceId,
                        userCode: resultData.value.userCode,
                        workDate: DateUtility.convertUnixTimeToUserDateTime(resultData.value.workDateUserUnixTimeSec, TimeZoneConsts.Tokyo),
                        clockInDateTime: DateUtility.convertUnixTimeToUserDateTime(resultData.value.clockInDateUtcUnixTimeSec, TimeZoneConsts.Tokyo),
                        clockOutDateTime: resultData.value.clockOutDateUtcUnixTimeSec ?
                            DateUtility.convertUnixTimeToUserDateTime(resultData.value.clockOutDateUtcUnixTimeSec, TimeZoneConsts.Tokyo) : null,
                        breakInDateTime: resultData.value.breakInDateUtcUnixTimeSec ?
                            DateUtility.convertUnixTimeToUserDateTime(resultData.value.breakInDateUtcUnixTimeSec, TimeZoneConsts.Tokyo) : null,
                        breakOutDateTime: resultData.value.breakOutDateUtcUnixTimeSec ?
                            DateUtility.convertUnixTimeToUserDateTime(resultData.value.breakOutDateUtcUnixTimeSec, TimeZoneConsts.Tokyo) : null,
                        workingStyleState: resultData.value.workingStyleState,
                        reason: ""
                    } as AttendanceUpdateInputModel;
                } else {
                    messageTitle = textError;
                    message = resultData.errorMessage;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<AttendanceRecordsResult>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message,
            value: attendance
        } as UILogicResultModel<AttendanceUpdateInputModel>;
    }

    /**
     * 月別勤怠データ取得
     * @param attendanceGetForMonthParam 取得条件
     * @returns 結果
     */
    async AttendanceGetForMonth(attendanceGetForMonthParam: AttendanceGetForMonthParam): Promise<UILogicResultModel<AttendanceListModel[]>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";
        let textError = this._msg.getMsg("TextError");
        let attendanceArray: AttendanceListModel[] | null = null;

        /** 勤怠データ取得 */
        await apisService.Attendances.attendanceGetForMonth(attendanceGetForMonthParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess
                    && resultData.value
                    && resultData.value.length > 0) {

                    isSuccess = true;

                    let tmpAttendanceModelArray: AttendanceListModel[] = [];
                    resultData.value.forEach((r) => {

                        let tmpAttendanceHistoryModelArray: AttendanceHistoryModel[] = [];

                        if (r.history) {

                            r.history.forEach((h) => {
                                let tmpAttendancehistoryModel: AttendanceHistoryModel = {
                                    generateDateTime: DateUtility.convertUnixTimeToUserDateTime(h.generateDateUtcUnixTimeSec, TimeZoneConsts.Tokyo) ?? null,
                                    reason: h.reason
                                };

                                tmpAttendanceHistoryModelArray.push(tmpAttendancehistoryModel);
                            });
                        }

                        let tmpAttendanceModel: AttendanceListModel = {
                            attendanceId: r.attendanceId,
                            userCode: r.userCode,
                            workDate: DateUtility.convertUnixTimeToUtcDateTime(r.workDateUserUnixTimeSec),
                            clockInDateTime: DateUtility.convertUnixTimeToUserDateTime(r.clockInDateUtcUnixTimeSec, TimeZoneConsts.Tokyo),
                            clockOutDateTime: r.clockOutDateUtcUnixTimeSec ?
                                DateUtility.convertUnixTimeToUserDateTime(r.clockOutDateUtcUnixTimeSec, TimeZoneConsts.Tokyo) : null,
                            breakInDateTime: r.breakInDateUtcUnixTimeSec ?
                                DateUtility.convertUnixTimeToUserDateTime(r.breakInDateUtcUnixTimeSec, TimeZoneConsts.Tokyo) : null,
                            breakOutDateTime: r.breakOutDateUtcUnixTimeSec ?
                                DateUtility.convertUnixTimeToUserDateTime(r.breakOutDateUtcUnixTimeSec, TimeZoneConsts.Tokyo) : null,
                            workingStyleState: r.workingStyleState,
                            history: tmpAttendanceHistoryModelArray ?? null
                        };

                        tmpAttendanceModelArray.push(tmpAttendanceModel);
                    });

                    attendanceArray = tmpAttendanceModelArray;
                } else {
                    messageTitle = `${textError}[ErrorCode:${resultData.errorCode}]`;
                    message = `${resultData.errorMessage}`;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<AttendanceRecordsResult[]>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message,
            value: attendanceArray
        } as UILogicResultModel<AttendanceListModel[]>;
    }

    /**
      * 勤怠登録状態
      * @returns 結果
      */
    async clockInState(): Promise<UILogicResultModel<null>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";
        let textError = this._msg.getMsg("TextError");

        /** 出勤 */
        await apisService.Attendances.attendanceClockInState().then(result => {
            const resultData = result.data;
            if (resultData.isSuccess) {
                if (result.statusCode === HttpStatusCodeConsts.Accepted) {
                    /** 直前の勤怠が未退勤の場合確認 */
                    isSuccess = true;
                    messageTitle = this._msg.getMsg("TextConfirm");
                    message = this._msg.getMsg("MessageConfirmAttendanceExistData");
                }
                else if (resultData.value) {
                    isSuccess = true;
                } else {
                    messageTitle = textError;
                    message = resultData.errorMessage;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            console.log(error);

            const resultData = error.response.data as ServiceResult<boolean>;
            if (resultData) {
                messageTitle = `${textError}[ErrorCode:${resultData.errorCode}]`;
                message = `${resultData.errorMessage}`;
            } else {
                messageTitle = `${textError}[ErrorCode:${error.status}]`;
                message = this._msg.getMsg("MessageCommunicationError");
            }
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message
        } as UILogicResultModel<null>;
    }

    /**
      * 出勤
      * @returns 結果
      */
    async ClockIn(): Promise<UILogicResultModel<null>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";
        let textError = this._msg.getMsg("TextError");

        let attendanceClockInOutParam: AttendanceClockInOutParam = {
            clockInOutDateTimeUserUnixTimeSec: DateUtility.getUnixTime().toString()
        };

        /** 出勤 */
        await apisService.Attendances.attendanceClockIn(attendanceClockInOutParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess) {
                    isSuccess = true;

                    messageTitle = this._msg.getMsg("TextInfo");
                    message = this._msg.getMsg("MessageAttendanceClockIn");
                } else {
                    messageTitle = textError;
                    message = resultData.errorMessage;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<boolean>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message
        } as UILogicResultModel<null>;
    }

    /**
      * 退勤
      * @returns 結果
      */
    async ClockOut(): Promise<UILogicResultModel<null>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";
        let textError = this._msg.getMsg("TextError");

        let attendanceClockInOutParam: AttendanceClockInOutParam = {
            clockInOutDateTimeUserUnixTimeSec: DateUtility.getUnixTime().toString()
        };

        /** 退勤 */
        await apisService.Attendances.attendanceClockOut(attendanceClockInOutParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess) {
                    isSuccess = true;

                    messageTitle = this._msg.getMsg("TextInfo");
                    message = this._msg.getMsg("MessageAttendanceClockOut");
                } else {
                    messageTitle = textError;
                    message = resultData.errorMessage;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<boolean>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message
        } as UILogicResultModel<null>;
    }

    /**
      * 勤怠情報更新
      * @param attendanceUpdate 更新前データ
      * @param attendanceUpdateModel 更新後データ
      * @returns 結果
      */
    async AttendanceUpdate(attendanceUpdate: AttendanceUpdateInputModel, attendanceUpdateModel: AttendanceUpdateInputModel): Promise<UILogicResultModel<AttendanceUpdateInputModel>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";
        let textError = this._msg.getMsg("TextError");

        // 検証（細かいチェックは省略）
        if ((attendanceUpdate?.attendanceId == attendanceUpdateModel.attendanceId)
            && (attendanceUpdate?.userCode == attendanceUpdateModel.userCode)
            && (attendanceUpdate?.clockInDateTime?.getTime() == attendanceUpdateModel.clockInDateTime?.getTime())
            && (attendanceUpdate?.clockOutDateTime?.getTime() == attendanceUpdateModel.clockOutDateTime?.getTime())
            && (attendanceUpdate?.breakInDateTime?.getTime() == attendanceUpdateModel.breakInDateTime?.getTime())
            && (attendanceUpdate?.breakOutDateTime?.getTime() == attendanceUpdateModel.breakOutDateTime?.getTime())
            && (attendanceUpdate?.workingStyleState == attendanceUpdateModel.workingStyleState)) {
            return {
                isSuccess: isSuccess,
                messageTitle: textError,
                message: this._msg.getMsg("MessageValidateAttendanceUpdate")
            } as UILogicResultModel<AttendanceUpdateInputModel>;
        }

        let attendance: AttendanceUpdateInputModel | null = null;

        let attendanceUpdateParam: AttendanceUpdateParam = {
            attendanceId: attendanceUpdateModel.attendanceId,
            userCode: attendanceUpdateModel.userCode,
            clockInUtcDateUnixTimeSec: attendanceUpdateModel.clockInDateTime ?
                DateUtility.convertDateTimeToUnixTimeWithTimeZone(attendanceUpdateModel.clockInDateTime, true).toString() : null,
            clockOutUtcDateUnixTimeSec: attendanceUpdateModel.clockOutDateTime ?
                DateUtility.convertDateTimeToUnixTimeWithTimeZone(attendanceUpdateModel.clockOutDateTime, true).toString() : null,
            breakInDateUtcUnixTimeSec: attendanceUpdateModel.breakInDateTime ?
                DateUtility.convertDateTimeToUnixTimeWithTimeZone(attendanceUpdateModel.breakInDateTime, true).toString() : null,
            breakOutDateUtcUnixTimeSec: attendanceUpdateModel.breakOutDateTime ?
                DateUtility.convertDateTimeToUnixTimeWithTimeZone(attendanceUpdateModel.breakOutDateTime, true).toString() : null,
            workingStyleState: attendanceUpdateModel.workingStyleState ?? 0,
            reason: attendanceUpdateModel.reason ?? ""
        };

        /** 勤怠情報更新 */
        await apisService.Attendances.attendanceUpdate(attendanceUpdateParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess) {
                    isSuccess = true;

                    messageTitle = this._msg.getMsg("TextInfo");
                    message = this._msg.getMsg("MessageAttendanceDateUpdate");
                } else {
                    messageTitle = `${textError}[ErrorCode:${resultData.errorCode}]`;
                    message = `${resultData.errorMessage}`;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<boolean>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message,
            value: attendance
        } as UILogicResultModel<AttendanceUpdateInputModel>;
    }

    /**
  * 勤怠情報削除
  * @param attendanceDeleteParam 削除パラメータ
  * @returns 結果
  */
    async AttendanceDelete(attendanceDeleteParam: AttendanceDeleteParam): Promise<UILogicResultModel<null>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";
        let textError = this._msg.getMsg("TextError");

        /** 勤怠情報削除 */
        await apisService.Attendances.attendanceDelete(attendanceDeleteParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess) {
                    isSuccess = true;

                    messageTitle = this._msg.getMsg("TextInfo");
                    message = this._msg.getMsg("MessageAttendanceDateDelete");
                } else {
                    messageTitle = `${textError}[ErrorCode:${resultData.errorCode}]`;
                    message = `${resultData.errorMessage}`;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<boolean>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message
        } as UILogicResultModel<null>;
    }
}