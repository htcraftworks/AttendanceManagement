/** CSS */
import "./css/TextInputGeneral.css"

/** npm */
import { useState } from "react";
import { useField } from "formik";
import { Form } from "react-bootstrap"

/** Props*/
interface Props {

    /** TEXTAreaのplaceholder属性値 */
    placeholder: string | undefined;

    /** TEXTのname属性値  */
    name: string;

    /** TEXTのclassName属性値 */
    className?: string | undefined;

    /** TEXTのvalue属性値 */
    value?: string | undefined;

    /** TEXTのlabel属性値 */
    label?: string | undefined;

    /** TEXTのtype属性値 */
    type?: string | undefined;

    /** TEXTのas属性値 */
    as?: React.ElementType;

    /** TEXTのrows属性値 */
    rows?: number | undefined;

    /** TEXTのdisabled属性値 */
    disabled?: boolean | undefined;
}

/**
 * テキスト入力カスタム
 * @param props Props
 * @returns コンポーネント
 */
function TextInputGeneral(props: Props) {
    const [field, meta, helpers] = useField(props.name);
    const [currentValue, setCurrentValue] = useState(props.value);
    return (
        <>
            <Form.Group className="text_input_unit d-flex flex-column">
                {props.label && <Form.Label>{props.label}</Form.Label>}
                <Form.Control {...field} {...props}
                    value={currentValue ?? ""}
                    onChange={e => {
                        setCurrentValue(e.target.value);
                        helpers.setValue(e.target.value);
                    }} ></Form.Control>
                {meta.touched && meta.error ? (
                    <Form.Label className="text-start text-danger">{meta.error}</Form.Label>
                ) : null}
            </Form.Group>
        </>
    )
}

/** エクスポート */
export default TextInputGeneral;