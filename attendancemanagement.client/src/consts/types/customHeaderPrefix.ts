/** types */
import ConstBase from "./constBase"

/** カスタムヘッダーのプレフィックス定義 */
export default class CustomHeaderPrefix extends ConstBase {

    /** カスタムヘッダー名 */
    private _headerName: string;

    /** カスタムヘッダー名（小文字） */
    private _headerNameLowercase: string;

    /** プレフィックス */
    private _prefix: string;

    /**
     * コンストラクタ
     * @param id ID
     * @param headerName カスタムヘッダー名
     * @param headerNameLowercase カスタムヘッダー名（小文字）
     * @param prefix プレフィックス
     */
    public constructor(id: string, headerName: string, headerNameLowercase: string, prefix: string) {
        super(id);
        this._headerName = headerName;
        this._headerNameLowercase = headerNameLowercase;
        this._prefix = prefix;
    }

    /// <summary>
    /// カスタムヘッダー名
    /// </summary>
    public get headerName(): string {
        return this._headerName;
    }

    /// <summary>
    /// カスタムヘッダー名（小文字）
    /// </summary>
    public get headerNameLowercase(): string {
        return this._headerNameLowercase;
    }

    /// <summary>
    /// プレフィックス
    /// </summary>
    public get prefix(): string {
        return this._prefix;
    }
}