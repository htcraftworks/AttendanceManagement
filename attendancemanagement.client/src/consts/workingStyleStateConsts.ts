/** types */
import WorkingStyleState from "./types/workingStyleState"

/** 
 * 勤務形態定義
 */
export default class WorkingStyleStateConsts {

    /// <summary>
    /// 出社
    /// </summary>
    public static Office: WorkingStyleState = new WorkingStyleState("Office", 1, "TextOfficeIn");

    /// <summary>
    /// 外出
    /// </summary>
    public static GoOut: WorkingStyleState = new WorkingStyleState("GoOut", 2, "TextGoOut");

    /// <summary>
    /// 在宅
    /// </summary>
    public static Home: WorkingStyleState = new WorkingStyleState("Home", 3, "TextHome");
}