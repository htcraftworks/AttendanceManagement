/** types */
import ConstBase from "./constBase"

/** 勤務形態定義 */
export default class WorkingStyleState extends ConstBase {

    /** 勤務形態 */
    private _state: number;

    /** メッセージリソースKEY */
    private _messageResourceKey: string;

    /**
     * コンストラクタ
     * @param id ID
     * @param state 勤務形態
     */
    public constructor(id: string, state: number, description: string) {
        super(id);
        this._state = state;
        this._messageResourceKey = description;
    }

    /// <summary>
    /// 勤務形態
    /// </summary>
    public get state(): number {
        return this._state;
    }

    /// <summary>
    /// メッセージリソースKEY
    /// </summary>
    public get messageResourceKey(): string {
        return this._messageResourceKey;
    }
}