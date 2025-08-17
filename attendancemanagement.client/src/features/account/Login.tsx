/** npm */
import { Formik } from "formik";
import { useEffect, useRef } from "react";
import { Form } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { Col, Container, Row } from "react-bootstrap";

/** consts */
import viewRouteAccountConsts from "../../consts/viewRouteAccountConsts";
import viewRouteAttendanceConsts from "../../consts/viewRouteAttendanceConsts";

/** utils */
import yupExtendsUtility from "../../utils/yupExtendsUtility";

/** provider */
import { useLoading } from "../../provider/LoadingProvider";
import { useAccount } from '../../provider/AccountProvider';

/** components */
import MessageModal from "../../components/MessageModal";
import TextInputGeneral from "../../components/TextInputGeneral";
import ButtonGeneral from "../../components/ButtonGeneral";

/** types */
import { type LoginModel } from "./types/LoginModel";
import { type LoginParam } from "../../services/api/types/param/LoginParam";
import AccountLogic from "./logic/accountLogic";

/**
 * ログイン画面
 * @param setUserModel ユーザー情報
 * @returns コンポーネント
 */
const Login = () => {

    const navigate = useNavigate();
    const { setLoading } = useLoading();
    const { setUser } = useAccount();

    const {
        confirm: messageDialog,
        dialogElement: messageModalElement,
    } = MessageModal();

    const loginModel: LoginModel =
    {
        userCode: "",
        password: "",
        error: null
    };

    const isFirstRender = useRef(true);

    const logic = new AccountLogic();

    useEffect(() => {
        if (isFirstRender.current) {

            setLoading(true);

            /** StrictMode対策 */
            isFirstRender.current = false;

            // 同期処理
            const onActionCurrentUser = async () => {
                try {
                    const result = await logic.CurrentUser();

                    /** ダイアログ表示 */
                    if (result.value) {
                        setUser(result.value);
                        navigate(viewRouteAttendanceConsts.AttendanceMenu);
                    }
                } catch (error) {
                    console.error(error);
                } finally {
                    setLoading(false);
                }
            };

            void onActionCurrentUser();
        }
    }, []);

    /**
     * ログイン
     * @param loginModel ユーザー情報
     */
    const onActionLogin = async (loginModel: LoginModel) => {

        /** スピナー表示 */
        setLoading(true);

        let loginParam: LoginParam = {
            userCode: loginModel.userCode,
            password: loginModel.password
        };

        const result = await logic.Login(loginParam);

        setLoading(false);

        /** ダイアログ表示 */
        if (!result.isSuccess
            || !result.value) {

            messageDialog({
                title: result.messageTitle ?? "",
                message: result.message ?? "",
            });
        } else {
            setUser(result.value);
            navigate(viewRouteAttendanceConsts.AttendanceMenu);
        }
    };

    return (
        <>
            <Container fluid className="login__view view__area d-flex">
                <Row className="form_with_advert__zone bg-success text-center">
                    <Col md={4} className="form_with_advert_advert__section text-center">
                        <img className="img-fluid" src="./images/attendance_logo.png" />
                        <h4>My software makes you happy.</h4>
                    </Col>
                    <Col md={8} xs={12} sm={12} className="form_with_advert_input__section">
                        <Row>
                            <Col><h2 className="form_with_advert_form__title">{logic._msg.getMsg("TextLogin")}</h2></Col>
                        </Row>
                        <Row>
                            <Formik
                                enableReinitialize
                                initialValues={loginModel}
                                validationSchema={yupExtendsUtility.object().shape({
                                    userCode: yupExtendsUtility.string().label(logic._msg.getMsg("TextUserId")).required(logic._msg.getMsg("MessageInputRequired")).max(6, logic._msg.getMsgConvert("MessageInputMax", ["6"])).hanAlphaNumber(),
                                    password: yupExtendsUtility.string().label(logic._msg.getMsg("TextPassword")).required(logic._msg.getMsg("MessageInputRequired")).max(7, logic._msg.getMsgConvert("MessageInputMax", ["7"])).hanAlphaNumber()
                                })}
                                onSubmit={onActionLogin}>
                                {({ handleSubmit, isSubmitting, isValid, dirty}) => (
                                    <Form className="form-group form_with_advert_form__group" onSubmit={handleSubmit} autoComplete="off">
                                        <Row>
                                            <Col>
                                                <TextInputGeneral className="form_with_advert_form__input" name="userCode" placeholder={logic._msg.getMsg("TextUserId")} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col>
                                                <TextInputGeneral className="form_with_advert_form__input" name="password" placeholder={logic._msg.getMsg("TextPassword")} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col className="my-3 mx-5">
                                                <ButtonGeneral
                                                    type="submit"
                                                    className="green"
                                                    titleLabel={logic._msg.getMsg("TextLogin")}
                                                    disabled={!isValid || !dirty || isSubmitting} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col className="m-3">
                                                <p>
                                                    <Link to={viewRouteAccountConsts.UserCreate}>{logic._msg.getMsg("TextAccountCreate")}</Link>
                                                </p>
                                            </Col>
                                        </Row>
                                    </Form>
                                )}
                            </Formik>
                        </Row>
                    </Col>
                </Row>
            </Container>
            {messageModalElement}
        </>
    );
}

/** エクスポート */
export default Login;