/** types */
import ConstBase from "./constBase"

/** タイムゾーン情報定義 */
export default class TimeZoneConst extends ConstBase {

    /** ロケール */
    private _local: string;

    /** タイムゾーン */
    private _timeZone: string;

    /**
     * コンストラクタ
     * @param id ID
     * @param locale ロケール
     * @param timeZone タイムゾーン
     */
    public constructor(id: string, local: string, timeZone: string) {
        super(id);
        this._local = local;
        this._timeZone = timeZone;
    }

    /// <summary>
    /// ロケール
    /// </summary>
    public get locale(): string {
        return this._local;
    }

    /// <summary>
    /// タイムゾーン
    /// </summary>
    public get timeZone(): string {
        return this._timeZone;
    }
}