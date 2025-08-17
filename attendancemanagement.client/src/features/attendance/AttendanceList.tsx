/** CSS */
import "./css/AttendanceList.css";

/** npm */
import { Formik } from "formik";
import { useEffect, useRef, useState } from "react";
import { Col, Container, Form, Row } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

/** consts */
import WorkingStyleStateConsts from "../../consts/workingStyleStateConsts";
import viewRouteAttendanceConsts from "../../consts/viewRouteAttendanceConsts";
import TimeZoneConsts from "../../consts/timeZoneConsts"

/** utils */
import DateUtility from "../../utils/dateUtility";
import yupExtendsUtility from "../../utils/yupExtendsUtility";

/** provider */
import { useLoading } from "../../provider/LoadingProvider";

/** components */
import DateInputGeneral from "../../components/DateInputGeneral";
import ViewTitle from "../App/components/ViewTitle";
import ButtonGeneral from "../../components/ButtonGeneral";

/** types */
import { type AttendanceListModel } from "./types/AttendanceListModel";
import MessageModal from "../../components/MessageModal";
import { type AttendanceGetForMonthParam } from "../../services/api/types/param/AttendanceGetForMonthParam";
import { type DictionaryModel } from "../../types/DictionaryModel";
import { type AttendanceListInputModel } from "./types/AttendanceListInputModel";
import AttendanceLogic from "./logic/attendanceLogic";

/**
 * 勤怠一覧
 * @returns コンポーネント
 */
