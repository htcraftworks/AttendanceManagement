/** CSS */
import "bootstrap/dist/css/bootstrap.min.css";

/** npm */
import { Container } from "react-bootstrap";
import { useLocation, useNavigate } from "react-router-dom";

/** consts */
import viewRouteAccountConsts from "../../consts/viewRouteAccountConsts";

/** components */
import MessageResourceService from "../../services/message/messageResourceService";
import ButtonGeneral from "../../components/ButtonGeneral";

/**
 * 不正アクセス画面
 * @returns コンポーネント
 */
const NotFound = () => {

    const location = useLocation();
    const navigate = useNavigate();

    const msg = new MessageResourceService();

    return (
        <>
            <Container fluid className="not_found__view view__area">
                <div className="mx-3 py-4">
                    <div className="d-flex justify-content-center p-4">
                        <h1 >{`"${location.pathname}"`} 📖</h1>
                    </div>
                    <div className="d-flex justify-content-center p-4">
                        <p>{msg.getMsg("MessageNotFoundPage")}</p>
                    </div>
                    <div className="d-flex justify-content-center p-4">
                        <ButtonGeneral
                            type="button"
                            className="orange"
                            titleLabel={msg.getMsg("TextBackLogin")}
                            onClick={() => navigate(viewRouteAccountConsts.Login)} />
                    </div>
                </div>
            </Container>
        </>

    );
};

/** エクスポート */
export default NotFound;