/** css */
import "../css/Footer.css";

/** npm */
import { Container, Row } from "react-bootstrap";

/**
 * フッター
 * @param userModel ユーザー情報
 * @returns コンポーネント
 */
const Footer = () => {

    return (
        <Container fluid className="footer__area">
            <Row>
                <p className="text-center m-3">Coded by <a href="https://github.com/htcraftworks/AttendanceManagement/">GitHub:htcraftworks</a></p>
            </Row>
        </Container>
    )
}

/** エクスポート */
export default Footer;