/** CSS */
import "../css/ViewTitle.css";

/** npm */
import { Col, Row } from "react-bootstrap";

/** Props*/
interface Props {
    /** タイトル名称 */
    title: string;
}

/**
 * 画面タイトル
 *  @param props Props
 * @returns コンポーネント
 */
function ViewTitle(props: Props) {

    return (
        <>
            <Row>
                <Col md={12} className="view_title__unit rounded p-3">
                    <h4 className="text-center mb-0">{props.title}</h4>
                </Col>
            </Row>
        </>
    )
}

/** エクスポート */
export default ViewTitle;