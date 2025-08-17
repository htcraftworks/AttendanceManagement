/** ベース定義クラス */
export default class ConstBase {

    /** ID */
    private _id: string;

    /**
     *  コンストラクタ
     * @param id ID
     */
    protected constructor(id: string) {
        this._id = id;
    }

    /// <summary>
    /// ID
    /// </summary>
    public get id(): string {
        return this._id;
    }
}