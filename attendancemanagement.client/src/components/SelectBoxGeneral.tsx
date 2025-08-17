/** npm */
import { useField } from "formik";
import { Form } from "react-bootstrap"

/** types */
import { type DictionaryModel } from "../types/DictionaryModel";

/** Props*/
interface Props {

    /** SelectBoxのOption属性値 */
    options: DictionaryModel[];

    /** SelectBoxのname属性値 */
    name: string;

    /** タイトルラベルのタイトル */
    title?: string;

    /** SelectBoxのclass属性値 */
    className?: string | undefined;
}

/**
 * セレクトボックスカスタム
 * @param props Props
 * @returns コンポーネント
 */
function SelectBoxGeneral(props: Props) {
    const [field, meta] = useField(props.name);

    return (
        <Form.Group className="d-flex flex-column align-items-start">
            {props.title && <Form.Label>{props.title}</Form.Label>}
            <select {...field} {...props}>
                {props.options.map((dic) => (
                    <option key={dic.key} value={dic.key} label={dic.value} />
                ))}
            </select>
            {meta.touched && meta.error ? (
                <div>{meta.error}</div>
            ) : null}
            </Form.Group>
    )
}

/** エクスポート */
export default SelectBoxGeneral;