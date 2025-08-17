/** npm */
import { Container, Row } from "react-bootstrap";
import { Outlet } from "react-router-dom";

/** consts */
import viewRouteAttendanceConsts from "../../consts/viewRouteAttendanceConsts";

/** provider */
import { useLoading } from "../../provider/LoadingProvider";

/** components */
import MessageModal from "../../components/MessageModal"
import MenuButton from "./components/MenuButton"
import ViewTitle from "../App/components/ViewTitle";

/** types */
import AttendanceLogic from "./logic/attendanceLogic";

/**
 * 勤怠メニュー
 * @returns コンポーネント
 */
const AttendanceMenu = () => {

    const { setLoading } = useLoading();
    const { confirm: messageDialog, dialogElement: messageModalElement } = MessageModal();

    const logic = new AttendanceLogic();

    /** 勤怠登録状態 */
    const onActionClockInState = async () => {

        /** スピナー表示 */
        setLoading(true);

        const result = await logic.clockInState();

        setLoading(false);

        if (result.isSuccess
            && result.message == "") {
            onActionClockIn();
        } else {

            /** ダイアログ表示 */
            messageDialog({
                title: result.messageTitle ?? "",
                message: result.message ?? "",
                onOk: () => {
                    if (result.isSuccess) {
                        onActionClockIn();
                    }
                }
            });
        }
    };

    /** 出勤登録 */
    const onActionClockIn = async () => {

        setLoading(true);

        const result = await logic.ClockIn();

        setLoading(false);

        messageDialog({
            title: result.messageTitle ?? "",
            message: result.message ?? "",
        });
    };

    /** 退勤登録 */
    const onActionClockOut = async () => {

        setLoading(true);

        const result = await logic.ClockOut();

        setLoading(false);

        messageDialog({
            title: result.messageTitle ?? "",
            message: result.message ?? "",
        });
    };

    return (
        <>
            <Container fluid className="view__area">
                <div className="p-3">
                    <ViewTitle title={logic._msg.getMsg("TextAttendanceMenu")} />
                    <Row>
                        <MenuButton
                            title={logic._msg.getMsg("TextClockIn")}
                            onAction={onActionClockInState}
                            imgPath="./images/clock_in.png"
                        />
                        <MenuButton
                            title={logic._msg.getMsg("TextClockOut")}
                            onAction={onActionClockOut}
                            imgPath="./images/clock_out.png"
                        />
                        <MenuButton
                            title={logic._msg.getMsg("TextAttendanceData")}
                            linkPath={viewRouteAttendanceConsts.AttendanceList}
                            imgPath="./images/maintenance.png"
                        />
                    </Row>
                </div>
            </Container>
            <Outlet />
            {messageModalElement}
        </>
    )
}

/** エクスポート */
export default AttendanceMenu;