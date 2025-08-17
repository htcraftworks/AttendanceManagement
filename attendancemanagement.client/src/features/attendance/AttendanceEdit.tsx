/** CSS */
import 'react-datepicker/dist/react-datepicker.css';
import "./css/AttendanceEdit.css";

/** npm */
import { Form, Formik } from "formik";
import { useEffect, useRef, useState } from "react";
import { Col, Container, Row, Table } from "react-bootstrap";
import { useLocation, useNavigate, Navigate } from "react-router-dom";

/** consts */
import WorkingStyleStateConsts from "../../consts/workingStyleStateConsts";
import viewRouteAttendanceConsts from "../../consts/viewRouteAttendanceConsts";

/** utils */
import yupExtendsUtility from "../../utils/yupExtendsUtility";
import DateUtility from "../../utils/dateUtility";

/** provider */
import { useLoading } from "../../provider/LoadingProvider";

/** components */
import SelectBoxGeneral from "../../components/SelectBoxGeneral";
import DateInputGeneral from "../../components/DateInputGeneral";
import TextInputGeneral from "../../components/TextInputGeneral";
import MessageModal from "../../components/MessageModal"
import ViewTitle from "../App/components/ViewTitle";
import ButtonGeneral from "../../components/ButtonGeneral";

/** types */
import { type AttendanceUpdateInputModel } from "./types/AttendanceUpdateInputModel";
import { type AttendanceGetParam } from "../../services/api/types/param/AttendanceGetParam";
import { type AttendanceDeleteParam } from "../../services/api/types/param/AttendanceDeleteParam";
import AttendanceLogic from "./logic/attendanceLogic";

/**
 * 勤怠情報更新
 * @returns コンポーネント
 */
