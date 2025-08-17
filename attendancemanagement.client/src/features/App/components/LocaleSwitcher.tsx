/** npm */
import { Col, Row } from "react-bootstrap";

/** provider */
import { useAccount } from '../../../provider/AccountProvider';
import localeConsts from "../../../consts/localeConsts";

/** components */
import ButtonGeneral from '../../../components/ButtonGeneral';
import MessageResourceService from "../../../services/message/messageResourceService";

/**
 * ロケール変更
 * @returns コンポーネント
 */
const LocaleSwitcher = () => {
    const { setLocale } = useAccount();

    const msg = new MessageResourceService();

    return (
        <Row>
            <Col xs={4} sm={4} md={12} className="p-1">
                <ButtonGeneral type="button" className="siann" titleLabel={msg.getMsg("TextJapanese")} onClick={() => setLocale(localeConsts.Japanese)} />
            </Col>
            <Col xs={4} sm={4} md={12} className="p-1">
                <ButtonGeneral type="button" className="siann" titleLabel={msg.getMsg("TextEnglish")} onClick={() => setLocale(localeConsts.English)} />
            </Col>
        </Row>
    );
};

/** エクスポート */
export default LocaleSwitcher;