/** types */
import { type YupMessageModel } from "./YupMessageModel"

/** yup検証種別モデル */
export type YupStringValidationModel = {
    name: string
    errorMessage: (prm: YupMessageModel) => string
    isValid: (value: string) => boolean
}
