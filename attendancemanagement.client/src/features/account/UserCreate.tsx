/** npm */
import { Formik } from "formik";
import { Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";

/** consts */
import viewRouteAccountConsts from "../../consts/viewRouteAccountConsts";

/** utils */
import yupExtendsUtility from "../../utils/yupExtendsUtility";

/** provider */
import { useLoading } from "../../provider/LoadingProvider";

/** components */
import MessageModal from "../../components/MessageModal"
import TextInputGeneral from "../../components/TextInputGeneral";
import ButtonGeneral from "../../components/ButtonGeneral";

/** types */
import { type UserCreateModel } from "./types/UserCreateModel";
import { type UserCreateParam } from "../../services/api/types/param/UserCreateParam";
import AccountLogic from "./logic/accountLogic";

/**
 * アカウント作成画面
 * @returns コンポーネント
 */
const UserCreate = () => {

    const navigate = useNavigate();
    const { setLoading } = useLoading();
    const { confirm: messageDialog, dialogElement: messageModalElement } = MessageModal();

    const userCreateModel: UserCreateModel =
    {
        userName: "",
        userCode: "",
        password: "",
        error: null
    };

    const logic = new AccountLogic();

    /** ユーザ作成 */
    const onActionCreate = async (userCreateModel: UserCreateModel) => {

        /** スピナー表示 */
        setLoading(true);

        const loginParam: UserCreateParam =
        {
            userName: userCreateModel.userName,
            userCode: userCreateModel.userCode,
            password: userCreateModel.password
        };

        const result = await logic.CreateUser(loginParam);

        setLoading(false);

        /** ダイアログ表示 */
        messageDialog({
            title: result.messageTitle ?? "",
            message: result.message ?? "",
            onOk: () => {
                if (result.isSuccess) {
                    navigate(viewRouteAccountConsts.Login);
                }
            }
        });
    };

    return (
        <>
            <Container fluid className="user_create__view view__area d-flex">
                <Row className="form_with_advert__zone bg-success text-center">
                    <Col md={{ order: 'last', span: 4 }} className="form_with_advert_advert__section__reverse text-center">
                        <img className="img-fluid" src="./images/attendance_logo.png" />
                        <h4>My software makes you happy.</h4>
                    </Col>
                    <Col md={{ order: 'first', span: 8 }} xs={12} sm={12} className="form_with_advert_input__section__reverse">
                        <Row>
                            <Col><h2 className="form_with_advert_form__title">{logic._msg.getMsg("TextAccountCreate")}</h2></Col>
                        </Row>
                        <Row>
                            <Formik
                                enableReinitialize
                                initialValues={userCreateModel}
                                validationSchema={yupExtendsUtility.object().shape({
                                    userCode: yupExtendsUtility.string().label(logic._msg.getMsg("TextUserId")).required(logic._msg.getMsg("MessageInputRequired")).max(6, logic._msg.getMsgConvert("MessageInputMax", ["6"])).hanAlphaNumber(),
                                    password: yupExtendsUtility.string().label(logic._msg.getMsg("TextPassword")).required(logic._msg.getMsg("MessageInputRequired")).max(7, logic._msg.getMsgConvert("MessageInputMax", ["7"])).hanAlphaNumber(),
                                    userName: yupExtendsUtility.string().label(logic._msg.getMsg("TextUserName")).required(logic._msg.getMsg("MessageInputRequired")).max(5, logic._msg.getMsgConvert("MessageInputMax", ["5"]))
                                })}
                                onSubmit={values => onActionCreate(values)}>
                                {({ handleSubmit, isSubmitting, isValid, dirty }) => (
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
                                            <Col>
                                                <TextInputGeneral className="form_with_advert_form__input" name="userName" placeholder={logic._msg.getMsg("TextUserName")} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col className="my-3 mx-5">
                                                <ButtonGeneral
                                                    type="submit"
                                                    className="green"
                                                    titleLabel={logic._msg.getMsg("TextCreate")}
                                                    disabled={!isValid || !dirty || isSubmitting} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col className="m-3">
                                                <p>
                                                    <Link to={viewRouteAccountConsts.Login}>{logic._msg.getMsg("TextLogin")}</Link>
                                                </p>
                                            </Col>
                                        </Row>
                                    </Form>
                                )}
                            </Formik>
                        </Row>
                    </Col>
                </Row>
            </Container >
            {messageModalElement}
        </>
    );
}

/** エクスポート */
export default UserCreate;