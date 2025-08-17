/** css */
import "../css/MenuButton.css";

/** npm */
import { Col } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

/** クリックハンドラー */
type ActionHandler = () => void | Promise<void>;

/** Props*/
interface Props {
    /** メニュータイトル */
    title: string;

    /** Linkパス */
    linkPath?: string | undefined;

    /** ボタンクリックアクション */
    onAction?: ActionHandler | undefined;

    /** 画像ファイルパス */
    imgPath?: string | undefined;
}

/**
 * メニューボタン
 * @returns コンポーネント
 */
const MenuButton = ({ title, linkPath, onAction, imgPath }: Props) => {

    const navigate = useNavigate();

    return (
        <>
            <Col xs={6} md={3} className="py-3">
                <div className="menu_btn__unit d-flex flex-column rounded overflow-hidden h-100">

                    <div onClick={() => {
                        if (linkPath) {
                            navigate(linkPath);
                            return;
                        }

                        if (onAction) {
                            onAction();
                            return;
                        }
                    }}>
                        <div className="position-relative border rounded border-success">
                            <div className="menu_btn__unit_title text-center p-3">
                                <h4>{title}</h4>
                            </div>
                            {imgPath && (
                                <img className="menu_btn__unit_img" src={imgPath} alt="" />
                            )}
                            <div className="menu_btn__unit_overlay" />
                        </div>
                    </div>
                </div>
            </Col>
        </>
    )
}

/** エクスポート */
export default MenuButton;