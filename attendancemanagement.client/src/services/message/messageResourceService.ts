/** resource */
import Messages from "../../../public/resource/messageResource";

/** consts */
import localeConsts from "../../consts/localeConsts";

/** provider */
import { useAccount } from '../../provider/AccountProvider';

/** メッセージリソースユーティリティ */
export default class MessageResourceService {

    /** コンストラクタ */
    constructor() {
        this.locale = useAccount().locale;
    }

    /** ロケール */
    private locale: string = localeConsts.Japanese;

    /**
     * 埋め込み形式のメッセージを取得します
     * @param key KEY
     * @returns メッセージ
     */
    public getMsg = (key: string) => {
        return this.getMsgConvert(key, null);
    };

    /**
     * 埋め込み形式のメッセージを取得します
     * NOTE:hook(CONTEXT)は制約があるのでストレージ参照
     * @param key KEY
     * @param array 埋め込み用の文字配列
     * 　　　　　　 例:メッセージ「{0}は必須です」の場合、{0}に配列[0]の値が置換される
     * @returns メッセージ
     */
    public getMsgConvert = (key: string, array: string[] | null) => {

        let message = Messages[this.locale].find(r => r.key == key)?.value ?? "";

        if (array) {
            let index = 0;
            array.forEach((p) => {
                message = message.replace(`{${index}}`, p);
                index++;
            });
        }

        return message;
    };
}