const AttendanceEdit = () => {

    const navigate = useNavigate();
    const location = useLocation();
    const { setLoading } = useLoading();
    const logic = new AttendanceLogic();

    const {
        confirm: messageDialog,
        dialogElement: messageModalElement,
    } = MessageModal();

    const isFirstRender = useRef(true);
    const [attendanceUpdate, setAttendanceUpdate] = useState<AttendanceUpdateInputModel>();

    if (!location.state || !location.state.attendanceId) {
        /** 直接アクセス無効 */
        return <Navigate to={viewRouteAttendanceConsts.AttendanceList} />;
    }

    const attendanceId = location.state.attendanceId;



    useEffect(() => {
        if (isFirstRender.current) {

            /** StrictMode対策 */
            isFirstRender.current = false;

            onActionGet();
        }
    }, []);

    /** 初回勤怠データ取得 */
    const onActionGet = async () => {

        /** スピナー表示 */
        setLoading(true);

        let attendanceGetParam: AttendanceGetParam = {
            attendanceId: attendanceId
        };

        const result = await logic.AttendanceGet(attendanceGetParam);

        /** スピナー非表示 */
        setLoading(false);

        if (!result.isSuccess
            || !result.value) {

            /** ダイアログ表示 */
            messageDialog({
                title: result.messageTitle ?? "",
                message: result.message ?? "",
                onOk: () => {
                    navigate(viewRouteAttendanceConsts.AttendanceList);
                }
            });
        }
        else {
            setAttendanceUpdate(result.value);
        }
    };

    /**
     * 勤怠情報更新
     * @param attendanceUpdateModel 更新情報
     */
    const onActionUpdate = async (attendanceUpdateModel: AttendanceUpdateInputModel) => {

        setLoading(true);

        const result = await logic.AttendanceUpdate(attendanceUpdate as AttendanceUpdateInputModel, attendanceUpdateModel);

        setLoading(false);

        messageDialog({
            title: result.messageTitle ?? "",
            message: result.message ?? "",
            onOk: () => {
                if (result.isSuccess) {
                    navigate(viewRouteAttendanceConsts.AttendanceList);
                }
            }
        });
    };

    /** 
     * 削除確認
     * @param attendanceUpdateModel 削除情報
     */
    const onActionDeleteConfirm = (attendanceUpdateModel: AttendanceUpdateInputModel) => {
        messageDialog({
            contentProps: {
                hasClose: true,
            },
            title: logic._msg.getMsg("TextConfirm"),
            message: logic._msg.getMsg("MessageConfirmAttendanceDelete"),
            onOk: () => {
                onActionDelete(attendanceUpdateModel);
            }
        });
    };

    /**
     * 勤怠情報削除
     * @param attendanceUpdateModel 削除情報
     */
    const onActionDelete = async (attendanceUpdateModel: AttendanceUpdateInputModel) => {

        setLoading(true);

        let attendanceDeleteParam: AttendanceDeleteParam = {
            attendanceId: attendanceUpdateModel.attendanceId,
            reason: attendanceUpdateModel.reason ?? ""
        };

        const result = await logic.AttendanceDelete(attendanceDeleteParam);

        setLoading(false);

        messageDialog({
            title: result.messageTitle ?? "",
            message: result.message ?? "",
            onOk: () => {
                if (result.isSuccess) {
                    navigate(viewRouteAttendanceConsts.AttendanceList);
                }
            }
        });
    };

    /**
 * 勤怠情報削除
 * @param attendanceUpdateModel 削除情報
 */
    const deleteValidate = (attendanceUpdateModel: AttendanceUpdateInputModel) => {

        if (attendanceUpdateModel.reason) {
            return false;
        }

        return true;
    };

    return (
        <>
            {attendanceUpdate &&
                <Container fluid className="attendance_edit__view view__area">
                    <div className="p-3">

                        <ViewTitle title={logic._msg.getMsg("TextAttendanceUpdate")} />

                        <Row className="card my-3">
                            <div className="card-body table-responsive px-4">
                                <Formik
                                    enableReinitialize
                                    initialValues={attendanceUpdate}
                                    validationSchema={yupExtendsUtility.object().shape({
                                        workingStyleState: yupExtendsUtility.string().label(logic._msg.getMsg("TextWorkingStyleState")).required(logic._msg.getMsg("MessageInputRequired")).max(1, logic._msg.getMsgConvert("MessageInputMax", ["1"])),
                                        clockInDateTime: yupExtendsUtility.date().label(logic._msg.getMsg("TextClockIn")).required(logic._msg.getMsg("MessageInputRequired")),
                                        clockOutDateTime: yupExtendsUtility.date().label(logic._msg.getMsg("TextClockOut")).notRequired(),
                                        breakInDateTime: yupExtendsUtility.date().label(logic._msg.getMsg("TextBreakIn")).notRequired(),
                                        breakOutDateTime: yupExtendsUtility.date().label(logic._msg.getMsg("TextBreakOut")).notRequired(),
                                        reason: yupExtendsUtility.string().label(logic._msg.getMsg("TextChangeReason")).required(logic._msg.getMsg("MessageInputRequired")).max(100, logic._msg.getMsgConvert("MessageInputMax", ["100"]))
                                    })}
                                    onSubmit={values => {
                                        onActionUpdate(values)
                                    }}>
                                    {({ handleSubmit, isValid, dirty, values }) => (
                                        <Form onSubmit={handleSubmit} autoComplete="off">
                                            <Row>
                                                <Table className="table align-middle">
                                                    <tbody className="attendance_list_table_body__group">
                                                        <tr>
                                                            <th className="attendance_edit_table_header">
                                                                <div className="vertical-item d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextWorkDate")}
                                                                </div>
                                                            </th>
                                                            <td colSpan={4} className="attendance_edit_table_item fw-light">
                                                                {attendanceUpdate.workDate ? DateUtility.convertToString(attendanceUpdate.workDate, DateUtility.FORMAT_DATE) : ""}
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th className="attendance_edit_table_header">
                                                                <div className="vertical-item d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextWorkingStyleState")}<span className="text-danger">*</span>
                                                                </div>
                                                            </th>
                                                            <td colSpan={4} className="attendance_edit_table_item">
                                                                <SelectBoxGeneral name="workingStyleState" className="fw-light" options={
                                                                    [
                                                                        { key: WorkingStyleStateConsts.Office.state.toString(), value: logic._msg.getMsg(WorkingStyleStateConsts.Office.messageResourceKey) },
                                                                        { key: WorkingStyleStateConsts.GoOut.state.toString(), value: logic._msg.getMsg(WorkingStyleStateConsts.GoOut.messageResourceKey) },
                                                                        { key: WorkingStyleStateConsts.Home.state.toString(), value: logic._msg.getMsg(WorkingStyleStateConsts.Home.messageResourceKey) }
                                                                    ]
                                                                } />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th className="attendance_edit_table_header">
                                                                <div className="vertical-item d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextClockInOutDate")}
                                                                </div>
                                                            </th>
                                                            <td className="attendance_edit_table_sub__header">
                                                                <div className="vertical-item fw-light d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextClockIn")}<span className="text-danger">*</span>
                                                                </div>
                                                            </td>
                                                            <td className="attendance_edit_table_item">
                                                                <DateInputGeneral
                                                                    value={attendanceUpdate.clockInDateTime ? attendanceUpdate.clockInDateTime.toISOString() : ""}
                                                                    placeholderText="yyyy/MM/dd HH:mm"
                                                                    name="clockInDateTime"
                                                                    className="fw-light"
                                                                    showTimeSelect
                                                                    dateFormat="yyyy/MM/dd HH:mm"
                                                                    timeIntervals={15} />
                                                            </td>
                                                            <td className="attendance_edit_table_sub__header ">
                                                                <div className="vertical-item fw-light d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextClockOut")}
                                                                </div>
                                                            </td>
                                                            <td className="attendance_edit_table_item">
                                                                <DateInputGeneral
                                                                    value={attendanceUpdate.clockOutDateTime ? attendanceUpdate.clockOutDateTime.toISOString() : ""}
                                                                    placeholderText="yyyy/MM/dd HH:mm"
                                                                    name="clockOutDateTime"
                                                                    className="fw-light"
                                                                    showTimeSelect
                                                                    dateFormat="yyyy/MM/dd HH:mm"
                                                                    timeIntervals={15} />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th className="attendance_edit_table_header">
                                                                <div className="vertical-item d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextBreakIn")}
                                                                </div>
                                                            </th>
                                                            <td className="attendance_edit_table_sub__header">
                                                                <div className="vertical-item fw-light d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextStart")}
                                                                </div>
                                                            </td>
                                                            <td className="attendance_edit_table_item">
                                                                <DateInputGeneral
                                                                    value={attendanceUpdate.breakInDateTime ? attendanceUpdate.breakInDateTime.toISOString() : ""}
                                                                    placeholderText="yyyy/MM/dd HH:mm"
                                                                    name="breakInDateTime"
                                                                    className="fw-light"
                                                                    showTimeSelect
                                                                    dateFormat="yyyy/MM/dd HH:mm"
                                                                    timeIntervals={15} />
                                                            </td>
                                                            <td className="attendance_edit_table_sub__header">
                                                                <div className="vertical-item fw-light d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextEnd")}
                                                                </div>
                                                            </td>
                                                            <td className="attendance_edit_table_item">
                                                                <DateInputGeneral
                                                                    value={attendanceUpdate.breakOutDateTime ? attendanceUpdate.breakOutDateTime.toISOString() : ""}
                                                                    placeholderText="yyyy/MM/dd HH:mm"
                                                                    name="breakOutDateTime"
                                                                    className="fw-light"
                                                                    showTimeSelect
                                                                    dateFormat="yyyy/MM/dd HH:mm"
                                                                    timeIntervals={15} />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th className="attendance_edit_table_header">
                                                                <div className="vertical-item d-flex align-items-center">
                                                                    {logic._msg.getMsg("TextChangeReason")}<span className="text-danger">*</span>
                                                                </div>
                                                            </th>
                                                            <td colSpan={4} className="attendance_edit_table_item">
                                                                <TextInputGeneral placeholder="TEXT" name="reason" as="textarea" rows={3} />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </Table>
                                            </Row>
                                            <Row>
                                                <Col xs={{ span: 5, offset: 0 }} sm={{ span: 4, offset: 0 }} md={{ span: 3, offset: 0 }}>
                                                    <ButtonGeneral
                                                        type="button"
                                                        titleLabel={logic._msg.getMsg("TextDelete")}
                                                        className="red"
                                                        disabled={deleteValidate(values)}
                                                        onClick={() => onActionDeleteConfirm(values)}
                                                    />
                                                </Col>
                                                <Col xs={{ span: 5, offset: 2 }} sm={{ span: 4, offset: 4 }} md={{ span: 3, offset: 6 }}>
                                                    <ButtonGeneral
                                                        type="submit"
                                                        titleLabel={logic._msg.getMsg("TextUpdate")}
                                                        className="green"
                                                        disabled={!isValid || !dirty}
                                                    />
                                                </Col>
                                            </Row>

                                        </Form>
                                    )}
                                </Formik>


                            </div>
                        </Row>
                    </div>
                </Container>
            }
            {messageModalElement}
        </>
    )
}

/** エクスポート */
export default AttendanceEdit;