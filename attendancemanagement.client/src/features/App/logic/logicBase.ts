/** components */
import MessageResourceService from "../../../services/message/messageResourceService";

/** types */
import { type ServiceResult } from "../../../services/api/types/result/ServiceResult";

/** ロジックベースクラス */
export default class LogicBase {

    /** コンストラクタ */
    constructor() {
        this._msg = new MessageResourceService();
    }

    /** メッセージリソース */
    public readonly _msg: MessageResourceService;

    /**
     * APIエラー処理を行います
     * @param error
     * @param messageTitle
     * @param message
     */
    protected executeApiError<T>(error: any): [string, string] {
        console.log(error);

        let textError = this._msg.getMsg("TextError");
        let messageTitle: string = "";
        let message: string = "";

        const resultData = error.response.data as ServiceResult<T>;
        if (resultData) {
            messageTitle = `${textError}[ErrorCode:${resultData.errorCode}]`;
            message = `${resultData.errorMessage}`;
        } else {
            messageTitle = `${textError}[ErrorCode:${error.status}]`;
            message = this._msg.getMsg("MessageCommunicationError");
        }

        return [messageTitle, message];
    }
}