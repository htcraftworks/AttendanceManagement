/** CSS */
import "./css/DateInputGeneral.css"

/** npm */
import { useField } from "formik";
import { Form } from "react-bootstrap";
import DatePicker, { type DatePickerProps, registerLocale } from "react-datepicker";
import { ja } from 'date-fns/locale/ja';

/**
 * 日時入力カスタム
 * @param props Props
 * @returns コンポーネント
 */
function DateInputGeneral(props: Partial<DatePickerProps>) {
    const [field, meta, helpers] = useField(props.name);
    registerLocale('ja', ja)

    return (
        <Form.Group className="d-flex flex-column">
            {props.title && <Form.Label>{props.title}</Form.Label>}
            <DatePicker
                {...field}
                locale='ja'
                id={props.id}
                name={props.name}
                className={props.className}
                showTimeSelect={props.showTimeSelect}
                showMonthYearPicker={props.showMonthYearPicker}
                dateFormat={props.dateFormat}
                popperClassName="custom-datepicker-popper"
                dateFormatCalendar="yyyy年 MM月"
                timeFormat="HH:mm"
                timeCaption="時間"
                timeIntervals={props.timeIntervals}
                placeholderText={props.placeholderText}
                selected={(field.value && new Date(field.value)) || null}
                onChange={value =>
                    helpers.setValue(value)
                }
            />
            {meta.touched && meta.error ? (
                <Form.Label className="text-start text-danger">{meta.error}</Form.Label>
            ) : null}
        </Form.Group>
    )
}

/** エクスポート */
export default DateInputGeneral;