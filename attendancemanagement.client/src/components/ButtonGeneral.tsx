/** CSS */
import "./css/ButtonGeneral.css";

/** npm */
import { Button } from "react-bootstrap";
import { type ReactNode } from "react";

/** Props*/
interface Props {

    /**children */
    children?: ReactNode;

    /** タイトル名称 */
    titleLabel: string;

    /** ButttonのclassName属性 */
    className?: string | undefined;

    /** Butttonのdisabled属性 */
    disabled?: boolean | undefined;

    /** Butttonのtype属性 */
    type?: ("button" | "submit" | "reset" | undefined);

    /** ボタンクリックアクション */
    onClick?: (() => void) | undefined;

    /**  Butttonのref属性  */
    ref?: React.RefObject<HTMLButtonElement | null>;
}

/**
 * ボタン
 *  @param props Props
 * @returns コンポーネント
 */
function ButtonGeneral({ children, titleLabel, className, ...otherProps }: Props) {

    const addClass = `${className ?? ""}`;

    return (
        <div className="button__general">
            <Button {...otherProps} className={addClass}>{titleLabel}{children}</Button>
        </div>
    )
}

/** エクスポート */
export default ButtonGeneral;