const AttendanceList = () => {

    const navigate = useNavigate();
    const isFirstRender = useRef(true);
    const { setLoading } = useLoading();
    const [targetMonth, setTargetMonth] = useState<Date>(DateUtility.getUserTimeNowForMonth(TimeZoneConsts.Tokyo));
    const [attendances, setAttendances] = useState<AttendanceListModel[]>();
    const [isHistoryHidden, setIsHistoryHidden] = useState<boolean>(true);

    const { confirm: messageDialog, dialogElement: messageModalElement } = MessageModal();

    const logic = new AttendanceLogic();

    const initFormData: AttendanceListInputModel =
    {
        displayMonth: targetMonth
    };

    const workingStyleStatesDic: DictionaryModel[] = [
        { key: WorkingStyleStateConsts.Office.state.toString(), value: logic._msg.getMsg(WorkingStyleStateConsts.Office.messageResourceKey) },
        { key: WorkingStyleStateConsts.GoOut.state.toString(), value: logic._msg.getMsg(WorkingStyleStateConsts.GoOut.messageResourceKey) },
        { key: WorkingStyleStateConsts.Home.state.toString(), value: logic._msg.getMsg(WorkingStyleStateConsts.Home.messageResourceKey) }
    ];


    useEffect(() => {
        if (isFirstRender.current) {

            /** StrictMode対策 */
            isFirstRender.current = false;

            onActionGet({ displayMonth: targetMonth });
        }
    }, []);

    /** 勤怠情報取得 */
    const onActionGet = async (value: AttendanceListInputModel) => {

        setLoading(true);
        setTargetMonth(value.displayMonth);
        setAttendances([]);

        let attendanceGetForMonthParam: AttendanceGetForMonthParam = {
            targetDateUtcUnixTimeSec: DateUtility.convertDateTimeToUnixTimeWithTimeZone(value.displayMonth, true).toString()
        };

        /** 勤怠情報更新 */
        const result = await logic.AttendanceGetForMonth(attendanceGetForMonthParam);

        /** スピナー非表示 */
        setLoading(false);

        if (!result.isSuccess
            || !result.value) {

            /** ダイアログ表示 */
            messageDialog({
                title: result.messageTitle ?? "",
                message: result.message ?? "",
            });
        } else {
            setAttendances(result.value);
        }
    };

    return (
        <>
            <Container fluid className="attendance_list__view view__area">
                <div className="p-3">

                    <ViewTitle title={logic._msg.getMsg("TextDailyAttendanceData")} />

                    <Row className="card my-3">
                        <Row className="m-0 py-1 px-3">
                            <Formik
                                enableReinitialize
                                initialValues={initFormData}
                                validationSchema={yupExtendsUtility.object().shape({
                                    displayMonth: yupExtendsUtility.date().label(logic._msg.getMsg("TextDisplayMonth")).required(logic._msg.getMsg("MessageInputRequired"))
                                })}
                                onSubmit={values => onActionGet(values)}>
                                {({ handleSubmit, isSubmitting, isValid, dirty }) => (
                                    <Form className="form-group attendance_search_form__group" onSubmit={handleSubmit} autoComplete="off">
                                        <Row className="d-flex align-items-center py-3">
                                            <Col xs={12} sm={8} md={9} lg={10} className="d-flex justify-content-start align-items-center m-0 py-0 px-2">
                                                <div className="attendance_search_form_title fw-medium mx-2">{logic._msg.getMsg("TextDisplayMonth")}</div>
                                                <DateInputGeneral
                                                    value={targetMonth.toDateString()}
                                                    placeholderText="yyyy/MM"
                                                    name="displayMonth"
                                                    dateFormat="yyyy/MM"
                                                    showMonthYearPicker />
                                                <ButtonGeneral
                                                    type="submit"
                                                    titleLabel={logic._msg.getMsg("TextSearch")}
                                                    className="siann mx-2"
                                                    disabled={!isValid || !dirty || isSubmitting} />
                                            </Col>
                                            <Col xs={6} sm={4} md={3} lg={2} className="align-self-center">

                                                <ButtonGeneral
                                                    type="button"
                                                    titleLabel={!isHistoryHidden ? logic._msg.getMsg("TextHistoryHide") : logic._msg.getMsg("TextHistoryDisplay")}
                                                    className="siann"
                                                    onClick={() => setIsHistoryHidden(!isHistoryHidden)} />
                                            </Col>
                                        </Row>
                                    </Form>
                                )}
                            </Formik>
                        </Row>
                        <Row className="m-0 pb-4 px-4">
                            <Row className="attendance_list_table_header__group m-0 p-0">
                                <Col xs={2} className="attendance_list_table_header d-flex align-items-center justify-content-center grid-item">
                                    <div className="text-center fw-medium vertical-item">
                                        {logic._msg.getMsg("TextWorkDate")}
                                    </div>
                                </Col>
                                <Col xs={2} className="attendance_list_table_header d-flex align-items-center justify-content-center grid-item">
                                    <div className="text-center fw-medium vertical-item">
                                        {logic._msg.getMsg("TextWorkingStyleState")}
                                    </div>
                                </Col>
                                <Col xs={2} className="attendance_list_table_header d-flex align-items-center justify-content-center grid-item">
                                    <div className="text-center fw-medium vertical-item">
                                        {logic._msg.getMsg("TextClockInDate")}
                                    </div>
                                </Col>
                                <Col xs={2} className="attendance_list_table_header d-flex align-items-center justify-content-center grid-item">
                                    <div className="text-center fw-medium vertical-item">
                                        {logic._msg.getMsg("TextClockOutDate")}
                                    </div>
                                </Col>
                                <Col xs={2} className="attendance_list_table_header d-flex align-items-center justify-content-center grid-item">
                                    <div className="text-center fw-medium vertical-item">
                                        {logic._msg.getMsg("TextBreakInDate")}
                                    </div>
                                </Col>
                                <Col xs={2} className="attendance_list_table_header d-flex align-items-center justify-content-center grid-item">
                                    <div className="text-center fw-medium vertical-item">
                                        {logic._msg.getMsg("TextBreakInDate")}
                                    </div>
                                </Col>
                            </Row>
                            {attendances && attendances.map((attendance) => (
                                <Row className="attendance_list_table_body__group m-0 p-0" key={attendance.attendanceId}>
                                    <Row className="attendance_list_table_body_items_row m-0 p-0" onClick={() => {
                                        navigate(viewRouteAttendanceConsts.AttendanceEdit, {
                                            state: {
                                                attendanceId: attendance.attendanceId
                                            }
                                        })
                                    }}>
                                        <Col xs={2} className="attendance_list_table_body_item d-flex align-items-center justify-content-center grid-item">
                                            <div className="text-center fw-light vertical-item">
                                                {attendance.workDate ? DateUtility.convertToString(attendance.workDate, logic._msg.getMsg("FormatDateYMD")) : ""}
                                            </div>
                                        </Col>
                                        <Col xs={2} className="attendance_list_table_body_item d-flex align-items-center justify-content-center grid-item">
                                            <div className="text-center fw-light vertical-item">
                                                {workingStyleStatesDic.find(r => r.key == attendance.workingStyleState.toString())?.value ?? ""}
                                            </div>
                                        </Col>
                                        <Col xs={2} className="attendance_list_table_body_item d-flex align-items-center justify-content-center grid-item">
                                            <div className="text-center fw-light vertical-item">
                                                {attendance.clockInDateTime ? DateUtility.convertToString(attendance.clockInDateTime, DateUtility.FORMAT_DATE_TIME) : ""}
                                            </div>
                                        </Col>
                                        <Col xs={2} className="attendance_list_table_body_item d-flex align-items-center justify-content-center grid-item">
                                            <div className="text-center fw-light vertical-item">
                                                {attendance.clockOutDateTime ? DateUtility.convertToString(attendance.clockOutDateTime, DateUtility.FORMAT_DATE_TIME) : ""}
                                            </div>
                                        </Col>
                                        <Col xs={2} className="attendance_list_table_body_item d-flex align-items-center justify-content-center grid-item">
                                            <div className="text-center fw-light vertical-item">
                                                {attendance.breakInDateTime ? DateUtility.convertToString(attendance.breakInDateTime, DateUtility.FORMAT_DATE_TIME) : ""}
                                            </div>
                                        </Col>
                                        <Col xs={2} className="attendance_list_table_body_item d-flex align-items-center justify-content-center grid-item">
                                            <div className="text-center fw-light vertical-item">
                                                {attendance.breakOutDateTime ? DateUtility.convertToString(attendance.breakOutDateTime, DateUtility.FORMAT_DATE_TIME) : ""}
                                            </div>
                                        </Col>
                                    </Row>
                                    {!isHistoryHidden && attendance.history && attendance.history.map((history, index) => (
                                        <Row className="attendance_list_table_body_history_items_row m-0 p-0 m-0 p-0" key={`${attendance.attendanceId}_${index}`}>
                                            <Col xs={2} className="attendance_list_table_body_history_item d-flex align-items-center justify-content-center grid-item">
                                                <div className="text-center fw-light vertical-item">
                                                    {logic._msg.getMsg("TextUpdateHistory")}
                                                </div>
                                            </Col>
                                            <Col xs={3} className="attendance_list_table_body_history_item d-flex align-items-center justify-content-center grid-item">
                                                <div className="text-center fw-light vertical-item">
                                                    {history.generateDateTime
                                                        ? DateUtility.convertToString(history.generateDateTime, DateUtility.FORMAT_DATE_TIME)
                                                        : ""}
                                                </div>
                                            </Col>
                                            <Col xs={7} className="attendance_list_table_body_history_item d-flex align-items-center justify-content-start grid-item">
                                                <div className="text-center fw-light">
                                                    {history.reason}
                                                </div>
                                            </Col>
                                        </Row>
                                    ))}
                                </Row>
                            ))}
                        </Row>
                    </Row>

                </div >
            </Container >
            {messageModalElement}
        </>
    )
}

/** エクスポート */
export default AttendanceList;